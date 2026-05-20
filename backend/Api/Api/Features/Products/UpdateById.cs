namespace Api.Features.Products;

public class UpdateProductByIdEndpoint :
    ICarterModule
{
    public void AddRoutes(
        IEndpointRouteBuilder app)
    {
        app.MapPut(
            "api/products/{id}",
            async (
                int id,
                UpdateProductByIdRequest request,
                ISender sender) =>
            {
                var command = request.Adapt<UpdateProductById.Command>();

                var result = await sender.Send(command);

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: (error) => Results.BadRequest(error));
            })
            .WithName("UpdateProductById")
            .WithDescription("Update product by id")
            .WithOpenApi();
    }
}

public class UpdateProductByIdRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int ProductCategoryId { get; set; }
}

public static class UpdateProductById
{
    public class Command :
        ICommand<Result<Response>>
    {
        public int Id { get; set; }
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
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id is required");

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
            var entity = await context
                .Products
                .FirstOrDefaultAsync(
                    x => x.Id == command.Id,
                    cancellationToken);

            if (entity is null)
            {
                return Result.Failure<Response>(
                    ProductErrors.NotFound(
                        command.Id));
            }

            if (command.Name != entity.Name
                && await context.Products.AnyAsync(
                    x => x.Name == command.Name, 
                    cancellationToken))
            {
                return Result.Failure<Response>(
                    ProductErrors.NameAlreadyExists(command.Name));
            }

            command.MapEntity(entity);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(
                new Response(command.Id));
        }
    }
}

public static class UpdateProductByIdMapper
{
    public static void MapEntity(
        this UpdateProductById.Command command,
        Product productEntity)
    {
        productEntity.Name = command.Name;
        productEntity.Description = command.Description;
        productEntity.Price = command.Price;
        productEntity.ProductCategoryId = command.ProductCategoryId;
        
        productEntity.UpdatedAt = DateTime.UtcNow;
        productEntity.IsActive = true;
    }
}
