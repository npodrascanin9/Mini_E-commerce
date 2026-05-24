namespace Api.Features.ProductCategories;

public class CreateProductCategoryEndpoint :
    ICarterModule
{
    public void AddRoutes(
        IEndpointRouteBuilder app)
    {
        app.MapPost(
            "api/productCategories", 
            async (
                CreateProductCategoryRequest request,
                ISender sender) =>
            {
                var command = request.Adapt<CreateProductCategory.Command>();

                var result = await sender.Send(command);
                
                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("CreateProductCategory")
            .WithDescription("Create Product Category")
            .WithOpenApi();
    }
}

public class CreateProductCategoryRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public static class CreateProductCategory
{
    public class Command :
        ICommand<Result<Response>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
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
            if (await context.ProductCategories.AnyAsync(x => x.Name == command.Name, cancellationToken))
            {
                return Result.Failure<Response>(
                    ProductCategoryErrors.NameAlreadyExists(
                        command.Name));
            }

            var entity = command.ToEntity();

            await context.ProductCategories.AddAsync(
                entity,
                cancellationToken);
            await context.SaveChangesAsync(
                cancellationToken);

            return Result.Success(
                new Response(Id: entity.Id));
        }
    }
}

public static class CreateProductCategoryMapper
{
    public static ProductCategory ToEntity(
        this CreateProductCategory.Command command)
    {
        return new()
        {
            Id = default,
            Name = command.Name,
            Description = command.Description,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
