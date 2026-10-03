using System.ComponentModel;
using BaseLib.AglomMode.Models;
using BaseLib.Models2;
using BaseLib.SlagMode.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BaseLib.Validation;

public static class CalculationInputValidator
{
    private static readonly HashSet<string> PercentNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "Fe", "FeO", "CaO", "SiO2", "MgO", "Al2O3", "TiO2", "S", "P", "Cr", "Zn", "MnO", "Si", "Mn", "C", "Ti",
        "Wet", "PMPP", "Sulfur", "AshAmount", "AshCaOFraction", "AshSiO2Fraction", "AshAl2O3Fraction", "AshMgOFraction",
        "PercentZola", "PercentSera", "PercentValotiles", "PercentC", "FeOinAgl", "DolomyteInAgl"
    };

    public static void Validate(string module, string json)
    {
        JObject payload;
        try { payload = JObject.Parse(json); }
        catch (JsonException) { throw new CalculationValidationException(new Dictionary<string, string[]> { ["request"] = ["Ожидается JSON-объект."] }); }
        var errors = new Dictionary<string, string[]>();
        var type = module switch
        {
            "aglom-mode" => typeof(AglomRequestData), "slag-mode" => typeof(RequestData), "gas-dynamic" => typeof(RequestModelV2),
            _ => throw new ArgumentException("Неизвестный расчётный модуль.")
        };
        CheckObject(payload, type, "", errors);
        if (errors.Count == 0)
        {
            if (module == "slag-mode")
            {
                CheckSum(Object(payload, "Slag"), ["CaO", "SiO2", "TiO2", "Al2O3", "MgO"], "slag", errors);
                CheckSum(Object(payload, "Iron"), ["Si", "S", "Mn", "C", "Ti", "Cr"], "iron", errors);
                CheckSum(Object(payload, "Coke"), ["AshCaOFraction", "AshSiO2Fraction", "AshAl2O3Fraction", "AshMgOFraction"], "coke.ash", errors);
                Positive(Object(payload, "Iron"), "Temp", "iron.temp", errors);
                Positive(Object(payload, "Slag"), "SiO2", "slag.siO2", errors);
                CheckComponents((JArray)Value(payload, "Components")!, "Consumption", ["Fe", "SiO2", "Al2O3", "CaO", "MgO", "S", "MnO", "TiO2"], "components", errors);
            }
            if (module == "aglom-mode")
            {
                CheckComponents((JArray)Value(payload, "ShihtaComponents")!, "Weight", ["CaO", "SiO2", "MgO", "Al2O3", "TiO2", "MnO"], "shihtaComponents", errors);
                CheckSum(Object(payload, "Cocksick"), ["PercentZola", "PercentSera", "PercentValotiles", "PercentC"], "cocksick", errors);
                Positive(Object(payload, "StartEnter"), "Osnovnost", "startEnter.osnovnost", errors);
            }
            if (module == "gas-dynamic")
            {
                var blast = Object(Object(payload, "BlastFurnaceInput"), "FuelAndBlast");
                foreach (var field in new[] { "Stepen_pryamogo_vost", "Stepen_vodorod", "Stepen_CO" })
                    if (Number(blast, field) > 1) errors[$"fuelAndBlast.{field}"] = ["Доля должна быть в диапазоне 0–1."];
                Positive(blast, "Udeln_koks", "fuelAndBlast.udeln_koks", errors);
                Positive(blast, "Kislorod_dut", "fuelAndBlast.kislorod_dut", errors);
            }
        }
        if (errors.Count > 0) throw new CalculationValidationException(errors);
    }

    private static void CheckObject(JObject obj, Type type, string prefix, Dictionary<string, string[]> errors)
    {
        foreach (var property in type.GetProperties().Where(p => p.CanWrite))
        {
            if (property.Name is "User" or "UserId") continue;
            var path = prefix + property.Name;
            var value = Value(obj, property.Name);
            if (value is null || value.Type == JTokenType.Null) { errors[path] = ["Обязательное поле."]; continue; }
            if (property.PropertyType == typeof(double))
            {
                if (value.Type is not (JTokenType.Integer or JTokenType.Float) || !double.IsFinite(value.Value<double>()))
                    errors[path] = ["Нужно конечное число."];
                else if (value.Value<double>() < 0) errors[path] = ["Значение не может быть отрицательным."];
                else if ((PercentNames.Contains(property.Name) || property.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                    .Cast<DisplayNameAttribute>().Any(x => x.DisplayName.Contains('%')) || property.Name.EndsWith("PMPP")) && value.Value<double>() > 100)
                    errors[path] = ["Процент должен быть в диапазоне 0–100."];
            }
            else if (property.PropertyType == typeof(string))
            {
                if (value.Type != JTokenType.String || string.IsNullOrWhiteSpace(value.Value<string>())) errors[path] = ["Нужно непустое название."];
            }
            else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                if (value is not JArray array) { errors[path] = ["Ожидается массив."]; continue; }
                for (var i = 0; i < array.Count; i++)
                    if (array[i] is JObject child) CheckObject(child, property.PropertyType.GenericTypeArguments[0], $"{path}[{i}].", errors);
                    else errors[$"{path}[{i}]"] = ["Ожидается объект."];
            }
            else if (property.PropertyType.IsClass)
            {
                if (value is JObject child) CheckObject(child, property.PropertyType, path + ".", errors);
                else errors[path] = ["Ожидается объект."];
            }
        }
    }

    private static JToken? Value(JObject obj, string field) => obj.GetValue(field, StringComparison.OrdinalIgnoreCase);
    private static JObject Object(JObject obj, string field) => (JObject)Value(obj, field)!;
    private static double Number(JObject obj, string field) => Value(obj, field)!.Value<double>();
    private static void Positive(JObject obj, string field, string path, Dictionary<string, string[]> errors)
    { if (Number(obj, field) <= 0) errors[path] = ["Значение должно быть больше нуля."]; }
    private static void CheckSum(JObject obj, string[] fields, string path, Dictionary<string, string[]> errors)
    { if (fields.Sum(f => Number(obj, f)) > 100.001) errors[path] = ["Сумма долей не может превышать 100%."]; }
    private static void CheckComponents(JArray array, string mass, string[] fields, string path, Dictionary<string, string[]> errors)
    {
        if (array.Count == 0 || array.Sum(x => Number((JObject)x, mass)) <= 0) errors[path] = ["Нужен компонент с положительным расходом."];
        for (var i = 0; i < array.Count; i++) CheckSum((JObject)array[i], fields, $"{path}[{i}]", errors);
    }
}
