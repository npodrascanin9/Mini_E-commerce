namespace Api.Features.Products.Articles;

public class GetArticlesForProductEndpoint :
    ICarterModule
{
    public void AddRoutes(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "api/products/{productId}/articles",
            async (
                int productId,
                ISender sender) =>
            {
                var query = new GetArticlesForProduct.Query(productId);

                var result = await sender.Send(query);

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: (error) => Results.BadRequest(error));
            })
            .WithName("GetArticlesForProduct")
            .WithDescription("Get filtered articles by productId")
            .WithOpenApi();
    }
}

public static class GetArticlesForProduct
{
    public record Query(
        int ProductId) : IQuery<Result<Response>>;

    public class Response
    {
        public int Count { get; set; }
        public List<RowDto> Rows { get; set; }
    }

    public record RowDto(
        int Id,
        int ProductId,
        string ProductName,
        string? Barcode,
        string? Size,
        DateTime? ExpirationDate,
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
            if (!await context.Products.AnyAsync(x => x.Id == query.ProductId, cancellationToken))
            {
                return Result.Failure<Response>(
                    ArticleForProductErrors.ProductNotFound(
                        query.ProductId));
            }

            IQueryable<Article> queryable = context
                .Articles
                .Include(x => x.Product)
                .Where(x => x.ProductId == query.ProductId)
                .AsNoTracking();

            Response response = new();
            response.Count = await queryable
                .CountAsync(cancellationToken);
            response.Rows = await queryable
                .Select(article => query.ToRowDto(article))
                .ToListAsync(cancellationToken);

            return Result.Success(response);
        }
    }
}

public static class GetArticlesForProductMapper
{
    public static GetArticlesForProduct.RowDto ToRowDto(
        this GetArticlesForProduct.Query query,
        Article entity)
    {
        return new(
            Id: entity.Id,
            ProductId: entity.ProductId,
            ProductName: entity.Product.Name,
            Barcode: entity.Barcode,
            Size: entity.Size,
            ExpirationDate: entity.ExpirationDate,
            IsActive: entity.IsActive,
            CreatedAt: entity.CreatedAt,
            UpdatedAt: entity.UpdatedAt);
    }
}
