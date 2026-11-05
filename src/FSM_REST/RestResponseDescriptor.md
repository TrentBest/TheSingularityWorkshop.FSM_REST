# RestResponseDescriptor

**Workshop moniker:** @TrentBest  
**Purpose:** Describe one documented response variant of a REST operation.

## Responsibilities
Store status-code identity, optional description, media types, and optional schema type/format hints.

## Boundaries
This is descriptive metadata only; it does not validate runtime response bodies or guarantee that a remote service returns the documented response.

## Source and verification
- Source: [RestResponseDescriptor.cs](RestResponseDescriptor.cs)
- Parent model: [RestOperationDescriptor](RestOperationDescriptor.md)
- Runtime result: [RestResponse](RestResponse.md)
- UML: [core architecture](../../docs/UML/FSM_REST.Core.UML.md)
- Tests: [FSM_REST tests](../../tests/FSM_REST.Tests/)
- Provenance: manually authored companion.
