namespace StorefrontComparison;

public sealed record PurchaseRecord(
    string ProductKey,
    string ProductName,
    DateOnly PurchasedOn,
    decimal PackageQuantity,
    string Unit,
    decimal Paid,
    string Retailer);

public sealed record CurrentOffer(
    string ProductKey,
    string ProductName,
    decimal PackageQuantity,
    string Unit,
    decimal Price,
    string Retailer);

public sealed record PackageComparison(
    decimal HistoricalUnitPrice,
    decimal CurrentUnitPrice,
    decimal UnitPriceDifference,
    decimal UnitPriceSavingsPercent,
    decimal HistoricalPackagePrice,
    decimal CurrentPackagePrice,
    decimal CurrentPackagePremium);

/// <summary>
/// Deterministic, arithmetic-only package comparison. It never claims that a larger
/// package will last longer; consumption rate is unknown unless separately measured.
/// </summary>
public static class PurchaseInsights
{
    public static PackageComparison Compare(PurchaseRecord history, CurrentOffer offer)
    {
        ArgumentNullException.ThrowIfNull(history);
        ArgumentNullException.ThrowIfNull(offer);

        if (!StringComparer.OrdinalIgnoreCase.Equals(history.ProductKey, offer.ProductKey))
            throw new ArgumentException("Historical purchase and current offer must identify the same comparable product.", nameof(offer));
        if (!StringComparer.OrdinalIgnoreCase.Equals(history.Unit, offer.Unit))
            throw new ArgumentException("Package quantities must use the same unit.", nameof(offer));
        if (history.PackageQuantity <= 0 || offer.PackageQuantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(offer), "Package quantities must be positive.");
        if (history.Paid < 0 || offer.Price < 0)
            throw new ArgumentOutOfRangeException(nameof(offer), "Prices cannot be negative.");

        var oldUnitPrice = history.Paid / history.PackageQuantity;
        var newUnitPrice = offer.Price / offer.PackageQuantity;
        var savingsPercent = oldUnitPrice == 0
            ? 0
            : (oldUnitPrice - newUnitPrice) / oldUnitPrice * 100m;

        return new PackageComparison(
            oldUnitPrice,
            newUnitPrice,
            oldUnitPrice - newUnitPrice,
            savingsPercent,
            history.Paid,
            offer.Price,
            offer.Price - history.Paid);
    }
}
