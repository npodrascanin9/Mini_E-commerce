namespace Api.UnitTests.Features.Products;

[TestFixture]
public class GetProductByIdMapperTests
{
    [Test]
    public void ShouldMapProductToResponse()
    {
        // Arrange
        var query = new GetProductById.Query(Id: 1);
        var product = new Product
        {
            Id = 1,
            Name = "TestProduct",
            Description = "TestDescription",
            Price = 99.99m,
            ProductCategoryId = 10,
            ProductCategory = new ProductCategory
            {
                Id = 10,
                Name = "CategoryName"
            }
        };

        // Act
        var response = query.ToResponse(product);

        // Assert
        response.Should().NotBeNull();
        response.Id.Should().Be(product.Id);
        response.Name.Should().Be(product.Name);
        response.Description.Should().Be(product.Description);
        response.Price.Should().Be(product.Price);
        response.ProductCategoryId.Should().Be(product.ProductCategoryId);
    }

    [Test]
    public void ShouldHandleNullDescription()
    {
        // Arrange
        var query = new GetProductById.Query(Id: 2);
        var product = new Product
        {
            Id = 2,
            Name = "ProductWithoutDescription",
            Description = null,
            Price = 50,
            ProductCategoryId = 20,
            ProductCategory = new ProductCategory
            {
                Id = 20,
                Name = "AnotherCategory"
            }
        };

        // Act
        var response = query.ToResponse(product);

        // Assert
        response.Description.Should().BeNull();
        response.Name.Should().Be(product.Name);
        response.ProductCategoryId.Should().Be(product.ProductCategoryId);
    }
}
