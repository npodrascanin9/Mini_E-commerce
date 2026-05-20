namespace Api.IntegrationTests.Features.Products;

using static Testing;

[TestFixture]
public class UpdateProductByIdTests :
    BaseIntegrationTest
{
    [Test]
    public async Task ShouldThrowValidationException()
    {
        // Arrange
        var command = new UpdateProductById.Command
        {
            Id = 0,
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
    public async Task ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new UpdateProductById.Command
        {
            Id = 999,
            Name = "NonExisting",
            Description = "Desc",
            Price = 10,
            ProductCategoryId = 1
        };

        // Act
        var result = await SendAsync(command);

        // Assert
        result.IsSuccess.Should()
            .BeFalse();
        result.IsFailure.Should()
            .BeTrue();
        result.Error.Should()
            .NotBeNull();
        result.Error.Code.Should()
            .Be(ProductErrors.NotFound(command.Id).Code);
        result.Error.Description.Should()
            .Be(ProductErrors.NotFound(command.Id).Description);
    }

    [Test]
    public async Task ShouldReturnNameAlreadyExistsError()
    {
        // Arrange
        var category = await SendAsync(new CreateProductCategory.Command
        {
            Name = "Category",
            Description = "Desc"
        });

        var product1 = await SendAsync(new CreateProduct.Command
        {
            Name = "Prod1",
            Description = "Desc1",
            Price = 10,
            ProductCategoryId = category.Value.Id
        });

        var product2 = await SendAsync(new CreateProduct.Command
        {
            Name = "Prod2",
            Description = "Desc2",
            Price = 20,
            ProductCategoryId = category.Value.Id
        });

        var command = new UpdateProductById.Command
        {
            Id = product2.Value.Id,
            Name = "Prod1", // već postoji
            Description = "Updated desc",
            Price = 30,
            ProductCategoryId = category.Value.Id
        };

        // Act
        var result = await SendAsync(command);

        // Assert
        result.IsSuccess.Should()
            .BeFalse();
        result.IsFailure.Should()
            .BeTrue();
        result.Error.Should()
            .NotBeNull();
        result.Error.Code.Should()
            .Be(ProductErrors.NameAlreadyExists(command.Name).Code);
        result.Error.Description.Should()
            .Be(ProductErrors.NameAlreadyExists(command.Name).Description);
    }

    [Test]
    public async Task ShouldUpdateProduct()
    {
        // Arrange
        var category = await SendAsync(new CreateProductCategory.Command
        {
            Name = "Category",
            Description = "Desc"
        });

        var product = await SendAsync(new CreateProduct.Command
        {
            Name = "Prod",
            Description = "Desc",
            Price = 10,
            ProductCategoryId = category.Value.Id
        });

        var command = new UpdateProductById.Command
        {
            Id = product.Value.Id,
            Name = "UpdatedProd",
            Description = "Updated desc",
            Price = 99,
            ProductCategoryId = category.Value.Id
        };

        // Act
        var result = await SendAsync(command);

        // Assert
        result.IsSuccess.Should()
            .BeTrue();
        result.Value.Id.Should()
            .Be(product.Value.Id);

        var foundEntity = await FindAsync<Product>(
            product.Value.Id);
        foundEntity.Should()
            .NotBeNull();
        foundEntity.Name.Should()
            .Be(command.Name);
        foundEntity.Description.Should()
            .Be(command.Description);
        foundEntity.Price.Should()
            .Be(command.Price);
        foundEntity.ProductCategoryId.Should()
            .Be(command.ProductCategoryId);
        foundEntity.UpdatedAt.Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(30));
        foundEntity.IsActive.Should()
            .BeTrue();
    }
}
