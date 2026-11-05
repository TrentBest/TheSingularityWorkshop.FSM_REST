# HttpClientRestTransport

**Workshop moniker:** @TrentBest  
**Purpose:** Adapt the host-supplied `HttpClient` to the transport-neutral `IRestTransport` interface.

## Responsibilities
- Convert a `RestRequest` into an `HttpRequestMessage`.
- Apply body content and content type, then apply request headers to either request or content headers.
- Send with response-headers-first completion and propagate cancellation.
- Return status, reason phrase, headers, and body as `RestResponse`.

## Boundaries
The one-argument constructor wraps a host-configured HttpClient without taking ownership. The overload accepting ownsClient makes ownership explicit for construction adapters: when true, disposing the transport disposes the client. This framework-neutral ownership switch allows optional integration packages to adapt factory-created clients without adding framework dependencies to core. The adapter does not define authentication, retries, caching, or domain interpretation. A header that cannot be applied is rejected instead of silently discarded.

## Source and verification
- Source: [HttpClientRestTransport.cs](HttpClientRestTransport.cs)
- Contract: [IRestTransport](IRestTransport.md)
- UML: [Core architecture](../../docs/UML/FSM_REST.Core.UML.md), [request execution](../../docs/UML/RestRequestExecution.UML.md)
- Tests: [FSM_REST tests](../../tests/FSM_REST.Tests/)
- Provenance: manually authored companion.
