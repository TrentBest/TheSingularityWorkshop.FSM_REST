# RestGuiOperationExecutor — UML and Use Cases

**Author / Workshop moniker:** @TrentBest  
**Target:** TheSingularityWorkshop.FSM_REST.GUI.RestGuiOperationExecutor  
**Source:** [RestGuiOperationExecutor.cs](../../src/FSM_REST.GUI/RestGuiOperationExecutor.cs)  
**Provenance:** Manually authored and source-checked; not generated.

## Class diagram

~~~mermaid
classDiagram
    direction TB
    class RestApiDescriptor {
      +Operations
    }
    class IRestTransport {
      +SendAsync(request, cancellationToken)
    }
    class RestOperationExecutor {
      +ExecuteAsync(operation, baseUri, transport, parameters, cancellationToken)
    }
    class RestGuiMapping {
      +OperationId
      +Inputs
      +ResultTarget
    }
    class RestGuiInputMapping {
      +ParameterName
      +SourcePath
      +Required
    }
    class RestResponse
    class RestGuiOperationExecutor {
      <<static>>
      +ExecuteAsync(api, baseUri, transport, mapping, resolveSource, userInputs, publishResult, cancellationToken) Task~RestResponse~
    }
    class HostStateCallbacks {
      <<host-owned>>
      +resolveSource(path)
      +publishResult(path, response)
    }
    RestGuiOperationExecutor ..> RestGuiMapping : reads
    RestGuiMapping o-- RestGuiInputMapping : contains
    RestGuiOperationExecutor ..> HostStateCallbacks : calls
    RestGuiOperationExecutor ..> RestApiDescriptor : finds operation
    RestGuiOperationExecutor ..> RestOperationExecutor : delegates execution
    RestOperationExecutor ..> IRestTransport : sends request
    RestGuiOperationExecutor ..> RestResponse : returns or publishes
~~~

## Use cases
1. **Resolve inputs.** Use the host callback when a source path exists; otherwise read from the user-input dictionary.
2. **Enforce required values.** A required value resolving to null or an empty string causes an argument validation exception before invocation.
3. **Execute with cancellation.** Resolve the mapped operation from the neutral API descriptor and delegate execution through the injected transport with cancellation.
4. **Publish a result.** If a result target and publisher callback exist, pass the raw RestResponse to the callback and also return it.

## Scope notes
The executor is host-framework-neutral. It does not render controls, implement loading/error state, deserialize domain models, authorize requests, or define retries. Callback and connection failures remain observable.

## Verification
- [Source](../../src/FSM_REST.GUI/RestGuiOperationExecutor.cs)
- [Mapping](../../src/FSM_REST/RestGuiMapping.cs)
- [Consumer guide](../CONSUMING.md)
- [Tests](../../tests/FSM_REST.Tests/RestGuiOperationExecutorTests.cs)

Re-check when invocation, input-resolution, cancellation, or publication behavior changes.