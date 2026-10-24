using TheSingularityWorkshop.FSM_REST;
using Xunit;

namespace TheSingularityWorkshop.FSM_REST.Tests;

public sealed class RestRequestHeaderValidationTests
{
    [Theory]
    [InlineData("Bad Header", "value")]
    [InlineData("X-Trace", "safe\r\nInjected: yes")]
    public void RequestFactoryRejectsMalformedCallerHeaders(string name, string value)
    {
        var operation = new RestOperationDescriptor(
            "GET", "/items", "listItems", null, null,
            Array.Empty<RestParameterDescriptor>(), null);

        Assert.Throws<ArgumentException>(() => RestRequestFactory.Create(
            operation,
            new Uri("https://example.test/api"),
            headers: new Dictionary<string, string> { [name] = value }));
    }

    [Fact]
    public void RequestFactoryRejectsHeaderParameterWithLineBreak()
    {
        var operation = new RestOperationDescriptor(
            "GET", "/items", "listItems", null, null,
            [new RestParameterDescriptor("X-Trace", "header", true, "string", null, null)],
            null);

        Assert.Throws<ArgumentException>(() => RestRequestFactory.Create(
            operation,
            new Uri("https://example.test/api"),
            new Dictionary<string, string?> { ["X-Trace"] = "safe\r\nInjected: yes" }));
    }

    [Fact]
    public void RequestFactoryRejectsInvalidCookieParameterName()
    {
        var operation = new RestOperationDescriptor(
            "GET", "/items", "listItems", null, null,
            [new RestParameterDescriptor("session; admin", "cookie", true, "string", null, null)],
            null);

        Assert.Throws<ArgumentException>(() => RestRequestFactory.Create(
            operation,
            new Uri("https://example.test/api"),
            new Dictionary<string, string?> { ["session; admin"] = "true" }));
    }

    [Fact]
    public void RequestFactoryRejectsPathParameterDeclarationMissingFromTemplate()
    {
        var operation = new RestOperationDescriptor(
            "GET", "/items", "getItem", null, null,
            [new RestParameterDescriptor("id", "path", true, "string", null, null)],
            null);

        var exception = Assert.Throws<ArgumentException>(() => RestRequestFactory.Create(
            operation,
            new Uri("https://example.test/api"),
            new Dictionary<string, string?> { ["id"] = "42" }));

        Assert.Contains("do not appear in the operation path", exception.Message);
    }

    [Fact]
    public void RequestFactoryRejectsUndeclaredPathTemplateParameter()
    {
        var operation = new RestOperationDescriptor(
            "GET", "/items/{id}", "getItem", null, null,
            Array.Empty<RestParameterDescriptor>(),
            null);

        var exception = Assert.Throws<ArgumentException>(() => RestRequestFactory.Create(
            operation,
            new Uri("https://example.test/api")));

        Assert.Contains("undeclared path parameter", exception.Message);
    }

    [Fact]
    public void RequestFactoryAcceptsOrdinaryHeadersAndTabs()
    {
        var operation = new RestOperationDescriptor(
            "GET", "/items", "listItems", null, null,
            [new RestParameterDescriptor("X-Trace", "header", true, "string", null, null)],
            null);

        var request = RestRequestFactory.Create(
            operation,
            new Uri("https://example.test/api"),
            new Dictionary<string, string?> { ["X-Trace"] = "trace-123" },
            new Dictionary<string, string> { ["X-Note"] = "part one\tpart two" });

        Assert.Equal("trace-123", request.Headers!["X-Trace"]);
        Assert.Equal("part one\tpart two", request.Headers["X-Note"]);
    }
}
