<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# GUI Integration UML

**Scope:** Optional GUI-facing adapters and input/result mapping. These pages describe presentation integration; they do not make GUI dependencies part of the FSM_REST core package.

## Target diagrams

- [RestGuiMapping](../RestGuiMapping.UML.md) — metadata mapping REST operations to GUI presentation.
- [RestGuiInputMapping](../RestGuiInputMapping.UML.md) — parameter-level input and state mapping.
- [RestGuiNodeFactory](../RestGuiNodeFactory.UML.md) — constructs optional GUI nodes.
- [RestGuiOperationExecutor](../RestGuiOperationExecutor.UML.md) — resolves inputs, invokes operations, and publishes results.

## Source companions

- [RestGuiNodeFactory source companion](../../../src/FSM_REST.GUI/RestGuiNodeFactory.md)
- [RestGuiOperationExecutor source companion](../../../src/FSM_REST.GUI/RestGuiOperationExecutor.md)

## Use case

A host application can describe how a REST operation appears in a GUI, bind user input to parameters, execute the operation, and map the result back to the host without adding GUI framework types to the transport/core contract.

Return to the [complete UML index](../README.md).

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
