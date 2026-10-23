# Consuming FSM_REST

This guide shows the supported path from an API description to one outbound request and one observed response. FSM_REST includes an initial OpenAPI 3.x JSON importer, but it is not a complete OpenAPI runtime, authentication system, or domain client.

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

FSM_REST provides a neutral descriptor/binding/request/transport/response path and an initial `OpenApiJsonImporter` for OpenAPI 3.x JSON. The importer reads operation paths, parameters, request-body media/schema hints, and response metadata. It does not itself:

- resolve OpenAPI `$ref` references or fully implement the OpenAPI standard (including YAML, composed schemas, and complete validation);
- implement array/object parameter serialization styles, `style`/`explode`, or schema-driven body validation;
- authenticate users, acquire credentials, authorize an operation, or decide whether data may be disclosed;
- deserialize response bodies into domain objects, retry requests, cache results, or define domain error meanings.

Treat operation descriptors as metadata, not permission grants. Validate the remote origin, credential scope, caller authorization, redirects, and data handling according to your application threat model. Do not let untrusted input choose arbitrary base URIs or inject privileged headers without explicit policy.

## Architectural boundary

FSM_REST owns reusable REST capability shapes and request transport contracts. The consuming domain owns the meaning of an operation and its policy. Optional composition integrations belong in their own integration package; the core remains independently consumable.

## Verify locally

From the repository root, run the test suite and build commands documented in the README and CI workflow. Add regression tests whenever an adapter changes encoding, header handling, status handling, cancellation, or other observable request semantics.