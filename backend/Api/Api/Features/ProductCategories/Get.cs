namespace Api.Features.ProductCategories;

public class GetProductCategoriesEndpoint :
    ICarterModule
{
    public void AddRoutes(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "api/productCategories",
            async (
                ISender sender,
                string searchOptions) =>
            {
                var query = JsonConvert.DeserializeObject<GetProductCategories.Query>(
                    searchOptions);

                var result = await sender.Send(query);

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("GetProductCategories")
            .WithDescription("Get Product Categories List")
            .WithOpenApi();
    }
}

public static class GetProductCategories
{
    public class Query :
        IQuery<Result<Response>>
    {
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
    }

    public class Response
    {
        public int Count { get; set; }
        public IEnumerable<RowDto> Rows { get; set; }

    }
    
    public record RowDto(
        int Id,
        string Name,
        string Description,
        bool isActive,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        int ProductsCount);

    internal sealed class QueryHandler(
        ApplicationDbContext context) :
        IQueryHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> Handle(
            Query query, 
            CancellationToken cancellationToken)
        {
            IQueryable<ProductCategory> queryable = context
                .ProductCategories
                .Include(x => x.Products)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                queryable = queryable.Where(x => x.Name.Contains(query.Name));
            }

            if (query.IsActive.HasValue)
            {
                queryable = queryable.Where(x => x.IsActive == query.IsActive.Value);
            }

            Response response = new();
            response.Count = await queryable
                .CountAsync(cancellationToken);
            response.Rows = await queryable
                .Select(productCategoryEntity => query.ToRowDto(productCategoryEntity))
                .ToListAsync(cancellationToken);

            return Result.Success(response);
        }
    }
}

public static class GetProductCategoriesMapper
{
    public static GetProductCategories.RowDto ToRowDto(
        this GetProductCategories.Query query,
        ProductCategory entity)
    {
        return new GetProductCategories.RowDto(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.IsActive,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.Products.Count);
    }
}
