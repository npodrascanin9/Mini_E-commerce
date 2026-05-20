namespace Api.UnitTests.Features.Products;

[TestFixture]
public class ProductErrorsTests
{
    [Test]
    public void ShouldReturnNameAlreadyExists()
    {
        // Arrange
        var productName = "TestProduct";

        // Act
        var error = ProductErrors.NameAlreadyExists(productName);

        // Assert
        error.Code.Should()
            .Be("Products.BadRequest");
        error.Description.Should()
            .Be($"Product Name='{productName}' already exists");
    }

    [Test]
    public void ShouldReturnNotFoundError()
    {
        // Arrange
        var id = 123;

        // Act
        var error = ProductErrors.NotFound(id);

        // Assert
        error.Code.Should()
            .Be("Products.NotFound");
        error.Description.Should()
            .Be($"Product with Id='{id}' not found");
    }

    [Test]
    public void ShouldReturnProductCategoryNotFound()
    {
        // Arrange
        var categoryId = 456;

        // Act
        var error = ProductErrors.ProductCategoryNotFound(categoryId);

        // Assert
        error.Code.Should()
            .Be("Products.NotFound");
        error.Description.Should()
            .Be($"Product category with Id='{categoryId}' not found");
    }

    [Test]
    public void ShouldReturnRollbackError()
    {
        // Arrange
        var message = "Rollback error occurred";

        // Act
        var error = ProductErrors.Rollback(message);

        // Assert
        error.Code.Should()
            .Be("Products.BadRequest");
        error.Description.Should()
            .Be(message);
    }
}
