<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# HttpClientRestTransport

**Workshop moniker:** @TrentBest  
**Purpose:** Adapt the host-supplied `HttpClient` to the transport-neutral `IRestTransport` interface.

## Responsibilities
- Convert a `RestRequest` into an `HttpRequestMessage`.
- Apply body content and content type, then apply request headers to either request or content headers.
- Send with response-headers-first completion and propagate cancellation.
- Return status, reason phrase, headers, and body as `RestResponse`.

## Boundaries
The host configures and owns the supplied `HttpClient`. This adapter does not define authentication, retries, caching, or domain interpretation. A header that cannot be applied is rejected instead of silently discarded.

## Source and verification
- Source: [HttpClientRestTransport.cs](HttpClientRestTransport.cs)
- Contract: [IRestTransport](IRestTransport.md)
- UML: [Core architecture](../../docs/UML/FSM_REST.Core.UML.md), [request execution](../../docs/UML/RestRequestExecution.UML.md)
- Tests: [FSM_REST tests](../../tests/FSM_REST.Tests/)
- Provenance: manually authored companion.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
