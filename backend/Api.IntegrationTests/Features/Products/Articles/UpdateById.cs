namespace Api.IntegrationTests.Features.Products.Articles;

using static Testing;

[TestFixture]
public class UpdateArticleForProductByIdTests :
    BaseIntegrationTest
{
    [Test]
    public async Task ShouldThrowValidationException()
    {
        // Arrange
        var command = new UpdateArticleForProductById.Command
        {
            Id = 0,
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
        var command = new UpdateArticleForProductById.Command
        {
            Id = 1,
            ProductId = 999,
            Barcode = "NOTFOUND",
            Color = "Red",
            Size = "M",
            ExpirationDate = DateTime.UtcNow.AddDays(10)
        };

        // Act
        var result = await SendAsync(command);

        // Assert
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
    public async Task ShouldReturnNotFoundError()
    {
        // Arrange
        var categoryCommand = new CreateProductCategory.Command
        {
            Name = "UpdateArticle Category",
            Description = "Desc"
        };
        var categoryCommandResult = await SendAsync(categoryCommand);

        var productCommand = new CreateProduct.Command
        {
            Name = "UpdateArticle Product",
            Description = "Desc",
            Price = 100,
            ProductCategoryId = categoryCommandResult.Value.Id
        };
        var productCommandResult = await SendAsync(productCommand);

        var command = new UpdateArticleForProductById.Command
        {
            Id = 999,
            ProductId = productCommandResult.Value.Id,
            Barcode = "NOTFOUND",
            Color = "Red",
            Size = "M",
            ExpirationDate = DateTime.UtcNow.AddDays(10)
        };

        // Act
        var result = await SendAsync(command);

        // Assert
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
    public async Task ShouldUpdateArticleForProduct()
    {
        // Arrange
        var categoryCommand = new CreateProductCategory.Command
        {
            Name = "UpdateArticle Category",
            Description = "Desc"
        };
        var categoryCommandResult = await SendAsync(categoryCommand);

        var productCommand = new CreateProduct.Command
        {
            Name = "UpdateArticle Product",
            Description = "Desc",
            Price = 100,
            ProductCategoryId = categoryCommandResult.Value.Id
        };
        var productCommandResult = await SendAsync(productCommand);

        var articleForProductCommand = new CreateArticleForProduct.Command
        {
            ProductId = productCommandResult.Value.Id,
            Barcode = "UPD001",
            Color = "Green",
            Size = "S",
            ExpirationDate = DateTime.UtcNow.AddDays(20)
        };
        var articleForProductCommandResult = await SendAsync(articleForProductCommand);

        var updateCommand = new UpdateArticleForProductById.Command
        {
            Id = articleForProductCommandResult.Value.Id,
            ProductId = productCommandResult.Value.Id,
            Barcode = "UPDATED001",
            Color = "Black",
            Size = "XL",
            ExpirationDate = DateTime.UtcNow.AddDays(40)
        };

        // Act
        var result = await SendAsync(updateCommand);

        // Assert
        result.IsSuccess.Should()
            .BeTrue();
        result.Value.Id.Should()
            .Be(articleForProductCommandResult.Value.Id);

        var foundEntity = await FindAsync<Article>(articleForProductCommandResult.Value.Id);
        foundEntity.Should()
            .NotBeNull();
        foundEntity.Barcode.Should()
            .Be(updateCommand.Barcode);
        foundEntity.Color.Should()
            .Be(updateCommand.Color);
        foundEntity.Size.Should()
            .Be(updateCommand.Size);
        foundEntity.ExpirationDate.Should()
            .NotBeNull();
        foundEntity.ExpirationDate.Value.Date.Should()
            .Be(updateCommand.ExpirationDate.Value.Date);
        foundEntity.IsActive.Should()
            .BeTrue();
        foundEntity.UpdatedAt.Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(30));
    }
}
