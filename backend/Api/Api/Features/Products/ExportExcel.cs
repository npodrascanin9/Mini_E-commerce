namespace Api.Features.Products;

public class ExportExcelProductsEndpoint :
    ICarterModule
{
    public void AddRoutes(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "api/products/exportExcel",
            async (
                ISender sender) =>
            {
                var query = new ExportExcelProducts.Query();

                var result = await sender.Send(query);

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("ExportExcelProducts")
            .WithDescription("Export Excel Products")
            .WithOpenApi();
    }
}

public static class ExportExcelProducts
{
    public record Query :
        IQuery<Result<Response>>;

    public record Response(
        string FileDownloadName,
        string ContentType,
        byte[] FileContents);

    public class ExcelRowDto
    {
        [ExcelColumn("Id")]
        public int Id { get; set; }

        [ExcelColumn("Product name")]
        public string Name { get; set; }
        [ExcelColumn("Product Description")]
        public string? Description { get; set; }

        [IgnoreExcelColumn]
        public int ProductCategoryId { get; set; }
        [ExcelColumn("Category name")]
        public string ProductCategoryName { get; set; }

        [ExcelColumn("Price")]
        public decimal Price { get; set; }
    }

    internal sealed class QueryHandler(
        ApplicationDbContext context,
        IExcelExporter excelExporter) :
        IQueryHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> Handle(
            Query query, 
            CancellationToken cancellationToken)
        {
            List<ExcelRowDto> records = await InitRecords(
                query, 
                cancellationToken);

            Response response = new(
                FileDownloadName: ExcelFileNameSetter.SetExcelFileName("List of products"),
                ContentType: ExcelFileConstants.ContentType,
                FileContents: excelExporter.ExportExcelFile(records));

            return Result.Success(response);
        }

        private async Task<List<ExcelRowDto>> InitRecords(
            Query query, 
            CancellationToken cancellationToken)
        {
            return await context
                .Products
                .Include(product => product.ProductCategory)
                .Select(product => query.ToExcelRowDto(product))
                .ToListAsync(cancellationToken);
        }
    }
}

public static class ExportExcelProductsMapper
{
    public static ExportExcelProducts.ExcelRowDto ToExcelRowDto(
        this ExportExcelProducts.Query query,
        Product entity)
    {
        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Price = entity.Price,
            Description = entity.Description,
            ProductCategoryId = entity.ProductCategoryId,
            ProductCategoryName = entity.ProductCategory.Name
        };
    }
}
