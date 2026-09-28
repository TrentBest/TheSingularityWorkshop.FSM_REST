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
