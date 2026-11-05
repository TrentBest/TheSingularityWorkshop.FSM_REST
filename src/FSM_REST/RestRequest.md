# RestRequest

**Workshop moniker:** @TrentBest  
**Purpose:** Carry a constructed request independently of the concrete transport.

## Responsibilities
Represent the HTTP method, target URI, optional headers, optional body, and optional content type in a transport-neutral record.

## Boundaries
The request does not own client lifetime, authentication, retries, or network execution. `IRestTransport` is responsible for sending it. The built-in HTTP adapter defaults to JSON content type when a body exists and no content type was supplied.

## Source and verification
- Source: [RestRequest.cs](RestRequest.cs)
- Construction: [RestRequestFactory](RestRequestFactory.md)
- Sending: [IRestTransport](IRestTransport.md)
- UML: [core architecture](../../docs/UML/FSM_REST.Core.UML.md), [request execution](../../docs/UML/RestRequestExecution.UML.md)
- Tests: [FSM_REST tests](../../tests/FSM_REST.Tests/)
- Provenance: manually authored companion.
