using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using TheSingularityWorkshop.FSM_REST;
using TheSingularityWorkshop.FSM_REST.HttpClientFactory;
using Xunit;

namespace TheSingularityWorkshop.FSM_REST.Tests;

public sealed class HttpClientFactoryIntegrationTests
{
    [Fact]
    public async Task FactoryCreatesNamedTransportAndDisposesFactoryClient()
    {
        var handler = new TrackingHandler();
        var factory = new StubHttpClientFactory(name =>
        {
            Assert.Equal("CatalogApi", name);
            return new HttpClient(handler);
        });

        using var transport = new HttpClientRestTransportFactory(factory).Create("CatalogApi");
        var response = await transport.SendAsync(new RestRequest("GET", new Uri("https://example.test/")));

        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("CatalogApi", factory.RequestedName);
    }

    [Fact]
    public async Task FactoryCreatesDefaultClientAndTransportOwnsItsLifetime()
    {
        var handler = new TrackingHandler();
        var factory = new StubHttpClientFactory(name =>
        {
            Assert.Equal(string.Empty, name);
            return new HttpClient(handler);
        });

        var transport = new HttpClientRestTransportFactory(factory).Create();
        var response = await transport.SendAsync(new RestRequest("GET", new Uri("https://example.test/default")));

        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(string.Empty, factory.RequestedName);
        transport.Dispose();
        transport.Dispose();
        Assert.True(handler.WasDisposed);
    }

    [Fact]
    public void FactoryRejectsNullClientFactory()
    {
        Assert.Throws<ArgumentNullException>(() => new HttpClientRestTransportFactory(null!));
    }

    [Fact]
    public void DependencyInjectionRegistersOptionalTransportFactory()
    {
        var services = new ServiceCollection();
        services.AddHttpClient("CatalogApi");
        services.AddHttpClientRestTransportFactory();

        using var provider = services.BuildServiceProvider();
        Assert.NotNull(provider.GetRequiredService<HttpClientRestTransportFactory>());
    }

    private sealed class StubHttpClientFactory(Func<string, HttpClient> createClient) : IHttpClientFactory
    {
        public string? RequestedName { get; private set; }
        public HttpClient CreateClient(string name)
        {
            RequestedName = name;
            return createClient(name);
        }
    }

    private sealed class TrackingHandler : HttpMessageHandler
    {
        private int _disposed;
        public bool WasDisposed => Volatile.Read(ref _disposed) > 0;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        protected override void Dispose(bool disposing)
        {
            if (disposing) Interlocked.Increment(ref _disposed);
            base.Dispose(disposing);
        }
    }
}