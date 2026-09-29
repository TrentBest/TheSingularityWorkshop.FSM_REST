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

**Still interested?** Read [What belongs here](#what-belongs-in-fsm_rest).

**Still interested?** Read [How to use the transport](#transport-boundary).

**Still interested?** Read [Why OpenAPI stays outside](docs/THEORY.md#protocol-descriptions-are-participants).

**Still interested?** Read the full [theory](docs/THEORY.md).

---

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
