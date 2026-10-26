using StorefrontComparison;
using Xunit;

namespace FSM_REST.Tests;

public sealed class PurchaseInsightsTests
{
    [Fact]
    public void ProjectConsumption_TwoObservationsEstimateVelocityWithoutAcceleration()
    {
        var observations = new[]
        {
            new InventoryObservation(new DateOnly(2026, 1, 1), 12m, "oz"),
            new InventoryObservation(new DateOnly(2026, 1, 5), 8m, "oz")
        };

        var result = PurchaseInsights.ProjectConsumption(observations, 2m);

        Assert.Equal(1m, result.AverageVelocityPerDay);
        Assert.Null(result.AccelerationPerDaySquared);
        Assert.Equal(2m, result.ProjectedConsumption);
        Assert.Equal(6m, result.ProjectedQuantityRemaining);
        Assert.Equal(8m, result.EstimatedDaysUntilDepleted);
    }

    [Fact]
    public void ProjectConsumption_ThreeObservationsEstimateAcceleration()
    {
        var observations = new[]
        {
            new InventoryObservation(new DateOnly(2026, 1, 1), 32m, "oz"),
            new InventoryObservation(new DateOnly(2026, 1, 8), 24m, "oz"),
            new InventoryObservation(new DateOnly(2026, 1, 15), 14m, "oz")
        };

        var result = PurchaseInsights.ProjectConsumption(observations, 7m);

        Assert.Equal(10m / 7m, result.AverageVelocityPerDay, 8);
        Assert.Equal(2m / 49m, result.AccelerationPerDaySquared!.Value, 8);
        Assert.Equal(11m, result.ProjectedConsumption, 8);
        Assert.Equal(3m, result.ProjectedQuantityRemaining, 8);
        Assert.NotNull(result.EstimatedDaysUntilDepleted);
    }

    [Fact]
    public void ProjectConsumption_RequiresAtLeastTwoObservations()
    {
        var observations = new[]
        {
            new InventoryObservation(new DateOnly(2026, 1, 1), 12m, "oz")
        };

        Assert.Throws<ArgumentException>(() => PurchaseInsights.ProjectConsumption(observations, 1m));
    }

    [Fact]
    public void ProjectConsumption_RejectsMixedUnitsAndDuplicateDates()
    {
        var mixedUnits = new[]
        {
            new InventoryObservation(new DateOnly(2026, 1, 1), 12m, "oz"),
            new InventoryObservation(new DateOnly(2026, 1, 2), 11m, "g")
        };
        var duplicateDates = new[]
        {
            new InventoryObservation(new DateOnly(2026, 1, 1), 12m, "oz"),
            new InventoryObservation(new DateOnly(2026, 1, 1), 11m, "oz")
        };

        Assert.Throws<ArgumentException>(() => PurchaseInsights.ProjectConsumption(mixedUnits, 1m));
        Assert.Throws<ArgumentException>(() => PurchaseInsights.ProjectConsumption(duplicateDates, 1m));
    }

    [Fact]
    public void Compare_ReportsUnitPriceAndUpfrontPackageDifference()
    {
        var history = new PurchaseRecord("milk", "Milk", new DateOnly(2026, 1, 1), 14.5m, "oz", 2.49m, "Store A");
        var offer = new CurrentOffer("milk", "Milk", 32m, "oz", 4.49m, "Store B");

        var result = PurchaseInsights.Compare(history, offer);

        Assert.Equal(2.49m / 14.5m, result.HistoricalUnitPrice);
        Assert.Equal(4.49m / 32m, result.CurrentUnitPrice);
        Assert.Equal(2m, result.CurrentPackagePremium);
        Assert.True(result.UnitPriceDifference > 0);
    }
}
