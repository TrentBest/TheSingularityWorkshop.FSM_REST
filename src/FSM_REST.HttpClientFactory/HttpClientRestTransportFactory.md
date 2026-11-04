# HttpClientRestTransportFactory

**Workshop moniker:** @TrentBest  
**Package:** TheSingularityWorkshop.FSM_Rest.HttpClientFactory

Creates short-lived HTTP-backed IRestTransport instances using the host's configured IHttpClientFactory.

## Usage

~~~csharp
using Microsoft.Extensions.DependencyInjection;
using TheSingularityWorkshop.FSM_REST;
using TheSingularityWorkshop.FSM_REST.HttpClientFactory;

services.AddHttpClient("CatalogApi", client =>
{
    client.BaseAddress = new Uri("https://catalog.example.test/");
    client.Timeout = TimeSpan.FromSeconds(15);
});
services.AddHttpClientRestTransportFactory();

var factory = serviceProvider.GetRequiredService<HttpClientRestTransportFactory>();
using IRestTransport transport = factory.Create("CatalogApi");
~~~

## Lifetime rules

- The optional package returns a new transport backed by a client from IHttpClientFactory.
- The transport owns and disposes that returned HttpClient.
- The factory continues to manage and pool underlying handlers.
- Dispose each transport when its own lifetime ends.
- Configure credentials and host policy through named-client registration; this factory adds no auth or retry policy.
- The singleton factory is safe to share. Do not register one disposable transport as a singleton.

## Boundary

This factory selects client configuration and creates the transport. It does not replace IRestTransport or add domain policy. The current RestRequest and RestResponse contracts still materialize bodies as strings; this API is not binary/streaming transport.

## Related source

- [Factory implementation](HttpClientRestTransportFactory.cs)
- [DI registration](HttpClientRestTransportServiceCollectionExtensions.md)
- [Package guide](README.md)
- [Consumer guide](../../docs/CONSUMING.md)
