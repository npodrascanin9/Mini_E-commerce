namespace Api.UnitTests.Features.ProductCategories;

public class GetProductCategoriesMapperTests :
    BaseUnitTest
{
    [Test]
    public void ShouldCreateRowDtoObject()
    {
        // Arrange
        var query = new GetProductCategories.Query();
        var entity = new ProductCategory()
        {
            Id = 1,
            Name = "MyCategory",
            Description = "Desc",
            IsActive = true,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now.AddDays(1),
            Products = new List<Product>()
            {
                new()
                {
                    Id = 1,
                    Name = "Product 1",
                    Description = "desc",
                    Price = 10,
                    ProductCategoryId = 1,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now.AddDays(2)
                }
            }
        };

        // Act
        var result = query.ToRowDto(entity);

        // Assert
        result.Should()
            .NotBeNull();
        result.Id.Should()
            .Be(entity.Id);
        result.Name.Should()
            .Be(entity.Name);
        result.Description.Should()
            .Be(entity.Description);
        result.isActive.Should()
            .Be(entity.IsActive);
        result.CreatedAt.Should()
            .Be(entity.CreatedAt);
        result.UpdatedAt.Should()
            .Be(entity.UpdatedAt);
        result.ProductsCount.Should()
            .Be(1);
    }
}
