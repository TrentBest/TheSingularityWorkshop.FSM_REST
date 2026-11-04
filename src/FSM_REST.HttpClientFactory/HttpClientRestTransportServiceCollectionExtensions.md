# HttpClientRestTransportServiceCollectionExtensions

**Workshop moniker:** @TrentBest  
**Package:** TheSingularityWorkshop.FSM_Rest.HttpClientFactory

Registers the optional HTTP transport factory with Microsoft.Extensions.DependencyInjection.

## Usage

~~~csharp
services.AddHttpClient("CatalogApi", client =>
{
    client.BaseAddress = new Uri("https://catalog.example.test/");
});

services.AddHttpClientRestTransportFactory();
~~~

This extension registers the standard IHttpClientFactory and registers HttpClientRestTransportFactory as a singleton only if no registration already exists. It does not register one global IRestTransport: applications may have multiple named API clients and should create/dispose a transport at the lifetime boundary appropriate to each operation or consumer.

## Related source

- [Registration extension](HttpClientRestTransportServiceCollectionExtensions.cs)
- [Transport factory](HttpClientRestTransportFactory.md)
- [Package guide](README.md)
- [Consumer guide](../../docs/CONSUMING.md)
