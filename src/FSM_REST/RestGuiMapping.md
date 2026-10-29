<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

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

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
