using System.Net;
using System.Text;
using TheSingularityWorkshop.FSM_REST;
using Xunit;

namespace TheSingularityWorkshop.FSM_REST.Tests;

public sealed class RestGuiOperationExecutorTests
{
    [Fact]
    public async Task ResolvesApplicationDataCallsEndpointAndPublishesResult()
    {
        const string definition = """
        {
          "openapi": "3.0.3",
          "info": { "title": "People API", "version": "1" },
          "servers": [{ "url": "/v1" }],
          "paths": {
            "/people/{id}": {
              "get": {
                "operationId": "getPerson",
                "parameters": [{ "name": "id", "in": "path", "required": true, "schema": { "type": "string" } }],
                "responses": { "200": { "description": "OK" } }
              }
            }
          }
        }
        """;
        var handler = new RoutingHandler(request =>
        {
            if (request.RequestUri!.AbsolutePath == "/openapi.json")
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(definition, Encoding.UTF8, "application/json") };
            if (request.RequestUri.AbsolutePath == "/v1/people/42")
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("""{"id":42,"name":"Ada"}""", Encoding.UTF8, "application/json") };
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        });
        using var client = new HttpClient(handler);
        var connection = await OpenApiRestApiConnection.ConnectAsync(new Uri("https://api.example.test/openapi.json"), client);
        var operation = Assert.Single(connection.Api.Operations);
        var mapping = new RestGuiMapping(
            operation,
            "Open person",
            "detail",
            [new RestGuiInputMapping("id", "Person ID", "text", "selection.personId", Required: true)],
            resultTarget: "views.person");

        string? publishedPath = null;
        RestResponse? publishedResponse = null;
        var response = await RestGuiOperationExecutor.ExecuteAsync(
            connection,
            mapping,
            path => path == "selection.personId" ? "42" : null,
            publishResult: (path, value) => { publishedPath = path; publishedResponse = value; });

        Assert.Equal(200, response.StatusCode);
        Assert.Equal("https://api.example.test/v1/people/42", handler.Requests[^1].AbsoluteUri);
        Assert.Equal("views.person", publishedPath);
        Assert.Same(response, publishedResponse);
        var person = response.Deserialize<Person>();
        Assert.NotNull(person);
        Assert.Equal(42, person.Id);
        Assert.Equal("Ada", person.Name);
    }

    [Fact]
    public async Task RejectsRequiredInputWhenDataSourceHasNoValue()
    {
        const string definition = """
        { "openapi": "3.0.3", "info": { "title": "People API", "version": "1" }, "paths": {
          "/people/{id}": { "get": { "operationId": "getPerson",
            "parameters": [{ "name": "id", "in": "path", "required": true, "schema": { "type": "string" } }],
            "responses": { "200": { "description": "OK" } } } } } }
        """;
        using var client = new HttpClient(new RoutingHandler(_ => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(definition) }));
        var connection = await OpenApiRestApiConnection.ConnectAsync(new Uri("https://api.example.test/openapi.json"), client);
        var mapping = new RestGuiMapping(
            Assert.Single(connection.Api.Operations),
            "Open person",
            "detail",
            [new RestGuiInputMapping("id", "Person ID", "text", "selection.personId", Required: true)]);

        await Assert.ThrowsAsync<ArgumentException>(() => RestGuiOperationExecutor.ExecuteAsync(
            connection, mapping, _ => null));
    }

    private sealed record Person(int Id, string Name);

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
