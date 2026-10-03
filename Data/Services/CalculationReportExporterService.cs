using System.Globalization;
using ClosedXML.Excel;
using Core.Models.Calculations;
using Newtonsoft.Json.Linq;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Data.Services;

public sealed class CalculationReportExporterService
{
    public byte[] Export(CalculationRecord calculation, string format)
    {
        var rows = Rows(calculation);
        if (format == "xlsx")
        {
            using var workbook = new XLWorkbook();
            var sheet = workbook.AddWorksheet("Расчёт");
            sheet.Cell(1, 1).Value = "Параметр"; sheet.Cell(1, 2).Value = "Значение";
            for (var i = 0; i < rows.Count; i++) { sheet.Cell(i + 2, 1).Value = rows[i].Key; sheet.Cell(i + 2, 2).Value = rows[i].Value; }
            sheet.Range(1, 1, rows.Count + 1, 2).CreateTable();
            sheet.Column(1).Width = 60; sheet.Column(2).Width = 60;
            sheet.CellsUsed().Style.Alignment.WrapText = true;
            sheet.SheetView.FreezeRows(1);
            using var stream = new MemoryStream(); workbook.SaveAs(stream); return stream.ToArray();
        }
        if (format != "pdf") throw new ArgumentException("Поддерживаются форматы pdf и xlsx.");
        return Document.Create(document => document.Page(page =>
        {
            page.Size(PageSizes.A4); page.Margin(28); page.DefaultTextStyle(style => style.FontSize(9));
            page.Header().PaddingBottom(12).Text("Отчёт о технологическом расчёте").FontSize(16).SemiBold();
            page.Content().Table(table =>
            {
                table.ColumnsDefinition(columns => { columns.RelativeColumn(1.2f); columns.RelativeColumn(); });
                table.Header(header => { header.Cell().Text("Параметр").SemiBold(); header.Cell().Text("Значение").SemiBold(); });
                foreach (var row in rows)
                {
                    table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(4).Text(row.Key);
                    table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(4).Text(row.Value);
                }
            });
            page.Footer().AlignCenter().Text(text => { text.Span("Страница "); text.CurrentPageNumber(); text.Span(" из "); text.TotalPages(); });
        })).GeneratePdf();
    }

    private static List<KeyValuePair<string, string>> Rows(CalculationRecord calculation)
    {
        var module = calculation.Module switch { "aglom-mode" => "Агломерационная шихта", "slag-mode" => "Шлаковый режим", "gas-dynamic" => "Газодинамический режим", "furnace" => "Тепловой баланс доменной печи", _ => calculation.Module };
        var rows = new List<KeyValuePair<string, string>>
        {
            new("Модуль", module), new("Пользователь", calculation.UserName), new("Дата расчёта (UTC)", calculation.CreatedAt.ToUniversalTime().ToString("O")),
            new("ID расчёта", calculation.Id.ToString()), new("CorrelationId", calculation.CorrelationId), new("Статус истории", calculation.Status)
        };
        void Walk(JToken value, string path)
        {
            if (value is JObject obj) foreach (var property in obj.Properties()) Walk(property.Value, path + "." + property.Name);
            else if (value is JArray array) for (var i = 0; i < array.Count; i++) Walk(array[i], $"{path}[{i}]");
            else rows.Add(new(path, Convert.ToString((value as JValue)?.Value, CultureInfo.InvariantCulture) ?? "—"));
        }
        Walk(JToken.Parse(calculation.RequestJson), "Входные данные"); Walk(JToken.Parse(calculation.ResponseJson), "Результаты");
        return rows;
    }
}
