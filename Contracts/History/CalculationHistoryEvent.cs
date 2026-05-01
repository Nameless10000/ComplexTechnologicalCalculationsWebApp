namespace Contracts.History;

public static class CalculationModules
{
    public const string AglomMode = "aglom-mode";
    public const string GasDynamic = "gas-dynamic";
    public const string SlagMode = "slag-mode";
}

public sealed class CalculationHistoryEvent
{
    public string Module { get; set; } = string.Empty;

    public int UserId { get; set; }

    public DateTime CreationDateTime { get; set; }

    public string RequestJson { get; set; } = string.Empty;

    public string ResponseJson { get; set; } = string.Empty;
}
