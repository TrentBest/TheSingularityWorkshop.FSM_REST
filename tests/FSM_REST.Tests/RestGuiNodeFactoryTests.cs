using TheSingularityWorkshop.FSM_REST;
using TheSingularityWorkshop.FSM_REST.GUI;
using Xunit;

namespace FSM_REST.Tests;

public sealed class RestGuiNodeFactoryTests
{
    [Fact]
    public void Create_MapsInputsAndStableCommandIntoGuiTree()
    {
        var operation = new RestOperationDescriptor(
            "GET", "/people/{id}", "getPerson", "Get person", null,
            new[]
            {
                new RestParameterDescriptor("id", "path", true, "string", null, "Person identifier")
            },
            null);
        var mapping = new RestGuiMapping(
            operation,
            "Load person",
            "form",
            new[] { new RestGuiInputMapping("id", "Person ID", "text", "selection.personId", true) },
            "views.person");

        var tree = RestGuiNodeFactory.Create(mapping, "person-form");

        Assert.Equal("getPerson", tree.Properties["restOperationId"]);
        Assert.Equal("form", tree.Properties["presentation"]);
        var input = tree.Find("person-form-input-id");
        Assert.Equal("id", input.Properties["parameterName"]);
        Assert.Equal("true", input.Properties["required"]);
        Assert.Equal("selection.personId", input.Properties["sourcePath"]);
        Assert.Equal("fsm-rest:getPerson", tree.Find("person-form-execute").Properties["onClickCommand"]);
        Assert.Equal("views.person", tree.Find("person-form-result").Properties["resultTarget"]);
    }

    [Fact]
    public void GetCommandId_RejectsBlankOperation()
    {
        Assert.Throws<ArgumentException>(() => RestGuiNodeFactory.GetCommandId(" "));
    }
}
