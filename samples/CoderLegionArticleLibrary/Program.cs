using System.Text.Json;
using TheSingularityWorkshop.FSM_REST;

const string BaseUrl = "https://coderlegion.com/api/v1/";
var apiKey = Environment.GetEnvironmentVariable("CODERLEGION_API_KEY");
var authorHandle = Environment.GetEnvironmentVariable("CODERLEGION_HANDLE");

if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.Error.WriteLine("Set CODERLEGION_API_KEY to a CoderLegion Personal API Key.");
    return 2;
}

if (args.Length == 0 || args[0] is "help" or "--help" or "-h")
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  dotnet run --project samples/CoderLegionArticleLibrary -- list [max-pages]");
    Console.WriteLine("  dotnet run --project samples/CoderLegionArticleLibrary -- show <post-id>");
    Console.WriteLine("Environment:");
    Console.WriteLine("  CODERLEGION_API_KEY  Required Personal API Key (never pass it on the command line).");
    Console.WriteLine("  CODERLEGION_HANDLE   Optional handle used to filter list results to your posts.");
    return 0;
}

using var client = new HttpClient();
IRestTransport transport = new HttpClientRestTransport(client);
var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
{
    ["X-API-Key"] = apiKey,
    ["Accept"] = "application/json"
};
var baseUri = new Uri(BaseUrl, UriKind.Absolute);

if (args[0] == "list")
{
    var maxPages = 100;
    if (args.Length > 1 &&
        (!int.TryParse(args[1], out maxPages) || maxPages < 1 || maxPages > 1000))
    {
        Console.Error.WriteLine("max-pages must be an integer from 1 through 1000.");
        return 2;
    }

    var operation = new RestOperationDescriptor(
        "GET", "/posts", "listPosts", "List posts",
        "Read a paginated page of CoderLegion posts.",
        [new RestParameterDescriptor("page", "query", false, "integer", null, "Page number.")],
        null,
        [new RestResponseDescriptor("200", "Paginated post collection.", ["application/json"], "array", null)]);

    var matched = 0;
    for (var page = 1; page <= maxPages; page++)
    {
        var response = await RestOperationExecutor.ExecuteAsync(
            operation, baseUri, transport,
            new Dictionary<string, string?> { ["page"] = page.ToString() },
            headers,
            cancellationToken: CancellationToken.None);

        if (!response.IsSuccessStatusCode)
        {
            Console.Error.WriteLine($"GET /posts?page={page} returned HTTP {response.StatusCode}.");
            Console.Error.WriteLine(response.Body);
            return 1;
        }

        using var document = JsonDocument.Parse(response.Body);
        if (!document.RootElement.TryGetProperty("data", out var data) ||
            data.ValueKind != JsonValueKind.Array)
        {
            Console.Error.WriteLine("Unexpected response: expected a JSON 'data' array.");
            return 1;
        }

        var countOnPage = 0;
        foreach (var post in data.EnumerateArray())
        {
            countOnPage++;
            var handle = ReadString(post, "author", "handle");
            if (!string.IsNullOrWhiteSpace(authorHandle) &&
                !string.Equals(handle, authorHandle, StringComparison.OrdinalIgnoreCase))
                continue;

            var id = ReadString(post, "id");
            var title = ReadString(post, "title") ?? "(untitled)";
            var created = ReadString(post, "created_at") ?? "(date unavailable)";
            var description = ReadString(post, "description");
            Console.WriteLine($"[{id ?? "?"}] {title}");
            Console.WriteLine($"    Author: {handle ?? "(unknown)"} | Created: {created}");
            if (!string.IsNullOrWhiteSpace(description))
                Console.WriteLine($"    {description}");
            matched++;
        }

        if (countOnPage == 0 || data.GetArrayLength() == 0)
        {
            Console.WriteLine($"Finished. Displayed {matched} matching post(s).");
            return 0;
        }
    }

    Console.WriteLine($"Stopped at the safety limit of {maxPages} page(s). Displayed {matched} matching post(s).");
    Console.WriteLine("Run again with a larger max-pages value if you need to continue scanning.");
    return 0;
}

if (args[0] == "show" && args.Length == 2 && int.TryParse(args[1], out var postId) && postId > 0)
{
    var operation = new RestOperationDescriptor(
        "GET", "/posts/{id}", "getPost", "Get post",
        "Retrieve full post content by ID.",
        [new RestParameterDescriptor("id", "path", true, "integer", null, "Post ID.")],
        null,
        [new RestResponseDescriptor("200", "Post detail.", ["application/json"], "object", null)]);

    var response = await RestOperationExecutor.ExecuteAsync(
        operation, baseUri, transport,
        new Dictionary<string, string?> { ["id"] = postId.ToString() },
        headers,
        cancellationToken: CancellationToken.None);

    if (!response.IsSuccessStatusCode)
    {
        Console.Error.WriteLine($"GET /posts/{postId} returned HTTP {response.StatusCode}.");
        Console.Error.WriteLine(response.Body);
        return 1;
    }

    using var document = JsonDocument.Parse(response.Body);
    if (!document.RootElement.TryGetProperty("data", out var post) ||
        post.ValueKind != JsonValueKind.Object)
    {
        Console.Error.WriteLine("Unexpected response: expected a JSON 'data' object.");
        return 1;
    }

    Console.WriteLine(ReadString(post, "title") ?? "(untitled)");
    Console.WriteLine($"Author: {ReadString(post, "author", "handle") ?? "(unknown)}");
    Console.WriteLine($"Created: {ReadString(post, "created_at") ?? "(date unavailable)"}");
    Console.WriteLine();
    Console.WriteLine(ReadString(post, "content") ?? "(No content field returned.)");
    return 0;
}

Console.Error.WriteLine("Unknown command. Use 'help' to see supported commands.");
return 2;

static string? ReadString(JsonElement element, params string[] path)
{
    foreach (var segment in path)
    {
        if (segment == "id" && element.ValueKind == JsonValueKind.Object &&
            element.TryGetProperty(segment, out var idValue))
        {
            if (idValue.ValueKind == JsonValueKind.Number)
                return idValue.GetRawText();
            if (idValue.ValueKind == JsonValueKind.String)
                return idValue.GetString();
        }

        if (element.ValueKind != JsonValueKind.Object ||
            !element.TryGetProperty(segment, out element))
            return null;
    }

    return element.ValueKind switch
    {
        JsonValueKind.String => element.GetString(),
        JsonValueKind.Number => element.GetRawText(),
        JsonValueKind.True => "true",
        JsonValueKind.False => "false",
        _ => null
    };
}
