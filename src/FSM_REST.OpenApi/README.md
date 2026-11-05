<!-- The Singularity Workshop moniker -->
<p align="center">
  <a href="https://github.com/TrentBest">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" />
  </a>
</p>

**Maintainer:** @TrentBest · [Shared documentation standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)

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

## Related resources

- [Repository guide](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/blob/development/README.md)
- [Gallery and examples](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/blob/development/Gallery.md)
- [Consumer guide](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/blob/development/docs/CONSUMING.md)
- [Source code](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/tree/development/src/FSM_REST.OpenApi)
- [License](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/blob/development/LICENSE.txt)

---

<p align="center">
  <a href="https://github.com/TrentBest">
    <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" height="200" />
  </a>
</p>

<p align="center">
  <em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br />
  <strong>Because state shouldn't be a mess.</strong>
</p>
