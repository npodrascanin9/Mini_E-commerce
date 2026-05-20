namespace Api.Features.ProductCategories;

public class GetProductCategoryByIdEndpoint :
    ICarterModule
{
    public void AddRoutes(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "api/productCategories/{id}",
            async (
                int id,
                ISender sender) =>
            {
                var query = new GetProductCategoryById.Query(id);

                var result = await sender.Send(query);

                return result.Match(
                   onSuccess: () => Results.Ok(result.Value),
                   onFailure: error => Results.BadRequest(error));
            })
            .WithName("GetProductCategoryById")
            .WithDescription("Get Product Category data By Id")
            .WithOpenApi();
    }
}

public static class GetProductCategoryById
{
    public record Query(
        int Id) : IQuery<Result<Response>>;

    public record Response(
        int Id,
        string Name,
        string Description,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt);

    public class Validator : 
        AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id is required");
        }
    }

    internal sealed class QueryHandler(
        ApplicationDbContext context) :
        IQueryHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> Handle(
            Query query, 
            CancellationToken cancellationToken)
        {
            var entity = await context
                .ProductCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == query.Id, 
                    cancellationToken);

            if (entity is null)
            {
                return Result.Failure<Response>(
                    ProductCategoryErrors.NotFound(query.Id));
            }

            var response = query.ToResponse(entity);

            return Result.Success(response);
        }
    }
}

public static class GetProductCategoryByIdMapper
{
    public static GetProductCategoryById.Response ToResponse(
        this GetProductCategoryById.Query query,
        ProductCategory entity)
    {
        return new(
            Id: entity.Id,
            Name: entity.Name,
            Description: entity.Description,
            IsActive: entity.IsActive,
            CreatedAt: entity.CreatedAt,
            UpdatedAt: entity.UpdatedAt);
    }
}
