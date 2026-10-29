# HTTP transport factory dependency-injection registration

**Workshop moniker:** @TrentBest  
**Purpose:** Register `HttpClientRestTransportFactory` and ensure the standard `IHttpClientFactory` service is available.

## Usage

```csharp
services.AddHttpClient("CatalogApi", client =>
{
    client.BaseAddress = new Uri("https://catalog.example.test/");
});

services.AddHttpClientRestTransportFactory();
```

This extension registers the factory as a singleton only if no registration already exists, and registers the standard HTTP client factory. It deliberately does not register one global `IRestTransport`: applications may have multiple named API clients and should create/dispose a transport at the lifetime boundary appropriate to each operation or consumer.

## Related source

- [Registration extension](HttpClientRestTransportServiceCollectionExtensions.cs)
- [Transport factory](HttpClientRestTransportFactory.md)
- [Fluent builder](HttpClientRestTransportBuilder.md)
- [Consumer guide](../../docs/CONSUMING.md)
