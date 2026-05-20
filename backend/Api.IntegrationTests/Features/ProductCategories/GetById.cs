namespace Api.IntegrationTests.Features.ProductCategories;

using static Testing;

[TestFixture]
public class GetProductCategoryByIdTests :
    BaseIntegrationTest
{
    [Test]
    public async Task ShouldReturnNotFoundError()
    {
        // Arrange
        var query = new GetProductCategoryById.Query(9999);

        // Act
        var result = await SendAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should()
            .BeTrue();
        result.Error.Should()
            .NotBeNull();
        result.Error.Code.Should()
            .Be(ProductCategoryErrors.NotFound(query.Id).Code);
    }

    [Test]
    public async Task ShouldReturnData()
    {
        // Arrange
        var createCommand = new CreateProductCategory.Command
        {
            Name = "Category by id",
            Description = "Desc"
        };

        var createResult = await SendAsync(createCommand);
        var categoryId = createResult.Value.Id;

        var query = new GetProductCategoryById.Query(categoryId);

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
            .Be(categoryId);
        result.Value.Name.Should()
            .Be("Category by id");
        result.Value.Description.Should()
            .Be("Desc");
    }
}
