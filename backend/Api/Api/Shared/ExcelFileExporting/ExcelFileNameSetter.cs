namespace Api.Shared.ExcelFileExporting;

public class ExcelFileNameSetter
{
    public static string SetExcelFileName(
        string name)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(name);

        if (name.EndsWith(".xlsx"))
        {
            return name;
        }

        return $"{name}.xlsx";
    }
}
