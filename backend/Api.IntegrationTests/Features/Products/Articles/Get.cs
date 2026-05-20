namespace Api.IntegrationTests.Features.Products.Articles;

using static Testing;

[TestFixture]
public class GetArticlesForProductTests :
    BaseIntegrationTest
{
    [Test]
    public async Task ShouldReturnProductNotFoundError()
    {
        // Arrange
        var query = new GetArticlesForProduct.Query(999);

        // Act
        var result = await SendAsync(query);

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
            .Be(ArticleForProductErrors.ProductNotFound(query.ProductId).Code);
        result.Error.Description.Should()
            .Be(ArticleForProductErrors.ProductNotFound(query.ProductId).Description);
    }

    [Test]
    public async Task ShouldReturnEmptyList()
    {
        // Arrange
        var categoryCommand = await SendAsync(new CreateProductCategory.Command
        {
            Name = "GetArticles Category",
            Description = "Desc"
        });

        var productCommand = await SendAsync(new CreateProduct.Command
        {
            Name = "GetArticles Product",
            Description = "Desc",
            Price = 100,
            ProductCategoryId = categoryCommand.Value.Id
        });

        var query = new GetArticlesForProduct.Query(
            productCommand.Value.Id);

        // Act
        var result = await SendAsync(query);

        // Assert
        result.IsSuccess.Should()
            .BeTrue();
        result.Value.Count.Should()
            .Be(0);
        result.Value.Rows.Should()
            .NotBeNull()
            .And.BeEmpty();
    }

    [Test]
    public async Task ShouldReturnArticlesForProduct()
    {
        // Arrange
        var categoryCommand = new CreateProductCategory.Command
        {
            Name = "GetArticles Category",
            Description = "Desc"
        };
        var categoryCommandResult = await SendAsync(categoryCommand);

        var productCommand = new CreateProduct.Command
        {
            Name = "GetArticles Product",
            Description = "Desc",
            Price = 100,
            ProductCategoryId = categoryCommandResult.Value.Id
        };
        var productCommandResult = await SendAsync(productCommand);

        var articleForProductCommand1 = new CreateArticleForProduct.Command
        {
            ProductId = productCommandResult.Value.Id,
            Barcode = "ART001",
            Color = "Red",
            Size = "M",
            ExpirationDate = DateTime.UtcNow.AddDays(30)
        };
        var articleForProductCommandResult1 = await SendAsync(articleForProductCommand1);

        var articleForProductCommand2 = new CreateArticleForProduct.Command
        {
            ProductId = productCommandResult.Value.Id,
            Barcode = "ART002",
            Color = "Blue",
            Size = "L",
            ExpirationDate = DateTime.UtcNow.AddDays(60)
        };
        var articleForProductCommandResult2 = await SendAsync(articleForProductCommand2);

        var query = new GetArticlesForProduct.Query(productCommandResult.Value.Id);

        // Act
        var result = await SendAsync(query);

        // Assert
        result.IsSuccess.Should()
            .BeTrue();
        result.Value.Count.Should()
            .Be(2);
        result.Value.Rows.Should()
            .HaveCount(2);

        var row1 = result.Value.Rows.First(
            x => x.Id == articleForProductCommandResult1.Value.Id);
        row1.ProductId.Should()
            .Be(productCommandResult.Value.Id);
        row1.ProductName.Should()
            .Be(productCommand.Name);
        row1.Barcode.Should()
            .Be(articleForProductCommand1.Barcode);
        row1.Size.Should()
            .Be(articleForProductCommand1.Size);
        row1.IsActive.Should()
            .BeTrue();

        var row2 = result.Value.Rows.First(
            x => x.Id == articleForProductCommandResult2.Value.Id);
        row2.ProductId.Should()
            .Be(productCommandResult.Value.Id);
        row2.ProductName.Should()
            .Be(productCommand.Name);
        row2.Barcode.Should()
            .Be(articleForProductCommand2.Barcode);
        row2.Size.Should()
            .Be(articleForProductCommand2.Size);
        row2.IsActive.Should()
            .BeTrue();
    }
}
