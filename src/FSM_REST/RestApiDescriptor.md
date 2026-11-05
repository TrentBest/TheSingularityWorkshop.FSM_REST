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
