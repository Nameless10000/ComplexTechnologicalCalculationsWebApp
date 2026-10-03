using Core.Models.Calculations;
using Newtonsoft.Json.Linq;

namespace Data.Services;

public sealed record SlagTransition(Guid SourceCalculationId, JObject Input, IReadOnlyList<string> MappedFields);

public sealed class AglomSlagTransitionService
{
    public SlagTransition Map(CalculationRecord source, JObject? existing = null)
    {
        if (source.Module != "aglom-mode") throw new ArgumentException("Для перехода нужен расчёт Aglom Mode.");
        var result = JObject.Parse(source.ResponseJson);
        var rows = result.GetValue("Components", StringComparison.OrdinalIgnoreCase) as JArray ?? throw new ArgumentException("В расчёте нет состава агломерата.");
        var totals = rows.OfType<JObject>().Where(x => string.Equals(x.GetValue("ComponentName", StringComparison.OrdinalIgnoreCase)?.Value<string>(), "Итог", StringComparison.OrdinalIgnoreCase)).ToList();
        if (totals.Count != 1) throw new ArgumentException("Не найдена однозначная строка «Итог» в результате Aglom.");
        var input = existing?.DeepClone() as JObject ?? new JObject();
        var components = input.GetValue("components", StringComparison.OrdinalIgnoreCase) as JArray;
        if (components is null) { components = new JArray(); input["components"] = components; }
        var component = components.OfType<JObject>().FirstOrDefault(x => x.GetValue("sourcename", StringComparison.OrdinalIgnoreCase)?.Value<string>() == "Agglomerate23");
        if (component is null) { component = new JObject { ["sourcename"] = "Agglomerate23", ["consumption"] = 0 }; components.Add(component); }
        var mapped = new List<string>();
        foreach (var (report, target) in new[] { ("ReportFe", "fe"), ("ReportS", "s"), ("ReportCaO", "caO"), ("ReportSiO2", "siO2"), ("ReportAl2O3", "al2O3"), ("ReportMgO", "mgO"), ("ReportMnO", "mnO"), ("ReportTiO2", "tiO2") })
        {
            var value = totals[0].GetValue(report, StringComparison.OrdinalIgnoreCase);
            if (value is null || value.Type == JTokenType.Null) continue;
            if (value.Type is not (JTokenType.Float or JTokenType.Integer) || !double.IsFinite(value.Value<double>()) || value.Value<double>() is < 0 or > 100)
                throw new ArgumentException($"Некорректное значение {report} в результате Aglom.");
            var property = component.Properties().FirstOrDefault(x => x.Name.Equals(target, StringComparison.OrdinalIgnoreCase));
            if (property is null) component[target] = value.DeepClone(); else property.Value = value.DeepClone();
            mapped.Add(target);
        }
        if (mapped.Count == 0) throw new ArgumentException("В итоговой строке нет переносимых значений.");
        // ReportComponentOfShihta is kg/100, not kg/t iron: keep the target consumption for user review.
        return new(source.Id, input, mapped);
    }
}
