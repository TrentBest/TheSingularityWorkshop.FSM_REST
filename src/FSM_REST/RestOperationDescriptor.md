<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# RestOperationDescriptor

**Workshop moniker:** @TrentBest  
**Purpose:** Describe one reusable REST operation capability, including its route and declared inputs/outputs.

## Responsibilities
- Store method, relative path, stable operation ID, summary/description, parameter definitions, optional request-body metadata, and optional response descriptors.
- Produce a palette label from the summary, falling back to method and path.
- Expose an empty response collection when response metadata was not supplied.

## Boundaries
This descriptor is not a runtime invocation, request, transport, or guarantee that the remote operation exists or is authorized.

## Source and verification
- Source: [RestOperationDescriptor.cs](RestOperationDescriptor.cs)
- API collection: [RestApiDescriptor](RestApiDescriptor.md)
- Parameters/body/responses: [RestParameterDescriptor](RestParameterDescriptor.md), [RestRequestBodyDescriptor](RestRequestBodyDescriptor.md), [RestResponseDescriptor](RestResponseDescriptor.md)
- UML: [RestOperationDescriptor UML](../../docs/UML/RestOperationDescriptor.UML.md), [core architecture](../../docs/UML/FSM_REST.Core.UML.md)
- Tests: [FSM_REST tests](../../tests/FSM_REST.Tests/)
- Provenance: manually authored companion.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
