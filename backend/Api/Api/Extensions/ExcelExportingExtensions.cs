namespace Api.Extensions;

public static class ExcelExportingExtensions
{
    public static IServiceCollection AddExcelExporting(
        this IServiceCollection services)
    {
        services.AddScoped<IExcelExporter, ExcelExporter>();

        return services;
    }
}
