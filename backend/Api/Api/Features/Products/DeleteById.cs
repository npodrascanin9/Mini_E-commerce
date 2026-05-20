namespace Api.Features.Products;

public class DeleteProductByIdEndpoint :
    ICarterModule
{
    public void AddRoutes(
        IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "api/products/{id}",
            async (
                int id,
                ISender sender) =>
            {
                var command = new DeleteProductById.Command(id);

                var result = await sender.Send(command);

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: (error) => Results.BadRequest(error));
            })
            .WithName("DeleteProductById")
            .WithDescription("Delete product by id")
            .WithOpenApi();
    }
}

public static class DeleteProductById
{
    public record Command(
        int Id) : ICommand<Result<Response>>;

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
            var productEntity = await context
                .Products
                .FirstOrDefaultAsync(
                    x => x.Id == command.Id,
                    cancellationToken);

            if (productEntity is null)
            {
                return Result.Failure<Response>(
                    ProductErrors.NotFound(command.Id));
            }

            using var transaction = await context
                .Database
                .BeginTransactionAsync(cancellationToken);
            try
            {
                var filteredArticleEntities = await context
                    .Articles
                    .Where(x => x.ProductId == productEntity.Id)
                    .ToListAsync(cancellationToken);

                if (filteredArticleEntities.Any())
                {
                    context
                        .Articles
                        .RemoveRange(filteredArticleEntities);
                }

                context.Remove(productEntity);
                await context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
                return Result.Success(
                    new Response(command.Id));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(
                    cancellationToken);

                StringBuilder errorBuilder = new();
                errorBuilder.AppendLine($"ex.Message: {ex.Message}");
                errorBuilder.AppendLine($"ex.StackTrace: {ex.StackTrace}");
                errorBuilder.AppendLine($"inner ex.Message: {ex?.InnerException?.Message}");
                errorBuilder.AppendLine($"inner ex.StackTrace: {ex?.InnerException?.StackTrace}");

                return Result.Failure<Response>(
                    ProductErrors.Rollback(errorBuilder.ToString()));
            }
        }
    }
}
