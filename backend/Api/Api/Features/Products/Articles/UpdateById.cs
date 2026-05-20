namespace Api.Features.Products.Articles;

public class UpdateArticleForProductByIdEndpoint :
    ICarterModule
{
    public void AddRoutes(
        IEndpointRouteBuilder app)
    {
        app.MapPut(
            "api/products/{productId}/articles/{id}",
            async (
                int productId,
                int id,
                UpdateArticleForProductByIdRequest request,
                ISender sender) =>
            {
                var command = request.Adapt<UpdateArticleForProductById.Command>();

                if (command.Id != id)
                {
                    return Results.BadRequest("Ids don't match");
                }

                if (command.ProductId != productId)
                {
                    return Results.BadRequest("ProductIds don't match");
                }

                var result = await sender.Send(command);

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: (error) => Results.BadRequest(error));
            })
            .WithName("UpdateArticleForProductById")
            .WithDescription("Update article record by Id and productId")
            .WithOpenApi();
    }
}

public class UpdateArticleForProductByIdRequest
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? Barcode { get; set; }

    public string? Color { get; set; }

    public string? Size { get; set; }

    public DateTime? ExpirationDate { get; set; }
}

public static class UpdateArticleForProductById
{
    public class Command :
        ICommand<Result<Response>>
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? Barcode { get; set; }

        public string? Color { get; set; }

        public string? Size { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }

    public record Response(
        int Id);

    public class Validator :
        AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id is required");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId is required");
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
            if (!await context.Products.AnyAsync(x => x.Id == command.ProductId, cancellationToken))
            {
                return Result.Failure<Response>(
                    ArticleForProductErrors.ProductNotFound(
                        command.ProductId));
            }

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

            command.MapEntity(entity);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(
                new Response(command.Id));
        }
    }
}

public static class UpdateArticleForProductByIdMapper
{
    public static void MapEntity(
        this UpdateArticleForProductById.Command command,
        Article entity)
    {
        entity.Barcode = command.Barcode;
        entity.Size = command.Size;
        entity.Color = command.Color;
        entity.ExpirationDate = command.ExpirationDate;

        entity.IsActive = true;
        entity.UpdatedAt = DateTime.UtcNow;
    }
}
