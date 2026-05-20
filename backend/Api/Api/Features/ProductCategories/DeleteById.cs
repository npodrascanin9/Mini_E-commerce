namespace Api.Features.ProductCategories;

public class DeleteProductCategoryByIdEndpoint :
    ICarterModule
{
    public void AddRoutes(
        IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "api/productCategories/{id}",
            async (
                int id,
                ISender sender) =>
            {
                var command = new DeleteProductCategoryById.Command(
                    id);

                var result = await sender.Send(command);

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("DeleteProductCategoryById")
            .WithDescription("Delete Product category record by id")
            .WithOpenApi();
    }
}

public static class DeleteProductCategoryById
{
    public record Command(
        int Id) : ICommand<Result<Response>>;

    public record Response(
        int Id);

    public class Validator :
        AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id is required");
        }
    }

    internal sealed class CommandHandler(
        ApplicationDbContext context) :
        ICommandHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(
            Command command, 
            CancellationToken cancellationToken)
        {
            var entity = await context
                .ProductCategories
                .FirstOrDefaultAsync(
                    x => x.Id == command.Id,
                    cancellationToken);

            if (entity is null)
            {
                return Result.Failure<Response>(
                    ProductCategoryErrors.NotFound(command.Id));
            }

            context.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(
                new Response(command.Id));
        }
    }
}
