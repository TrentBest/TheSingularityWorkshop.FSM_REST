using TheSingularityWorkshop.FSM_REST;
using Xunit;

namespace TheSingularityWorkshop.FSM_REST.Tests;

public sealed class RestGuiMappingTests
{
    private static RestOperationDescriptor CreateOperation() => new(
        Method: "GET",
        Path: "/items/{id}",
        OperationId: "getItem",
        Summary: "Get item",
        Description: "Loads one item.",
        Parameters:
        [
            new RestParameterDescriptor("id", "path", true, "string", null, "Item identity"),
            new RestParameterDescriptor("locale", "query", false, "string", null, "Preferred locale")
        ],
        RequestBody: null);

    [Fact]
    public void MapsGuiInputsToOperationParametersAndApplicationDataPaths()
    {
        var mapping = new RestGuiMapping(
            CreateOperation(),
            label: "Open item",
            presentation: "detail",
            inputs:
            [
                new RestGuiInputMapping("id", "Item ID", "text", "selection.itemId", Required: true),
                new RestGuiInputMapping("locale", "Language", "choice", Required: false)
            ],
            resultTarget: "views.selectedItem",
            showLoadingState: true,
            showErrorState: true);

        Assert.Equal("getItem", mapping.OperationId);
        Assert.Equal("Open item", mapping.Label);
        Assert.Equal("detail", mapping.Presentation);
        Assert.Equal("selection.itemId", mapping.Inputs[0].SourcePath);
        Assert.Null(mapping.Inputs[1].SourcePath);
        Assert.Equal("views.selectedItem", mapping.ResultTarget);
        Assert.True(mapping.ShowLoadingState);
        Assert.True(mapping.ShowErrorState);
    }

    [Fact]
    public void RejectsInputThatDoesNotBelongToOperation()
    {
        Assert.Throws<ArgumentException>(() => new RestGuiMapping(
            CreateOperation(),
            "Invalid",
            "form",
            [new RestGuiInputMapping("missing", "Missing input")]));
    }

    [Fact]
    public void RejectsDuplicateInputMappings()
    {
        Assert.Throws<ArgumentException>(() => new RestGuiMapping(
            CreateOperation(),
            "Invalid",
            "form",
            [
                new RestGuiInputMapping("id", "First"),
                new RestGuiInputMapping("id", "Second")
            ]));
    }

    [Fact]
    public void RejectsEmptySourcePath()
    {
        Assert.Throws<ArgumentException>(() => new RestGuiMapping(
            CreateOperation(),
            "Invalid",
            "form",
            [new RestGuiInputMapping("id", "Item ID", SourcePath: " ")]));
    }
}
