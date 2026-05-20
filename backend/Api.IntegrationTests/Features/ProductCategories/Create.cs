namespace Api.IntegrationTests.Features.ProductCategories;

using static Testing;

[TestFixture]
public class CreateProductCategoryTests :
    BaseIntegrationTest
{
    [Test]
    public async Task ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateProductCategory.Command
        {
            Name = "",
            Description = ""
        };

        // Act & Assert
        _ = await FluentActions.Invoking(
            async () => await SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateProduct()
    {
        // Arrange
        var command = new CreateProductCategory.Command
        {
            Name = "Test Category",
            Description = "Test desc"
        };

        // Act
        var result = await SendAsync(command);

        // Arrange
        result.Should()
            .NotBeNull();
        result.IsSuccess.Should()
            .BeTrue();
        result.IsFailure.Should()
            .BeFalse();
        result.Value.Should()
            .NotBeNull();
        result.Value.Id.Should()
            .BeGreaterThan(0);
    }
} 
