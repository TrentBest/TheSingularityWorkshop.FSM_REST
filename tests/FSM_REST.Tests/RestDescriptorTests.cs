using TheSingularityWorkshop.FSM_REST;
using TheSingularityWorkshop.FSM_REST.COS;
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


    [Fact]
    public void OperationBindingPreservesCapabilityAndCopiesRuntimeValues()
    {
        var operation = new RestOperationDescriptor(
            "GET",
            "/users/{id}",
            "getUser",
            "Get user",
            null,
            [new RestParameterDescriptor("id", "path", true, "string", null, null)],
            null);

        var values = new Dictionary<string, string?> { ["id"] = "42" };
        var binding = new RestOperationBinding(operation, values);
        values["id"] = "changed";

        Assert.Same(operation, binding.Operation);
        Assert.Equal("42", binding.Parameters["id"]);
    }

    [Fact]
    public void RequestFactoryAcceptsOperationBinding()
    {
        var operation = new RestOperationDescriptor(
            "GET",
            "/users/{id}",
            "getUser",
            null,
            null,
            [new RestParameterDescriptor("id", "path", true, "string", null, null)],
            null);

        var binding = new RestOperationBinding(
            operation,
            new Dictionary<string, string?> { ["id"] = "42" });

        var request = RestRequestFactory.Create(
            binding,
            new Uri("https://example.test/api"));

        Assert.Equal("https://example.test/api/users/42", request.Uri.AbsoluteUri);
    }

    [Fact]
    public void ApiDescriptorCanBecomeAnEcosystemMicroBundle()
    {
        var api = new RestApiDescriptor(
            "Store Catalog",
            "1.0",
            [
                new RestOperationDescriptor(
                    "GET",
                    "/products",
                    "listProducts",
                    null,
                    null,
                    Array.Empty<RestParameterDescriptor>(),
                    null)
            ]);

        var bundle = api.ToMicroBundle(0x2001UL);

        Assert.Equal(0x2001UL, bundle.Id);
        Assert.Equal("1.0", bundle.Descriptor.Version);
        Assert.Same(api, bundle.Api);
        Assert.Contains(bundle.Descriptor.Providers, provider => provider.Id == "rest:Store Catalog");
        Assert.Empty(bundle.Dependencies);
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

    [Theory]
    [InlineData("/../admin")]
    [InlineData("/%2e%2e/admin")]
    [InlineData("https://evil.example/steal")]
    [InlineData("//evil.example/steal")]
    [InlineData("/users?admin=true")]
    [InlineData("/users#fragment")]
    public void RequestFactoryRejectsPathsThatEscapeOrAlterTheBaseUri(string path)
    {
        var operation = new RestOperationDescriptor(
            "GET", path, "unsafePath", null, null,
            Array.Empty<RestParameterDescriptor>(), null);

        Assert.Throws<ArgumentException>(() =>
            RestRequestFactory.Create(operation, new Uri("https://example.test/api")));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("GET /users")]
    [InlineData("G@T")]
    public void RequestFactoryRejectsInvalidHttpMethodToken(string method)
    {
        var operation = new RestOperationDescriptor(
            method, "/users", "invalidMethod", null, null,
            Array.Empty<RestParameterDescriptor>(), null);

        Assert.Throws<ArgumentException>(() =>
            RestRequestFactory.Create(operation, new Uri("https://example.test/api")));
    }

    [Theory]
    [InlineData("a/b", "a%2Fb")]
    [InlineData("a?b&c=d", "a%3Fb%26c%3Dd")]
    [InlineData("100% ready", "100%25%20ready")]
    [InlineData("Māori name", "M%C4%81ori%20name")]
    public void RequestFactoryPercentEncodesReservedAndUnicodeQueryValues(string value, string encoded)
    {
        var operation = new RestOperationDescriptor(
            "GET", "/search", "search", null, null,
            [new RestParameterDescriptor("q", "query", true, "string", null, null)],
            null);

        var request = RestRequestFactory.Create(
            operation,
            new Uri("https://example.test/api"),
            new Dictionary<string, string?> { ["q"] = value });

        Assert.Equal($"https://example.test/api/search?q={encoded}", request.Uri.AbsoluteUri);
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

    [Theory]
    [InlineData("https://example.test/api?tenant=one")]
    [InlineData("https://example.test/api#section")]
    public void RequestFactoryRejectsBaseUriWithQueryOrFragment(string baseUri)
    {
        var operation = new RestOperationDescriptor(
            "GET", "/users", "listUsers", null, null,
            Array.Empty<RestParameterDescriptor>(),
            null);

        var exception = Assert.Throws<ArgumentException>(() =>
            RestRequestFactory.Create(operation, new Uri(baseUri)));

        Assert.Equal("baseUri", exception.ParamName);
    }

    [Fact]
    public void OperationBindingExposesRuntimeValuesAsReadOnlySnapshot()
    {
        var operation = new RestOperationDescriptor(
            "GET", "/users", "listUsers", null, null,
            Array.Empty<RestParameterDescriptor>(), null);
        var input = new Dictionary<string, string?> { ["page"] = "1" };
        var binding = new RestOperationBinding(operation, input);

        input["page"] = "2";

        Assert.Equal("1", binding.Parameters["page"]);
        Assert.Throws<NotSupportedException>(() =>
            ((IDictionary<string, string?>)binding.Parameters)["page"] = "3");
    }
    [Fact]
    public void RequestFactoryPreservesCallerCookiesWhenAddingCookieParameters()
    {
        var operation = new RestOperationDescriptor(
            "GET", "/users", "listUsers", null, null,
            [new RestParameterDescriptor("session", "cookie", false, "string", null, null)],
            null);

        var request = RestRequestFactory.Create(
            operation,
            new Uri("https://example.test/api"),
            headers: new Dictionary<string, string> { ["Cookie"] = "tenant=work" },
            parameters: new Dictionary<string, string?> { ["session"] = "abc 123" });

        Assert.Equal("tenant=work; session=abc%20123", request.Headers!["Cookie"]);
    }

    [Fact]
    public void RequestFactoryRejectsUndeclaredPathTemplate()
    {
        var operation = new RestOperationDescriptor(
            "GET", "/users/{id}", "getUser", null, null,
            Array.Empty<RestParameterDescriptor>(), null);

        var exception = Assert.Throws<ArgumentException>(() =>
            RestRequestFactory.Create(operation, new Uri("https://example.test/api")));

        Assert.Equal("operation", exception.ParamName);
    }


    [Fact]
    public void RequestFactoryRejectsUndeclaredRuntimeParameters()
    {
        var operation = new RestOperationDescriptor(
            "GET", "/users", "listUsers", null, null,
            [new RestParameterDescriptor("page", "query", false, "integer", null, null)],
            null);

        var exception = Assert.Throws<ArgumentException>(() =>
            RestRequestFactory.Create(
                operation,
                new Uri("https://example.test/api"),
                new Dictionary<string, string?> { ["paeg"] = "2" }));

        Assert.Equal("binding", exception.ParamName);
        Assert.Contains("paeg", exception.Message);
    }

    [Theory]
    [InlineData("a+b", "a%2Bb")]
    [InlineData("", "")]
    public void RequestFactoryEncodesPlusAndPreservesExplicitEmptyQueryValues(string value, string encoded)
    {
        var operation = new RestOperationDescriptor(
            "GET", "/search", "search", null, null,
            [new RestParameterDescriptor("q", "query", false, "string", null, null)],
            null);

        var request = RestRequestFactory.Create(
            operation,
            new Uri("https://example.test/api"),
            new Dictionary<string, string?> { ["q"] = value });

        Assert.Equal($"https://example.test/api/search?q={encoded}", request.Uri.AbsoluteUri);
    }

    [Fact]
    public void RequestFactoryTreatsMissingAndNullAsAbsentForOptionalParameters()
    {
        var operation = new RestOperationDescriptor(
            "GET", "/search", "search", null, null,
            [
                new RestParameterDescriptor("missing", "query", false, "string", null, null),
                new RestParameterDescriptor("nullValue", "query", false, "string", null, null)
            ],
            null);

        var request = RestRequestFactory.Create(
            operation,
            new Uri("https://example.test/api"),
            new Dictionary<string, string?> { ["nullValue"] = null });

        Assert.Equal("https://example.test/api/search", request.Uri.AbsoluteUri);
    }

    [Fact]
    public void RequestFactoryRejectsNullForRequiredParameter()
    {
        var operation = new RestOperationDescriptor(
            "GET", "/search", "search", null, null,
            [new RestParameterDescriptor("q", "query", true, "string", null, null)],
            null);

        Assert.Throws<ArgumentException>(() => RestRequestFactory.Create(
            operation,
            new Uri("https://example.test/api"),
            new Dictionary<string, string?> { ["q"] = null }));
    }

    [Fact]
    public void RequestFactoryRejectsEmptyPathParameter()
    {
        var operation = new RestOperationDescriptor(
            "GET", "/users/{id}", "getUser", null, null,
            [new RestParameterDescriptor("id", "path", true, "string", null, null)],
            null);

        Assert.Throws<ArgumentException>(() => RestRequestFactory.Create(
            operation,
            new Uri("https://example.test/api"),
            new Dictionary<string, string?> { ["id"] = "" }));
    }

    [Theory]
    [InlineData("query", "q", "query", "q")]
    [InlineData("header", "X-Trace", "header", "x-trace")]
    public void RequestFactoryRejectsDuplicateParameterDescriptors(
        string firstLocation,
        string firstName,
        string secondLocation,
        string secondName)
    {
        var operation = new RestOperationDescriptor(
            "GET", "/search", "search", null, null,
            [
                new RestParameterDescriptor(firstName, firstLocation, false, "string", null, null),
                new RestParameterDescriptor(secondName, secondLocation, false, "string", null, null)
            ],
            null);

        Assert.Throws<ArgumentException>(() => RestRequestFactory.Create(
            operation,
            new Uri("https://example.test/api")));
    }

    [Fact]
    public void RequestFactoryRejectsUnsupportedParameterLocationEvenWhenNoValueWasSupplied()
    {
        var operation = new RestOperationDescriptor(
            "GET", "/search", "search", null, null,
            [new RestParameterDescriptor("q", "matrix", false, "string", null, null)],
            null);

        Assert.Throws<ArgumentException>(() => RestRequestFactory.Create(
            operation,
            new Uri("https://example.test/api")));
    }

}
