namespace Api.UnitTests.Features.ProductCategories;

public class UpdateProductCategoryMapperTests :
    BaseUnitTest
{
    [Test]
    public void ShouldMapToExistingEntity()
    {
        // Arrange
        string oldName = "My old name";
        string oldDescription = "My old description";
        var entity = new ProductCategory
        {
            Id = 1,
            Name = oldName,
            IsActive = false,
            Description = oldDescription,
            CreatedAt = DateTime.Now.AddDays(-1),
            UpdatedAt = DateTime.Now.AddDays(1)
        };
        var command = new UpdateProductCategory.Command()
        {
            Id = entity.Id,
            Description = "New desc",
            Name = "New name"
        };

        // Act
        command.MapEntity(entity);

        // Assert
        entity.Id.Should()
            .Be(command.Id);
        entity.Name.Should()
            .NotBe(oldName)
            .And.Be(command.Name);
        entity.Description.Should()
            .NotBe(oldDescription)
            .And.Be(command.Description);
        entity.CreatedAt.Should()
            .BeCloseTo(DateTime.Now.AddDays(-1), TimeSpan.FromSeconds(30));
        entity.UpdatedAt.Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(30));
        entity.IsActive.Should()
            .BeTrue();
    }
}
