namespace Api.IntegrationTests.Features.Products;

using static Testing;

[TestFixture]
public class CreateProductTests :
    BaseIntegrationTest
{
    [Test]
    public async Task ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateProduct.Command
        {
            Name = "",
            Description = "",
            Price = 0,
            ProductCategoryId = 0
        };

        // Act & Assert
        _ = await FluentActions.Invoking(
            async () => await SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldReturnProductCategoryNotFoundError()
    {
        // Arrange
        var command = new CreateProduct.Command
        {
            Name = "Test Product",
            Description = "Test desc",
            Price = 10,
            ProductCategoryId = 999 // doesn't exist
        };

        // Act
        var result = await SendAsync(command);

        // Assert
        result.Should()
            .NotBeNull();
        result.IsSuccess.Should()
            .BeFalse();
        result.IsFailure.Should()
            .BeTrue();
        result.Error.Should()
            .NotBeNull();
        result.Error.Code.Should()
            .Be(ProductErrors.ProductCategoryNotFound(command.ProductCategoryId).Code);
        result.Error.Description.Should()
            .Be(ProductErrors.ProductCategoryNotFound(command.ProductCategoryId).Description);
    }

    [Test]
    public async Task ShouldCreateProduct()
    {
        // Arrange
        var category = new CreateProductCategory.Command
        {
            Name = "Test Category",
            Description = "Test desc"
        };

        var categoryResult = await SendAsync(category);

        var command = new CreateProduct.Command
        {
            Name = "Test Product",
            Description = "Test desc",
            Price = 10,
            ProductCategoryId = categoryResult.Value.Id
        };

        // Act
        var result = await SendAsync(command);

        // Assert
        result.Should()
            .NotBeNull();
        result.IsSuccess.Should()
            .BeTrue();
        result.IsFailure.Should()
            .BeFalse();
        result.Value.Should()
            .NotBeNull();
        result.Value.Id.Should()
            .BeGreaterThan(0);
        var foundEntity = await FindAsync<Product>(result.Value.Id);
        foundEntity.Should()
            .NotBeNull();
        foundEntity.Id.Should()
            .Be(result.Value.Id);
        foundEntity.Name.Should()
            .Be(command.Name);
        foundEntity.Description.Should()
            .Be(command.Description);
        foundEntity.Price.Should()
            .Be(command.Price);
        foundEntity.ProductCategoryId.Should()
            .Be(command.ProductCategoryId);
        foundEntity.IsActive.Should()
            .BeTrue();
        foundEntity.CreatedAt.Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(30));
        foundEntity.UpdatedAt.Should()
           .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(30));
    }

    [Test]
    public async Task ShouldReturnNameAlreadyExistsError()
    {
        // Arrange
        var category = new CreateProductCategory.Command
        {
            Name = "Duplicate Category",
            Description = "Desc"
        };

        var categoryResult = await SendAsync(category);

        var command1 = new CreateProduct.Command
        {
            Name = "Duplicate Product",
            Description = "Desc",
            Price = 10,
            ProductCategoryId = categoryResult.Value.Id
        };

        var command2 = new CreateProduct.Command
        {
            Name = "Duplicate Product",
            Description = "Desc",
            Price = 20,
            ProductCategoryId = categoryResult.Value.Id
        };

        await SendAsync(command1);

        // Act
        var result = await SendAsync(command2);

        // Assert
        result.Should()
            .NotBeNull();
        result.IsSuccess.Should()
            .BeFalse();
        result.IsFailure.Should()
            .BeTrue();
        result.Error.Should()
            .NotBeNull();
        result.Error.Code.Should()
            .Be(ProductErrors.NameAlreadyExists(command2.Name).Code);
        result.Error.Description.Should()
            .Be(ProductErrors.NameAlreadyExists(command2.Name).Description);
    }
}
