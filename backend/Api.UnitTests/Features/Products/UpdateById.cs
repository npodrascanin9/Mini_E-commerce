namespace Api.UnitTests.Features.Products;

[TestFixture]
public class UpdateProductByIdMapperTests
{
    [Test]
    public void ShouldMapCommandToProductEntity()
    {
        // Arrange
        var command = new UpdateProductById.Command
        {
            Id = 1,
            Name = "UpdatedProduct",
            Description = "UpdatedDescription",
            Price = 199.99m,
            ProductCategoryId = 5
        };

        var product = new Product
        {
            Id = 1,
            Name = "OldProduct",
            Description = "OldDescription",
            Price = 99.99m,
            ProductCategoryId = 2,
            UpdatedAt = DateTime.UtcNow.AddDays(-1),
            IsActive = false
        };

        // Act
        command.MapEntity(product);

        // Assert
        product.Name.Should().Be(command.Name);
        product.Description.Should().Be(command.Description);
        product.Price.Should().Be(command.Price);
        product.ProductCategoryId.Should().Be(command.ProductCategoryId);
        product.IsActive.Should().BeTrue();
        product.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Test]
    public void ShouldHandleNullDescription()
    {
        // Arrange
        var command = new UpdateProductById.Command
        {
            Id = 2,
            Name = "ProductWithoutDescription",
            Description = null,
            Price = 50,
            ProductCategoryId = 10
        };

        var product = new Product
        {
            Id = 2,
            Name = "OldName",
            Description = "OldDescription",
            Price = 40,
            ProductCategoryId = 8,
            UpdatedAt = DateTime.UtcNow.AddDays(-2),
            IsActive = false
        };

        // Act
        command.MapEntity(product);

        // Assert
        product.Description.Should().BeNull();
        product.Name.Should().Be(command.Name);
        product.Price.Should().Be(command.Price);
        product.ProductCategoryId.Should().Be(command.ProductCategoryId);
        product.IsActive.Should().BeTrue();
        product.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }
}
