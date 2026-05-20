namespace Api.IntegrationTests.Features.ProductCategories;

using static Testing;

[TestFixture]
public class DeleteProductCategoryByIdTests :
    BaseIntegrationTest
{
    [Test]
    public async Task ShouldThrowValidationException()
    {
        // Arrange
        var command = new DeleteProductCategoryById.Command(0);

        // Act & Assert
        await FluentActions.Invoking(
            async () => await SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new DeleteProductCategoryById.Command(9999);

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
            .Be(ProductCategoryErrors.NotFound(command.Id).Code);
        result.Error.Description.Should()
            .Be(ProductCategoryErrors.NotFound(command.Id).Description);
    }

    [Test]
    public async Task ShouldDeleteProductCategory()
    {
        // Arrange
        var createCommand = new CreateProductCategory.Command
        {
            Name = "Category to delete",
            Description = "Desc"
        };

        var createResult = await SendAsync(createCommand);
        var categoryId = createResult.Value.Id;

        var deleteCommand = new DeleteProductCategoryById.Command(categoryId);

        // Act
        var deleteResult = await SendAsync(deleteCommand);

        // Assert
        deleteResult.Should()
            .NotBeNull();
        deleteResult.IsSuccess.Should()
            .BeTrue();
        deleteResult.Value.Should()
            .NotBeNull();
        deleteResult.Value.Id.Should()
            .Be(categoryId);

        // Double-check that entity is gone
        var foundEntity = await FindAsync<ProductCategory>(categoryId);
        foundEntity.Should()
            .BeNull();
    }
}
