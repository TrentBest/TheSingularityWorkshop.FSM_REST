# TheSingularityWorkshop.FSM_REST

**REST capability and transport substrate for the FSM ecosystem.**

FSM_REST is intentionally **not an implementation of a REST API**, and it is not a container for every REST description format.

It provides the reusable forms from which REST capabilities can be composed:

~~~text
REST MicroBundle / external description
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

> **FSM_REST provides the composition surface. Domain and protocol-specific MicroBundles provide the things composed through it.**

## If you only have a minute

FSM_REST answers one question:

> **How does a REST capability enter the Workshop without bringing its entire description format, GUI, domain, or transport policy with it?**

The answer is a small set of neutral forms:

```text
description/provider
       |
       v
RestApiDescriptor
       |
       v
RestOperationDescriptor
       |
       +----> GUI / FSM / Experience
       |
       v
RestRequest
       |
       v
IRestTransport
       |
       v
RestResponse
```





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

## Execute the capability

When an experience needs the data, the capability becomes a request:

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

## REST endpoint → MicroBundle

A provider can translate an external description into the neutral FSM_REST vocabulary and carry that capability as MicroBundle data.

~~~text
external description
        │
        ▼
provider / adapter
        │
        ▼
RestApiDescriptor
        │
   ┌────┼────┐
   ▼    ▼    ▼
 op A  op B  op C
   └────┼────┘
        ▼
 REST MicroBundle
        │
        ▼
    FSM_COS / host
        │
   ┌────┴────┐
   ▼         ▼
  GUI       FSM
   └────┬────┘
        ▼
    Experience
~~~

The source could be OpenAPI, a hand-authored definition, or another provider.

**FSM_REST does not need to know which.**

See [REST Capability Model](docs/REFLECTION.md), [REST MicroBundles](docs/MICROBUNDLE.md), and [Theory](docs/THEORY.md).

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

## What belongs in FSM_REST

The package owns the protocol-neutral REST vocabulary needed by the hosting ecosystem:

- RestApiDescriptor — a collection of REST operations.
- RestOperationDescriptor — an operation that can become a capability.
- RestParameterDescriptor — parameter metadata.
- RestRequestBodyDescriptor — request-body metadata.
- RestResponseDescriptor — response metadata.
- RestRequest — an executable transport request.
- RestResponse — an observed transport response.
- IRestTransport — the transport boundary.
- HttpClientRestTransport — the default .NET HTTP adapter.

These types deliberately do not require ASP.NET Core, OpenAPI, a GUI framework, or a particular domain.

## What does *not* belong here

A description format is a participant in the ecosystem, not the ecosystem itself.

For example, **OpenAPI is not a citizen of FSM_REST**.

An OpenAPI MicroBundle can be supplied separately. That bundle can understand OpenAPI documents, expose an OpenAPI provider, and translate the OpenAPI-specific representation into the neutral REST capability forms supplied by FSM_REST.

Conceptually:

~~~text
             MicroBundleDomain
                    |
                    v
             OpenAPI MicroBundle
                    |
             IProvider<OpenAPI>
                    |
                    v
                FSM_REST
                    |
          RestApiDescriptor
          RestOperationDescriptor
                    |
          +---------+---------+
          |                   |
        GUI                  FSM
          |                   |
          +---------+---------+
                    |
                Experience
~~~

The same pattern applies to other description formats or REST capability families. They should arrive as separately loadable MicroBundles rather than becoming permanent dependencies of the REST substrate.

## REST MicroBundles

A concrete REST MicroBundle is where a particular capability belongs.

A bundle might provide:

- a remote service;
- a family of REST operations;
- an API description adapter;
- authentication behavior;
- domain-specific request construction;
- GUI manifestation providers.

The hosting environment loads the MicroBundle and supplies the composition/runtime infrastructure. FSM_REST supplies the REST-specific forms the bundle can use.

This keeps the dependency direction clean:

~~~text
MicroBundle
    |
    +---- MicroBundleDomain
    +---- FSM_COS
    +---- FSM_REST
    |
    v
concrete REST capability
~~~

FSM_REST should never grow upward into a catalog of concrete REST services.

## Transport boundary

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
| FSM_COS | runtime MicroBundle composition |
| FSM_REST | REST capability and transport substrate |
| FSM_Serialization | serialized representation |
| GUI | visual manifestation |

Concrete protocol/domain packages remain separately owned and publishable.

## Alpha 3 boundary

**TheSingularityWorkshop.FSM_Rest 0.1.0-alpha.3**

Alpha 3 hardens the public contract and makes the package easier to consume without changing the deliberately small runtime boundary.

The current package intentionally does **not** include:

- OpenAPI parsing;
- OpenAPI $ref resolution;
- YAML parsing;
- authentication implementations;
- URL discovery;
- GUI generation;
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
