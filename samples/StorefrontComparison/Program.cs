using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using StorefrontComparison;
using TheSingularityWorkshop.FSM_REST;

var liveFredMeyer = args.Length > 0 && args[0] == "--live-fred-meyer";
var term = liveFredMeyer && args.Length > 1 ? args[1] : "milk";
using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(20) };
IRestTransport transport = new HttpClientRestTransport(http);

IStorefrontProvider walmart = new WalmartDemoProvider(new DemoTransport());
IStorefrontProvider fredMeyer = liveFredMeyer
    ? new FredMeyerKrogerProvider(transport)
    : new FredMeyerDemoProvider(new DemoTransport());

Console.WriteLine("Storefront Composition proof: one workflow, two providers");
Console.WriteLine("Default mode is deterministic and makes no network calls.");
Console.WriteLine();

foreach (var provider in new[] { walmart, fredMeyer })
{
    Console.WriteLine("[" + provider.Name + "] search: " + term);
    try
    {
        var products = await provider.SearchProductsAsync(term);
        foreach (var product in products.Take(5))
            Console.WriteLine("  " + product.Name + " | " + product.PriceLabel + " | id=" + product.ProviderProductId);

        var first = products.FirstOrDefault();
        if (first is not null)
        {
            var preview = new ShoppingCart(first, 1);
            Console.WriteLine("  Local order preview only: " + preview.Quantity + " x " + first.Name + " = " + preview.TotalLabel);
        }
        else Console.WriteLine("  No products returned.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("  Provider call failed: " + ex.Message);
    }
    Console.WriteLine();
}
Console.WriteLine("Purchase-history insight (illustrative fixture; not a live offer):");
var pastPurchase = new PurchaseRecord(
    "demo-milk", "Milk", new DateOnly(2026, 7, 26), 14.5m, "oz", 2.49m, "Example Store");
var largerOffer = new CurrentOffer(
    "demo-milk", "Milk", 32m, "oz", 4.49m, "Example Store");
var insight = PurchaseInsights.Compare(pastPurchase, largerOffer);
Console.WriteLine($"  Previous purchase: {pastPurchase.PurchasedOn:MMM d, yyyy} — {pastPurchase.PackageQuantity} {pastPurchase.Unit} for {pastPurchase.Paid:C}.");
Console.WriteLine($"  Larger package today: {largerOffer.PackageQuantity} {largerOffer.Unit} for {largerOffer.Price:C} ({largerOffer.Retailer}).");
Console.WriteLine($"  Unit price: {insight.HistoricalUnitPrice:C3}/{pastPurchase.Unit} then vs {insight.CurrentUnitPrice:C3}/{largerOffer.Unit} now.");
if (insight.UnitPriceDifference > 0)
    Console.WriteLine($"  Unit-price improvement: {insight.UnitPriceDifference:C3}/{pastPurchase.Unit} ({insight.UnitPriceSavingsPercent:F1}%).");
else if (insight.UnitPriceDifference < 0)
    Console.WriteLine($"  Unit-price increase: {Math.Abs(insight.UnitPriceDifference):C3}/{pastPurchase.Unit} ({Math.Abs(insight.UnitPriceSavingsPercent):F1}%).");
else
    Console.WriteLine("  Unit price is unchanged.");
Console.WriteLine($"  Upfront package difference: {insight.CurrentPackagePremium:C}.");
Console.WriteLine("  Not buying again yet: not estimated here; that requires a consumption-rate estimate from the shopper's own history.");
Console.WriteLine();
Console.WriteLine("No checkout, payment, cart write, or real order submission is performed.");

public sealed record StoreProduct(string Provider, string ProviderProductId, string Name, decimal? Price)
{
    public string PriceLabel => Price.HasValue ? Price.Value.ToString("C") : "price unavailable";
}
public sealed record ShoppingCart(StoreProduct Product, int Quantity)
{
    public string TotalLabel => Product.Price.HasValue
        ? (Product.Price.Value * Quantity).ToString("C")
        : "total unavailable (no price returned)";
}
public interface IStorefrontProvider
{
    string Name { get; }
    Task<IReadOnlyList<StoreProduct>> SearchProductsAsync(string term, CancellationToken cancellationToken = default);
}

// Default test transport: fixed responses, no network.
public sealed class DemoTransport : IRestTransport
{
    public Task<RestResponse> SendAsync(RestRequest request, CancellationToken cancellationToken = default)
    {
        var isKroger = request.Uri.Host.Contains("kroger", StringComparison.OrdinalIgnoreCase);
        var body = isKroger
            ? """{"data":[{"productId":"fred-demo-001","description":"Fred Meyer Demo Milk","items":[{"price":{"regular":4.29}}]}]}"""
            : """{"products":[{"id":"walmart-demo-001","name":"Walmart Demo Milk","price":3.98}]}""";
        return Task.FromResult(new RestResponse(200, "OK",
            new Dictionary<string, string> { ["content-type"] = "application/json" }, body));
    }
}
public sealed class WalmartDemoProvider(IRestTransport transport) : IStorefrontProvider
{
    public string Name => "Walmart demo (live shopper OPD requires approved partner access)";
    public async Task<IReadOnlyList<StoreProduct>> SearchProductsAsync(string term, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("GET", new Uri("https://walmart-demo.invalid/products?q=" + Uri.EscapeDataString(term)));
        var response = await transport.SendAsync(request, cancellationToken);
        EnsureSuccess(response);
        using var json = JsonDocument.Parse(response.Body);
        return json.RootElement.GetProperty("products").EnumerateArray().Select(p =>
            new StoreProduct("Walmart", p.GetProperty("id").GetString() ?? "",
                p.GetProperty("name").GetString() ?? "",
                p.TryGetProperty("price", out var price) && price.ValueKind == JsonValueKind.Number ? price.GetDecimal() : null)).ToArray();
    }
    private static void EnsureSuccess(RestResponse response)
    {
        if (!response.IsSuccessStatusCode) throw new HttpRequestException("HTTP " + response.StatusCode);
    }
}
public sealed class FredMeyerDemoProvider(IRestTransport transport) : IStorefrontProvider
{
    public string Name => "Fred Meyer demo (Kroger API shape)";
    public async Task<IReadOnlyList<StoreProduct>> SearchProductsAsync(string term, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("GET", new Uri("https://api.kroger.com/v1/products?filter.term=" + Uri.EscapeDataString(term)));
        var response = await transport.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode) throw new HttpRequestException("HTTP " + response.StatusCode);
        using var json = JsonDocument.Parse(response.Body);
        return ParseKrogerProducts(json.RootElement);
    }
    internal static IReadOnlyList<StoreProduct> ParseKrogerProducts(JsonElement root) =>
        root.GetProperty("data").EnumerateArray().Select(p =>
        {
            decimal? price = null;
            if (p.TryGetProperty("items", out var items) && items.GetArrayLength() > 0 &&
                items[0].TryGetProperty("price", out var priceNode) &&
                priceNode.TryGetProperty("regular", out var regular) && regular.ValueKind == JsonValueKind.Number)
                price = regular.GetDecimal();
            return new StoreProduct("Fred Meyer", p.GetProperty("productId").GetString() ?? "",
                p.GetProperty("description").GetString() ?? "", price);
        }).ToArray();
}
public sealed class FredMeyerKrogerProvider(IRestTransport transport) : IStorefrontProvider
{
    public string Name => "Fred Meyer live Kroger product search (read-only)";
    public async Task<IReadOnlyList<StoreProduct>> SearchProductsAsync(string term, CancellationToken cancellationToken = default)
    {
        var id = Environment.GetEnvironmentVariable("KROGER_CLIENT_ID");
        var secret = Environment.GetEnvironmentVariable("KROGER_CLIENT_SECRET");
        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(secret))
            throw new InvalidOperationException("Set KROGER_CLIENT_ID and KROGER_CLIENT_SECRET from a registered Kroger developer app.");

