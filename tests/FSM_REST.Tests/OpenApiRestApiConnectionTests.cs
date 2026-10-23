using System.Net;
using System.Text;
using TheSingularityWorkshop.FSM_REST;
using Xunit;

namespace TheSingularityWorkshop.FSM_REST.Tests;

public sealed class OpenApiRestApiConnectionTests
{
    [Fact]
    public async Task ConnectsFromDefinitionUrlAndExecutesImportedOperation()
    {
        const string definition = """
        {
          "openapi": "3.0.3",
          "info": { "title": "Example API", "version": "1" },
          "servers": [{ "url": "/v1" }],
          "paths": {
            "/items/{id}": {
              "get": {
                "operationId": "getItem",
                "parameters": [{ "name": "id", "in": "path", "required": true, "schema": { "type": "string" } }],
                "responses": { "200": { "description": "OK" } }
              }
            }
          }
        }
        """;

        var handler = new RoutingHandler(request =>
        {
            if (request.RequestUri!.AbsolutePath == "/swagger.json")
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(definition, Encoding.UTF8, "application/json") };

            if (request.RequestUri.AbsolutePath == "/v1/items/42")
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"id\":42}", Encoding.UTF8, "application/json") };

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        });
        using var client = new HttpClient(handler);

        var connection = await OpenApiRestApiConnection.ConnectAsync(
            new Uri("https://api.example.test/swagger.json"), client);
        var response = await connection.ExecuteAsync(
            "getItem",
            new Dictionary<string, string?> { ["id"] = "42" });

        Assert.Equal("Example API", connection.Api.Title);
        Assert.Equal(new Uri("https://api.example.test/v1"), connection.BaseUri);
        Assert.Equal(1, connection.Api.OperationCount);
        Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)response.StatusCode);
        Assert.Equal("{\"id\":42}", response.Body);
        Assert.Equal(2, handler.Requests.Count);
        Assert.Equal("https://api.example.test/swagger.json", handler.Requests[0].AbsoluteUri);
        Assert.Equal("https://api.example.test/v1/items/42", handler.Requests[1].AbsoluteUri);
    }

    [Fact]
    public async Task RejectsUnknownOperationIds()
    {
        const string definition = """
        { "openapi": "3.0.3", "info": { "title": "Empty", "version": "1" }, "paths": {} }
        """;
        using var client = new HttpClient(new RoutingHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(definition) }));

        var connection = await OpenApiRestApiConnection.ConnectAsync(new Uri("https://api.example.test/openapi.json"), client);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => connection.ExecuteAsync("missing"));
    }

    private sealed class RoutingHandler(Func<HttpRequestMessage, HttpResponseMessage> route) : HttpMessageHandler
    {
        public List<Uri> Requests { get; } = [];

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add(request.RequestUri!);
            return Task.FromResult(route(request));
        }
    }
}
