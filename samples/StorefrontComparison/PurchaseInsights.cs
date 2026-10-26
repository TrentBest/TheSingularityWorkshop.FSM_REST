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

/// <summary>A measured inventory quantity at a known point in time.</summary>
public sealed record InventoryObservation(DateOnly ObservedOn, decimal QuantityRemaining, string Unit);

/// <summary>
/// A deterministic local projection. Rates are inferred only from measured remaining
/// inventory, not purchase dates alone. Acceleration is unavailable with fewer than
/// three observations. Units for velocity are quantity/day; acceleration is quantity/day².
/// </summary>
public sealed record ConsumptionProjection(
    decimal CurrentQuantity,
    decimal AverageVelocityPerDay,
    decimal? AccelerationPerDaySquared,
    decimal ProjectedConsumption,
    decimal ProjectedQuantityRemaining,
    decimal? EstimatedDaysUntilDepleted);

/// <summary>Arithmetic-only shopping and consumption calculations; no AI or network dependency.</summary>
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

    /// <summary>
    /// Projects inventory from observations of quantity remaining. Two observations
    /// establish a velocity; three establish a rough acceleration from the last two
    /// interval velocities. The model assumes that acceleration continues over the
    /// requested horizon, so projections are estimates, not promises.
    /// </summary>
    public static ConsumptionProjection ProjectConsumption(
        IReadOnlyList<InventoryObservation> observations,
        decimal daysAhead)
    {
        ArgumentNullException.ThrowIfNull(observations);
        if (observations.Count < 2)
            throw new ArgumentException("At least two dated inventory observations are required to estimate velocity.", nameof(observations));
        if (daysAhead < 0)
            throw new ArgumentOutOfRangeException(nameof(daysAhead), "Projection horizon cannot be negative.");

        var points = observations.OrderBy(x => x.ObservedOn).ToArray();
        var unit = points[0].Unit;
        if (string.IsNullOrWhiteSpace(unit) || points.Any(x => !StringComparer.OrdinalIgnoreCase.Equals(x.Unit, unit)))
            throw new ArgumentException("All inventory observations must use the same non-empty unit.", nameof(observations));
        if (points.Any(x => x.QuantityRemaining < 0))
            throw new ArgumentOutOfRangeException(nameof(observations), "Remaining quantity cannot be negative.");
        if (points.Select(x => x.ObservedOn).Distinct().Count() != points.Length)
            throw new ArgumentException("Observation dates must be unique.", nameof(observations));

        var previous = points[^2];
        var latest = points[^1];
        var lastDays = latest.ObservedOn.DayNumber - previous.ObservedOn.DayNumber;
        var velocity = (previous.QuantityRemaining - latest.QuantityRemaining) / lastDays;
        decimal? acceleration = null;

        if (points.Length >= 3)
        {
            var beforePrevious = points[^3];
            var firstDays = previous.ObservedOn.DayNumber - beforePrevious.ObservedOn.DayNumber;
            var earlierVelocity = (beforePrevious.QuantityRemaining - previous.QuantityRemaining) / firstDays;
            var earlierMidpoint = (beforePrevious.ObservedOn.DayNumber + previous.ObservedOn.DayNumber) / 2m;
            var laterMidpoint = (previous.ObservedOn.DayNumber + latest.ObservedOn.DayNumber) / 2m;
            acceleration = (velocity - earlierVelocity) / (laterMidpoint - earlierMidpoint);
        }

        var projectedConsumption = Math.Max(0m, velocity * daysAhead + 0.5m * (acceleration ?? 0m) * daysAhead * daysAhead);
        projectedConsumption = Math.Min(latest.QuantityRemaining, projectedConsumption);
        var remaining = latest.QuantityRemaining - projectedConsumption;
        decimal? depletionDays = null;

        if (latest.QuantityRemaining == 0)
            depletionDays = 0;
        else if (velocity > 0 && acceleration is null)
            depletionDays = latest.QuantityRemaining / velocity;
        else if (acceleration.HasValue)
        {
            // Solve q - v*t - 0.5*a*t² = 0 and keep the smallest non-negative real root.
            var a = acceleration.Value;
            if (a == 0)
            {
                if (velocity > 0)
                    depletionDays = latest.QuantityRemaining / velocity;
            }
            else
            {
                var discriminant = velocity * velocity + 2m * a * latest.QuantityRemaining;
                if (discriminant >= 0)
                {
                    var root = (decimal)Math.Sqrt((double)discriminant);
                    var candidates = new[] { (-velocity + root) / a, (-velocity - root) / a }
                        .Where(x => x >= 0)
                        .ToArray();
                    if (candidates.Length > 0)
                        depletionDays = candidates.Min();
                }
            }
        }

        return new ConsumptionProjection(
            latest.QuantityRemaining, velocity, acceleration,
            projectedConsumption, remaining, depletionDays);
    }
}