        var basic = Convert.ToBase64String(Encoding.UTF8.GetBytes(id + ":" + secret));
        var tokenRequest = new RestRequest("POST", new Uri("https://api.kroger.com/v1/connect/oauth2/token"),
            new Dictionary<string, string> { ["Authorization"] = "Basic " + basic },
            "grant_type=client_credentials&scope=product.compact", "application/x-www-form-urlencoded");
        var tokenResponse = await transport.SendAsync(tokenRequest, cancellationToken);
        if (!tokenResponse.IsSuccessStatusCode) throw new HttpRequestException("Kroger token HTTP " + tokenResponse.StatusCode);
        using var tokenJson = JsonDocument.Parse(tokenResponse.Body);
        var token = tokenJson.RootElement.GetProperty("access_token").GetString();
        if (string.IsNullOrWhiteSpace(token)) throw new InvalidOperationException("Kroger returned no access token.");

        var url = "https://api.kroger.com/v1/products?filter.term=" + Uri.EscapeDataString(term) + "&filter.limit=10";
        var productsResponse = await transport.SendAsync(new RestRequest("GET", new Uri(url),
            new Dictionary<string, string> { ["Authorization"] = "Bearer " + token, ["Accept"] = "application/json" }), cancellationToken);
        if (!productsResponse.IsSuccessStatusCode) throw new HttpRequestException("Kroger products HTTP " + productsResponse.StatusCode);
        using var productsJson = JsonDocument.Parse(productsResponse.Body);
        return FredMeyerDemoProvider.ParseKrogerProducts(productsJson.RootElement);
    }
}
