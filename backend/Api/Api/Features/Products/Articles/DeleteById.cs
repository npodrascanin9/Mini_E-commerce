namespace Api.Features.Products.Articles;

public class DeleteArticleForProductEndpoint :
    ICarterModule
{
    public void AddRoutes(
        IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "api/products/{productId}/articles/{id}",
            async (
                int productId,
                int id,
                ISender sender) =>
            {
                var command = new DeleteArticleForProduct.Command(
                    Id: id,
                    ProductId: productId);

                var result = await sender.Send(command);

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: (error) => Results.BadRequest(error));
            })
            .WithName("DeleteArticleForProduct")
            .WithDescription("Delete article for product by id")
            .WithOpenApi();
    }
}

public static class DeleteArticleForProduct
{
    public record Command(
        int Id,
        int ProductId) :
        ICommand<Result<Response>>;

    public record Response(
        int Id);

    internal sealed class CommandHandler(
        ApplicationDbContext context) :
        ICommandHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(
            Command command, 
            CancellationToken cancellationToken)
        {
            var entity = await context
                .Articles
                .FirstOrDefaultAsync(
                    x => x.Id == command.Id
                        && x.ProductId == command.ProductId,
                    cancellationToken);

            if (entity is null)
            {
                return Result.Failure<Response>(
                    ArticleForProductErrors.NotFound(
                        id: command.Id,
                        productId: command.ProductId));
            }

            context.Articles.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(
                new Response(command.Id));
        }
    }
}
