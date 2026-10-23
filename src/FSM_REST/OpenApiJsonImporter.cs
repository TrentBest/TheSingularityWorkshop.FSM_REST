using System.Text.Json;

namespace TheSingularityWorkshop.FSM_REST;

/// <summary>
/// Imports the operation-level subset of an OpenAPI 3 JSON document into neutral FSM_REST descriptors.
/// </summary>
/// <remarks>
/// This importer deliberately does not claim full OpenAPI conformance. It preserves the operation,
/// parameter, request-body, and response metadata represented by FSM_REST. Unsupported constructs
/// and unresolved references are reported in <see cref="OpenApiImportResult.Warnings"/>.
/// </remarks>
public static class OpenApiJsonImporter
{
    private static readonly string[] HttpMethods =
        ["get", "put", "post", "delete", "options", "head", "patch", "trace"];

    /// <summary>Imports an OpenAPI 3.x document encoded as JSON.</summary>
    /// <param name="json">The JSON document.</param>
    /// <returns>The normalized API descriptor and any fidelity warnings.</returns>
    /// <exception cref="ArgumentException">The document is not valid OpenAPI 3 JSON.</exception>
    public static OpenApiImportResult Import(string json)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(json);

        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        if (root.ValueKind != JsonValueKind.Object ||
            !TryGetString(root, "openapi", out var openApiVersion) ||
            !openApiVersion.StartsWith("3.", StringComparison.Ordinal))
        {
            throw new ArgumentException("A valid OpenAPI 3.x JSON document is required.", nameof(json));
        }

        var warnings = new List<string>();
        var title = "Untitled API";
        var version = openApiVersion;
        string? description = null;

        if (root.TryGetProperty("info", out var info) && info.ValueKind == JsonValueKind.Object)
        {
            title = GetString(info, "title") ?? title;
            version = GetString(info, "version") ?? version;
            description = GetString(info, "description");
        }

        if (root.TryGetProperty("security", out var security) &&
            security.ValueKind == JsonValueKind.Array && security.GetArrayLength() > 0)
        {
            warnings.Add("Document-level security requirements were found but are not represented by RestApiDescriptor; configure authentication in the consuming provider/host.");
        }

        var operations = new List<RestOperationDescriptor>();
        if (!root.TryGetProperty("paths", out var paths) || paths.ValueKind != JsonValueKind.Object)
        {
            throw new ArgumentException("The OpenAPI document must contain a paths object.", nameof(json));
        }

        foreach (var pathItem in paths.EnumerateObject())
        {
            var resolvedPathItem = ResolveReference(root, pathItem.Value, $"path '{pathItem.Name}'", warnings);
            if (resolvedPathItem.ValueKind != JsonValueKind.Object)
            {
                warnings.Add($"Path '{pathItem.Name}' was skipped because its path item is not an object.");
                continue;
            }

            var pathParameters = ReadParameters(root, resolvedPathItem, pathItem.Name, warnings);
            foreach (var property in resolvedPathItem.EnumerateObject())
            {
                var method = property.Name.ToLowerInvariant();
                 if (!HttpMethods.Contains(method, StringComparer.Ordinal) ||
                    property.Value.ValueKind != JsonValueKind.Object)
                {
                    continue;
                }

                var operationElement = property.Value;
                var operationId = GetString(operationElement, "operationId") ?? $"{method}_{pathItem.Name}";
                var parameters = new Dictionary<string, RestParameterDescriptor>(StringComparer.Ordinal);
                foreach (var parameter in pathParameters)
                {
                    parameters[ParameterKey(parameter)] = parameter;
                }

                foreach (var parameter in ReadParameters(root, operationElement, $"{method.ToUpperInvariant()} {pathItem.Name}", warnings))
                {
                    // OpenAPI operation-level parameters override path-level parameters with the same name and location.
                    parameters[ParameterKey(parameter)] = parameter;
                }

                var requestBody = ReadRequestBody(root, operationElement, $"{method.ToUpperInvariant()} {pathItem.Name}", warnings);
                var responses = ReadResponses(root, operationElement, $"{method.ToUpperInvariant()} {pathItem.Name}", warnings);

                if (operationElement.TryGetProperty("security", out var operationSecurity) &&
                    operationSecurity.ValueKind == JsonValueKind.Array && operationSecurity.GetArrayLength() > 0)
                {
                    warnings.Add($"Operation '{operationId}' has security requirements that are not represented in the neutral descriptor; configure authentication in the consuming provider/host.");
                }

                operations.Add(new RestOperationDescriptor(
                    method.ToUpperInvariant(),
                    pathItem.Name,
                    operationId,
                    GetString(operationElement, "summary"),
                    GetString(operationElement, "description"),
                    parameters.Values.ToArray(),
                    requestBody,
                    responses));
            }
        }

