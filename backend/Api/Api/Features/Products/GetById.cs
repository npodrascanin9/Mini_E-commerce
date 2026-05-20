namespace Api.Features.Products;

public class GetProductByIdEndpoint :
    ICarterModule
{
    public void AddRoutes(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "api/products/{id}",
            async (
                int id,
                ISender sender) =>
            {
                var query = new GetProductById.Query(id);

                var result = await sender.Send(query);

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: (error) => Results.BadRequest(error));
            })
            .WithName("GetProductById")
            .WithDescription("Get Product By Id")
            .WithOpenApi();
    }
}

public static class GetProductById
{
    public record Query(
        int Id) : IQuery<Result<Response>>;

    public record Response(
        int Id,
        string Name,
        string? Description,
        decimal Price,
        int ProductCategoryId);

    internal sealed class QueryHandler(
        ApplicationDbContext context) :
        IQueryHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> Handle(
            Query query, 
            CancellationToken cancellationToken)
        {
            var entity = await context
                .Products
                .FirstOrDefaultAsync(
                    x => x.Id == query.Id, 
                    cancellationToken);

            if (entity is null)
            {
                return Result.Failure<Response>(
                    ProductErrors.NotFound(
                        query.Id));
            }

            return Result.Success(
                query.ToResponse(entity));
        }
    }
}

public static class GetProductByIdMapper
{
    public static GetProductById.Response ToResponse(
        this GetProductById.Query query,
        Product entity)
    {
        return new(
            Id: entity.Id,
            Name: entity.Name,
            Description: entity.Description,
            Price: entity.Price,
            ProductCategoryId: entity.ProductCategoryId);
    }
}
