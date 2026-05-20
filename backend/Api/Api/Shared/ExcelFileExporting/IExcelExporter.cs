using ClosedXML.Excel;

namespace Api.Shared.ExcelFileExporting;

public interface IExcelExporter
{
    byte[] ExportExcelFile<T>(
        IEnumerable<T> rows,
        string sheetName = "Export");
}

public class ExcelExporter : IExcelExporter
{
    public byte[] ExportExcelFile<T>(
        IEnumerable<T> rows,
        string sheetName = "Export")
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(sheetName);

        var properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.GetCustomAttribute<IgnoreExcelColumnAttribute>() == null)
            .ToArray();

        // HEADER
        for (int i = 0; i < properties.Length; i++)
        {
            var prop = properties[i];

            var attr = prop.GetCustomAttribute<ExcelColumnAttribute>();

            var columnName = attr?.Name ?? prop.Name;

            var cell = worksheet.Cell(1, i + 1);

            cell.Value = columnName;
            cell.Style.Font.Bold = true;
        }

        // ROWS
        int rowIndex = 2;

        foreach (var row in rows)
        {
            for (int col = 0; col < properties.Length; col++)
            {
                var value = properties[col].GetValue(row);

                worksheet.Cell(rowIndex, col + 1).Value =
                    value?.ToString() ?? string.Empty;
            }

            rowIndex++;
        }

        worksheet.Columns().AdjustToContents();

        using var memoryStream = new MemoryStream();

        workbook.SaveAs(memoryStream);

        return memoryStream.ToArray();
    }
}
