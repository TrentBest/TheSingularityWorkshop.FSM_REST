<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

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

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
