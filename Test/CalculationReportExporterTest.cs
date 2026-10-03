using ClosedXML.Excel;
using Core.Models.Calculations;
using Data.Services;

namespace Test;

public class CalculationReportExporterTest
{
    [Fact]
    public void ExportsMetadataInputsResultsAndCyrillic()
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        var record = new CalculationRecord { Id = Guid.NewGuid(), Module = "furnace", UserName = "Евгений", CreatedAt = DateTime.UtcNow, RequestJson = "{\"coke_rate\":420}", ResponseJson = "{\"heat_balance\":{\"C15\":3500}}" };
        var exporter = new CalculationReportExporterService();
        using var workbook = new XLWorkbook(new MemoryStream(exporter.Export(record, "xlsx")));
        var text = string.Join(" ", workbook.Worksheet(1).CellsUsed().Select(x => x.GetString()));
        Assert.Contains("Евгений", text); Assert.Contains(record.Id.ToString(), text); Assert.Contains("coke_rate", text); Assert.Contains("C15", text);
        var pdf = exporter.Export(record, "pdf");
        Assert.Equal("%PDF", System.Text.Encoding.ASCII.GetString(pdf, 0, 4));
        Assert.Throws<ArgumentException>(() => exporter.Export(record, "csv"));
    }
}
