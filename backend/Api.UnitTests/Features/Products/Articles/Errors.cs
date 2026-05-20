namespace Api.UnitTests.Features.Products.Articles;

[TestFixture]
public class ArticleForProductErrorsTests
{
    [Test]
    public void ShouldReturnNotFoundError()
    {
        // Arrange
        var id = 123;
        var productId = 456;

        // Act
        var error = ArticleForProductErrors.NotFound(id, productId);

        // Assert
        error.Code.Should().Be("ArticleForProduct.NotFound");
        error.Description.Should().Be($"Article with Id='{id}' and ProductId='{productId}' not found");
    }

    [Test]
    public void ShouldReturnProductNotFoundError()
    {
        // Arrange
        var productId = 789;

        // Act
        var error = ArticleForProductErrors.ProductNotFound(productId);

        // Assert
        error.Code.Should().Be("ArticleForProduct.NotFound");
        error.Description.Should().Be($"Product with Id='{productId}' not found");
    }
}
