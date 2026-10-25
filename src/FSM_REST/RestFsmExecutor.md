# RestFsmExecutor

**Owner:** @TrentBest  
**Purpose:** Execute a REST request while using FSM_API to track the request's transport lifecycle.  
**Source:** [RestFsmExecutor.cs](RestFsmExecutor.cs).

## Contract

`RestFsmExecutor.SendAsync` drives a registered FSM_API definition through `Ready → Sending → Completed`, or to `Failed` / `Cancelled` when transport execution throws. The executor destroys its FSM instance after execution.

If recording `Failed` or `Cancelled` throws, that secondary lifecycle exception is attached to the original exception's `Data` under `FSM_REST.LifecycleRecordingException` when possible, and the original exception remains the one rethrown. The same preservation rule applies if `DestroyInstance` throws while another exception is already propagating. If destruction alone fails after an otherwise successful operation, that cleanup exception is allowed to surface.

A non-2xx HTTP response is still `Completed`: transport completion and HTTP/application success are different concepts. The returned `RestResponse` remains the source of status, headers, and body.

## Use cases

- Use this executor when the caller wants the REST request lifecycle explicitly represented by FSM_API.
- Supply an `IRestTransport` so tests can drive success, failure, and cancellation deterministically.
- Inspect the returned `RestResponse` for HTTP status; do not treat the FSM's `Completed` state as an assertion that the remote operation succeeded.

## Boundaries

This is an opt-in execution path. Existing direct `IRestTransport.SendAsync` use remains available for callers that do not need lifecycle orchestration. Authentication, retries, and domain interpretation remain outside this executor.

## Verification note

Exception-preservation behavior is source-reviewed; deterministic tests that force FSM lifecycle-recording or instance-destruction failures and a successful exact-commit CI run remain outstanding.
