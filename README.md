# TheSingularityWorkshop.FSM_REST

**REST reflection and transport boundary for the FSM ecosystem.**

FSM_REST is the layer that lets a REST API become something the Singularity Workshop can reason about, reflect, and eventually manifest through the GUI.

The important distinction is:

~~~text
External REST API
        |
        v
 API description / discovery
        |
        v
     FSM_REST
        |
        v
 capability descriptors
        |
        v
       GUI
        |
        v
 behavior assignment
        |
        v
     Experience
~~~

The first alpha establishes the reusable reflection model. The ASP.NET Core application in this repository remains a thin experimental host for transport work.

## The core idea

A REST API already exposes a vocabulary of capabilities:

- resources;
- operations;
- paths;
- parameters;
- request bodies;
- response contracts;
- authentication requirements.

FSM_REST should make that vocabulary available as structured data without requiring the GUI to understand OpenAPI, ASP.NET Core, or the implementation details of the remote service.

That is the beginning of the **REST → reflection → GUI** pipeline.

## Alpha package

The initial package is **TheSingularityWorkshop.FSM_Rest 0.1.0-alpha.1**.

The package is framework-agnostic .NET and contains the reflection descriptors and OpenAPI reflector.

### Core types

- RestApiDescriptor — the reflected API as a whole.
- RestOperationDescriptor — an HTTP operation that can become a GUI capability.
- RestParameterDescriptor — path, query, header, or cookie input metadata.
- RestRequestBodyDescriptor — request-body media types and basic schema metadata.
- OpenApiRestReflector — converts an OpenAPI 3.x JSON description into the descriptor model.

The core library does not execute HTTP requests and does not depend on ASP.NET Core.

## The Postman-like direction

The long-term experience is intentionally dynamic:

~~~text
                    REST API URL
                         |
                         v
                discover description
                         |
                         v
                  reflect API surface
                         |
                         v
                +------------------+
                |   GUI PALETTE    |
                |                  |
                | GET  /users      |
                | POST /users      |
                | GET  /users/{id} |
                | DELETE /users/id |
                +------------------+
                         |
                         v
                 configure inputs
                         |
                         v
                  assign behavior
                         |
                         v
                  compose GUI
                         |
                         v
                     execute
~~~

The critical architectural move is that **reflection produces capabilities; GUI produces manifestation**.

That means the same REST operation could eventually become a button, form, card, table action, workflow node, FSM transition, or another GUI primitive.

The REST API does not need to know which one.

## Why OpenAPI first?

OpenAPI already describes the structural information needed for the first reflection layer: paths, operations, parameters, request bodies, responses, and related metadata. OpenAPI also defines operation identifiers for tooling to identify operations.

FSM_REST therefore starts with OpenAPI rather than trying to infer an entire API by blindly probing arbitrary URLs.

A future discovery layer can accept a URL, locate an API description, retrieve it, validate it, and pass the resulting document to OpenApiRestReflector.

Discovery, reflection, execution, and GUI manifestation remain separate stages.

## Architecture

~~~text
src/FSM_REST
    |
    +-- REST descriptors
    +-- OpenAPI reflection
    |
    v
TheSingularityWorkshop.FSM_REST
    |
    +-- reusable package
    |
    +-- no ASP.NET dependency

TheSingularityWorkshop.FSM_REST.csproj
    |
    v
ASP.NET Core experimental host
~~~

This separation matters because a desktop application, WebPage, Unity integration, or another host should be able to consume the reflection model without becoming an ASP.NET application.

## Relationship to the ecosystem

| Project | Responsibility |
|---|---|
| FSM_API | state-machine execution |
| FSM_REST | REST reflection and transport boundary |
| FSM_Serialization | serialized representation |
| FSM_COS | runtime composition |
| GUI | visual manifestation and interaction |
| MicroBundleDomain | domain capabilities and composition |

FSM_REST should not absorb the responsibilities of these projects.

## Alpha limitations

- OpenAPI 3.x JSON is supported.
- OpenAPI YAML is not yet parsed.
- $ref resolution is not yet implemented.
- Response schemas are not yet fully reflected.
- Authentication discovery/configuration is not yet modeled.
- URL discovery is not yet implemented.
- HTTP execution is not yet part of the core package.
- GUI generation is intentionally downstream.

These limitations define the next increments rather than hidden requirements.

## Documentation

- REST reflection model: docs/REFLECTION.md
- Transport theory: docs/THEORY.md

## Development

Build and test the complete solution:

~~~bash
dotnet restore TheSingularityWorkshop.FSM_REST.slnx
dotnet build TheSingularityWorkshop.FSM_REST.slnx --configuration Release
dotnet test tests/FSM_REST.Tests/FSM_REST.Tests.csproj --configuration Release
~~~

Pack the alpha package:

~~~bash
dotnet pack src/FSM_REST/FSM_REST.csproj --configuration Release --output ./artifacts
~~~

The GitHub verification workflow is manual-only while the ecosystem is being stabilized.

## License

MIT. See LICENSE.txt.

---

## 🔗 Resources & Support

### 📦 Get FSM_API

- **Unity Asset Store:** [FSM_API for Unity](https://assetstore.unity.com/packages/slug/332450)
- **Core NuGet:** [TheSingularityWorkshop.FSM_API](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_API)
- **This Package:** [TheSingularityWorkshop.FSM_Rest](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_Rest)
- **Source Code:** [TheSingularityWorkshop.FSM_REST on GitHub](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST)

### 💖 Support The Singularity Workshop

- **Patreon:** [Support us on Patreon](https://www.patreon.com/c/TheSingularityWorkshop)
- **PayPal:** [Make a donation](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

<p align="center">
  <a href="https://github.com/TrentBest/FSM_API">
    <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" height="200">
  </a>
</p>

<p align="center">
  <em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br>
  <strong>Because state shouldn't be a mess.</strong>
</p>
