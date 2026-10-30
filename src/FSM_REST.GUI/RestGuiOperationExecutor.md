<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

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

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
