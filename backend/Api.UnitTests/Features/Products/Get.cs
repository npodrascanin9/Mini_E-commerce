namespace Api.UnitTests.Features.Products;

[TestFixture]
public class GetProductsMapperTests
{
    [Test]
    public void ShouldMapProductToRowDto()
    {
        // Arrange
        var query = new GetProducts.Query();
        var productCategory = new ProductCategory
        {
            Id = 10,
            Name = "CategoryName"
        };

        var product = new Product
        {
            Id = 1,
            Name = "TestProduct",
            Price = 99.99m,
            Description = "TestDescription",
            ProductCategoryId = productCategory.Id,
            ProductCategory = productCategory,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = query.ToRowDto(product);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(product.Id);
        dto.Name.Should().Be(product.Name);
        dto.Description.Should().Be(product.Description);
        dto.Price.Should().Be(product.Price);
        dto.ProductCategoryId.Should().Be(product.ProductCategoryId);
        dto.ProductCategoryName.Should().Be(productCategory.Name);
        dto.IsActive.Should().BeTrue();
        dto.CreatedAt.Should().BeCloseTo(product.CreatedAt, TimeSpan.FromSeconds(1));
        dto.UpdatedAt.Should().BeCloseTo(product.UpdatedAt, TimeSpan.FromSeconds(1));
    }

    [Test]
    public void ShouldHandleNullDescription()
    {
        // Arrange
        var query = new GetProducts.Query();
        var productCategory = new ProductCategory
        {
            Id = 20,
            Name = "AnotherCategory"
        };

        var product = new Product
        {
            Id = 2,
            Name = "ProductWithoutDescription",
            Price = 50,
            Description = null,
            ProductCategoryId = productCategory.Id,
            ProductCategory = productCategory,
            IsActive = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = query.ToRowDto(product);

        // Assert
        dto.Description.Should().BeNull();
        dto.ProductCategoryName.Should().Be(productCategory.Name);
        dto.IsActive.Should().BeFalse();
    }
}
