namespace Api.IntegrationTests.Features.ProductCategories;

using static Testing;

[TestFixture]
public class GetProductCategoriesTests :
    BaseIntegrationTest
{
    [Test]
    public async Task ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetProductCategories.Query();

        // Act
        var result = await SendAsync(query);

        // Assert
        result.Should()
            .NotBeNull();
        result.IsSuccess.Should()
            .BeTrue();
        result.Value.Count.Should()
            .Be(0);
        result.Value.Rows.Should()
            .NotBeNull()
            .And.BeEmpty();
    }

    [Test]
    public async Task ShouldReturnAllCategories()
    {
        // Arrange
        await SendAsync(new CreateProductCategory.Command
        {
            Name = "Cat1",
            Description = "Desc1"
        });

        await SendAsync(new CreateProductCategory.Command
        {
            Name = "Cat2",
            Description = "Desc2"
        });

        var query = new GetProductCategories.Query();

        // Act
        var result = await SendAsync(query);

        // Assert
        result.IsSuccess.Should()
            .BeTrue();
        result.Value.Count.Should()
            .Be(2);
        result.Value.Rows.Should()
            .HaveCount(2);
    }

    [Test]
    public async Task ShouldFilterByName()
    {
        // Arrange
        await SendAsync(new CreateProductCategory.Command
        {
            Name = "Sports",
            Description = "Desc"
        });

        await SendAsync(new CreateProductCategory.Command
        {
            Name = "Electronics",
            Description = "Desc"
        });

        var query = new GetProductCategories.Query
        {
            Name = "Sport"
        };

        // Act
        var result = await SendAsync(query);

        // Assert
        result.IsSuccess.Should()
            .BeTrue();
        result.Value.Count.Should()
            .Be(1);
        result.Value.Rows.First().Name.Should()
            .Be("Sports");
    }

    [Test]
    public async Task ShouldFilterByIsActive()
    {
        // Arrange
        await AddAsync(new ProductCategory
        {
            Id = 0,
            IsActive = true,
            Name = "ActiveCat",
            Description = "Desc",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        await AddAsync(new ProductCategory
        {
            Id = 0,
            IsActive = false,
            Name = "InactiveCat",
            Description = "Desc inactive",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        var query = new GetProductCategories.Query
        {
            IsActive = true
        };

        // Act
        var result = await SendAsync(query);

        // Assert
        result.IsSuccess.Should()
            .BeTrue();
        result.Value.Count.Should()
            .Be(1);
        result.Value.Rows.First().Name.Should()
            .Be("ActiveCat");
    }
}
