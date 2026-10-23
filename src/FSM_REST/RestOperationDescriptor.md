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
