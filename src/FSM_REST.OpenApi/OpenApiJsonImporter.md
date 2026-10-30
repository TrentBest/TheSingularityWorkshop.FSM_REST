<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# OpenApiJsonImporter

**Workshop moniker:** @TrentBest  
**Purpose:** Translate a supported operation-level subset of OpenAPI 3.x JSON into neutral REST descriptors from the optional OpenAPI package.

## Responsibilities
- Parse and validate that input is a JSON object declaring an OpenAPI 3.x version.
- Extract API identity and supported operation, parameter, request-body, response, server, and reference metadata as implemented.
- Return normalized metadata with fidelity warnings for unsupported or unresolved constructs, including explicit serialization style/explode and array/object parameters that cannot be represented by the scalar binding model.
- Skip parameters whose `in` location is not one of the supported OpenAPI locations (`path`, `query`, `header`, or `cookie`) and report the omission.

## Boundaries
This importer is not a full OpenAPI validator or runtime. YAML, external references, complete schema composition, security policy, and unsupported OpenAPI semantics must not be implied as supported. Check the current implementation and warnings before relying on any particular construct.

## Source and verification
- Source: [OpenApiJsonImporter.cs](OpenApiJsonImporter.cs)
- Connection wrapper: [OpenApiRestApiConnection](OpenApiRestApiConnection.md)
- Core concepts: [Gallery](../../Gallery.md), [consumer guide](../../docs/CONSUMING.md)
- UML: [Importer](../../docs/UML/OpenApiJsonImporter.UML.md), [import result](../../docs/UML/OpenApiImportResult.UML.md), [core architecture](../../docs/UML/FSM_REST.Core.UML.md)
- Tests: [FSM_REST tests](../../tests/FSM_REST.Tests/)
- Provenance: manually authored companion.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