        return new OpenApiImportResult(
            new RestApiDescriptor(title, version, operations, description),
            warnings.AsReadOnly());
    }

    private static List<RestParameterDescriptor> ReadParameters(
        JsonElement root, JsonElement owner, string context, List<string> warnings)
    {
        var result = new List<RestParameterDescriptor>();
        if (!owner.TryGetProperty("parameters", out var parameters))
        {
            return result;
        }

        if (parameters.ValueKind != JsonValueKind.Array)
        {
            warnings.Add($"Parameters for '{context}' were skipped because they are not an array.");
            return result;
        }

        foreach (var parameter in parameters.EnumerateArray())
        {
            if (parameter.ValueKind != JsonValueKind.Object)
            {
                warnings.Add($"A parameter in '{context}' was skipped because it is not an object.");
                continue;
            }

            parameter = ResolveReference(root, parameter, $"parameter in '{context}'", warnings);
            if (parameter.ValueKind != JsonValueKind.Object) continue;

            var name = GetString(parameter, "name");
            var location = GetString(parameter, "in");
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(location))
            {
                warnings.Add($"A parameter in '{context}' was skipped because name or in is missing.");
                continue;
            }

            var schema = parameter.TryGetProperty("schema", out var schemaElement) ? schemaElement : default;
            if (schema.ValueKind == JsonValueKind.Undefined &&
                parameter.TryGetProperty("content", out var parameterContent) &&
                parameterContent.ValueKind == JsonValueKind.Object)
            {
                warnings.Add($"Parameter '{name}' in '{context}' uses content-based serialization, which is not represented; FSM_REST currently binds scalar values.");
            }
            schema = ResolveReference(root, schema, $"{context} parameter '{name}' schema", warnings);
            ReportSchemaReference(schema, $"{context} parameter '{name}'", warnings);
            result.Add(new RestParameterDescriptor(
                name,
                location,
                GetBoolean(parameter, "required") || location == "path",
                GetString(schema, "type"),
                GetString(schema, "format"),
                GetString(parameter, "description")));

            if (GetString(parameter, "style") is not null || parameter.TryGetProperty("explode", out _))
            {
                warnings.Add($"Parameter serialization style for '{name}' in '{context}' is not represented; FSM_REST currently binds scalar values.");
            }
        }

        return result;
    }

    private static RestRequestBodyDescriptor? ReadRequestBody(
        JsonElement root, JsonElement operation, string context, List<string> warnings)
    {
        if (!operation.TryGetProperty("requestBody", out var body))
        {
            return null;
        }

        if (body.ValueKind != JsonValueKind.Object)
        {
            warnings.Add($"Request body for '{context}' was skipped because it is not an object.");
            return null;
        }

        body = ResolveReference(root, body, $"request body for '{context}'", warnings);
        if (body.ValueKind != JsonValueKind.Object) return null;

        var mediaTypes = new List<string>();
        string? schemaType = null;
        string? schemaFormat = null;
        if (body.TryGetProperty("content", out var content) && content.ValueKind == JsonValueKind.Object)
        {
            foreach (var media in content.EnumerateObject())
            {
                mediaTypes.Add(media.Name);
                if (media.Value.ValueKind == JsonValueKind.Object &&
                    media.Value.TryGetProperty("schema", out var schema))
                {
                    schema = ResolveReference(root, schema, $"{context} request-body schema", warnings);
                    ReportSchemaReference(schema, $"{context} request body", warnings);
                    schemaType ??= GetString(schema, "type");
                    schemaFormat ??= GetString(schema, "format");
                }
            }
        }

        return new RestRequestBodyDescriptor(GetBoolean(body, "required"), mediaTypes, schemaType, schemaFormat);
    }

    private static IReadOnlyList<RestResponseDescriptor> ReadResponses(
        JsonElement root, JsonElement operation, string context, List<string> warnings)
    {
        var responses = new List<RestResponseDescriptor>();
        if (!operation.TryGetProperty("responses", out var responseObject) ||
            responseObject.ValueKind != JsonValueKind.Object)
        {
            return responses;
        }

        foreach (var response in responseObject.EnumerateObject())
        {
            if (response.Value.ValueKind != JsonValueKind.Object)
            {
                warnings.Add($"Response '{response.Name}' for '{context}' was skipped because it is not an object.");
                continue;
            }

            var responseValue = ResolveReference(root, response.Value, $"response '{response.Name}' for '{context}'", warnings);
            if (responseValue.ValueKind != JsonValueKind.Object) continue;

            var mediaTypes = new List<string>();
            string? schemaType = null;
            string? schemaFormat = null;
            if (responseValue.TryGetProperty("content", out var content) && content.ValueKind == JsonValueKind.Object)
            {
                foreach (var media in content.EnumerateObject())
                {
                    mediaTypes.Add(media.Name);
                    if (media.Value.ValueKind == JsonValueKind.Object &&
                        media.Value.TryGetProperty("schema", out var schema))
                    {
                        schema = ResolveReference(root, schema, $"{context} response '{response.Name}' schema", warnings);
                        ReportSchemaReference(schema, $"{context} response '{response.Name}'", warnings);
                        schemaType ??= GetString(schema, "type");
                        schemaFormat ??= GetString(schema, "format");
                    }
                }
            }

            responses.Add(new RestResponseDescriptor(
                response.Name,
                GetString(responseValue, "description"),
                mediaTypes,
                schemaType,
                schemaFormat));
        }

        return responses.AsReadOnly();
    }

    private static JsonElement ResolveReference(
        JsonElement root, JsonElement element, string context, List<string> warnings)
    {
        var visited = new HashSet<string>(StringComparer.Ordinal);
        while (element.ValueKind == JsonValueKind.Object &&
               element.TryGetProperty("$ref", out var referenceElement))
        {
            var reference = referenceElement.ValueKind == JsonValueKind.String ? referenceElement.GetString() : null;
            if (string.IsNullOrWhiteSpace(reference) || !reference.StartsWith("#/", StringComparison.Ordinal))
            {
                warnings.Add($"Reference in {context} is external or malformed and could not be resolved: '{reference ?? "(missing)"}'.");
                return default;
            }
            if (!visited.Add(reference))
            {
                warnings.Add($"Reference cycle in {context} could not be resolved: '{reference}'.");
                return default;
            }

            var target = root;
            var found = true;
            foreach (var rawSegment in reference[2..].Split('/'))
            {
                var segment = rawSegment.Replace("~1", "/", StringComparison.Ordinal).Replace("~0", "~", StringComparison.Ordinal);
                if (target.ValueKind != JsonValueKind.Object || !target.TryGetProperty(segment, out target))
                {
                    found = false;
                    break;
                }
            }
            if (!found)
            {
                warnings.Add($"{(context.Contains("parameter", StringComparison.OrdinalIgnoreCase) ? "A parameter reference" : context.Contains("schema", StringComparison.OrdinalIgnoreCase) ? "Schema reference" : "A reference")} in {context} points to a missing local component: '{reference}'.");
                return default;
            }
            element = target;
        }
        return element;
    }

    private static void ReportSchemaReference(JsonElement schema, string context, List<string> warnings)
    {
        if (schema.ValueKind == JsonValueKind.Object && schema.TryGetProperty("$ref", out _))
            warnings.Add($"Schema reference in {context} was not resolved; schema composition and validation are outside this importer.");
    }

    private static string ParameterKey(RestParameterDescriptor parameter)
    {
        // Header names are case-insensitive; other OpenAPI parameter names retain their spelling.
        var location = parameter.Location.ToLowerInvariant();
        var name = location == "header" ? parameter.Name.ToLowerInvariant() : parameter.Name;
        return $"{location}:{name}";
    }

    private static string? GetString(JsonElement element, string propertyName) =>
        TryGetString(element, propertyName, out var value) ? value : null;

    private static bool TryGetString(JsonElement element, string propertyName, out string value)
    {
        value = string.Empty;
        if (element.ValueKind != JsonValueKind.Object ||
            !element.TryGetProperty(propertyName, out var property) ||
            property.ValueKind != JsonValueKind.String)
        {
            return false;
        }

        value = property.GetString() ?? string.Empty;
        return true;
    }

    private static bool GetBoolean(JsonElement element, string propertyName) =>
        element.ValueKind == JsonValueKind.Object &&
        element.TryGetProperty(propertyName, out var property) &&
        property.ValueKind == JsonValueKind.True;
}

/// <summary>Result of importing an OpenAPI JSON document.</summary>
/// <param name="Api">The normalized REST API descriptor.</param>
/// <param name="Warnings">Loss-of-fidelity and unsupported-feature notices.</param>
public sealed record OpenApiImportResult(
    RestApiDescriptor Api,
    IReadOnlyList<string> Warnings);
