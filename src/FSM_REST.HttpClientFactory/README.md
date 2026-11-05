<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_REST HttpClientFactory integration

**Workshop moniker:** @TrentBest  
**Package:** TheSingularityWorkshop.FSM_Rest.HttpClientFactory

This optional package adds integration with the standard .NET IHttpClientFactory and dependency injection. It depends on FSM_REST core and Microsoft.Extensions.Http; the core package does not depend on either abstraction.

## Register named clients

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

## Lifetime and ownership

- Register named client configuration in the host with AddHttpClient.
- The factory is safe to register as a singleton and creates a fresh transport/client for each call.
- The returned transport owns and disposes its HttpClient.
- IHttpClientFactory continues to manage pooled handler lifetime.
- Dispose each transport when its operation/consumer lifetime ends.
- The integration adds no authentication, retry, caching, or domain policy.

## Keep the dependency optional

If the application does not use IHttpClientFactory, reference only TheSingularityWorkshop.FSM_Rest and construct a transport with a host-owned HttpClient, or use the core builder's UseClient / UseHandler APIs.

The package does not change the IRestTransport contract and does not add streaming or binary body support.

## Source

Source code: https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
