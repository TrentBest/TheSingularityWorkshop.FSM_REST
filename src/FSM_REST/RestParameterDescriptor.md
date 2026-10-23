# RestParameterDescriptor

**Workshop moniker:** @TrentBest  
**Purpose:** Describe one parameter accepted by a REST operation.

## Responsibilities
Store the parameter name, location, required flag, optional logical type and format hints, and human-readable description.

## Boundaries
The descriptor stores metadata; it does not serialize arrays/objects or apply the full OpenAPI style/explode rules. Request construction validates supported locations and required values according to the current factory implementation.

## Source and verification
- Source: [RestParameterDescriptor.cs](RestParameterDescriptor.cs)
- Parent model: [RestOperationDescriptor](RestOperationDescriptor.md)
- UML: [core architecture](../../docs/UML/FSM_REST.Core.UML.md)
- Tests: [FSM_REST tests](../../tests/FSM_REST.Tests/)
- Provenance: manually authored companion.
