using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Http;
using TheSingularityWorkshop.FSM_REST;
using Xunit;

namespace TheSingularityWorkshop.FSM_REST.Tests;

public sealed class RestTransportTests
{
    [Fact]
    public async Task OperationExecutorBuildsAndExecutesDescribedOperation()
    {
        var handler = new StubHandler(request =>
        {
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.Equal("https://example.test/v1/people/42?expand=profile", request.RequestUri!.AbsoluteUri);
            Assert.Equal("trace-456", request.Headers.GetValues("X-Trace").Single());
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"id":42}""")
            };
        });

        using var client = new HttpClient(handler);
        var transport = new HttpClientRestTransport(client);
        var operation = new RestOperationDescriptor(
            "GET",
            "/people/{id}",
            "getPerson",
            "Get person",
            null,
            [
                new RestParameterDescriptor("id", "path", true, "string", null, null),
                new RestParameterDescriptor("expand", "query", false, "string", null, null)
            ],
            null);

        var response = await RestOperationExecutor.ExecuteAsync(
            operation,
            new Uri("https://example.test/v1"),
            transport,
            new Dictionary<string, string?> { ["id"] = "42", ["expand"] = "profile" },
            new Dictionary<string, string> { ["X-Trace"] = "trace-456" });

        Assert.Equal(200, response.StatusCode);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal("""{"id":42}""", response.Body);
    }

    [Fact]
    public async Task HttpClientTransportExecutesRequestAndCapturesResponse()
    {
        var handler = new StubHandler(request =>
        {
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("trace-123", request.Headers.GetValues("X-Trace").Single());

            return new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent(
                    "{\"id\":42}",
                    System.Text.Encoding.UTF8,
                    "application/json")
            };
        });

        using var client = new HttpClient(handler);
        var transport = new HttpClientRestTransport(client);

        var response = await transport.SendAsync(
            new RestRequest(
                "POST",
                new Uri("https://example.test/users"),
                new Dictionary<string, string> { ["X-Trace"] = "trace-123" },
                "{\"name\":\"Ada\"}"));

        Assert.Equal(201, response.StatusCode);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal("{\"id\":42}", response.Body);
        Assert.Equal("application/json; charset=utf-8", response.Headers["Content-Type"]);
    }

    [Fact]
    public async Task HttpClientTransportAppliesContentHeadersToRequestContent()
    {
        var handler = new StubHandler(request =>
        {
            Assert.NotNull(request.Content);
            Assert.Equal("en", request.Content.Headers.GetValues("Content-Language").Single());
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        using var client = new HttpClient(handler);
        var transport = new HttpClientRestTransport(client);

        await transport.SendAsync(new RestRequest(
            "POST",
            new Uri("https://example.test/content"),
            new Dictionary<string, string> { ["Content-Language"] = "en" },
            "payload",
            "text/plain"));
    }

    [Fact]
    public async Task TransportPreservesNonSuccessStatus()
    {
        var handler = new StubHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent("missing")
            });

        using var client = new HttpClient(handler);
        var transport = new HttpClientRestTransport(client);

        var response = await transport.SendAsync(
            new RestRequest("GET", new Uri("https://example.test/missing")));

        Assert.Equal(404, response.StatusCode);
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal("missing", response.Body);
    }

    [Fact]
    public async Task TransportHonorsCancellation()
    {
        using var client = new HttpClient(new BlockingHandler());
        var transport = new HttpClientRestTransport(client);
        using var cancellation = new CancellationTokenSource();

        var task = transport.SendAsync(
            new RestRequest("GET", new Uri("https://example.test/slow")),
            cancellation.Token);

        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);
    }

    [Fact]
    public void TransportRejectsNullHttpClient()
    {
        Assert.Throws<ArgumentNullException>(() => new HttpClientRestTransport(null!));
    }

    [Fact]
    public async Task TransportRejectsNullRequest()
    {
        using var client = new HttpClient(new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)));
        var transport = new HttpClientRestTransport(client);

        await Assert.ThrowsAsync<ArgumentNullException>(() => transport.SendAsync(null!));
    }

    [Fact]
    public async Task FsmExecutorTracksTransportCompletionWithoutReclassifyingHttpStatus()
    {
        var transport = new DelegateTransport((_, _) => Task.FromResult(
            new RestResponse(404, "Not Found", new Dictionary<string, string>(), "missing")));

        var response = await RestFsmExecutor.SendAsync(
            new RestRequest("GET", new Uri("https://example.test/missing")),
            transport);

        Assert.Equal(404, response.StatusCode);
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task FsmExecutorPropagatesTransportFailure()
    {
        var transport = new DelegateTransport((_, _) =>
            Task.FromException<RestResponse>(new HttpRequestException("offline")));

        await Assert.ThrowsAsync<HttpRequestException>(() => RestFsmExecutor.SendAsync(
            new RestRequest("GET", new Uri("https://example.test/offline")),
            transport));
    }

    [Fact]
    public async Task TransportPreservesNoContentStatusAndEmptyBody()
    {
        var handler = new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.NoContent));

        using var client = new HttpClient(handler);
        var transport = new HttpClientRestTransport(client);

        var response = await transport.SendAsync(
            new RestRequest("GET", new Uri("https://example.test/empty")));

        Assert.Equal((int)HttpStatusCode.NoContent, response.StatusCode);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(string.Empty, response.Body);
    }


    [Fact]
    public async Task BuilderDoesNotDisposeCallerOwnedClient()
    {
        using var client = new HttpClient(new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)));
        var transport = HttpClientRestTransportBuilder.Create().UseClient(client).Build();

        transport.Dispose();

        using var response = await client.GetAsync("https://example.test/");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BuilderDisposesClientItCreates()
    {
        var transport = HttpClientRestTransportBuilder.Create()
            .ConfigureClient(client => client.Timeout = TimeSpan.FromSeconds(3))
            .Build();

        transport.Dispose();

        await Assert.ThrowsAsync<ObjectDisposedException>(() => transport.SendAsync(
            new RestRequest("GET", new Uri("https://example.test/"))));
    }

    [Fact]
    public async Task BuilderUsesInjectedHandler()
    {
        var called = false;
        var handler = new StubHandler(_ =>
        {
            called = true;
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        });

        using var transport = HttpClientRestTransportBuilder.Create()
            .UseHandler(handler)
            .Build();

        var response = await transport.SendAsync(
            new RestRequest("GET", new Uri("https://example.test/")));

        Assert.True(called);
        Assert.Equal((int)HttpStatusCode.Accepted, response.StatusCode);
    }


    [Fact]
    public async Task BuilderUsesNamedHttpClientFactoryClient()
    {
        var factory = new StubHttpClientFactory(name =>
        {
            Assert.Equal("CatalogApi", name);
            return new HttpClient(new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)));
        });

        using var transport = HttpClientRestTransportBuilder.Create()
            .UseFactory(factory, "CatalogApi")
            .Build();

        var response = await transport.SendAsync(
            new RestRequest("GET", new Uri("https://example.test/")));

        Assert.Equal(200, response.StatusCode);
        Assert.Equal("CatalogApi", factory.RequestedName);
    }

    [Fact]
    public void BuilderCannotBuildTwice()
    {
        var builder = HttpClientRestTransportBuilder.Create()
            .UseHandler(new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)));

        using var transport = builder.Build();

        Assert.Throws<InvalidOperationException>(() => builder.Build());
        Assert.Throws<InvalidOperationException>(() => builder.ConfigureClient(_ => { }));
    }

    private sealed class DelegateTransport(
        Func<RestRequest, CancellationToken, Task<RestResponse>> send) : IRestTransport
    {
        public Task<RestResponse> SendAsync(RestRequest request, CancellationToken cancellationToken = default) =>
            send(request, cancellationToken);
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

    private sealed class BlockingHandler : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }

    private sealed class StubHandler(
        Func<HttpRequestMessage, HttpResponseMessage> responder) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) =>
            Task.FromResult(responder(request));
    }
}
