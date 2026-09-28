using TheSingularityWorkshop.FSM_REST;
using Xunit;

namespace TheSingularityWorkshop.FSM_REST.Tests;

public sealed class OpenApiRestReflectorTests
{
    [Fact]
    public void ReflectsOperationsAndParameters()
    {
        const string document = """
        {
          "openapi": "3.0.3",
          "info": { "title": "Example API", "version": "1.2.0" },
          "paths": {
            "/users/{id}": {
              "parameters": [
                {
                  "name": "id",
                  "in": "path",
                  "required": true,
                  "schema": { "type": "string", "format": "uuid" }
                }
              ],
              "get": {
                "operationId": "getUser",
                "summary": "Get a user",
                "parameters": [
                  {
                    "name": "includePosts",
                    "in": "query",
                    "schema": { "type": "boolean" }
                  }
                ],
                "responses": { "200": { "description": "OK" } }
              }
            }
          }
        }
        """;

        var api = OpenApiRestReflector.Reflect(document);

        Assert.Equal("Example API", api.Title);
        Assert.Equal("1.2.0", api.Version);
        Assert.Single(api.Operations);

        var operation = api.Operations[0];
        Assert.Equal("GET", operation.Method);
        Assert.Equal("/users/{id}", operation.Path);
        Assert.Equal("getUser", operation.OperationId);
        Assert.Equal("Get a user", operation.PaletteLabel);
        Assert.Equal(2, operation.Parameters.Count);
        Assert.Contains(operation.Parameters, p =>
            p.Name == "id" && p.Location == "path" && p.Required &&
            p.Type == "string" && p.Format == "uuid");
        Assert.Contains(operation.Parameters, p =>
            p.Name == "includePosts" && p.Location == "query" &&
            !p.Required && p.Type == "boolean");
    }

    [Fact]
    public void ReflectsRequestBody()
    {
        const string document = """
        {
          "openapi": "3.0.3",
          "info": { "title": "Write API", "version": "1.0" },
          "paths": {
            "/users": {
              "post": {
                "operationId": "createUser",
                "requestBody": {
                  "required": true,
                  "content": {
                    "application/json": {
                      "schema": { "type": "object" }
                    }
                  }
                },
                "responses": { "201": { "description": "Created" } }
              }
            }
          }
        }
        """;

        var body = Assert.Single(OpenApiRestReflector.Reflect(document).Operations).RequestBody;

        Assert.NotNull(body);
        Assert.True(body.Required);
        Assert.Contains("application/json", body.MediaTypes);
        Assert.Equal("object", body.SchemaType);
    }

    [Fact]
    public void RejectsNonOpenApiThreeDocuments()
    {
        Assert.Throws<ArgumentException>(() =>
            OpenApiRestReflector.Reflect("""{"openapi":"2.0","info":{"title":"Legacy","version":"1"}}"""));
    }

    [Fact]
    public void IgnoresUnsupportedOperationsAndReferencesForNow()
    {
        const string document = """
        {
          "openapi": "3.0.3",
          "info": { "title": "Partial API", "version": "1" },
          "paths": {
            "/users": {
              "get": { "$ref": "#/components/pathItems/UserList" },
              "post": {
                "operationId": "createUser",
                "responses": { "201": { "description": "Created" } }
              }
            }
          }
        }
        """;

        var api = OpenApiRestReflector.Reflect(document);

        Assert.Single(api.Operations);
        Assert.Equal("createUser", api.Operations[0].OperationId);
    }
}
