using System.Text.Json;

namespace TheSingularityWorkshop.FSM_REST;

public static class OpenApiRestReflector
{
    private static readonly string[] HttpMethods =
    [
        "get", "put", "post", "delete", "options", "head", "patch", "trace"
    ];

    public static RestApiDescriptor Reflect(string document)
    {
        if (string.IsNullOrWhiteSpace(document))
            throw new ArgumentException("An OpenAPI document is required.", nameof(document));

        using var json = JsonDocument.Parse(document);
        var root = json.RootElement;

        if (!root.TryGetProperty("openapi", out var versionElement) ||
            !versionElement.GetString()?.StartsWith("3.", StringComparison.Ordinal) == true)
            throw new ArgumentException("FSM_REST currently reflects OpenAPI 3.x documents.", nameof(document));

        var info = root.TryGetProperty("info", out var infoElement) ? infoElement : default;
        var title = GetString(info, "title") ?? "REST API";
        var version = GetString(info, "version") ?? "unknown";
        var description = GetString(info, "description");
        var operations = new List<RestOperationDescriptor>();

        if (root.TryGetProperty("paths", out var paths) && paths.ValueKind == JsonValueKind.Object)
        {
            foreach (var path in paths.EnumerateObject())
            {
                foreach (var method in HttpMethods)
                {
                    if (!path.Value.TryGetProperty(method, out var operation) ||
                        operation.ValueKind != JsonValueKind.Object ||
                        operation.TryGetProperty("$ref", out _))
                        continue;

                    var operationId = GetString(operation, "operationId") ?? $"{method}:{path.Name}";
                    operations.Add(new RestOperationDescriptor(
                        method.ToUpperInvariant(),
                        path.Name,
                        operationId,
                        GetString(operation, "summary"),
                        GetString(operation, "description"),
                        ReadParameters(path.Value, operation),
                        ReadRequestBody(operation)));
                }
            }
        }

        return new RestApiDescriptor(title, version, operations, description);
    }

    private static IReadOnlyList<RestParameterDescriptor> ReadParameters(JsonElement path, JsonElement operation)
    {
        var parameters = new Dictionary<string, RestParameterDescriptor>(StringComparer.Ordinal);
        ReadParameterArray(path, parameters);
        ReadParameterArray(operation, parameters);
        return parameters.Values.ToArray();
    }

    private static void ReadParameterArray(JsonElement owner, IDictionary<string, RestParameterDescriptor> target)
    {
        if (!owner.TryGetProperty("parameters", out var parameters) ||
            parameters.ValueKind != JsonValueKind.Array)
            return;

        foreach (var parameter in parameters.EnumerateArray())
        {
            if (parameter.ValueKind != JsonValueKind.Object ||
                parameter.TryGetProperty("$ref", out _))
                continue;

            var name = GetString(parameter, "name");
            var location = GetString(parameter, "in");
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(location))
                continue;

            string? type = null;
            string? format = null;
            if (parameter.TryGetProperty("schema", out var schema) &&
                schema.ValueKind == JsonValueKind.Object)
            {
                type = GetString(schema, "type");
                format = GetString(schema, "format");
            }

            target[$"{location}:{name}"] = new RestParameterDescriptor(
                name,
                location,
                GetBoolean(parameter, "required"),
                type,
                format,
                GetString(parameter, "description"));
        }
    }

    private static RestRequestBodyDescriptor? ReadRequestBody(JsonElement operation)
    {
        if (!operation.TryGetProperty("requestBody", out var body) ||
            body.ValueKind != JsonValueKind.Object ||
            body.TryGetProperty("$ref", out _))
            return null;

        var mediaTypes = new List<string>();
        string? schemaType = null;
        string? schemaFormat = null;

        if (body.TryGetProperty("content", out var content) &&
            content.ValueKind == JsonValueKind.Object)
        {
            foreach (var media in content.EnumerateObject())
            {
                mediaTypes.Add(media.Name);

                if (schemaType is null &&
                    media.Value.TryGetProperty("schema", out var schema) &&
                    schema.ValueKind == JsonValueKind.Object)
                {
                    schemaType = GetString(schema, "type");
                    schemaFormat = GetString(schema, "format");
                }
            }
        }

        return new RestRequestBodyDescriptor(
            GetBoolean(body, "required"),
            mediaTypes,
            schemaType,
            schemaFormat);
    }

    private static string? GetString(JsonElement element, string propertyName) =>
        element.ValueKind == JsonValueKind.Object &&
        element.TryGetProperty(propertyName, out var property) &&
        property.ValueKind == JsonValueKind.String
            ? property.GetString()
            : null;

    private static bool GetBoolean(JsonElement element, string propertyName) =>
        element.ValueKind == JsonValueKind.Object &&
        element.TryGetProperty(propertyName, out var property) &&
        property.ValueKind == JsonValueKind.True;
}
