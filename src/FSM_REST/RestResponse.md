# RestResponse

**Workshop moniker:** @TrentBest  
**Purpose:** Represent the response observed at the transport boundary without assigning domain meaning.

## Responsibilities
- Store status code, reason phrase, headers, and raw body.
- Expose whether the status code is in the HTTP 2xx range.
- Offer JSON deserialization into a consumer-owned type using web defaults unless options are supplied.

## Boundaries
Deserialization is not schema validation. The caller interprets non-success status codes, domain errors, and the meaning of returned data.

## Source and verification
- Source: [RestResponse.cs](RestResponse.cs)
- Request/transport: [RestRequest](RestRequest.md), [IRestTransport](IRestTransport.md)
- UML: [core architecture](../../docs/UML/FSM_REST.Core.UML.md), [request execution](../../docs/UML/RestRequestExecution.UML.md)
- Tests: [FSM_REST tests](../../tests/FSM_REST.Tests/)
- Provenance: manually authored companion.
