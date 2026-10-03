using Core.Models.Calculations;
using Data.Services;
using Newtonsoft.Json.Linq;

namespace Test;

public class AglomSlagTransitionTest
{
    [Fact]
    public void TransfersOnlyKnownCompositionAndKeepsTargetConsumption()
    {
        var record = new CalculationRecord { Module = "aglom-mode", ResponseJson = "{\"components\":[{\"componentName\":\"Итог\",\"reportFe\":58.124,\"reportCaO\":8.468,\"reportComponentOfShihta\":110.092}]}" };
        var target = JObject.Parse("{\"iron\":{\"temp\":1450},\"components\":[{\"sourcename\":\"Agglomerate23\",\"consumption\":441.5,\"siO2\":6.048}]}");
        var result = new AglomSlagTransitionService().Map(record, target);
        Assert.Equal(441.5, result.Input["components"]![0]!["consumption"]!.Value<double>());
        Assert.Equal(58.124, result.Input["components"]![0]!["fe"]!.Value<double>());
        Assert.Equal(6.048, result.Input["components"]![0]!["siO2"]!.Value<double>());
        Assert.Null(target["components"]![0]!["fe"]);
    }
    [Fact]
    public void RejectsWrongModuleAndMissingTotal()
    {
        var service = new AglomSlagTransitionService();
        Assert.Throws<ArgumentException>(() => service.Map(new CalculationRecord { Module = "furnace" }));
        Assert.Throws<ArgumentException>(() => service.Map(new CalculationRecord { Module = "aglom-mode", ResponseJson = "{\"components\":[]}" }));
    }
}
