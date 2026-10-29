<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# RestGuiMapping — UML and Use Cases

**Author / Workshop moniker:** @TrentBest  
**Target:** TheSingularityWorkshop.FSM_REST.RestGuiMapping  
**Source:** [RestGuiMapping.cs](../../src/FSM_REST/RestGuiMapping.cs)  
**Provenance:** Manually authored and source-checked; not generated.

## Class diagram

~~~mermaid
classDiagram
    direction TB
    class RestOperationDescriptor {
      +OperationId
      +Parameters
    }
    class RestGuiMapping {
      +OperationId
      +Label
      +Presentation
      +Inputs
      +ResultTarget
      +ShowLoadingState
      +ShowErrorState
    }
    class RestGuiInputMapping {
      +ParameterName
      +Label
      +ValueType
      +SourcePath
      +Required
    }
    class RestGuiOperationExecutor {
      <<static>>
      +ExecuteAsync(connection, mapping, resolveSource, userInputs, publishResult, cancellationToken)
    }
    RestGuiMapping ..> RestOperationDescriptor : validates against at construction
    RestGuiMapping o-- "0..*" RestGuiInputMapping : contains
    RestGuiOperationExecutor ..> RestGuiMapping : consumes
~~~

## Purpose and invariants
A mapping stores presentation and data-flow metadata for one operation. Construction requires an operation, nonblank label and presentation, unique input parameter mappings, and input names declared by the operation. A supplied result path cannot be blank. Loading/error-state intent is metadata, not behavior.

## Use cases
1. Declare a form or action using a presentation hint and human-readable label.
2. Bind an operation parameter to a host-owned source path, or leave it for user input.
3. Declare an optional result target for host-side publication.
4. Reject duplicate or undeclared parameter mappings before execution.

## Scope notes
Source-path syntax and state semantics belong to the host. This class does not perform I/O, rendering, authorization, state lookup, or result publication.

## Verification
- [Source](../../src/FSM_REST/RestGuiMapping.cs)
- [Input mapping](../../src/FSM_REST/RestGuiInputMapping.cs)
- [Executor](../../src/FSM_REST.GUI/RestGuiOperationExecutor.cs)
- [Tests](../../tests/FSM_REST.Tests/RestGuiMappingTests.cs)

Re-check the diagram when public properties or validation behavior change.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
