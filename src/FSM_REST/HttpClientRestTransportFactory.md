# HTTP REST transport factory

**Workshop moniker:** @TrentBest  
**Purpose:** Create short-lived HTTP-backed `IRestTransport` instances using the host's configured `IHttpClientFactory`.

## Register configured clients

```csharp
using Microsoft.Extensions.DependencyInjection;
using TheSingularityWorkshop.FSM_REST;

var services = new ServiceCollection();

services.AddHttpClient("CatalogApi", client =>
{
    client.BaseAddress = new Uri("https://catalog.example.test/");
    client.Timeout = TimeSpan.FromSeconds(15);
});

services.AddHttpClientRestTransportFactory();

using var provider = services.BuildServiceProvider();
var factory = provider.GetRequiredService<HttpClientRestTransportFactory>();

using var transport = factory.Create("CatalogApi");
var response = await transport.SendAsync(
    new RestRequest("GET", new Uri("https://catalog.example.test/items")));
```

## Lifetime rules

- Register each configured API client with `AddHttpClient`; names identify separate host configuration.
- The factory returns a new transport backed by a client from `IHttpClientFactory`.
- Dispose each transport when its own lifetime ends. The transport disposes its returned `HttpClient`; the factory continues to manage and pool underlying handlers.
- Concurrent sends are supported. Disposal is thread-safe and idempotent, but it is not a drain operation: coordinate disposal with active sends because disposing the transport may interrupt in-flight requests.
- The singleton factory is safe to share. Do not register a single transport as a singleton when it may be disposed or when request-specific state/lifetime is expected.
- If you already have an `IHttpClientFactory`, construct `HttpClientRestTransportFactory` directly or use `HttpClientRestTransportBuilder.UseFactory`; the DI extension is optional convenience.

## Boundary

This factory only selects client configuration and creates the transport. It does not infer credentials, retries, authorization, endpoint policy, caching, telemetry, or domain error semantics. Configure host policy explicitly. `RestRequest` and `RestResponse` currently represent bodies as strings; this API is not binary/streaming transport.

## Related source

- [Factory implementation](HttpClientRestTransportFactory.cs)
- [DI registration](HttpClientRestTransportServiceCollectionExtensions.cs)
- [Fluent builder](HttpClientRestTransportBuilder.md)
- [Transport contract](IRestTransport.md)
- [Tests](../../tests/FSM_REST.Tests/)
