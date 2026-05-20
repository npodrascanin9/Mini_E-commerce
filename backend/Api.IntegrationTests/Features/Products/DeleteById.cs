namespace Api.IntegrationTests.Features.Products;

using static Testing;

[TestFixture]
public class DeleteProductByIdTests :
    BaseIntegrationTest
{
    [Test]
    public async Task ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new DeleteProductById.Command(999);

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
            .Be(ProductErrors.NotFound(command.Id).Code);
        result.Error.Description.Should()
            .Be(ProductErrors.NotFound(command.Id).Description);
    }

    [Test]
    public async Task ShouldDeleteProductWithoutArticles()
    {
        // Arrange
        var category = new CreateProductCategory.Command
        {
            Name = "Delete Category",
            Description = "Desc"
        };

        var categoryResult = await SendAsync(category);

        var product = new CreateProduct.Command
        {
            Name = "Delete Product",
            Description = "Desc",
            Price = 10,
            ProductCategoryId = categoryResult.Value.Id
        };

        var productResult = await SendAsync(product);

        var command = new DeleteProductById.Command(productResult.Value.Id);

        // Act
        var result = await SendAsync(command);

        // Assert
        result.Should()
            .NotBeNull();
        result.IsSuccess.Should()
            .BeTrue();
        result.IsFailure.Should()
            .BeFalse();
        result.Value.Should()
            .NotBeNull();
        result.Value.Id.Should()
            .Be(productResult.Value.Id);

        var foundEntity = await FindAsync<Product>(result.Value.Id);
        foundEntity.Should()
            .BeNull();
    }

    [Test]
    public async Task ShouldDeleteProductWithArticles()
    {
        // Arrange
        var category = new CreateProductCategory.Command
        {
            Name = "Delete Category With Articles",
            Description = "Desc"
        };

        var categoryResult = await SendAsync(category);

        var product = new CreateProduct.Command
        {
            Name = "Delete Product With Articles",
            Description = "Desc",
            Price = 10,
            ProductCategoryId = categoryResult.Value.Id
        };

        var productResult = await SendAsync(product);

        var article = new CreateArticleForProduct.Command
        {
            ProductId = productResult.Value.Id,
            Barcode = "12345",
            Color = "Red",
            Size = "M",
            ExpirationDate = DateTime.UtcNow.AddDays(30)
        };

        await SendAsync(article);

        var command = new DeleteProductById.Command(
            productResult.Value.Id);

        // Act
        var result = await SendAsync(command);

        // Assert
        result.Should()
            .NotBeNull();
        result.IsSuccess.Should()
            .BeTrue();
        result.IsFailure.Should()
            .BeFalse();
        result.Value.Should()
            .NotBeNull();
        result.Value.Id.Should()
            .Be(productResult.Value.Id);

        var foundProduct = await FindAsync<Product>(
            result.Value.Id);
        foundProduct.Should()
            .BeNull();

        var foundArticles = await GetList<Article>(
            x => x.ProductId == productResult.Value.Id);
        foundArticles.Should()
            .BeEmpty();
    }
}
