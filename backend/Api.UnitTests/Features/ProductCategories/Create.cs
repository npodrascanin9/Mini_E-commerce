namespace Api.UnitTests.Features.ProductCategories;

public class CreateProductCategoryMapperTests :
    BaseUnitTest
{
    [Test]
    public void ShouldCreateEntityObject()
    {
        // Arrange
        var command = new CreateProductCategory.Command()
        {
            Name = "Category name",
            Description = "Category description"
        };

        // Act
        var result = command.ToEntity();

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeOfType<ProductCategory>();
        result.Id.Should()
            .Be(0);
        result.Name.Should()
            .Be("Category name");
        result.Description.Should()
            .Be("Category description");
        result.IsActive.Should()
            .BeTrue();
        result.CreatedAt.Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(30));
        result.UpdatedAt.Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(30));
    }
}
