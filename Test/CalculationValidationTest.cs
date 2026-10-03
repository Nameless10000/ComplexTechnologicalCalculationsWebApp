using BaseLib.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Test;

public class CalculationValidationTest
{
    [Fact]
    public void AcceptsExistingSlagSample() => CalculationInputValidator.Validate("slag-mode", JsonConvert.SerializeObject(SlagModeTest.PrepareData()));
    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void RejectsInvalidPercent(double value)
    {
        var data = SlagModeTest.PrepareData(); data.Slag.CaO = value;
        Assert.Throws<CalculationValidationException>(() => CalculationInputValidator.Validate("slag-mode", JsonConvert.SerializeObject(data)));
    }
    [Fact]
    public void RejectsMissingField()
    {
        var data = JObject.FromObject(SlagModeTest.PrepareData()); ((JObject)data["Iron"]!).Remove("Temp");
        Assert.Throws<CalculationValidationException>(() => CalculationInputValidator.Validate("slag-mode", data.ToString()));
    }
    [Fact]
    public void RejectsInvalidSum()
    {
        var data = SlagModeTest.PrepareData(); data.Slag.CaO = 90;
        Assert.Throws<CalculationValidationException>(() => CalculationInputValidator.Validate("slag-mode", JsonConvert.SerializeObject(data)));
    }
}
