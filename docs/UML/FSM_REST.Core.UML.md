# FSM_REST Core — UML and Use Cases

## Class diagram

The central model is the operation descriptor. Runtime binding and request construction lead to the transport boundary; HTTP is one implementation of that boundary.

```mermaid
classDiagram
    direction TB

    class RestApiDescriptor {
      +string Title
      +string Version
      +IReadOnlyList~RestOperationDescriptor~ Operations
      +int OperationCount
    }
    class RestOperationDescriptor {
      +string Method
      +string Path
      +string OperationId
      +string PaletteLabel
      +IReadOnlyList~RestParameterDescriptor~ Parameters
      +RestRequestBodyDescriptor RequestBody
      +IReadOnlyList~RestResponseDescriptor~ ResponseDescriptors
    }
    class RestParameterDescriptor
    class RestRequestBodyDescriptor
    class RestResponseDescriptor
    class RestOperationBinding {
      +RestOperationDescriptor Operation
      +IReadOnlyDictionary~string,string~ Parameters
    }
    class RestRequestFactory {
      +Create(...)
    }
    class RestRequest
    class IRestTransport {
      <<interface>>
      +SendAsync(request, cancellationToken) Task~RestResponse~
    }
    class HttpClientRestTransport {
      -HttpClient _client
      +SendAsync(request, cancellationToken) Task~RestResponse~
    }
    class RestResponse

    RestApiDescriptor "1" o-- "*" RestOperationDescriptor : describes
    RestOperationDescriptor "1" o-- "*" RestParameterDescriptor : declares
    RestOperationDescriptor "1" o-- "0..1" RestRequestBodyDescriptor : describes
    RestOperationDescriptor "1" o-- "*" RestResponseDescriptor : documents
    RestOperationBinding --> RestOperationDescriptor : invokes
    RestRequestFactory ..> RestOperationBinding : consumes
    RestRequestFactory ..> RestRequest : creates
    IRestTransport ..> RestRequest : sends
    IRestTransport ..> RestResponse : returns
    HttpClientRestTransport ..|> IRestTransport : implements
```

## Use cases

### UC-1 — Describe a remote capability

A provider or application creates a `RestApiDescriptor` containing operation descriptors. It need not use OpenAPI or any particular source format.

### UC-2 — Construct an invocation

A caller supplies runtime values through `RestOperationBinding` and invokes `RestRequestFactory.Create`. The factory validates required inputs and supported parameter locations and returns a request. No network call occurs during construction.

### UC-3 — Execute through HTTP

A host configures and owns an `HttpClient`, wraps it in `HttpClientRestTransport`, and calls `SendAsync`. Cancellation flows through the transport. The returned `RestResponse` is an observation, not a domain interpretation.

### UC-4 — Substitute a test or custom transport

A test or host implements `IRestTransport`. Capability descriptors and request construction remain independent of the concrete communication mechanism.

### UC-5 — Compose with FSM_COS

A consumer referencing the optional adapter constructs `RestMicroBundle` from a `RestApiDescriptor`. Core REST consumers do not need the FSM_COS adapter.

## Relationship and scope notes

- The diagram is a source-grounded conceptual UML view, not generated reflection output.
- It omits constructor overloads and incidental members to keep the architecture readable.
- The core project is `src/FSM_REST/FSM_REST.csproj`; the root web project is a non-packable experimental host.
