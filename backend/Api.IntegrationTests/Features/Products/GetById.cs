namespace Api.IntegrationTests.Features.Products;

using static Testing;

[TestFixture]
public class GetProductByIdTests :
    BaseIntegrationTest
{
    [Test]
    public async Task ShouldReturnNotFoundError()
    {
        // Arrange
        var query = new GetProductById.Query(999);

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
            .Be(ProductErrors.NotFound(query.Id).Code);
        result.Error.Description.Should()
            .Be(ProductErrors.NotFound(query.Id).Description);
    }

    [Test]
    public async Task ShouldReturnData()
    {
        // Arrange
        var categoryCommand = new CreateProductCategory.Command
        {
            Name = "GetById Category",
            Description = "Desc"
        };

        var categoryResult = await SendAsync(categoryCommand);

        var productCommand = new CreateProduct.Command
        {
            Name = "GetById Product",
            Description = "Desc",
            Price = 100,
            ProductCategoryId = categoryResult.Value.Id
        };

        var productResult = await SendAsync(productCommand);

        var query = new GetProductById.Query(productResult.Value.Id);

        // Act
        var result = await SendAsync(query);

        // Assert
        result.Should()
            .NotBeNull();
        result.IsSuccess.Should()
            .BeTrue();
        result.Value.Should()
            .NotBeNull();
        result.Value.Id.Should()
            .Be(productResult.Value.Id);
        result.Value.Name.Should()
            .Be(productCommand.Name);
        result.Value.Description.Should()
            .Be(productCommand.Description);
        result.Value.Price.Should()
            .Be(productCommand.Price);
        result.Value.ProductCategoryId.Should()
            .Be(categoryResult.Value.Id);
    }
}
