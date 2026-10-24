# TheSingularityWorkshop.FSM_Rest.OpenApi

Optional OpenAPI 3.x JSON import and definition-URL connection support for the neutral REST capability substrate.

This package depends on `TheSingularityWorkshop.FSM_Rest`. The core package does not depend on OpenAPI or any description format.

## Supported path

1. Import a supported OpenAPI 3.x JSON document into neutral REST descriptors.
2. Inspect `OpenApiImportResult.Warnings` for unsupported or unresolved constructs.
3. Optionally connect to a definition URL and execute imported operations by `operationId`.

## Important limits

This is not full OpenAPI conformance. YAML, external references, server-variable substitution, complete schema semantics, security policy, and array/object parameter serialization are not provided. Do not treat warning-free import as schema validation or proof that a remote endpoint is correct.

## Example

```csharp
using TheSingularityWorkshop.FSM_REST.OpenApi;

var imported = OpenApiJsonImporter.Import(openApiJson);
foreach (var warning in imported.Warnings)
    Console.WriteLine(warning);

var api = imported.Api;
```

See the [repository README](../../README.md), [Gallery](../../Gallery.md), and [domain gap assessment](../../docs/DOMAIN_GAP_ASSESSMENT.md).

MIT licensed. See [LICENSE.txt](../../LICENSE.txt).
