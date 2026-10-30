<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# RestRequestFactory

**Workshop moniker:** @TrentBest  
**Purpose:** Bind operation metadata and runtime values into a transport-ready `RestRequest`.

## Responsibilities
- Accept an operation plus values or an existing `RestOperationBinding`.
- Validate the base API URI, HTTP method token, relative operation path, and required inputs.
- Reject absolute/network paths, query/fragment-bearing paths, and literal or percent-encoded dot segments that could escape the API base path.
- Apply supported path, query, header, and cookie parameter behavior as implemented.
- Treat a missing key and a `null` value as absent; required parameters fail when absent.
- Preserve explicit empty query/header/cookie values, while rejecting empty path values.
- Reject unsupported locations, invalid names, duplicate/ambiguous parameter names, and runtime values whose keys are not declared by the operation.
- Percent-encode query plus signs as `%2B`; the current model does not support array or repeated parameter values.
- Produce a request without performing network I/O.

## Boundaries
This factory does not send requests, authenticate, cache responses, retry, or interpret payloads. Parameter serialization is intentionally narrower than full OpenAPI style/explode support.

## Source and verification
- Source: [RestRequestFactory.cs](RestRequestFactory.cs)
- Binding: [RestOperationBinding](RestOperationBinding.md)
- Output: [RestRequest](RestRequest.md)
- UML: [core architecture](../../docs/UML/FSM_REST.Core.UML.md), [request execution](../../docs/UML/RestRequestExecution.UML.md)
- Tests: [FSM_REST tests](../../tests/FSM_REST.Tests/), including undeclared-runtime-parameter rejection.
- Provenance: manually authored companion.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
