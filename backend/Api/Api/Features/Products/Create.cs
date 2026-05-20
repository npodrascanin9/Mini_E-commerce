namespace Api.Features.Products;

public class CreateProductEndpoint :
    ICarterModule
{
    public void AddRoutes(
        IEndpointRouteBuilder app)
    {
        app.MapPost(
            "api/products",
            async (
                CreateProductRequest request,
                ISender sender) =>
            {
                var command = request.Adapt<CreateProduct.Command>();

                var result = await sender.Send(command);

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("CreateProduct")
            .WithDescription("Create Product")
            .WithOpenApi();
    }
}

public class CreateProductRequest
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int ProductCategoryId { get; set; }
}

public static class CreateProduct
{
    public class Command : 
        ICommand<Result<Response>>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int ProductCategoryId { get; set; }
    }

    public record Response(
        int Id);

    public class Validator :
        AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price is required");

            RuleFor(x => x.ProductCategoryId)
                .GreaterThan(0).WithMessage("ProductCategoryId is required");
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
            if (await context
                .Products
                .AnyAsync(x => x.Name == command.Name, cancellationToken))
            {
                return Result.Failure<Response>(
                    ProductErrors.NameAlreadyExists(
                        command.Name));
            }

            if (!await context
                .ProductCategories
                .AnyAsync(x => x.Id == command.ProductCategoryId, cancellationToken))
            {
                return Result.Failure<Response>(
                    ProductErrors.ProductCategoryNotFound(
                        command.ProductCategoryId));
            }

            var entity = command.ToEntity();

            await context
                .Products
                .AddAsync(entity, cancellationToken);
            await context
                .SaveChangesAsync(cancellationToken);

            return Result.Success(
                new Response(entity.Id));
        }
    }
}

public static class CreateProductMapper
{
    public static Product ToEntity(
        this CreateProduct.Command command)
    {
        return new()
        {
            Id = default,
            Name = command.Name,
            Description = command.Description,
            ProductCategoryId = command.ProductCategoryId,
            Price = command.Price,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
