# FSM_REST Gallery

FSM_REST provides a neutral model for describing REST capabilities and a replaceable boundary for executing requests. The [architecture contract](docs/ARCHITECTURE.md) defines the package boundaries and verification levels. This gallery links the concepts to the implementation rather than implying that every planned REST ecosystem feature is present.

## 1. Describe a capability

A `RestApiDescriptor` groups `RestOperationDescriptor` instances. An operation records its method, path, identity, summary/description, parameter metadata, optional request-body metadata, and response descriptors.

- [Core descriptor types](src/FSM_REST/)
- [Reflection and descriptor model](docs/REFLECTION.md)
- [RestApiDescriptor UML and use cases](docs/UML/RestApiDescriptorUML.md) — source-checked target-centered map with explicit scan scope.

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

## 3. Run the complete core path

The deterministic [REST capability demo](samples/RestCapabilityDemo/Program.md) exercises descriptor → binding → request → transport → response with an in-memory handler. It requires no live service or credentials and is run by CI.

## 4. Send through a replaceable transport

`IRestTransport` is the communication boundary. `HttpClientRestTransport` is the provided HTTP adapter; a host or test can substitute another implementation without changing capability descriptors.

```csharp
using var client = new HttpClient();
IRestTransport transport = new HttpClientRestTransport(client);
RestResponse response = await transport.SendAsync(request);
```

This example assumes `operation` has already been described and `request` constructed. The transport reports status, headers, and body. `RestResponse.Deserialize<T>()` can deserialize JSON into a consumer-owned type; schema validation and domain meaning remain downstream.

## 5. Connect from a definition URL

The optional `src/FSM_REST.OpenApi` project provides `OpenApiRestApiConnection.ConnectAsync`, which downloads a supported OpenAPI 3.x JSON document, imports the operation catalogue, selects the first concrete server, and exposes operation execution by `operationId`. The caller owns the `HttpClient` and authentication policy. See the [consumer guide](docs/CONSUMING.md) for the complete flow.

## 6. Map an operation to GUI and data

`RestGuiMapping` describes a renderer-neutral presentation hint, operation identity, GUI input mappings, optional application-state source paths, a result target, and loading/error-state intent. `RestGuiInputMapping` maps declared API parameters either to a state path or to user-provided input. Construction validates that mappings refer to declared parameters and rejects duplicates. `RestGuiOperationExecutor` resolves source paths through a host callback, combines those values with user-entered inputs, invokes the operation, and publishes the raw response to the configured result target through a host callback. A GUI adapter still needs to render controls and implement the state callbacks; deserialize the result into a consumer-owned model when typed GUI data is required.

## 7. Import an OpenAPI definition (optional extension)

The core package does not parse OpenAPI. The separate `TheSingularityWorkshop.FSM_Rest.OpenApi` package imports a supported JSON subset and reports fidelity warnings. See [the OpenAPI package README](src/FSM_REST.OpenApi/README.md) and [importer tests](tests/FSM_REST.Tests/OpenApiJsonImporterTests.cs).

## 8. Carry a capability into FSM_COS

The optional `src/FSM_REST.COS` project defines `RestMicroBundle`, an adapter that carries a `RestApiDescriptor` as an `IMicroBundle`. It is integration, not a dependency required by the core transport model.

- [FSM_COS adapter source](src/FSM_REST.COS/RestMicroBundle.cs)
- [MicroBundle theory](docs/MICROBUNDLE.md)

## 9. Model request execution with FSM_API

The core package directly references `TheSingularityWorkshop.FSM_API` and offers `RestFsmExecutor.SendAsync` as an opt-in lifecycle wrapper around an existing `IRestTransport`. The shared FSM definition describes `Ready → Sending → Completed/Failed/Cancelled`; each invocation creates its own context and FSM instance, then destroys that instance in `finally`.

- [RestFsmExecutor type page](docs/RestFsmExecutor.md) — lifecycle semantics, example, dependency boundary, and remaining verification needs.
- [RestFsmExecutor UML and use cases](docs/UML/RestFsmExecutor.UML.md) — manually maintained, source-checked neighborhood and use-case view.
- Source: [RestFsmExecutor.cs](src/FSM_REST/RestFsmExecutor.cs) · [Core project dependency](src/FSM_REST/FSM_REST.csproj).

