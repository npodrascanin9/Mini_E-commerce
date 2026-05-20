namespace Api.UnitTests.Features.Products.Articles;

[TestFixture]
public class CreateArticleForProductMapperTests
{
    [Test]
    public void ShouldMapCommandToArticleEntity()
    {
        // Arrange
        var command = new CreateArticleForProduct.Command
        {
            ProductId = 1,
            Barcode = "BARCODE123",
            Color = "Red",
            ExpirationDate = DateTime.UtcNow.AddDays(30),
            Size = "M"
        };

        // Act
        var article = command.ToEntity();

        // Assert
        article.Should().NotBeNull();
        article.Id.Should().Be(0); // default
        article.ProductId.Should().Be(command.ProductId);
        article.Barcode.Should().Be(command.Barcode);
        article.Color.Should().Be(command.Color);
        article.ExpirationDate.Should().Be(command.ExpirationDate);
        article.Size.Should().Be(command.Size);
        article.IsActive.Should().BeTrue();
        article.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        article.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Test]
    public void ShouldHandleNullOptionalFields()
    {
        // Arrange
        var command = new CreateArticleForProduct.Command
        {
            ProductId = 2,
            Barcode = null,
            Color = null,
            ExpirationDate = null,
            Size = null
        };

        // Act
        var article = command.ToEntity();

        // Assert
        article.Barcode.Should().BeNull();
        article.Color.Should().BeNull();
        article.ExpirationDate.Should().BeNull();
        article.Size.Should().BeNull();
        article.IsActive.Should().BeTrue();
    }
}
