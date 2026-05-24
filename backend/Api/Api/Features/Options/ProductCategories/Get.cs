namespace Api.Features.Options.ProductCategories;

public class GetProductCategoryOptionsEndpoint :
    ICarterModule
{
    public void AddRoutes(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "api/options/productCategory", 
            async (ISender sender) =>
            {
                var query = new GetProductCategoryOptions.Query();

                var result = await sender.Send(query);

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: (error) => Results.BadRequest(error));
            })
            .WithName("GetProductCategoryOptions")
            .WithDescription("Get Product Category Options")
            .WithOpenApi();
    }
}

public static class GetProductCategoryOptions
{
    public record Query : 
        IQuery<Result<List<SelectOption>>>;

    internal sealed class QueryHandler(
        ApplicationDbContext context) :
        IQueryHandler<Query, Result<List<SelectOption>>>
    {
        public async Task<Result<List<SelectOption>>> Handle(
            Query query, 
            CancellationToken cancellationToken)
        {
            var response = await context
                .ProductCategories
                .Select(x => new SelectOption
                {
                    Key = x.Id,
                    Value = x.Name
                })
                .ToListAsync(cancellationToken);

            return Result.Success(response);
        }
    }
}
