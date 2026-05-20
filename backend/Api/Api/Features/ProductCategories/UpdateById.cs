namespace Api.Features.ProductCategories;

public class UpdateProductCategoryByIdEndpoint :
    ICarterModule
{
    public void AddRoutes(
        IEndpointRouteBuilder app)
    {
        app.MapPut(
            "api/productCategories/{id}",
            async (
                int id,
                UpdateProductCategoryRequest request,
                ISender sender) =>
            {
                if (request.Id != id)
                {
                    return Results.BadRequest("Ids don't match");
                }

                var command = request.Adapt<UpdateProductCategory.Command>();

                var result = await sender.Send(command);

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("UpdateProductCategoryById")
            .WithDescription("Update product category record by id")
            .WithOpenApi();
    }
}

public class UpdateProductCategoryRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public static class UpdateProductCategory
{
    public class Command :
        ICommand<Result<Response>>
    {
        public int Id { get; set; }
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

            command.MapEntity(entity);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(
                new Response(
                    Id: command.Id));
        }
    }
}

public static class UpdateProductCategoryMapper
{
    public static void MapEntity(
        this UpdateProductCategory.Command command,
        ProductCategory entity)
    {
        if (command.Id != entity.Id)
        {
            throw new ArgumentException("Ids don't match");
        }

        entity.Name = command.Name;
        entity.Description = command.Description;

        entity.IsActive = true;
        entity.UpdatedAt = DateTime.UtcNow;
    }
}
