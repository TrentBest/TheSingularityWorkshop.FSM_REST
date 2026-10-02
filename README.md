# TheSingularityWorkshop.FSM_REST

**REST capability and transport substrate for the FSM ecosystem.**

[![NuGet](https://img.shields.io/nuget/v/TheSingularityWorkshop.FSM_Rest?style=flat-square&logo=nuget&logoColor=white)](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_Rest)
[![NuGet downloads](https://img.shields.io/nuget/dt/TheSingularityWorkshop.FSM_Rest?style=flat-square&logo=nuget&logoColor=white)](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_Rest)
[![Build](https://img.shields.io/github/actions/workflow/status/TrentBest/TheSingularityWorkshop.FSM_REST/build.yml?branch=master&style=flat-square&logo=github)](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/workflows/build.yml)
[![Coverage](https://codecov.io/gh/TrentBest/TheSingularityWorkshop.FSM_REST/graph/badge.svg)](https://codecov.io/gh/TrentBest/TheSingularityWorkshop.FSM_REST)
[![License](https://img.shields.io/badge/license-MIT-yellow.svg)](LICENSE.txt)

FSM_REST is intentionally **not an implementation of a REST API**, and it is not a container for every REST description format.

It provides the reusable forms from which REST capabilities can be composed:

~~~text
external REST description
            |
            v
        FSM_REST
            |
     +------+------+
     |             |
 capability     transport
 descriptors    boundary
     |             |
     +------+------+
            |
            v
     GUI / FSM / Experience
~~~

The important architectural rule is:

> **FSM_REST provides the REST capability vocabulary. A consuming composition layer decides how that capability becomes a MicroBundle or runtime capability.**

<p align="center">
  <img src="docs/assets/fsm-rest-capability-recipe.svg" alt="REST capability recipe flowing from a MicroBundle into a request and current remote data" width="900">
</p>

## If you only have a minute

FSM_REST answers one question:

> **How does a REST capability enter the Workshop without bringing its entire description format, GUI, domain, or transport policy with it?**

The answer is a small neutral vocabulary for capability description, request construction, and transport.

---

## The capability recipe

The useful trick is that a REST API can be enormous at runtime while being small as a capability description.

**The MicroBundle stores what is needed to obtain the capability—not the remote payload itself.**

~~~text
CAPABILITY RECIPE
      │
      ▼
RestOperationDescriptor
      │
      │ bind runtime values
      ▼
RestRequest
      │
      ▼
IRestTransport
      │
      ▼
REMOTE API
      │
      │ current / user-specific data
      ▼
RestResponse
      │
      ▼
GUI / FSM / Experience
~~~

For a storefront, the bundle can carry the API identity, operation identity, method, path, parameter definitions, request rules, response metadata, and provider-specific behavior.

The current catalog, inventory, prices, user-specific results, and other changing payloads remain runtime data.

| Capability recipe | Runtime result |
|---|---|
| method + path | current records |
| parameters | current prices |
| request rules | current inventory |
| response metadata | current response |
| provider behavior | transient transport data |

**The response is runtime data. The MicroBundle is the capability recipe.**

This means an endpoint can be cheap to describe, distribute, compose, and replace without copying the dataset it exposes.

## Build a capability

The smallest useful REST capability is just an operation description:

~~~csharp
var operation = new RestOperationDescriptor(
    "GET",
    "/products",
    "listProducts",
    "List products",
    "Returns the current product catalog.",
    [
        new RestParameterDescriptor(
            "page", "query", false, "integer", null, "Page number.")
    ],
    null,
    [
        new RestResponseDescriptor(
            "200", "Product collection.",
            ["application/json"], "array", null)
    ]);

var api = new RestApiDescriptor(
    "Store Catalog",
    "1.0",
    [operation]);
~~~

Nothing has been fetched, cached, or rendered.

You have described a capability that can now participate in the Workshop.

## Bind and execute the capability

When an experience needs the data, the reusable capability is first bound to runtime values:

~~~csharp
var binding = new RestOperationBinding(
    operation,
    new Dictionary<string, string?> { ["page"] = "1" });

var request = RestRequestFactory.Create(
    binding,
    new Uri("https://example.test"));
~~~

The binding is one invocation. The operation descriptor remains reusable.

When an experience needs to send that request:

~~~csharp
using var httpClient = new HttpClient();
IRestTransport transport = new HttpClientRestTransport(httpClient);

var request = new RestRequest(
    "GET",
    new Uri("https://example.test/products?page=1"));

RestResponse response = await transport.SendAsync(request);

if (response.IsSuccessStatusCode)
{
    Console.WriteLine(response.Body);
}
~~~

The transport communicates.

It does not decide what the response means. Interpretation remains downstream.

<p align="center">
  <img src="docs/assets/fsm-rest-execution-flow.svg" alt="REST execution flow from reusable operation through request construction and transport to current remote data" width="900">
</p>

## REST capability → composition boundary

FSM_REST deliberately stops before the composition kernel.

A REST description can be carried by a MicroBundle, but the adapter that implements the composition kernel's `IMicroBundle` contract belongs at the composition boundary—not inside the standalone REST substrate.

~~~text
external description
        |
        v
provider / adapter
        |
        v
RestApiDescriptor
        |
        +----------------------+
        |                      |
        v                      v
 direct REST use       consuming composition
                              |
                              v
                          FSM_COS
                              |
                         RestMicroBundle
~~~

This keeps the dependency direction one-way:

~~~text
FSM_REST  ──>  no FSM_COS dependency

FSM_COS  ──>  FSM_REST
~~~

When FSM_COS needs REST composition, it can reference FSM_REST and provide the small adapter there. Another composition engine can do the same without forcing FSM_REST to know about either engine.

> **A capability vocabulary should not depend on the engine that consumes it.**

See [REST MicroBundles](docs/MICROBUNDLE.md) for the boundary in detail.

## Why the response is not the bundle

The response is transient, changing, and often user-specific.

The bundle is reusable capability data.

~~~text
SMALL DESCRIPTION
      │
      │ method / path / parameters /
      │ request rules / response metadata
      ▼
REMOTE CAPABILITY
      │
      │ current state
      ▼
LARGE / DYNAMIC RESULT
~~~

This does not claim every REST integration is physically small. It means the bundle does not need to duplicate the remote dataset merely to describe how that dataset can be obtained.

That is the property that makes REST capabilities especially attractive as MicroBundle content.

## Transport boundary

Before transport, RestRequestFactory can bind operation parameters into a concrete request:

~~~csharp
var request = RestRequestFactory.Create(
    operation,
    new Uri("https://example.test/api"),
    new Dictionary<string, string?>
    {
        ["id"] = "42",
        ["page"] = "1"
    });
~~~

It handles path, query, header, and cookie parameter locations while leaving authentication, retries, caching, and transport policy outside the core.

The package provides a minimal executable boundary:

~~~csharp
var request = new RestRequest(
    "POST",
    new Uri("https://example.test/users"),
    new Dictionary<string, string>
    {
        ["X-Trace"] = "trace-123"
    },
    "{\"name\":\"Ada\"}");

IRestTransport transport = new HttpClientRestTransport(httpClient);

RestResponse response = await transport.SendAsync(request);
~~~

IRestTransport is the important boundary. HttpClientRestTransport is merely one adapter.

That means a host can replace the HTTP implementation without changing the capability model.

## The operational theater

The intended direction is larger than an API client:

~~~text
external capability
        |
        v
MicroBundle / provider
        |
        v
   REST capability
        |
        +----------------+
        |                |
        v                v
      GUI              FSM behavior
        |                |
        +-------+--------+
                |
                v
           Experience
~~~

A REST operation might ultimately manifest as a button, form, table action, workflow node, FSM transition, automated behavior, or another composition primitive.

FSM_REST does not choose the manifestation.

## Relationship to the ecosystem

| Project | Responsibility |
|---|---|
| FSM_API | state-machine execution |
| MicroBundleDomain | domain-side MicroBundle description |
| FSM_COS | runtime MicroBundle composition and composition-side adapters |
| FSM_REST | REST capability and transport substrate |
| FSM_Serialization | serialized representation |
| GUI | visual manifestation |

Concrete protocol/domain packages remain separately owned and publishable. A composition layer such as FSM_COS may consume FSM_REST and provide its own small adapter when the REST capability itself is a MicroBundle.

## 1.0.0 release posture

This branch prepares FSM_REST 1.0.0 as a **standalone foundation package**.

~~~text
FSM_REST 1.0.0
├── capability descriptors
├── request construction
└── transport boundary

(no FSM_COS dependency)
        |
        v
consumer / composition layer
        |
        v
FSM_COS + composition adapters
~~~

A project can add FSM_REST and use its descriptors, request factory, and transport boundary without installing FSM_COS. This is intentional: FSM_REST is infrastructure that can be useful before any composition engine is introduced.

FSM_COS may later depend on stable FSM_REST to turn REST capabilities into composed MicroBundles.

### Release gate

CI packs the package and rejects any stable first-party dependency that is still prerelease. Publication requires an explicit workflow dispatch with `publish: true`.

See [Theory](docs/THEORY.md), [REST Capability Model](docs/REFLECTION.md), and the package release workflow for the implementation boundary.

<p align="center">
  <img src="docs/assets/release-frontier.svg" alt="FSM_REST 1.0.0 as an independent foundation below the composition boundary">
</p>

## Previous alpha boundary

**TheSingularityWorkshop.FSM_Rest 1.0.0 release candidate**

Alpha 4 established the reusable capability model. The 1.0 boundary deliberately moves ecosystem-specific MicroBundle adapters into the consuming composition layer.

The current package intentionally does **not** include:

- OpenAPI parsing;
- OpenAPI $ref resolution;
- YAML parsing;
- authentication implementations;
- URL discovery;
- GUI generation;
- composition-engine adapters;
- concrete REST services;
- domain-specific MicroBundles.

Those are composition opportunities for separate packages.

## Development

~~~bash
dotnet restore TheSingularityWorkshop.FSM_REST.slnx
dotnet build TheSingularityWorkshop.FSM_REST.slnx --configuration Release
dotnet test tests/FSM_REST.Tests/FSM_REST.Tests.csproj --configuration Release
dotnet pack src/FSM_REST/FSM_REST.csproj --configuration Release --output ./artifacts
~~~

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
