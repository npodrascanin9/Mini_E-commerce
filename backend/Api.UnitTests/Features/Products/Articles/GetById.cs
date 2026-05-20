namespace Api.UnitTests.Features.Products.Articles;

[TestFixture]
public class GetArticleForProductByIdMapperTests
{
    [Test]
    public void ShouldMapArticleToResponse()
    {
        // Arrange
        var query = new GetArticleForProductById.Query(
            Id: 1, 
            ProductId: 10);

        var article = new Article
        {
            Id = 1,
            ProductId = 10,
            Barcode = "BARCODE123",
            Size = "XL",
            ExpirationDate = DateTime.UtcNow.AddDays(30),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var response = query.ToResponse(article);

        // Assert
        response.Should().NotBeNull();
        response.Id.Should().Be(article.Id);
        response.ProductId.Should().Be(article.ProductId);
        response.Barcode.Should().Be(article.Barcode);
        response.Size.Should().Be(article.Size);
        response.ExpirationDate.Should().Be(article.ExpirationDate);
        response.CreatedAt.Should().BeCloseTo(article.CreatedAt, TimeSpan.FromSeconds(1));
        response.UpdatedAt.Should().BeCloseTo(article.UpdatedAt, TimeSpan.FromSeconds(1));
    }

    [Test]
    public void ShouldHandleNullOptionalFields()
    {
        // Arrange
        var query = new GetArticleForProductById.Query(
            Id: 2, 
            ProductId: 20);

        var article = new Article
        {
            Id = 2,
            ProductId = 20,
            Barcode = null,
            Size = null,
            ExpirationDate = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var response = query.ToResponse(article);

        // Assert
        response.Barcode.Should().BeNull();
        response.Size.Should().BeNull();
        response.ExpirationDate.Should().BeNull();
        response.ProductId.Should().Be(article.ProductId);
    }
}
