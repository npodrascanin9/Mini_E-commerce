namespace Api.UnitTests.Features.Products;

[TestFixture]
public class ExportExcelProductsMapperTests
{
    [Test]
    public void ShouldMapProductToExcelRowDto()
    {
        // Arrange
        var query = new ExportExcelProducts.Query();
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
            ProductCategory = productCategory
        };

        // Act
        var dto = query.ToExcelRowDto(product);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(product.Id);
        dto.Name.Should().Be(product.Name);
        dto.Price.Should().Be(product.Price);
        dto.Description.Should().Be(product.Description);
        dto.ProductCategoryId.Should().Be(product.ProductCategoryId);
        dto.ProductCategoryName.Should().Be(productCategory.Name);
    }

    [Test]
    public void ShouldHandleNullDescription()
    {
        // Arrange
        var query = new ExportExcelProducts.Query();
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
            ProductCategory = productCategory
        };

        // Act
        var dto = query.ToExcelRowDto(product);

        // Assert
        dto.Description.Should().BeNull();
        dto.ProductCategoryName.Should().Be(productCategory.Name);
    }
}
