# OpenApiRestApiConnection

**Workshop moniker:** @TrentBest  
**Purpose:** Download a remote OpenAPI 3.x JSON definition, import supported metadata, and expose operation execution through the optional OpenAPI package.

## Responsibilities
- Retain the definition URI, selected API base URI, imported descriptor, warnings, and transport.
- Use the caller-provided `HttpClient` for definition retrieval and HTTP operation calls.
- Choose the first declared concrete server or the definition origin when no server is declared.
- Execute operations by their imported identity through the transport boundary.

## Boundaries
The caller owns the client and authentication configuration. Server-variable substitution, environment selection, complete OpenAPI conformance, and automatic schema validation are not implied.

## Source and verification
- Source: [OpenApiRestApiConnection.cs](OpenApiRestApiConnection.cs)
- Importer: [OpenApiJsonImporter](OpenApiJsonImporter.md)
- Execution contract: [RestOperationExecutor](RestOperationExecutor.md)
- UML: [Core architecture](../../docs/UML/FSM_REST.Core.UML.md)
- Tests: [FSM_REST tests](../../tests/FSM_REST.Tests/)
- Provenance: manually authored companion.
