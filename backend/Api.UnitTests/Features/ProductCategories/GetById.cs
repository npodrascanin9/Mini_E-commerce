namespace Api.UnitTests.Features.ProductCategories;

public class GetProductCategoryByIdMapperTests :
    BaseUnitTest
{
    [Test]
    public void ShouldCreateResponseObject()
    {
        // Arrange
        var entity = new ProductCategory
        {
            Id = 1,
            Description = "My Desc",
            IsActive = true,
            Name = "My Name",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now.AddDays(1)
        };
        var query = new GetProductCategoryById.Query(
            entity.Id);

        // Act
        var result = query.ToResponse(entity);

        // Assert
        result.Should()
            .NotBeNull();
        result.Id.Should()
            .Be(entity.Id);
        result.Name.Should()
            .Be(entity.Name);
        result.Description.Should()
            .Be(entity.Description);
        result.CreatedAt.Should()
            .Be(entity.CreatedAt);
        result.UpdatedAt.Should()
            .Be(entity.UpdatedAt);
    }
}
