# FSM_REST Domain

## Domain statement

**FSM_REST models REST capabilities and separates their reusable description from the runtime act of making a request.** It provides a neutral vocabulary for describing an API and its operations, binding invocation values, constructing requests, and crossing a replaceable transport boundary.

The package is a capability substrate. It is not itself a REST service, API-description parser, service registry, GUI, or domain-specific client.

## Core concepts

| Concept | Meaning | Implementation |
|---|---|---|
| API capability | A named/versioned collection of operations | `RestApiDescriptor` |
| Operation capability | Method, relative path, stable identity, documentation, parameters, request-body and response metadata | `RestOperationDescriptor` |
| Parameter metadata | Declared parameter name, location, requiredness and descriptive type information | `RestParameterDescriptor` |
| Invocation binding | Runtime values for one use of a reusable operation | `RestOperationBinding` |
| Request | One concrete outbound operation request | `RestRequest` |
| Request construction | Binds values and validates supported parameter locations while composing URI, headers and optional body | `RestRequestFactory` |
| Transport contract | Asynchronous send boundary with cancellation | `IRestTransport` |
| HTTP adapter | Maps the neutral request/response contract to `HttpClient` | `HttpClientRestTransport` |
| Response | Observed status, headers and body returned by transport | `RestResponse` |
| FSM_COS integration | Carries a REST API descriptor as an ecosystem MicroBundle | `src/FSM_REST.COS/RestMicroBundle.cs` |

## Domain boundaries

### Owned by FSM_REST

- Neutral API and operation descriptors.
- Runtime operation binding, separate from reusable descriptor data.
- Request construction from an absolute base URI and runtime inputs.
- A transport abstraction and an `HttpClient` adapter.
- A response envelope that does not interpret domain payloads.

### Explicitly outside the domain

- Parsing OpenAPI, YAML, or any other external description format.
- Resolving format-specific references or generating clients.
- Defining concrete business APIs, domain entities, or remote service behavior.
- Authentication policy, credential discovery, retries, caching, rate limiting, and service discovery.
- Interpreting response bodies into domain objects.
- Rendering controls or choosing GUI, workflow, or experience semantics.
- Hosting an API server. The root host project is a non-packable experimental harness, not the published library.

These exclusions are architectural boundaries, not claims that those concerns are unimportant. They belong in adapters, domain MicroBundles, or host policy so the substrate stays composable.

## Package boundaries

- **Published core library:** `src/FSM_REST/FSM_REST.csproj`, package ID `TheSingularityWorkshop.FSM_Rest`.
- **Optional FSM_COS adapter:** `src/FSM_REST.COS/FSM_REST.COS.csproj`, which depends on the REST model and MicroBundle abstractions to expose `RestMicroBundle`.
- **Experimental host:** root `TheSingularityWorkshop.FSM_REST.csproj`; `IsPackable=false`. It must not be mistaken for the NuGet library.
- **Tests:** `tests/FSM_REST.Tests`.

## Domain invariants

1. A descriptor is reusable capability metadata; a binding represents one invocation.
2. Request construction does not send network traffic.
3. A transport sends a request and reports a response; it does not decide what the payload means.
4. HTTP is an adapter behind `IRestTransport`, not the definition of the abstraction.
5. Description-format-specific semantics remain with the provider/adapter that translates them.
6. GUI, FSM, serialization, and experience layers consume the capability; they are not dependencies of the core REST model.

## Typical flow

```text
External API description / authored capability
                    |
                    v
             provider adapter
                    |
                    v
             RestApiDescriptor
                    |
                    v
          RestOperationDescriptor
                    |
            bind runtime values
                    v
            RestOperationBinding
                    |
                    v
             RestRequestFactory
                    |
                    v
                RestRequest
                    |
                    v
               IRestTransport
                    |
                    v
               RestResponse
                    |
                    v
       downstream interpretation / experience
```

See [Theory](THEORY.md), [Reflection](REFLECTION.md), and [MicroBundle integration](MICROBUNDLE.md) for the reasoning and detailed flow.
