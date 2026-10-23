# RestApiDescriptorExtensions

**Workshop moniker:** @TrentBest  
**Purpose:** Provide the optional FSM_COS conversion entry point for a neutral REST API descriptor.

## Responsibilities
- Expose `ToMicroBundle(...)` as an extension on `RestApiDescriptor`.
- Forward the supplied ID, optional version, dependencies, and providers to `RestMicroBundle`.
- Reject a null API descriptor before constructing the adapter.

## Boundaries
This helper belongs to the optional `FSM_REST.COS` integration project. It does not add FSM_COS or MicroBundleDomain dependencies to REST core, execute HTTP, or change the descriptor.

## Source and verification
- Source: [RestApiDescriptorExtensions.cs](RestApiDescriptorExtensions.cs)
- Target UML: [FSM_REST core architecture](../../docs/UML/FSM_REST.Core.UML.md) and [UML index](../../docs/UML/README.md)
- Tests: [FSM_REST tests](../../tests/FSM_REST.Tests/)
- Provenance: manually authored companion; verify against source when the public signature changes.
