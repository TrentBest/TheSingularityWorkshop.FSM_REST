<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# RestGuiNodeFactory

**Author / Workshop moniker:** @TrentBest  
**Purpose:** Convert renderer-neutral REST GUI mapping metadata into an immutable Workshop GUI tree without adding GUI dependencies to the REST core.

## Responsibilities
- Validate the mapping and caller-provided node ID.
- Build a panel with a title, semantic input nodes, an execute button, and an optional result marker.
- Preserve operation identity and presentation metadata.
- Assign a stable command identity through GetCommandId.

## Boundaries
Create describes UI structure; it does not execute HTTP, evaluate application-state paths, authorize access, or update loading/error state. The host must dispatch the emitted onClickCommand to RestGuiOperationExecutor and provide state/input/result callbacks. RestInput is a semantic node kind; renderer support is a separate integration concern.

## UML and use cases
See [RestGuiNodeFactory UML and use cases](../../docs/UML/RestGuiNodeFactory.UML.md).

## Source and verification
- Source: [RestGuiNodeFactory.cs](RestGuiNodeFactory.cs)
- Mapping: [RestGuiMapping](../FSM_REST/RestGuiMapping.md)
- Consumer workflow: [Consuming FSM_REST](../../docs/CONSUMING.md)
- Regression tests: [RestGuiNodeFactoryTests.cs](../../tests/FSM_REST.Tests/RestGuiNodeFactoryTests.cs)

This companion is manually authored, not generated UML or proof of runtime renderer behavior.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
