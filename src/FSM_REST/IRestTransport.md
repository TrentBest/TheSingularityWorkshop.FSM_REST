# IRestTransport

**Workshop moniker:** @TrentBest  
**Purpose:** Define the replaceable communication boundary for sending a REST request and observing a response.

## Responsibilities
- Accept a `RestRequest` and optional cancellation token.
- Return a `Task<RestResponse>`.
- Permit HTTP adapters, custom transports, and test doubles without coupling descriptors to a concrete client.

## Boundaries
The interface does not define request construction, authentication policy, retries, or response-domain semantics.

## Source and verification
- Source: [IRestTransport.cs](IRestTransport.cs)
- HTTP implementation: [HttpClientRestTransport](HttpClientRestTransport.md)
- Request model: [RestRequest](RestRequest.md); response model: [RestResponse](RestResponse.md)
- UML: [Core architecture](../../docs/UML/FSM_REST.Core.UML.md), [request execution](../../docs/UML/RestRequestExecution.UML.md)
- Provenance: manually authored companion.
