<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# RestParameterDescriptor

**Workshop moniker:** @TrentBest  
**Purpose:** Describe one parameter accepted by a REST operation.

## Responsibilities
Store the parameter name, location, required flag, optional logical type and format hints, and human-readable description. The descriptor is metadata only; `RestRequestFactory` requires supported locations and unambiguous binding names.

## Boundaries
The descriptor stores metadata; it does not serialize arrays/objects or apply the full OpenAPI style/explode rules. Request construction validates supported locations and required values according to the current factory implementation.

## Source and verification
- Source: [RestParameterDescriptor.cs](RestParameterDescriptor.cs)
- Parent model: [RestOperationDescriptor](RestOperationDescriptor.md)
- UML: [core architecture](../../docs/UML/FSM_REST.Core.UML.md)
- Tests: [FSM_REST tests](../../tests/FSM_REST.Tests/)
- Provenance: manually authored companion.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
