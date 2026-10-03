namespace BaseLib.Validation;

public sealed class CalculationValidationException(IReadOnlyDictionary<string, string[]> errors)
    : ArgumentException("Проверьте входные параметры расчёта.")
{
    public IReadOnlyDictionary<string, string[]> Errors { get; } = errors;
}
