namespace Api.Features.Products.Articles;

public class CreateArticleForProductEndpoint :
    ICarterModule
{
    public void AddRoutes(
        IEndpointRouteBuilder app)
    {
        app.MapPost(
            "api/products/{productId}/articles",
            async (
                int productId,
                CreateArticleForProductRequest request,
                ISender sender) =>
            {
                var command = request.Adapt<CreateArticleForProduct.Command>();

                if (command.ProductId != productId)
                {
                    return Results.BadRequest("ProductIds don't match");
                }

                var result = await sender.Send(command);

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: (error) => Results.BadRequest(error));
            })
            .WithName("CreateArticleForProduct")
            .WithDescription("Create Article for one product")
            .WithOpenApi();
    }
}

public class CreateArticleForProductRequest
{
    public int ProductId { get; set; }
    public string? Barcode { get; set; }

    public string? Color { get; set; }

    public string? Size { get; set; }

    public DateTime? ExpirationDate { get; set; }
}

public static class CreateArticleForProduct
{
    public class Command :
        ICommand<Result<Response>>
    {
        public int ProductId { get; set; }
        public string? Barcode { get; set; }

        public string? Color { get; set; }

        public string? Size { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }

    public record Response(
        int Id,
        int ProductId);

    public class Validator :
        AbstractValidator<Command>
    {
        public Validator()
        {
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

            Article articleEntity = command.ToEntity();
            await context
                .Articles
                .AddAsync(articleEntity, cancellationToken);
            await context
                .SaveChangesAsync(cancellationToken);

            return Result.Success(
                new Response(
                    Id: articleEntity.Id,
                    ProductId: articleEntity.ProductId));
        }
    }
}

public static class CreateArticleForProductMapper
{
    public static Article ToEntity(
        this CreateArticleForProduct.Command command)
    {
        return new()
        {
            Id = default,
            ProductId = command.ProductId,
            Barcode = command.Barcode,
            Color = command.Color,
            ExpirationDate = command.ExpirationDate,
            Size = command.Size,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
