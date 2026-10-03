using Core.Models.Calculations;
using Newtonsoft.Json.Linq;

namespace Data.Services;

public sealed record CalculationDifference(string Parameter, object? Left, object? Right, double? AbsoluteDifference, double? PercentageDifference, bool MissingLeft, bool MissingRight);
public sealed record CalculationComparison(Guid LeftId, Guid RightId, string Module, IReadOnlyList<CalculationDifference> Inputs, IReadOnlyList<CalculationDifference> Results);

public sealed class CalculationComparisonService
{
    public CalculationComparison Compare(CalculationRecord left, CalculationRecord right)
    {
        if (left.Module != right.Module) throw new ArgumentException("Сравнивать можно только расчёты одного модуля.");
        return new(left.Id, right.Id, left.Module, Differences(left.RequestJson, right.RequestJson), Differences(left.ResponseJson, right.ResponseJson));
    }

    private static List<CalculationDifference> Differences(string leftJson, string rightJson)
    {
        var left = Flatten(JToken.Parse(leftJson)); var right = Flatten(JToken.Parse(rightJson));
        var rows = new List<CalculationDifference>();
        foreach (var path in left.Keys.Union(right.Keys, StringComparer.OrdinalIgnoreCase).OrderBy(x => x))
        {
            var hasLeft = left.TryGetValue(path, out var a); var hasRight = right.TryGetValue(path, out var b);
            if (hasLeft && hasRight && JToken.DeepEquals(a, b)) continue;
            double? delta = null; double? percentage = null;
            if (a?.Type is JTokenType.Integer or JTokenType.Float && b?.Type is JTokenType.Integer or JTokenType.Float)
            {
                var av = a.Value<double>(); var bv = b.Value<double>();
                // JSON 1 and 1.0 represent the same input despite different token types.
                if (hasLeft && hasRight && av == bv) continue;
                var difference = bv - av;
                if (double.IsFinite(difference)) delta = difference;
                if (av != 0 && double.IsFinite(difference / av * 100)) percentage = difference / av * 100;
            }
            rows.Add(new(path, (a as JValue)?.Value, (b as JValue)?.Value, delta, percentage, !hasLeft, !hasRight));
        }
        return rows;
    }

    private static Dictionary<string, JToken> Flatten(JToken root)
    {
        var result = new Dictionary<string, JToken>(StringComparer.OrdinalIgnoreCase);
        void Walk(JToken node, string path)
        {
            if (node is JObject obj) foreach (var property in obj.Properties()) Walk(property.Value, path.Length == 0 ? property.Name : path + "." + property.Name);
            else if (node is JArray array) for (var i = 0; i < array.Count; i++) Walk(array[i], $"{path}[{i}]");
            else result[path] = node;
        }
        Walk(root, ""); return result;
    }
}
