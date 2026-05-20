namespace Api.IntegrationTests.Features.ProductCategories;

using static Testing;

[TestFixture]
public class UpdateProductCategoryTests :
    BaseIntegrationTest
{
    [Test]
    public async Task ShouldThrowValidationException()
    {
        // Arrange
        var command = new UpdateProductCategory.Command
        {
            Id = 1,
            Name = "",
            Description = "Desc"
        };

        // Act & Assert
        await FluentActions.Invoking(
            async () => await SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new UpdateProductCategory.Command
        {
            Id = 9999,
            Name = "Updated",
            Description = "Updated desc"
        };

        // Act
        var result = await SendAsync(command);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be(ProductCategoryErrors.NotFound(command.Id).Code);
    }

    [Test]
    public async Task ShouldUpdateRecord()
    {
        // Arrange
        var createCommand = new CreateProductCategory.Command
        {
            Name = "Original",
            Description = "Original desc"
        };

        var createResult = await SendAsync(createCommand);
        var categoryId = createResult.Value.Id;

        var updateCommand = new UpdateProductCategory.Command
        {
            Id = categoryId,
            Name = "Updated",
            Description = "Updated desc"
        };

        // Act
        var result = await SendAsync(updateCommand);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(categoryId);

        var updatedEntity = await FindAsync<ProductCategory>(categoryId);
        updatedEntity.Should().NotBeNull();
        updatedEntity!.Name.Should().Be("Updated");
        updatedEntity.Description.Should().Be("Updated desc");
        updatedEntity.IsActive.Should().BeTrue();
    }
}
