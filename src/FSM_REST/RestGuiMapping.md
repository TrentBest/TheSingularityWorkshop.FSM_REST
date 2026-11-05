# RestGuiMapping

**Author / Workshop moniker:** @TrentBest  
**Purpose:** Declare how a REST operation is presented by a consuming GUI and how its parameters/results connect to host-owned application state.

## Responsibilities
- Preserve operation ID, label, and renderer-neutral presentation hint.
- Declare input mappings and optional result target.
- Record loading and error state intent.
- Reject missing operation metadata, blank labels/presentation, blank supplied result paths, duplicate input mappings, and inputs not declared by the operation.

## Boundaries
This record contains metadata only. It does not render controls, evaluate paths, execute the operation, authorize access, or mutate state. Source paths are identifiers supplied to host callbacks, not expressions evaluated by FSM_REST.

## UML and use cases
See [RestGuiMapping UML and use cases](../../docs/UML/RestGuiMapping.UML.md).

## Source and verification
- Source: [RestGuiMapping.cs](RestGuiMapping.cs)
- Input contract: [RestGuiInputMapping.cs](RestGuiInputMapping.cs)
- Execution: [RestGuiOperationExecutor.cs](RestGuiOperationExecutor.cs)
- Consumer guide: [Consuming FSM_REST](../../docs/CONSUMING.md)
- Tests: [RestGuiMappingTests.cs](../../tests/FSM_REST.Tests/RestGuiMappingTests.cs)

This is a manually authored source companion; it does not claim generated structural analysis.