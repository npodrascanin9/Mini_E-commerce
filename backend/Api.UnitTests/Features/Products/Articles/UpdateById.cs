namespace Api.UnitTests.Features.Products.Articles;

[TestFixture]
public class UpdateArticleForProductByIdMapperTests
{
    [Test]
    public void ShouldMapCommandToArticleEntity()
    {
        // Arrange
        var command = new UpdateArticleForProductById.Command
        {
            Id = 1,
            ProductId = 10,
            Barcode = "UPDATED123",
            Size = "L",
            Color = "Blue",
            ExpirationDate = DateTime.UtcNow.AddDays(60)
        };

        var article = new Article
        {
            Id = 1,
            ProductId = 10,
            Barcode = "OLD123",
            Size = "M",
            Color = "Red",
            ExpirationDate = DateTime.UtcNow.AddDays(30),
            IsActive = false,
            UpdatedAt = DateTime.UtcNow.AddDays(-1)
        };

        // Act
        command.MapEntity(article);

        // Assert
        article.Barcode.Should().Be(command.Barcode);
        article.Size.Should().Be(command.Size);
        article.Color.Should().Be(command.Color);
        article.ExpirationDate.Should().Be(command.ExpirationDate);
        article.IsActive.Should().BeTrue();
        article.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Test]
    public void ShouldHandleNullOptionalFields()
    {
        // Arrange
        var command = new UpdateArticleForProductById.Command
        {
            Id = 2,
            ProductId = 20,
            Barcode = null,
            Size = null,
            Color = null,
            ExpirationDate = null
        };

        var article = new Article
        {
            Id = 2,
            ProductId = 20,
            Barcode = "OLD",
            Size = "S",
            Color = "Green",
            ExpirationDate = DateTime.UtcNow.AddDays(10),
            IsActive = false,
            UpdatedAt = DateTime.UtcNow.AddDays(-2)
        };

        // Act
        command.MapEntity(article);

        // Assert
        article.Barcode.Should().BeNull();
        article.Size.Should().BeNull();
        article.Color.Should().BeNull();
        article.ExpirationDate.Should().BeNull();
        article.IsActive.Should().BeTrue();
        article.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }
}
