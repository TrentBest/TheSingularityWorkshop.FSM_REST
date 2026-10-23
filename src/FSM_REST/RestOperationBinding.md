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
