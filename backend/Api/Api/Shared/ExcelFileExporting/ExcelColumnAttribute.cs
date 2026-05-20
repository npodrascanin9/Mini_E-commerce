namespace Api.Shared.ExcelFileExporting;


[AttributeUsage(AttributeTargets.Property)]
public sealed class ExcelColumnAttribute : 
    Attribute
{
    public string Name { get; }

    public ExcelColumnAttribute(string name)
    {
        Name = name;
    }
}
