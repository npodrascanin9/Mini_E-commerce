namespace Api.IntegrationTests.Features.Products.Articles;

using static Testing;

[TestFixture]
public class CreateArticleForProductTests :
    BaseIntegrationTest
{
    [Test]
    public async Task ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateArticleForProduct.Command
        {
            ProductId = 0,
            Barcode = null,
            Color = null,
            Size = null,
            ExpirationDate = null
        };

        // Act & Assert
        await FluentActions.Invoking(
            async () => await SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldReturnProductNotFoundError()
    {
        // Arrange
        var command = new CreateArticleForProduct.Command
        {
            ProductId = 999,
            Barcode = "ABC123",
            Color = "Red",
            Size = "M",
            ExpirationDate = DateTime.UtcNow.AddDays(30)
        };

        // Act
        var result = await SendAsync(command);

        // Assert
        result.Should()
            .NotBeNull();
        result.IsSuccess.Should()
            .BeFalse();
        result.IsFailure.Should()
            .BeTrue();
        result.Error.Should()
            .NotBeNull();
        result.Error.Code.Should()
            .Be(ArticleForProductErrors.ProductNotFound(command.ProductId).Code);
        result.Error.Description.Should()
            .Be(ArticleForProductErrors.ProductNotFound(command.ProductId).Description);
    }

    [Test]
    public async Task ShouldCreateArticleForProduct()
    {
        // Arrange
        var categoryCommand = new CreateProductCategory.Command
        {
            Name = "Article Category",
            Description = "Desc"
        };

        var categoryResult = await SendAsync(categoryCommand);

        var productCommand = new CreateProduct.Command
        {
            Name = "Article Product",
            Description = "Desc",
            Price = 50,
            ProductCategoryId = categoryResult.Value.Id
        };

        var productResult = await SendAsync(productCommand);

        var command = new CreateArticleForProduct.Command
        {
            ProductId = productResult.Value.Id,
            Barcode = "BAR123",
            Color = "Blue",
            Size = "L",
            ExpirationDate = DateTime.UtcNow.AddDays(60)
        };

        // Act
        var result = await SendAsync(command);

        // Assert
        result.Should()
            .NotBeNull();
        result.IsSuccess.Should()
            .BeTrue();
        result.Value.Should()
            .NotBeNull();
        result.Value.Id.Should()
            .BeGreaterThan(0);
        result.Value.ProductId.Should()
            .Be(productResult.Value.Id);

        var foundEntity = await FindAsync<Article>(result.Value.Id);
        foundEntity.Should()
            .NotBeNull();
        foundEntity.Id.Should()
            .Be(result.Value.Id);
        foundEntity.ProductId.Should()
            .Be(command.ProductId);
        foundEntity.Barcode.Should()
            .Be(command.Barcode);
        foundEntity.Color.Should()
            .Be(command.Color);
        foundEntity.Size.Should()
            .Be(command.Size);
        foundEntity.ExpirationDate.Should()
            .NotBeNull();
        foundEntity.ExpirationDate.Value.ToString("yyyy-MM-dd").Should()
            .Be(command.ExpirationDate.Value.ToString("yyyy-MM-dd"));
        foundEntity.IsActive.Should()
            .BeTrue();
        foundEntity.CreatedAt.Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(30));
        foundEntity.UpdatedAt.Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(30));
    }
}
