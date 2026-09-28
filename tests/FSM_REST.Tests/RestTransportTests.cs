using System.Net;
using System.Net.Http;
using TheSingularityWorkshop.FSM_REST;
using Xunit;

namespace TheSingularityWorkshop.FSM_REST.Tests;

public sealed class RestTransportTests
{
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

    private sealed class StubHandler(
        Func<HttpRequestMessage, HttpResponseMessage> responder) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) =>
            Task.FromResult(responder(request));
    }
}
