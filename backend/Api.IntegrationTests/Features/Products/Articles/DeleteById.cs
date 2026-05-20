namespace Api.IntegrationTests.Features.Products.Articles;

using static Testing;

[TestFixture]
public class DeleteArticleForProductTests :
    BaseIntegrationTest
{
    [Test]
    public async Task ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new DeleteArticleForProduct.Command(
            Id: 999,
            ProductId: 999);

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
            .Be(ArticleForProductErrors.NotFound(command.Id, command.ProductId).Code);
        result.Error.Description.Should()
            .Be(ArticleForProductErrors.NotFound(command.Id, command.ProductId).Description);
    }

    [Test]
    public async Task ShouldDeleteArticleForProduct()
    {
        // Arrange
        var categoryCommand = new CreateProductCategory.Command
        {
            Name = "DeleteArticle Category",
            Description = "Desc"
        };

        var categoryResult = await SendAsync(categoryCommand);

        var productCommand = new CreateProduct.Command
        {
            Name = "DeleteArticle Product",
            Description = "Desc",
            Price = 50,
            ProductCategoryId = categoryResult.Value.Id
        };

        var productResult = await SendAsync(productCommand);

        var articleCommand = new CreateArticleForProduct.Command
        {
            ProductId = productResult.Value.Id,
            Barcode = "DEL123",
            Color = "Green",
            Size = "XL",
            ExpirationDate = DateTime.UtcNow.AddDays(90)
        };

        var articleResult = await SendAsync(articleCommand);

        var command = new DeleteArticleForProduct.Command(
            Id: articleResult.Value.Id,
            ProductId: productResult.Value.Id);

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
            .Be(articleResult.Value.Id);

        var foundEntity = await FindAsync<Article>(
            articleResult.Value.Id);
        foundEntity.Should()
            .BeNull();
    }
}
