using Core.Models.Calculations;
using Newtonsoft.Json.Linq;

namespace Data.Services;

public sealed class FurnaceTransitionService
{
    public SlagTransition Map(CalculationRecord source, JObject? existing = null)
    {
        var input = existing?.DeepClone() as JObject ?? new JObject();
        var output = JObject.Parse(source.ResponseJson);
        var mapped = new List<string>();
        void Transfer(JObject values, string from, string to, double scale = 1, double maximum = double.MaxValue)
        {
            var value = values.GetValue(from, StringComparison.OrdinalIgnoreCase);
            if (value is null || value.Type == JTokenType.Null) return;
            if (value.Type is not (JTokenType.Float or JTokenType.Integer)) throw new ArgumentException($"Некорректное значение {from}.");
            var number = value.Value<double>();
            if (!double.IsFinite(number) || number < 0 || number > maximum || !double.IsFinite(number * scale))
                throw new ArgumentException($"Некорректное значение {from}.");
            var property = input.Properties().FirstOrDefault(p => p.Name.Equals(to, StringComparison.OrdinalIgnoreCase));
            if (property is null) input[to] = number * scale; else property.Value = number * scale;
            mapped.Add(to);
        }
        if (source.Module == "slag-mode")
        {
            Transfer(output, "SlagOut", "slag_rate"); // kg/t iron in both modules.
            Transfer(output, "SContentInCastIron", "S", maximum: 100); // mass percent.
            Transfer(output, "CastIronTemp", "T_iron"); // degrees Celsius.
        }
        else if (source.Module == "gas-dynamic")
        {
            var furnace = output.GetValue("BlastFurnance", StringComparison.OrdinalIgnoreCase) as JObject
                ?? throw new ArgumentException("Нет результатов доменной печи.");
            if (furnace.GetValue("MaterialConsumption", StringComparison.OrdinalIgnoreCase) is JObject material)
                Transfer(material, "Udeln_Koks_1000", "coke_rate", 1000); // t/t -> kg/t.
            if (furnace.GetValue("TopGas", StringComparison.OrdinalIgnoreCase) is JObject gas)
            {
                foreach (var component in new[] { "CO2", "CO", "H2", "N2" })
                    Transfer(gas, $"Kolgaz_{component}", $"top_{component}", 100, 1); // fraction -> percent.
                // Furnace has no top_CH4 input: don't renormalize the remaining components.
            }
        }
        else throw new ArgumentException("Для теплового баланса поддержаны результаты шлакового и газодинамического режимов.");
        if (mapped.Count == 0) throw new ArgumentException("В результате нет переносимых значений.");
        return new(source.Id, input, mapped);
    }
}
