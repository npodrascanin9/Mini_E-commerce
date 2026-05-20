namespace Api.UnitTests.Features.Products.Articles;

[TestFixture]
public class GetArticlesForProductMapperTests
{
    [Test]
    public void ShouldMapArticleToRowDto()
    {
        // Arrange
        var product = new Product
        {
            Id = 10,
            Name = "TestProduct"
        };
        var query = new GetArticlesForProduct.Query(
            product.Id);

        var article = new Article
        {
            Id = 1,
            ProductId = product.Id,
            Product = product,
            Barcode = "BARCODE123",
            Size = "XL",
            ExpirationDate = DateTime.UtcNow.AddDays(30),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = query.ToRowDto(article);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(article.Id);
        dto.ProductId.Should().Be(article.ProductId);
        dto.ProductName.Should().Be(product.Name);
        dto.Barcode.Should().Be(article.Barcode);
        dto.Size.Should().Be(article.Size);
        dto.ExpirationDate.Should().Be(article.ExpirationDate);
        dto.IsActive.Should().BeTrue();
        dto.CreatedAt.Should().BeCloseTo(article.CreatedAt, TimeSpan.FromSeconds(1));
        dto.UpdatedAt.Should().BeCloseTo(article.UpdatedAt, TimeSpan.FromSeconds(1));
    }

    [Test]
    public void ShouldHandleNullOptionalFields()
    {
        // Arrange
        var product = new Product
        {
            Id = 20,
            Name = "AnotherProduct"
        };
        var query = new GetArticlesForProduct.Query(
            product.Id);

        var article = new Article
        {
            Id = 2,
            ProductId = product.Id,
            Product = product,
            Barcode = null,
            Size = null,
            ExpirationDate = null,
            IsActive = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = query.ToRowDto(article);

        // Assert
        dto.Barcode.Should().BeNull();
        dto.Size.Should().BeNull();
        dto.ExpirationDate.Should().BeNull();
        dto.IsActive.Should().BeFalse();
        dto.ProductName.Should().Be(product.Name);
    }
}
