using TheSingularityWorkshop.FSM_REST;
using Xunit;

namespace TheSingularityWorkshop.FSM_REST.Tests;

public sealed class OpenApiJsonImporterTests
{
    [Fact]
    public void ImportsOperationsParametersRequestBodyAndResponses()
    {
        const string json = """
        {
          "openapi": "3.0.3",
          "info": { "title": "People API", "version": "2.1", "description": "People records." },
          "paths": {
            "/people/{id}": {
              "parameters": [
                { "name": "id", "in": "path", "required": true, "schema": { "type": "string" } }
              ],
              "get": {
                "operationId": "getPerson",
                "summary": "Get a person",
                "responses": {
                  "200": {
                    "description": "Found",
                    "content": { "application/json": { "schema": { "type": "object" } } }
                  },
                  "404": { "description": "Not found" }
                }
              },
              "post": {
                "requestBody": {
                  "required": true,
                  "content": { "application/json": { "schema": { "type": "object" } } }
                },
                "responses": { "201": { "description": "Created" } }
              }
            }
          }
        }
        """;

        var result = OpenApiJsonImporter.Import(json);

        Assert.Equal("People API", result.Api.Title);
        Assert.Equal("2.1", result.Api.Version);
        Assert.Equal("People records.", result.Api.Description);
        Assert.Equal(2, result.Api.OperationCount);
        Assert.Empty(result.Warnings);

        var get = Assert.Single(result.Api.Operations, operation => operation.OperationId == "getPerson");
        Assert.Equal("GET", get.Method);
        Assert.Equal("/people/{id}", get.Path);
        var id = Assert.Single(get.Parameters);
        Assert.Equal("id", id.Name);
        Assert.Equal("path", id.Location);
        Assert.True(id.Required);
        Assert.Equal("object", Assert.Single(get.ResponseDescriptors, response => response.StatusCode == "200").SchemaType);

        var post = Assert.Single(result.Api.Operations, operation => operation.Method == "POST");
        Assert.True(post.RequestBody!.Required);
        Assert.Contains("application/json", post.RequestBody.MediaTypes);
        Assert.Equal("object", post.RequestBody.SchemaType);
    }

    [Fact]
    public void ReportsSecurityAndUnresolvedReferencesInsteadOfSilentlyClaimingFullImport()
    {
        const string json = """
        {
          "openapi": "3.1.0",
          "info": { "title": "Secured API", "version": "1" },
          "security": [{ "bearerAuth": [] }],
          "paths": {
            "/items": {
              "get": {
                "operationId": "listItems",
                "security": [{ "bearerAuth": [] }],
                "parameters": [{ "$ref": "#/components/parameters/Limit" }],
                "responses": {
                  "200": {
                    "description": "Items",
                    "content": { "application/json": { "schema": { "$ref": "#/components/schemas/Items" } } }
                  }
                }
              }
            }
          }
        }
        """;

        var result = OpenApiJsonImporter.Import(json);

        Assert.Single(result.Api.Operations);
        Assert.Contains(result.Warnings, warning => warning.Contains("Document-level security", StringComparison.Ordinal));
        Assert.Contains(result.Warnings, warning => warning.Contains("Operation 'listItems'", StringComparison.Ordinal));
        Assert.Contains(result.Warnings, warning => warning.Contains("parameter reference", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(result.Warnings, warning => warning.Contains("Schema reference", StringComparison.Ordinal));
    }

    [Fact]
    public void MergesDuplicatePathParametersAndAllowsOperationOverride()
    {
        const string json = """
        {
          "openapi": "3.0.3",
          "info": { "title": "Parameters", "version": "1" },
          "paths": {
            "/items/{id}": {
              "parameters": [
                { "name": "id", "in": "path", "schema": { "type": "string" } },
                { "name": "id", "in": "path", "schema": { "type": "integer" } }
              ],
              "get": {
                "parameters": [
                  { "name": "id", "in": "path", "schema": { "type": "integer", "format": "int64" } }
                ],
                "responses": { "200": { "description": "OK" } }
              }
            }
          }
        }
        """;

        var result = OpenApiJsonImporter.Import(json);
        var operation = Assert.Single(result.Api.Operations);
        var id = Assert.Single(operation.Parameters);

        Assert.Equal("integer", id.Type);
        Assert.Equal("int64", id.Format);
    }

    [Fact]
    public void WarnsWhenParameterUsesUnsupportedContentSerialization()
    {
        const string json = """
        {
          "openapi": "3.0.3",
          "info": { "title": "Content parameter", "version": "1" },
          "paths": {
            "/items": {
              "get": {
                "parameters": [
                  { "name": "filter", "in": "query", "content": { "application/json": { "schema": { "type": "object" } } } }
                ],
                "responses": { "200": { "description": "OK" } }
              }
            }
          }
        }
        """;

        var result = OpenApiJsonImporter.Import(json);

        Assert.Contains(result.Warnings, warning => warning.Contains("content-based serialization", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{ \"openapi\": \"2.0\", \"paths\": {} }")]
    [InlineData("{ \"openapi\": \"3.0.3\", \"info\": {} }")]
    public void RejectsDocumentsOutsideSupportedOpenApiJsonShape(string json)
    {
        Assert.Throws<ArgumentException>(() => OpenApiJsonImporter.Import(json));
    }
}
