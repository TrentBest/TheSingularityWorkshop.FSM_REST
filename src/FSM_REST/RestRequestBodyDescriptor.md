# RestRequestBodyDescriptor

**Workshop moniker:** @TrentBest  
**Purpose:** Describe body metadata associated with a REST operation.

## Responsibilities
Store whether a request body is required, accepted media types, and optional logical schema type/format hints.

## Boundaries
The descriptor does not validate or serialize a body against a schema. The consumer or provider supplies the actual runtime body.

## Source and verification
- Source: [RestRequestBodyDescriptor.cs](RestRequestBodyDescriptor.cs)
- Parent model: [RestOperationDescriptor](RestOperationDescriptor.md)
- UML: [core architecture](../../docs/UML/FSM_REST.Core.UML.md)
- Tests: [FSM_REST tests](../../tests/FSM_REST.Tests/)
- Provenance: manually authored companion.
