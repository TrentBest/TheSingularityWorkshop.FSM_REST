<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# RestFsmExecutor — FSM-backed REST execution

**Target:** `TheSingularityWorkshop.FSM_REST.RestFsmExecutor`  
**Source:** [RestFsmExecutor.cs](../../../src/FSM_REST/RestFsmExecutor.cs)  
**Provenance:** Manually maintained, source-grounded diagram.

## Lifecycle diagram

```mermaid
stateDiagram-v2
    [*] --> Ready
    Ready --> Sending : FSM_API transition
    Sending --> Completed : transport returns response
    Sending --> Failed : transport throws
    Sending --> Cancelled : requested cancellation
    Completed --> [*] : destroy instance
    Failed --> [*] : destroy instance
    Cancelled --> [*] : destroy instance
```

## Responsibilities

- FSM_API owns the state transition mechanism.
- RestFsmExecutor owns the request/transport call and updates the lifecycle context.
- IRestTransport owns communication behavior.
- RestResponse preserves the HTTP status and payload.

## Use cases

1. **Track a request lifecycle.** Use `RestFsmExecutor.SendAsync` when the caller wants explicit FSM state progression around transport execution.
2. **Separate transport outcome from HTTP status.** A returned 404 reaches `Completed`; inspect `RestResponse.IsSuccessStatusCode` separately.
3. **Test failures without networking.** Inject an `IRestTransport` fake that returns a response or throws.

## Scope

The lifecycle is opt-in; direct transport calls remain supported. Authentication, retries, schema validation, and domain-level success criteria are not handled here.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
