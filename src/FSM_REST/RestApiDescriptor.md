<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# RestApiDescriptor

**Workshop moniker:** @TrentBest  
**Purpose:** Describe a REST API as a protocol-neutral collection of operation descriptors.

## Responsibilities
- Store API title, version, operation list, and optional description.
- Expose a derived operation count.

## Boundaries
The descriptor is metadata only. It does not discover, execute, authenticate, render, or host the API. Description-format importers translate their source format into this model.

## Source and verification
- Source: [RestApiDescriptor.cs](RestApiDescriptor.cs)
- Target-centered UML and use cases: [RestApiDescriptorUML.md](../../docs/UML/RestApiDescriptorUML.md)
- Related operation model: [RestOperationDescriptor](RestOperationDescriptor.md)
- Tests: [FSM_REST tests](../../tests/FSM_REST.Tests/)
- Provenance: manually authored companion.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
