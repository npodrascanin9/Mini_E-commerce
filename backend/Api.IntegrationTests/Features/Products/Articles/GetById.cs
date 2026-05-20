namespace Api.IntegrationTests.Features.Products.Articles;

using static Testing;

[TestFixture]
public class GetArticleForProductByIdTests :
    BaseIntegrationTest
{
    [Test]
    public async Task ShouldReturnNotFoundError()
    {
        // Arrange
        var query = new GetArticleForProductById.Query(
            Id: 999,
            ProductId: 999);

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
            .Be(ArticleForProductErrors.NotFound(query.Id, query.ProductId).Code);
        result.Error.Description.Should()
            .Be(ArticleForProductErrors.NotFound(query.Id, query.ProductId).Description);
    }

    [Test]
    public async Task ShouldReturnExpectedData()
    {
        // Arrange
        var categoryCommand = new CreateProductCategory.Command
        {
            Name = "GetArticleById Category",
            Description = "Desc"
        };
        var categoryCommandResult = await SendAsync(categoryCommand);

        var productCommand = new CreateProduct.Command
        {
            Name = "GetArticleById Product",
            Description = "Desc",
            Price = 100,
            ProductCategoryId = categoryCommandResult.Value.Id
        };
        var productCommandResult = await SendAsync(productCommand);

        var articleForProductCommand = new CreateArticleForProduct.Command
        {
            ProductId = productCommandResult.Value.Id,
            Barcode = "ARTBYID",
            Color = "Black",
            Size = "XL",
            ExpirationDate = DateTime.UtcNow.AddDays(45)
        };
        var articleForProductCommandResult = await SendAsync(articleForProductCommand);

        var query = new GetArticleForProductById.Query(
            Id: articleForProductCommandResult.Value.Id,
            ProductId: productCommandResult.Value.Id);

        // Act
        var result = await SendAsync(query);

        // Assert
        result.IsSuccess.Should()
            .BeTrue();
        result.Value.Should()
            .NotBeNull();
        result.Value.Id.Should()
            .Be(articleForProductCommandResult.Value.Id);
        result.Value.ProductId.Should()
            .Be(productCommandResult.Value.Id);
        result.Value.Barcode.Should()
            .Be(articleForProductCommand.Barcode);
        result.Value.Size.Should()
            .Be(articleForProductCommand.Size);
        result.Value.ExpirationDate.Should()
            .NotBeNull();
        result.Value.ExpirationDate.Value.Date.Should()
            .Be(articleForProductCommand.ExpirationDate.Value.Date);
        result.Value.CreatedAt.Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(30));
        result.Value.UpdatedAt.Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(30));
    }
}
