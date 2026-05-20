namespace Api.UnitTests.Features.ProductCategories;

public class ProductCategoryErrorTests :
    BaseUnitTest
{
    [Test]
    public void ShouldCreateNotFoundErrorObject()
    {
        // Arrange
        const int id = 5;

        // Act
        var result = ProductCategoryErrors.NotFound(id);

        // Assert
        result.Should()
            .NotBeNull();
        result.Code.Should()
            .Be("ProductCategory.NotFound");
        result.Description.Should()
            .Be($"Product category record with Id='{id}' not found");
    }
}
