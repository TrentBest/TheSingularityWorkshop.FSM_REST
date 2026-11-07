# FSM_API Integration Assessment

**Owner / Workshop moniker:** @TrentBest  
**Scope:** FSM_REST development source and the FSM_API 1.0.13 package contract  
**Status:** Evidence-based design assessment; proposed consumer-facing improvements are not yet implemented.

## Executive finding

FSM_REST is intentionally coupled to FSM_API. That coupling should be a product advantage: REST invocations should be able to participate in the Workshop's state-machine model without requiring every consumer to hand-build the same lifecycle logic.

Alpha 5 establishes the first real integration through `RestFsmExecutor`. It registers a shared FSM definition in the `FSM_REST` processing group and records `Ready → Sending → Completed/Failed/Cancelled`. It correctly treats an HTTP non-2xx response as a completed transport exchange. Direct `IRestTransport.SendAsync` remains available.

The current integration is nevertheless narrow. It uses FSM_API to record internal phase changes, but keeps the `IStateContext` and `FSMHandle` private and returns only `RestResponse`. Consumers cannot observe or extend the lifecycle through the FSM contract. The current state machine therefore adds internal structure, but does not yet expose most of the value that motivated the direct dependency.

## What FSM_API already offers

The reviewed FSM_API 1.0.13 source/package surface provides:

- `FSM_API.Create.CreateFiniteStateMachine` and `FSMBuilder` for named reusable definitions, states, transitions, guards, and state actions.
- `IStateContext` for per-instance state data and validity.
- `FSMHandle` for current-state inspection, explicit stepping, forced transitions, reset, and instance identity.
- Processing groups and process-rate control for grouping and scheduling FSM instances.
- `FSM_API.Interaction.Update`, instance destruction, and deferred structural modification.
- Context invalidation during handle destruction, allowing lifecycle cleanup to be represented explicitly.

FSM_REST currently uses the builder, a private context, a handle, manual state stepping, and handle destruction. It does not expose state actions, guards, the handle, context, or processing-group participation as consumer extension points.

## Current implementation observations

### 1. The FSM is a lifecycle recorder, not an async workflow scheduler

The REST transport is asynchronous. FSM_API's state stepping is synchronous. The current separation—await the transport in ordinary async code, then step the FSM at meaningful lifecycle boundaries—is sound. FSM_API should not be made responsible for awaiting HTTP or owning transport policy.

The definition uses process rate `0` (event-driven/manual). Calling `FSMHandle.Update` for each transition is consistent with that model; putting every network operation on a frame/tick loop would add scheduling complexity without improving the transport contract.

### 2. Consumers cannot currently benefit from lifecycle visibility

The private execution context contains only `Name`, `IsValid`, and `TargetState`. The private handle is destroyed before the method returns. There is no consumer callback or result object that exposes the lifecycle state, operation identity, elapsed time, response, or failure.

Consequently, a consumer cannot naturally bind loading indicators, progress views, audit records, workflow guards, or downstream FSM transitions to the REST execution state without wrapping the executor independently.

### 3. FSM stepping failures need stronger verification

`FSMHandle.Update` catches exceptions raised during FSM stepping and reports them through FSM_API rather than rethrowing them. The executor checks that the FSM reached `Sending` and `Completed`, but its failure/cancellation recording path does not verify that the requested terminal state was actually reached.

A transport exception must remain the primary exception. Lifecycle-recording failures should remain secondary, but must be observable and must not silently claim that a terminal transition succeeded.

### 4. Shared definition identity and global registry need explicit protection

The definition name and processing group are fixed strings in a process-wide FSM_API registry. FSM_API's builder can return a builder for an existing definition with the same name/group. The current names are appropriately namespaced, but the integration should still verify that it never mutates a definition owned by another component and that initialization is safe when multiple consumers initialize concurrently.

### 5. Concurrency guarantees are not yet established

