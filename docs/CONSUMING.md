# Consuming FSM_REST

This guide shows the supported path from an API description to one outbound request and one observed response. FSM_REST includes an initial OpenAPI 3.x JSON importer, but it is not a complete OpenAPI runtime, authentication system, or domain client.

## Connect from a definition URL

When the API publishes a Swagger/OpenAPI 3.x JSON document, pass its URL to `OpenApiRestApiConnection.ConnectAsync`. The connection downloads and imports the document, selects the first declared concrete server URL (or the definition's origin if no server is declared), and lets the caller execute imported operations by `operationId`.

```csharp
using System.Net.Http;
using TheSingularityWorkshop.FSM_REST;

using var client = new HttpClient();
// Configure client.DefaultRequestHeaders or a handler for the API's authentication policy.
var connection = await OpenApiRestApiConnection.ConnectAsync(
    new Uri("https://api.example.com/openapi.json"),
    client,
    cancellationToken);

foreach (var warning in connection.Warnings)
    Console.WriteLine($"OpenAPI import warning: {warning}");

RestResponse response = await connection.ExecuteAsync(
    "getPerson",
    parameters: new Dictionary<string, string?> { ["id"] = "person-123" },
    cancellationToken: cancellationToken);
```

The supplied `HttpClient` is used both to retrieve the definition and to call the API; the connection does not dispose it. Authentication remains a host concern, so configure the client with the credentials and scopes required by that API. The first declared server is used; server-variable substitution and choosing among multiple server environments are not automatic. Local component references are resolved for the supported operation metadata subset; external references and full OpenAPI schema composition are not.

## Map an operation into a GUI and application data

An imported operation can be mapped to renderer-neutral GUI metadata without adding a GUI-framework dependency to FSM_REST. The GUI adapter chooses the concrete controls; the mapping declares which operation inputs come from application state, which are supplied by the user, and where the result should be stored.

```csharp
var operation = connection.Api.Operations.Single(item => item.OperationId == "getPerson");
var guiMapping = new RestGuiMapping(
    operation,
    label: "Open person",
    presentation: "detail",
    inputs:
    [
        new RestGuiInputMapping(
            ParameterName: "id",
            Label: "Person ID",
            ValueType: "text",
            SourcePath: "selection.personId",
            Required: true)
    ],
    resultTarget: "views.person",
    showLoadingState: true,
    showErrorState: true);
```

A GUI adapter can render this mapping as an action/detail view, read `selection.personId` from its application data context, pass that value as the `id` operation parameter, invoke `connection.ExecuteAsync(guiMapping.OperationId, parameters)`, then place the returned response at `views.person` according to the host's response parsing and state-update policy. Inputs without a `SourcePath` are intended to be collected from the user. The `presentation` value is a renderer-neutral hint, not a promise that FSM_REST itself generates controls.

The mapping constructor rejects undeclared or duplicate operation parameters. It deliberately does not evaluate arbitrary data paths, execute operations, deserialize payloads, or bypass authorization: those are responsibilities of the GUI/data-context adapter and host policy.

## The invocation lifecycle

1. Keep a `RestOperationDescriptor` as reusable capability metadata.
2. Create a `RestOperationBinding` for each invocation. Its parameter values are copied into a read-only snapshot.
3. Use `RestRequestFactory` to bind values into a `RestRequest`.
4. Send the request through `IRestTransport` (the built-in `HttpClientRestTransport` is one adapter).
5. Interpret the returned `RestResponse` in your domain or host code.

## Example

The following illustrates the core call shape. Replace the endpoint, operation metadata, and body with the contract your application actually consumes.

```csharp
using TheSingularityWorkshop.FSM_REST;

var operation = new RestOperationDescriptor(
    Method: "GET",
    Path: "/users/{id}",
    OperationId: "getUser",
    Summary: "Get a user",
    Description: null,
    Parameters: new[]
    {
        new RestParameterDescriptor(
            Name: "id",
            Location: "path",
            Required: true,
            Type: "string",
            Format: null,
            Description: "User identifier")
    },
    RequestBody: null);

var values = new Dictionary<string, string?> { ["id"] = "user-123" };
var binding = new RestOperationBinding(operation, values);
var request = RestRequestFactory.Create(
    binding,
    new Uri("https://api.example.com/"));

using var client = new HttpClient();
IRestTransport transport = new HttpClientRestTransport(client);
RestResponse response = await transport.SendAsync(request, cancellationToken);

if (!response.IsSuccessStatusCode)
{
    // Apply your domain-specific error, retry, and observability policy.
}

// Parse response.Body using the schema and serializer owned by your application.
```

## Execute an operation in one call

When you do not need to inspect or modify the intermediate `RestRequest`, use `RestOperationExecutor` to bind the supplied values and send through your chosen transport:

```csharp
RestResponse response = await RestOperationExecutor.ExecuteAsync(
    operation,
    new Uri("https://api.example.com/"),
    transport,
    parameters: new Dictionary<string, string?> { ["id"] = "user-123" },
    cancellationToken: cancellationToken);
```

This helper returns the same raw `RestResponse` as the transport. It does not add authentication, retries, response deserialization, or domain-model generation.

## Request construction rules

- The base URI must be absolute and must not include a query or fragment.
- Required parameters must have non-empty values. Parameter values are scalar strings.
- Path and query values are percent-encoded. Query values are appended to the operation path.
- Supported parameter locations are path, query, header, and cookie. Unsupported locations fail rather than silently inventing serialization rules.
- A path template placeholder left unresolved after declared path parameters are applied is rejected.
- A caller-supplied `Cookie` header is preserved when declared cookie parameters are appended.
- Caller headers are copied into a case-insensitive dictionary. Avoid setting conflicting header values and validate any user-controlled header names and values at your application boundary.
- A request body is passed through as text. The default HTTP adapter uses `application/json` when a body exists and no content type was supplied; FSM_REST does not validate that the body is valid JSON.

## Response and failure handling

`HttpClientRestTransport` returns the observed HTTP status, reason phrase, response headers, and body. `RestResponse.IsSuccessStatusCode` is true only for status codes from 200 through 299. A non-2xx response is still a response; the transport does not automatically turn it into a domain exception or retry it.

Your host or domain capability decides how to handle authentication, authorization, cancellation, timeouts, retries, backoff, rate limits, caching, telemetry, response deserialization, schema validation, and domain-specific errors. The host also owns the lifetime and configuration of the supplied `HttpClient`.

## Scope and limitations

FSM_REST provides a neutral descriptor/binding/request/transport/response path, an `OpenApiJsonImporter` for OpenAPI 3.x JSON, and `OpenApiRestApiConnection` for loading a definition URL and invoking imported operations. The importer reads operation paths, parameters, request-body media/schema hints, and response metadata. It does not itself:

- load YAML definitions, resolve external references, substitute server variables, select among multiple server environments, or fully implement the OpenAPI standard (including composed schemas and complete validation);
- implement array/object parameter serialization styles, `style`/`explode`, or schema-driven body validation;
- authenticate users, acquire credentials, authorize an operation, or decide whether data may be disclosed;
- deserialize response bodies into domain objects, retry requests, cache results, or define domain error meanings.

Treat operation descriptors as metadata, not permission grants. Validate the remote origin, credential scope, caller authorization, redirects, and data handling according to your application threat model. Do not let untrusted input choose arbitrary base URIs or inject privileged headers without explicit policy.

## Architectural boundary

FSM_REST owns reusable REST capability shapes and request transport contracts. The consuming domain owns the meaning of an operation and its policy. Optional composition integrations belong in their own integration package; the core remains independently consumable.

## Verify locally

From the repository root, run the test suite and build commands documented in the README and CI workflow. Add regression tests whenever an adapter changes encoding, header handling, status handling, cancellation, or other observable request semantics.