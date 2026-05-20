namespace Api.UnitTests.Features.Products;

public class CreateProductMapperTests :
    BaseUnitTest
{
    [Test]
    public void ShouldCreateEntityObject()
    {
        // Arrange
        var command = new CreateProduct.Command()
        {
            Name = "My Name",
            Description = "Desc",
            Price = 10,
            ProductCategoryId = 1
        };

        // Act
        var result = command.ToEntity();

        // Assert
        result.Should()
            .NotBeNull();
        result.Id.Should()
            .Be(0);
        result.Name.Should()
            .Be(command.Name);
        result.Description.Should()
            .Be(command.Description);
        result.Price.Should()
            .Be(command.Price);
        result.ProductCategoryId.Should()
            .Be(command.ProductCategoryId);
        result.CreatedAt.Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(30));
        result.UpdatedAt.Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(30));
    }
}