The current REST test verifies that two concurrent calls return their own responses. That does not prove that FSM_API's shared definition registry, instance collections, error bookkeeping, and create/destroy operations are safe under concurrent continuations. FSM_REST must either establish the required FSM_API concurrency guarantees, serialize only the lifecycle-registry operations, or explicitly document a narrower guarantee. Do not add a broad global lock around network I/O.

### 6. Context ownership is correctly per invocation

Each request gets its own context and handle, while the immutable lifecycle definition is shared. This is the right conceptual split: definition is reusable behavior; context is invocation-specific state. The executor should preserve that ownership and ensure every handle is destroyed exactly once, including cancellation and exceptional paths.

## Recommended consumer-facing direction

The goal is not to force every REST consumer to use FSMs. Preserve the simple path:

```csharp
RestResponse response = await transport.SendAsync(request, cancellationToken);
```

Then make the FSM-backed path genuinely useful:

1. **Expose a typed execution lifecycle contract.** Provide a consumer-visible execution object or result that reports the FSM state and invocation data without exposing mutable global FSM internals.
2. **Allow per-invocation lifecycle observation.** Use FSM state actions against the invocation context to notify an optional observer at state entry/exit. Keep observer failures from corrupting transport outcomes; define their error semantics explicitly.
3. **Preserve one authoritative state model.** The public state value should be derived from the FSM handle/context, not maintained as a second independent state machine.
4. **Carry useful invocation context.** At minimum: operation/request identity, start/end timestamps or elapsed duration, response when received, exception when transport fails, and cancellation outcome. Avoid copying credentials or sensitive headers into diagnostic context by default.
5. **Support composition, not policy leakage.** Let a host or higher-level FSM react to REST lifecycle events. Do not move authentication, retries, caching, rate limits, endpoint selection, or business interpretation into the core executor.
6. **Keep async transport ownership outside FSM_API.** FSM_API governs state transitions; the async method owns awaiting, cancellation-token propagation, and transport exception propagation.
7. **Make cleanup and terminal-state evidence testable.** Verify every success/failure/cancellation path reaches the expected FSM state, destroys the handle once, invalidates its context, and preserves the original transport exception if cleanup or observation fails.

A later, carefully designed extension could let advanced consumers supply lifecycle behavior through a dedicated execution policy/observer contract. Avoid accepting arbitrary global FSM definition names or mutating shared definitions per request; FSM_API definitions are registered by name and processing group.

## Work sequence

### P0 — Prove the current integration

- Add tests that observe the actual FSM state reached for success, HTTP non-success, transport exception, requested cancellation, and cancellation not requested by the caller.
- Verify terminal-state transition failures are surfaced as secondary lifecycle failures without replacing the original transport exception.
- Test concurrent creation, state stepping, and destruction of many handles, not merely response independence.
- Verify definition initialization is idempotent and cannot silently adopt or mutate an incompatible pre-existing definition.
- Verify handle cleanup and context invalidation on all paths.

### P1 — Deliver consumer value

- Design the smallest public lifecycle observer/result contract that exposes useful state without leaking FSM_API's process-wide registry.
- Add state-entry hooks through the per-invocation context and define observer exception semantics.
- Include safe execution metadata and timing, with no implicit credential/header capture.
- Add consumer examples for loading/error UI state and for a higher-level FSM reacting to REST execution.

### P2 — Broaden only when consumers need it

- Consider retry attempts as explicit child executions or a higher-level workflow, not hidden transport behavior.
- Consider timeouts/deadlines and streaming/binary payload lifecycles only after their contracts are independently defined.
- Keep FSM_COS, GUI, OpenAPI, and concrete service policy in their existing optional boundaries.

## Architectural decision

**Use FSM_API as the authoritative lifecycle engine for the opt-in FSM-backed REST execution path, not as a mandatory wrapper around every transport call.** The default transport interface remains lightweight and framework-neutral. The FSM-backed path should expose enough lifecycle state and hooks that a Workshop consumer can compose it directly into its own FSM/Experience.

No FSM_API source change is proposed by this assessment. Any needed improvements to FSM_API itself should be justified by a demonstrated cross-consumer limitation and addressed in FSM_API, rather than worked around with reflection or access to internal registries here.
