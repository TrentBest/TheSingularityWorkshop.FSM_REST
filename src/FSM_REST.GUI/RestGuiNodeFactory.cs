using TheSingularityWorkshop.Workshop.Gui;

namespace TheSingularityWorkshop.FSM_REST.GUI;

public static class RestGuiNodeFactory
{
    public static GuiNode Create(RestGuiMapping mapping, string nodeId)
    {
        ArgumentNullException.ThrowIfNull(mapping);
        ArgumentException.ThrowIfNullOrWhiteSpace(nodeId);
        var column = GuiBuilders.Column(nodeId + "-content");
        column.Child(GuiBuilders.Text(nodeId + "-title", mapping.Label));
        foreach (var input in mapping.Inputs)
        {
            var field = GuiBuilder.Create("RestInput", nodeId + "-input-" + input.ParameterName)
                .Text(input.Label)
                .Property("parameterName", input.ParameterName)
                .Property("required", input.Required ? "true" : "false");
            if (input.SourcePath is not null) field.Property("sourcePath", input.SourcePath);
            if (input.ValueType is not null) field.Property("valueType", input.ValueType);
            column.Child(field);
        }
        column.Child(GuiBuilders.Button(nodeId + "-execute", mapping.Label)
            .WithOnClick("fsm-rest:" + mapping.OperationId));
        if (mapping.ResultTarget is not null)
            column.Child(GuiBuilders.Text(nodeId + "-result", "Result").Property("resultTarget", mapping.ResultTarget));
        return GuiBuilders.Panel(nodeId)
            .Property("restOperationId", mapping.OperationId)
            .Property("presentation", mapping.Presentation)
            .Child(column).Build();
    }
}
