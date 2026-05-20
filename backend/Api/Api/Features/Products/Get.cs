namespace Api.Features.Products;

public class GetProductsEndpoint :
    ICarterModule
{
    public void AddRoutes(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "api/products",
            async (
                string searchOptions,
                ISender sender) =>
            {
                var query = JsonConvert.DeserializeObject<GetProducts.Query>(
                    searchOptions);

                var result = await sender.Send(query);

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: (error) => Results.BadRequest(error));
            })
            .WithName("GetProducts")
            .WithDescription("Get Products")
            .WithOpenApi();
    }
}

public static class GetProducts
{
    public class Query : 
        IQuery<Result<Response>>
    {

    }

    public class Response
    {
        public int Count { get; set; }
        public List<RowDto> Rows { get; set; } 
    }

    public record RowDto(
        int Id,
        string Name,
        string? Description,
        decimal Price,
        int ProductCategoryId,
        string ProductCategoryName,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt);

    internal sealed class QueryHandler(
        ApplicationDbContext context) :
        IQueryHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> Handle(
            Query query, 
            CancellationToken cancellationToken)
        {
            IQueryable<Product> queryable = context
                .Products
                .Include(x => x.ProductCategory)
                .AsNoTracking();
            
            Response response = new();
            response.Count = await queryable
                .CountAsync(cancellationToken);
            response.Rows = await queryable
                .Select(product => query.ToRowDto(product))
                .ToListAsync(cancellationToken);
            return Result.Success(response);
        }
    }
}

public static class GetProductsMapper
{
    public static GetProducts.RowDto ToRowDto(
        this GetProducts.Query query,
        Product productEntity)
    {
        return new GetProducts.RowDto(
            Id: productEntity.Id,
            Name: productEntity.Name,
            Description: productEntity.Description,
            Price: productEntity.Price,
            ProductCategoryId: productEntity.ProductCategoryId,
            ProductCategoryName: productEntity.ProductCategory.Name,
            IsActive: productEntity.IsActive,
            CreatedAt: productEntity.CreatedAt,
            UpdatedAt: productEntity.UpdatedAt);
    }
}
