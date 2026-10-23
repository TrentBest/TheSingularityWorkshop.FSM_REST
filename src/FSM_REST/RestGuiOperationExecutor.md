# RestGuiOperationExecutor

**Author / Workshop moniker:** @TrentBest  
**Purpose:** Execute an operation described by a REST GUI mapping while leaving GUI framework, state access, and result publication under host control.

## Execution flow
1. Validate the connection, mapping, and source resolver.
2. Resolve each mapped input through the host source-path callback or user-input dictionary.
3. Reject a missing/empty value for a required input.
4. Invoke the operation with resolved values and cancellation token.
5. If a result target and publisher callback exist, publish the raw RestResponse.
6. Return the response to the caller.

## Boundaries
The executor does not render UI, update loading/error indicators, evaluate arbitrary paths, authorize access, deserialize domain objects, or define retry policy. Exceptions from validation, the connection, and callbacks remain observable.

## UML and use cases
See [RestGuiOperationExecutor UML and use cases](../../docs/UML/RestGuiOperationExecutor.UML.md).

## Source and verification
- Source: [RestGuiOperationExecutor.cs](RestGuiOperationExecutor.cs)
- Mapping: [RestGuiMapping.cs](RestGuiMapping.cs)
- Consumer guide: [Consuming FSM_REST](../../docs/CONSUMING.md)
- Tests: [RestGuiOperationExecutorTests.cs](../../tests/FSM_REST.Tests/RestGuiOperationExecutorTests.cs)

This companion is manually authored from current source; it does not imply generated UML.