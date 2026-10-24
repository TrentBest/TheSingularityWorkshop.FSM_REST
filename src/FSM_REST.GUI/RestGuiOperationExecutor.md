# RestGuiOperationExecutor

**Workshop moniker:** @TrentBest  
**Purpose:** Resolve mapped GUI inputs, execute a neutral REST operation through an injected transport, and optionally publish the response.

## Responsibilities
- Resolve state-backed inputs through a host-supplied callback and accept user-entered scalar values.
- Find the operation by stable identity in a supplied `RestApiDescriptor`.
- Invoke the neutral `RestOperationExecutor` with the host-supplied base URI and `IRestTransport`.
- Publish the raw response to an optional result target callback.

## Boundaries
This is an optional GUI adapter, not a core REST capability. It does not depend on OpenAPI or a concrete GUI framework. Rendering, application-state storage, credentials, authorization, retries, and interpretation of response data remain host responsibilities.

## Source and verification
- Source: [RestGuiOperationExecutor.cs](RestGuiOperationExecutor.cs)
- Mapping model: [RestGuiMapping](../FSM_REST/RestGuiMapping.md)
- Neutral execution: [RestOperationExecutor](../FSM_REST/RestOperationExecutor.md)
- Tests: [RestGuiOperationExecutorTests](../../tests/FSM_REST.Tests/RestGuiOperationExecutorTests.cs)
- UML: [RestGuiOperationExecutor](../../docs/UML/RestGuiOperationExecutor.UML.md)
- Provenance: manually authored companion.
