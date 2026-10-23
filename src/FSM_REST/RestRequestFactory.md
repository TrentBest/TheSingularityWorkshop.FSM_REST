# RestRequestFactory

**Workshop moniker:** @TrentBest  
**Purpose:** Bind operation metadata and runtime values into a transport-ready `RestRequest`.

## Responsibilities
- Accept an operation plus values or an existing `RestOperationBinding`.
- Validate the base API URI and required inputs.
- Apply supported path, query, header, and cookie parameter behavior as implemented.
- Produce a request without performing network I/O.

## Boundaries
This factory does not send requests, authenticate, cache responses, retry, or interpret payloads. Parameter serialization is intentionally narrower than full OpenAPI style/explode support.

## Source and verification
- Source: [RestRequestFactory.cs](RestRequestFactory.cs)
- Binding: [RestOperationBinding](RestOperationBinding.md)
- Output: [RestRequest](RestRequest.md)
- UML: [core architecture](../../docs/UML/FSM_REST.Core.UML.md), [request execution](../../docs/UML/RestRequestExecution.UML.md)
- Tests: [FSM_REST tests](../../tests/FSM_REST.Tests/)
- Provenance: manually authored companion.
