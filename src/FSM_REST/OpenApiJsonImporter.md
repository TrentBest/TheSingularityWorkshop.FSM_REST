# OpenApiJsonImporter

**Workshop moniker:** @TrentBest  
**Purpose:** Translate the supported operation-level subset of an OpenAPI 3.x JSON document into neutral FSM_REST descriptors.

## Responsibilities
- Parse and validate that input is a JSON object declaring an OpenAPI 3.x version.
- Extract API identity and supported operation, parameter, request-body, response, server, and reference metadata as implemented.
- Return normalized metadata with fidelity warnings for unsupported or unresolved constructs.

## Boundaries
This importer is not a full OpenAPI validator or runtime. YAML, external references, complete schema composition, security policy, and unsupported OpenAPI semantics must not be implied as supported. Check the current implementation and warnings before relying on any particular construct.

## Source and verification
- Source: [OpenApiJsonImporter.cs](OpenApiJsonImporter.cs)
- Connection wrapper: [OpenApiRestApiConnection](OpenApiRestApiConnection.md)
- Core concepts: [Gallery](../../Gallery.md), [consumer guide](../../docs/CONSUMING.md)
- UML: [Core architecture](../../docs/UML/FSM_REST.Core.UML.md)
- Tests: [FSM_REST tests](../../tests/FSM_REST.Tests/)
- Provenance: manually authored companion.