**Semantic boundary:** `Completed` means the transport returned a response. An HTTP 4xx/5xx response is not automatically an FSM failure; inspect the response status separately. The current source has explicit lifecycle handling, but this page does not claim those paths have fresh test evidence. Require exact-SHA CI and tests for normal completion, transport failure, requested cancellation, non-success HTTP status, concurrent calls, and instance cleanup before release.

## 10. Use FSM_REST against CoderLegion

The [CoderLegion Article Library sample](samples/CoderLegionArticleLibrary/README.md) is a credential-gated, read-only integration against the supplied CoderLegion API contract. It describes `GET /posts?page=N` and `GET /posts/{id}` as ordinary FSM_REST operations, passes the Personal API Key as an explicit `X-API-Key` host header, and handles the response envelope downstream.

From the repository root, set `CODERLEGION_API_KEY` in the environment and run:

```powershell
dotnet run --project samples/CoderLegionArticleLibrary -- list 100
dotnet run --project samples/CoderLegionArticleLibrary -- show 101
```

Set `CODERLEGION_HANDLE` to filter list results to that author handle. `GET /posts` is documented as a paginated browse endpoint rather than a dedicated “my posts” endpoint, so this sample filters pages locally and caps the scan. It does not create, edit, or publish content. The sample is a live integration recipe, not a claim that a request has already succeeded; run it with a valid Personal API Key to verify the service contract in practice.

This is the first real-service demonstration. Draft composition and explicit-confirmation publishing should follow only after the read-only path is verified.

## Visual guides

- [Capability recipe](docs/assets/fsm-rest-capability-recipe.svg)
- [Execution flow](docs/assets/fsm-rest-execution-flow.svg)
- [Composition map](docs/assets/fsm-rest-composition-map.svg)

## Living documentation and visual provenance

The SVGs linked above are explanatory diagrams of the capability recipe, execution flow, and composition boundary. Treat each as a teaching aid whose claims must remain aligned with the source; an SVG file is not automatically a generated or runtime-verified view.

The longer-term Workshop direction is to connect source and tests to a normalized semantic model, then render that model through GUI.SVG. Keep source structure, human-authored explanation, and runtime observations distinct:

- **Source-checked:** relationships and labels were checked against named source files or a stated scope.
- **Generated:** the diagram was produced by a documented generation pipeline from a declared model/source revision.
- **Runtime-observed:** the image captures actual execution or state, with the capture method recorded.
- **Conceptual:** the image explains an intended design and is explicitly not implementation evidence.

For every visual, document what it teaches, how it was produced, the scope of evidence, how to verify it, and what it does not prove. The recursive UML standard also requires stable type identity, explicit relationship kinds, unresolved-relationship reporting, and links to companion type/use-case documentation. Reflection alone is not a full syntax tree and cannot infer user intent.

## What the current core does—and does not—serialize

Request parameter values are scalar strings. The factory handles path, query, header, and cookie locations, but it is not a full OpenAPI parameter serializer: array/object encoding and style/explode rules are outside the current model. Request/response schema type and format fields remain descriptive and do not validate bodies. `RestResponse.Deserialize<T>()` can deserialize JSON into a consumer-owned model, but it does not validate that model against the API schema. The transport returns status, headers, and body for the caller to interpret, including non-success status codes.

## Verification

The repository contains descriptor and transport tests under [tests/FSM_REST.Tests](tests/FSM_REST.Tests/). CI definitions are in [build.yml](.github/workflows/build.yml) and [verify.yml](.github/workflows/verify.yml). Consult the current workflow results before treating a branch as release-ready.

## Not claimed by this gallery

FSM_REST supports an initial OpenAPI 3.x JSON subset and can connect to a definition URL; it does not parse YAML, resolve external references, provide authentication or retry policy, cache responses, render GUI controls, evaluate arbitrary data paths, validate arbitrary response schemas, or host production REST endpoints. Those require separately defined adapters or host responsibilities.


## Maintainer note: branch cleanup

Before removing any surplus branch, use the [branch reconciliation ledger](docs/BRANCH_RECONCILIATION.md). It records the comparison against `development` and calls out candidate material that needs review; it does not authorize automatic deletion.
