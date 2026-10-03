using Core.Models.Calculations;
using Data.Services;

namespace Test;

public class CalculationComparisonTest
{
    [Theory]
    [InlineData(10, 15, 5, 50d)]
    [InlineData(10, 5, -5, -50d)]
    [InlineData(0, 5, 5, null)]
    public void NumericDifferences(double a, double b, double delta, double? percentage)
    {
        var left = new CalculationRecord { Module = "furnace", RequestJson = $"{{\"x\":{a}}}" };
        var right = new CalculationRecord { Module = "furnace", RequestJson = $"{{\"x\":{b}}}" };
        var row = Assert.Single(new CalculationComparisonService().Compare(left, right).Inputs);
        Assert.Equal(delta, row.AbsoluteDifference); Assert.Equal(percentage, row.PercentageDifference);
    }
    [Fact]
    public void EqualAndDifferentModules()
    {
        var service = new CalculationComparisonService();
        var left = new CalculationRecord { Module = "furnace" };
        Assert.Empty(service.Compare(left, left).Inputs);
        Assert.Throws<ArgumentException>(() => service.Compare(left, new CalculationRecord { Module = "slag-mode" }));
    }
}
