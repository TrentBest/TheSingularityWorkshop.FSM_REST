<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# OpenAPI Integration UML

**Scope:** Optional OpenAPI import and connection helpers. Keep these adapters outside the dependency-light FSM_REST core.

## Target diagrams

- [OpenApiJsonImporter](../OpenApiJsonImporter.UML.md) — imports OpenAPI JSON into REST descriptors.
- [OpenApiRestApiConnection](../OpenApiRestApiConnection.UML.md) — connects to a definition URL and exposes the imported API description.

## Source companions

- [OpenApiJsonImporter source companion](../../../src/FSM_REST.OpenApi/OpenApiJsonImporter.md)
- [OpenApiRestApiConnection source companion](../../../src/FSM_REST.OpenApi/OpenApiRestApiConnection.md)
- [OpenAPI package overview](../../../src/FSM_REST.OpenApi/README.md)

## Use case

An application can load an OpenAPI definition through the optional package, obtain REST operation descriptors, and pass those descriptors to core execution components without requiring the core package to parse OpenAPI documents.

Return to the [complete UML index](../README.md).

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
