namespace Api.Features.Products.Articles;

public class GetArticleForProductByIdEndpoint :
    ICarterModule
{
    public void AddRoutes(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "api/products/{productId}/articles/{id}",
            async (
                int productId,
                int id,
                ISender sender) =>
            {
                var query = new GetArticleForProductById.Query(
                    Id: id,
                    ProductId: productId);

                var result = await sender.Send(query);

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: (error) => Results.BadRequest(error));
            })
            .WithName("GetArticleForProductById")
            .WithDescription("Get one article by filter criteria (productId, articleId)")
            .WithOpenApi();
    }
}

public static class GetArticleForProductById
{
    public record Query(
        int Id,
        int ProductId) :
        IQuery<Result<Response>>;

    public record Response(
        int Id,
        int ProductId,
        string? Barcode,
        string? Size,
        DateTime? ExpirationDate,
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
            var entity = await context
                .Articles
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == query.Id
                        && x.ProductId == query.ProductId,
                    cancellationToken);

            if (entity is null)
            {
                return Result.Failure<Response>(
                    ArticleForProductErrors.NotFound(
                        id: query.Id,
                        productId: query.ProductId));
            }

            return Result.Success(
                query.ToResponse(entity));
        }
    }
}

public static class GetArticleForProductByIdMapper
{
    public static GetArticleForProductById.Response ToResponse(
        this GetArticleForProductById.Query query,
        Article entity)
    {
        return new(
            Id: entity.Id,
            ProductId: entity.ProductId,
            Barcode: entity.Barcode,
            Size: entity.Size,
            ExpirationDate: entity.ExpirationDate,
            CreatedAt: entity.CreatedAt,
            UpdatedAt: entity.UpdatedAt);
    }
}
