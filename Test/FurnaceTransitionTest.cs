using Core.Models.Calculations;
using Data.Services;
using Newtonsoft.Json.Linq;

namespace Test;

public class FurnaceTransitionTest
{
    [Fact]
    public void GasConvertsUnitsWithoutRenormalizingAndPreservesOtherInputs()
    {
        var source = new CalculationRecord { Module = "gas-dynamic", ResponseJson = """
            {"BlastFurnance":{"MaterialConsumption":{"Udeln_Koks_1000":0.42},
            "TopGas":{"Kolgaz_CO2":0.175,"Kolgaz_CO":0.241,"Kolgaz_H2":0.07,"Kolgaz_N2":0.504,"Kolgaz_CH4":0.01}}}
            """ };
        var target = JObject.Parse("{\"coke_rate\":400,\"hot_blast_temp\":1140,\"S\":0.014}");
        var result = new FurnaceTransitionService().Map(source, target);
        Assert.Equal(420, result.Input["coke_rate"]!.Value<double>());
        Assert.Equal(17.5, result.Input["top_CO2"]!.Value<double>());
        Assert.Equal(50.4, result.Input["top_N2"]!.Value<double>());
        Assert.Equal(1140, result.Input["hot_blast_temp"]!.Value<double>());
        Assert.Equal(400, target["coke_rate"]!.Value<double>());
        Assert.Null(result.Input["top_CH4"]);
    }

    [Fact]
    public void SlagUpdatesSulfurTemperatureAndSlagOutputOnly()
    {
        var source = new CalculationRecord { Module = "slag-mode", ResponseJson = "{\"slagOut\":260,\"sContentInCastIron\":0.014,\"castIronTemp\":1405,\"materialCons\":1700}" };
        var target = JObject.Parse("{\"S\":0.02,\"ore_rate\":1716,\"coke_rate\":420}");
        var result = new FurnaceTransitionService().Map(source, target);
        Assert.Equal(3, result.MappedFields.Count);
        Assert.Equal(0.014, result.Input["S"]!.Value<double>());
        Assert.Equal(260, result.Input["slag_rate"]!.Value<double>());
        Assert.Equal(1405, result.Input["T_iron"]!.Value<double>());
        Assert.Equal(1716, result.Input["ore_rate"]!.Value<double>());
    }

    [Theory]
    [InlineData("aglom-mode", "{}")]
    [InlineData("slag-mode", "{}")]
    [InlineData("slag-mode", "{\"sContentInCastIron\":101}")]
    [InlineData("slag-mode", "{\"slagOut\":-1}")]
    [InlineData("gas-dynamic", "{\"blastFurnance\":{\"topGas\":{\"kolgaz_CO\":24.1}}}")]
    public void RejectsUnsupportedOrInvalidResults(string module, string output) =>
        Assert.Throws<ArgumentException>(() => new FurnaceTransitionService().Map(new CalculationRecord { Module = module, ResponseJson = output }));
}
