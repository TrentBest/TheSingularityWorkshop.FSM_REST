<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# RestOperationBinding

**Workshop moniker:** @TrentBest  
**Purpose:** Represent one invocation of a reusable REST operation with a snapshot of runtime parameter values.

## Responsibilities
- Retain the operation descriptor being invoked.
- Copy provided parameter values into an ordinal-keyed dictionary.
- Expose the copied values through a read-only dictionary so later caller mutations do not change the binding snapshot.

## Boundaries
The binding does not construct a URI, send a request, mutate the operation descriptor, or interpret a response.

## Source and verification
- Source: [RestOperationBinding.cs](RestOperationBinding.cs)
- Factory: [RestRequestFactory](RestRequestFactory.md)
- UML: [core architecture](../../docs/UML/FSM_REST.Core.UML.md), [request execution](../../docs/UML/RestRequestExecution.UML.md)
- Tests: [FSM_REST tests](../../tests/FSM_REST.Tests/)
- Provenance: manually authored companion.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
