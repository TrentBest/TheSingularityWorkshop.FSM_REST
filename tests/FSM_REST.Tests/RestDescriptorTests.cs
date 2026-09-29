using TheSingularityWorkshop.FSM_REST;
using Xunit;

namespace TheSingularityWorkshop.FSM_REST.Tests;

public sealed class RestDescriptorTests
{
    [Fact]
    public void OperationDescriptorProvidesStablePaletteLabel()
    {
        var operation = new RestOperationDescriptor(
            "GET",
            "/users/{id}",
            "getUser",
            "Get user",
            null,
            Array.Empty<RestParameterDescriptor>(),
            null);

        Assert.Equal("Get user", operation.PaletteLabel);
        Assert.Empty(operation.ResponseDescriptors);
    }

    [Fact]
    public void OperationDescriptorFallsBackToMethodAndPath()
    {
        var operation = new RestOperationDescriptor(
            "DELETE",
            "/users/{id}",
            "deleteUser",
            null,
            null,
            Array.Empty<RestParameterDescriptor>(),
            null);

        Assert.Equal("DELETE /users/{id}", operation.PaletteLabel);
    }

    [Fact]
    public void OperationDescriptorPreservesResponseDescriptors()
    {
        var responses = new[]
        {
            new RestResponseDescriptor("200", "Success", new[] { "application/json" }, "object", null),
            new RestResponseDescriptor("404", "Not found", Array.Empty<string>(), null, null)
        };

        var operation = new RestOperationDescriptor(
            "GET", "/users/{id}", "getUser", "Get user", "Retrieves one user.",
            Array.Empty<RestParameterDescriptor>(), null, responses);

        Assert.Equal(responses, operation.ResponseDescriptors);
    }

    [Fact]
    public void ApiDescriptorReportsOperationCount()
    {
        var operations = new[]
        {
            new RestOperationDescriptor(
                "GET",
                "/users",
                "listUsers",
                null,
                null,
                Array.Empty<RestParameterDescriptor>(),
                null)
        };

        var api = new RestApiDescriptor("Users", "1.0", operations);

        Assert.Equal(1, api.OperationCount);
    }
}

    [Fact]
    public void RequestFactoryBindsPathQueryHeadersAndCookies()
    {
        var operation = new RestOperationDescriptor(
            "GET",
            "/users/{id}",
            "getUser",
            "Get user",
            null,
            [
                new RestParameterDescriptor("id", "path", true, "string", null, null),
                new RestParameterDescriptor("page", "query", false, "integer", null, null),
                new RestParameterDescriptor("trace", "header", false, "string", null, null),
                new RestParameterDescriptor("session", "cookie", false, "string", null, null)
            ],
            null);

        var request = RestRequestFactory.Create(
            operation,
            new Uri("https://example.test/api"),
            new Dictionary<string, string?>
            {
                ["id"] = "Ada Lovelace",
                ["page"] = "2",
                ["trace"] = "trace-123",
                ["session"] = "abc 123"
            });

        Assert.Equal("GET", request.Method);
        Assert.Equal("https://example.test/api/users/Ada%20Lovelace?page=2", request.Uri.AbsoluteUri);
        Assert.Equal("trace-123", request.Headers!["trace"]);
        Assert.Equal("session=abc%20123", request.Headers["Cookie"]);
    }

    [Fact]
    public void RequestFactoryRejectsMissingRequiredParameter()
    {
        var operation = new RestOperationDescriptor(
            "GET", "/users/{id}", "getUser", null, null,
            [new RestParameterDescriptor("id", "path", true, "string", null, null)],
            null);

        Assert.Throws<ArgumentException>(() =>
            RestRequestFactory.Create(
                operation,
                new Uri("https://example.test/api")));
    }

    [Fact]
    public void RequestFactoryRejectsUnsupportedParameterLocation()
    {
        var operation = new RestOperationDescriptor(
            "GET", "/users", "listUsers", null, null,
            [new RestParameterDescriptor("x", "matrix", false, "string", null, null)],
            null);

        Assert.Throws<ArgumentException>(() =>
            RestRequestFactory.Create(
                operation,
                new Uri("https://example.test/api"),
                new Dictionary<string, string?> { ["x"] = "1" }));
    }

