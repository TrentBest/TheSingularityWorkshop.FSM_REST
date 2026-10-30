<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# RestOperationExecutor

**Workshop moniker:** @TrentBest  
**Purpose:** Build and send one operation request using an injected transport.

## Responsibilities
- Validate the transport reference.
- Delegate request construction to `RestRequestFactory`.
- Send the resulting request through `IRestTransport` and propagate cancellation.
- Return the raw observed response.

## Boundaries
Authentication, retry strategy, response deserialization, and domain-model creation remain host/provider responsibilities.

## Source and verification
- Source: [RestOperationExecutor.cs](RestOperationExecutor.cs)
- Request construction: [RestRequestFactory](RestRequestFactory.md)
- Transport contract: [IRestTransport](IRestTransport.md)
- UML: [request execution](../../docs/UML/RestRequestExecution.UML.md)
- Tests: [FSM_REST tests](../../tests/FSM_REST.Tests/)
- Provenance: manually authored companion.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
