<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# RestGuiInputMapping

**Workshop moniker:** @TrentBest  
**Purpose:** Describe how one declared REST parameter should be represented as a GUI input or populated from host application state.

## Responsibilities
- Store parameter name, display label, optional value-type hint, optional source path, and required flag.
- Validate nonblank parameter identity and label.
- Reject a supplied source path that is blank.

## Boundaries
This record does not render a control, resolve a state path, validate remote data, or collect user input. Source-path semantics and control behavior belong to the host/adapter.

## Source and verification
- Source: [RestGuiInputMapping.cs](RestGuiInputMapping.cs)
- Parent mapping: [RestGuiMapping](RestGuiMapping.md)
- GUI integration: [RestGuiNodeFactory](../FSM_REST.GUI/RestGuiNodeFactory.md)
- UML: [mapping UML](../../docs/UML/RestGuiMapping.UML.md)
- Tests: [FSM_REST tests](../../tests/FSM_REST.Tests/)
- Provenance: manually authored companion.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
