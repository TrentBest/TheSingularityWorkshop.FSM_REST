using TheSingularityWorkshop.FSM_REST;
using Xunit;

namespace TheSingularityWorkshop.FSM_REST.Tests;

public sealed class RestDescriptorImmutabilityTests
{
    [Fact]
    public void ApiDescriptorSnapshotsOperationsAndExposesReadOnlyCollection()
    {
        var original = new List<RestOperationDescriptor> { Operation("first") };
        var api = new RestApiDescriptor("API", "1.0", original);
        original.Add(Operation("second"));

        Assert.Single(api.Operations);
        Assert.Throws<NotSupportedException>(() =>
            ((IList<RestOperationDescriptor>)api.Operations).Add(Operation("third")));
    }

    [Fact]
    public void ApiDescriptorWithExpressionAlsoSnapshotsReplacementCollection()
    {
        var api = new RestApiDescriptor("API", "1.0", [Operation("first")]);
        var replacement = new List<RestOperationDescriptor> { Operation("second") };
        var copy = api with { Operations = replacement };
        replacement.Clear();

        Assert.Single(copy.Operations);
        Assert.Equal("second", copy.Operations[0].OperationId);
    }

    [Fact]
    public void OperationDescriptorSnapshotsParametersAndResponses()
    {
        var parameters = new List<RestParameterDescriptor>
        {
            new("id", "path", true, "string", null, null)
        };
        var responses = new List<RestResponseDescriptor>
        {
            new("200", "Success", new List<string> { "application/json" }, "object", null)
        };
        var operation = new RestOperationDescriptor(
            "GET", "/items/{id}", "getItem", null, null, parameters, null, responses);
        parameters.Clear();
        responses.Clear();

        Assert.Single(operation.Parameters);
        Assert.Single(operation.ResponseDescriptors);
        Assert.Throws<NotSupportedException>(() =>
            ((IList<RestParameterDescriptor>)operation.Parameters).Clear());
        Assert.Throws<NotSupportedException>(() =>
            ((IList<RestResponseDescriptor>)operation.ResponseDescriptors).Clear());
    }

    [Fact]
    public void RequestAndResponseBodyDescriptorsSnapshotMediaTypes()
    {
        var requestMediaTypes = new List<string> { "application/json" };
        var responseMediaTypes = new List<string> { "application/json" };
        var request = new RestRequestBodyDescriptor(true, requestMediaTypes, "object", null);
        var response = new RestResponseDescriptor("200", null, responseMediaTypes, "object", null);
        requestMediaTypes.Add("text/plain");
        responseMediaTypes.Clear();

        Assert.Equal(new[] { "application/json" }, request.MediaTypes);
        Assert.Equal(new[] { "application/json" }, response.MediaTypes);
        Assert.Throws<NotSupportedException>(() =>
            ((IList<string>)request.MediaTypes).Add("text/plain"));
        Assert.Throws<NotSupportedException>(() =>
            ((IList<string>)response.MediaTypes).Clear());
    }


    [Fact]
    public void OperationAndBodyDescriptorWithExpressionsSnapshotReplacementCollections()
    {
        var operation = Operation("getItems");
        var parameters = new List<RestParameterDescriptor>
        {
            new("id", "path", true, "string", null, null)
        };
        var responses = new List<RestResponseDescriptor>
        {
            new("200", "Success", Array.Empty<string>(), null, null)
        };
        var operationCopy = operation with { Parameters = parameters, Responses = responses };
        parameters.Clear();
        responses.Clear();

        Assert.Single(operationCopy.Parameters);
        Assert.Single(operationCopy.ResponseDescriptors);
        Assert.Throws<NotSupportedException>(() =>
            ((IList<RestParameterDescriptor>)operationCopy.Parameters).Clear());

        var mediaTypes = new List<string> { "application/json" };
        var requestBody = new RestRequestBodyDescriptor(true, Array.Empty<string>(), null, null)
            with { MediaTypes = mediaTypes };
        var response = new RestResponseDescriptor("200", null, Array.Empty<string>(), null, null)
            with { MediaTypes = mediaTypes };
        mediaTypes.Add("text/plain");

        Assert.Equal(new[] { "application/json" }, requestBody.MediaTypes);
        Assert.Equal(new[] { "application/json" }, response.MediaTypes);
        Assert.Throws<NotSupportedException>(() =>
            ((IList<string>)requestBody.MediaTypes).Clear());
        Assert.Throws<NotSupportedException>(() =>
            ((IList<string>)response.MediaTypes).Clear());
    }

    [Fact]
    public void DescriptorCollectionsRejectNullAndNullEntries()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new RestApiDescriptor("API", "1.0", null!));
        Assert.Throws<ArgumentException>(() =>
            new RestApiDescriptor("API", "1.0", new RestOperationDescriptor[] { null! }));
        Assert.Throws<ArgumentNullException>(() =>
            new RestOperationDescriptor("GET", "/", "root", null, null, null!, null));
        Assert.Throws<ArgumentException>(() =>
            new RestRequestBodyDescriptor(true, new string[] { null! }, null, null));
        Assert.Throws<ArgumentNullException>(() =>
            new RestResponseDescriptor("200", null, null!, null, null));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void RequestAndResponseDescriptorsRejectBlankMediaTypes(string mediaType)
    {
        Assert.Throws<ArgumentException>(() =>
            new RestRequestBodyDescriptor(true, new[] { mediaType }, null, null));
        Assert.Throws<ArgumentException>(() =>
            new RestResponseDescriptor("200", null, new[] { mediaType }, null, null));
    }


    [Fact]
    public void RequiredDescriptorTextRejectsNullOrWhitespace()
    {
        Assert.Throws<ArgumentException>(() => new RestApiDescriptor("", "1.0", Array.Empty<RestOperationDescriptor>()));
        Assert.Throws<ArgumentException>(() => new RestApiDescriptor("API", " ", Array.Empty<RestOperationDescriptor>()));
        Assert.Throws<ArgumentException>(() => new RestOperationDescriptor("", "/", "root", null, null, Array.Empty<RestParameterDescriptor>(), null));
        Assert.Throws<ArgumentException>(() => new RestOperationDescriptor("GET", " ", "root", null, null, Array.Empty<RestParameterDescriptor>(), null));
        Assert.Throws<ArgumentException>(() => new RestOperationDescriptor("GET", "/", " ", null, null, Array.Empty<RestParameterDescriptor>(), null));
        Assert.Throws<ArgumentException>(() => new RestParameterDescriptor(" ", "query", false, null, null, null));
        Assert.Throws<ArgumentException>(() => new RestParameterDescriptor("id", "", false, null, null, null));
        Assert.Throws<ArgumentException>(() => new RestResponseDescriptor(" ", null, Array.Empty<string>(), null, null));
    }

    [Fact]
    public void RecordWithExpressionsAlsoValidateRequiredText()
    {
        var api = new RestApiDescriptor("API", "1.0", Array.Empty<RestOperationDescriptor>());
        var operation = Operation("getItems");
        var parameter = new RestParameterDescriptor("id", "path", true, "string", null, null);
        var response = new RestResponseDescriptor("200", null, Array.Empty<string>(), null, null);

        Assert.Throws<ArgumentException>(() => api with { Title = " " });
        Assert.Throws<ArgumentException>(() => operation with { Method = "" });
        Assert.Throws<ArgumentException>(() => parameter with { Name = "" });
        Assert.Throws<ArgumentException>(() => response with { StatusCode = " " });
    }

    private static RestOperationDescriptor Operation(string id) =>
        new("GET", "/items", id, null, null, Array.Empty<RestParameterDescriptor>(), null);
}
