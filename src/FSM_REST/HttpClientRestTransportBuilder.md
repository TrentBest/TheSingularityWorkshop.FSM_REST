# HttpClientRestTransportBuilder

**Workshop moniker:** @TrentBest  
**Purpose:** Fluent construction of the HTTP-backed `IRestTransport` adapter while keeping client ownership explicit.

## Choose a construction path

- `Create().Build()` creates a new `HttpClient` owned by the resulting transport.
- `UseClient(client)` wraps a host-owned client without changing its configuration or disposing it.
- `UseHandler(handler, disposeHandler: true)` creates a client around a supplied handler. The handler is disposed with the transport by default; pass `false` when the handler is managed elsewhere.
- `UseFactory(factory, name)` obtains a configured client from `IHttpClientFactory`. The transport disposes the returned client; the factory continues to manage its handler lifetime.

## Configure a builder-owned client

```csharp
using var transport = HttpClientRestTransportBuilder.Create()
    .ConfigureClient(client =>
    {
        client.BaseAddress = new Uri("https://api.example.test/");
        client.Timeout = TimeSpan.FromSeconds(15);
    })
    .Build();
```

## Supply an existing client

```csharp
var client = httpClientFactory.CreateClient("CatalogApi");
var transport = HttpClientRestTransportBuilder.Create()
    .UseClient(client)
    .Build();

// Dispose the transport when its scope ends. The caller remains responsible for client lifetime.
```

Prefer `UseFactory(factory, "CatalogApi")` when the transport should obtain its own configured factory client. Configure factory-managed defaults through dependency injection, not `ConfigureClient`.

## Lifetime and boundaries

- Dispose each built transport when its lifetime ends.
- A client supplied with `UseClient` remains caller-owned and is not disposed by the transport.
- `ConfigureClient` is only valid for a client created by the builder; combining it with `UseClient` or `UseFactory` throws rather than silently mutating shared configuration.
- Each builder is single-use. Create a new builder for each transport; this prevents accidental reuse of a supplied handler.
- This is construction convenience, not a second transport abstraction. Authentication, retries, endpoint policy, and domain-specific behavior remain the host's responsibility.
- The current `RestRequest` / `RestResponse` contract materializes bodies as strings. This builder does not provide binary or streaming support.

## Source and verification

- Source: [HttpClientRestTransportBuilder.cs](HttpClientRestTransportBuilder.cs)
- Adapter: [HttpClientRestTransport](HttpClientRestTransport.md)
- Contract: [IRestTransport](IRestTransport.md)
- Tests: [FSM_REST tests](../../tests/FSM_REST.Tests/)
- Provenance: manually authored companion.
