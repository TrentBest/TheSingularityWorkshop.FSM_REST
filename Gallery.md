# FSM_REST Gallery

FSM_REST provides a neutral model for describing REST capabilities and a replaceable boundary for executing requests. This gallery links the concepts to the implementation rather than implying that every planned REST ecosystem feature is present.

## 1. Describe a capability

A `RestApiDescriptor` groups `RestOperationDescriptor` instances. An operation records its method, path, identity, summary/description, parameter metadata, optional request-body metadata, and response descriptors.

- [Core descriptor types](src/FSM_REST/)
- [Reflection and descriptor model](docs/REFLECTION.md)

## 2. Bind and construct one request

The operation descriptor is reusable. `RestOperationBinding` captures values for one invocation; `RestRequestFactory` turns the operation, absolute base URI, parameters, optional headers, and body into a `RestRequest`.

```csharp
var binding = new RestOperationBinding(
    operation,
    new Dictionary<string, string?> { ["page"] = "1" });

var request = RestRequestFactory.Create(
    binding,
    new Uri("https://example.test"));
```

Request construction does not send the request. It is a separate, testable step.

## 3. Send through a replaceable transport

`IRestTransport` is the communication boundary. `HttpClientRestTransport` is the provided HTTP adapter; a host or test can substitute another implementation without changing capability descriptors.

```csharp
using var client = new HttpClient();
IRestTransport transport = new HttpClientRestTransport(client);
RestResponse response = await transport.SendAsync(request);
```

This example assumes `operation` has already been described and `request` constructed. The transport reports status, headers, and body; downstream code decides what those values mean.

## 4. Carry a capability into FSM_COS

The optional `src/FSM_REST.COS` project defines `RestMicroBundle`, an adapter that carries a `RestApiDescriptor` as an `IMicroBundle`. It is integration, not a dependency required by the core transport model.

- [FSM_COS adapter source](src/FSM_REST.COS/RestMicroBundle.cs)
- [MicroBundle theory](docs/MICROBUNDLE.md)

## Visual guides

- [Capability recipe](docs/assets/fsm-rest-capability-recipe.svg)
- [Execution flow](docs/assets/fsm-rest-execution-flow.svg)
- [Composition map](docs/assets/fsm-rest-composition-map.svg)

## Verification

The repository contains descriptor and transport tests under [tests/FSM_REST.Tests](tests/FSM_REST.Tests/). CI definitions are in [build.yml](.github/workflows/build.yml) and [verify.yml](.github/workflows/verify.yml). Consult the current workflow results before treating a branch as release-ready.

## Not claimed by this gallery

FSM_REST does not currently claim to parse OpenAPI/YAML, provide authentication or retry policy, cache responses, generate GUI controls, interpret arbitrary response schemas, or host production REST endpoints. Those require separately defined adapters or host responsibilities.
