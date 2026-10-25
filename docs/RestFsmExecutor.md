# RestFsmExecutor

**Owner / author:** Trent Best  
**Purpose:** Execute one REST request while using FSM_API to model that invocation's transport lifecycle.  
**Evidence:** Source-checked against `src/FSM_REST/RestFsmExecutor.cs` on `development`; this page is not a claim that the example was executed independently.  
**Related:** [Architecture contract](ARCHITECTURE.md) · [Gallery](../Gallery.md) · [UML and use cases](UML/RestFsmExecutor.UML.md)

## What it does

`RestFsmExecutor.SendAsync` is the opt-in FSM-governed alternative to calling `IRestTransport.SendAsync` directly. It creates a distinct FSM_API instance for each invocation, moves that instance through the transport lifecycle, returns the received `RestResponse`, and destroys the instance in a `finally` block.

The reusable definition is registered once by the class's static initializer. Each request gets its own `ExecutionContext` and instance handle; concurrent invocations therefore do not share the per-request target state.

## Lifecycle

```mermaid
stateDiagram-v2
    [*] --> Ready
    Ready --> Sending: request begins
    Sending --> Completed: transport returns response
    Sending --> Failed: transport throws
    Sending --> Cancelled: requested cancellation throws
    Completed --> [*]: instance destroyed
    Failed --> [*]: instance destroyed
    Cancelled --> [*]: instance destroyed
```

The current implementation advances the FSM by updating the invocation context's `TargetState` and calling `handle.Update(ProcessingGroup)`. It checks that the handle reached `Sending` and `Completed`; a mismatch raises `InvalidOperationException`. The FSM definition's transition guards compare the requested target state with the transition destination.

## Important semantic boundary

- **Completed means the transport returned a response.** An HTTP 404 or 500 is not automatically an FSM failure; inspect `RestResponse.IsSuccessStatusCode` and the response content separately.
- **Failed means transport/execution threw an exception** on the normal send path.
- **Cancelled means an `OperationCanceledException` occurred while the supplied cancellation token was requested.** An unrelated cancellation exception follows the general failure path.
- The executor rethrows transport exceptions after attempting to record the corresponding lifecycle state. If that terminal-state update throws, the recording exception is attached to the original exception's `Data` under `FSM_REST.LifecycleRecordingException`, and the original transport exception remains the one rethrown. This is implemented in source but still needs exact-SHA test/CI verification.
- The instance is destroyed in `finally`; the state is an execution lifecycle, not a durable request history or telemetry store. Cleanup itself can still throw, so exception preservation during destruction remains a lifecycle edge to assess.
- The static FSM definition and processing group are registered by the class initializer. This is source behavior, not a claim about cross-process persistence.

## Usage

```csharp
using var client = new HttpClient();
IRestTransport transport = new HttpClientRestTransport(client);

RestResponse response = await RestFsmExecutor.SendAsync(
    request,
    transport,
    cancellationToken);

if (!response.IsSuccessStatusCode)
{
    // HTTP status is a response-level result, not a transport exception.
}
```

This snippet is **source-checked**, not independently executed as part of this document. The caller must supply an already constructed `RestRequest` and an `IRestTransport`.

## Verification still required

The current test source contains five focused cases: a non-success HTTP response (404), transport exception propagation, caller-requested cancellation, an unsolicited `OperationCanceledException`, and two overlapping invocations that must return their own responses. These tests are present in `tests/FSM_REST.Tests/RestFsmExecutorTests.cs`, but passing execution on the current development SHA has not yet been confirmed. Remaining lifecycle coverage includes explicit cleanup assertions after successful and exceptional paths. The implementation now attempts to preserve the original transport exception if recording `Failed` or `Cancelled` throws, attaching the secondary exception to `Exception.Data`. This new behavior is source-reviewed but not yet covered by a deterministic test or exact-SHA CI. Cleanup failure from `DestroyInstance` also remains an edge to assess. Use a successful exact-SHA CI run as evidence before release.

## Dependencies

`TheSingularityWorkshop.FSM_Rest` directly references `TheSingularityWorkshop.FSM_API` version `1.0.13` in the current development project file. FSM_API owns the state-machine definition and instance lifecycle primitives; FSM_REST owns the REST-specific state names and the decision about what transport outcomes mean. FSM_REST core does not depend on FSM_COS.
