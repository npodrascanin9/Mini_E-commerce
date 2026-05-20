namespace Api.IntegrationTests.Features.Products;

using static Testing;

[TestFixture]
public class GetProductsTests :
    BaseIntegrationTest
{
    [Test]
    public async Task ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetProducts.Query();

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
    public async Task ShouldReturnAllProducts()
    {
        // Arrange
        var category = await SendAsync(new CreateProductCategory.Command
        {
            Name = "Cat1",
            Description = "Desc1"
        });

        await SendAsync(new CreateProduct.Command
        {
            Name = "Prod1",
            Description = "Desc1",
            Price = 10,
            ProductCategoryId = category.Value.Id
        });

        await SendAsync(new CreateProduct.Command
        {
            Name = "Prod2",
            Description = "Desc2",
            Price = 20,
            ProductCategoryId = category.Value.Id
        });

        var query = new GetProducts.Query();

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
    public async Task ShouldReturnCorrectRowData()
    {
        // Arrange
        var categoryCommand = new CreateProductCategory.Command
        {
            Name = "Electronics",
            Description = "Desc"
        };

        var categoryResult = await SendAsync(categoryCommand);

        var productCommand = new CreateProduct.Command
        {
            Name = "Laptop",
            Description = "Gaming laptop",
            Price = 1500,
            ProductCategoryId = categoryResult.Value.Id
        };

        var productResult = await SendAsync(productCommand);

        var query = new GetProducts.Query();

        // Act
        var result = await SendAsync(query);

        // Assert
        result.IsSuccess.Should()
            .BeTrue();
        result.Value.Count.Should()
            .Be(1);

        var row = result.Value.Rows.First();
        row.Id.Should()
            .Be(productResult.Value.Id);
        row.Name.Should()
            .Be(productCommand.Name);
        row.Description.Should()
            .Be(productCommand.Description);
        row.Price.Should()
            .Be(productCommand.Price);
        row.ProductCategoryId.Should()
            .Be(categoryResult.Value.Id);
        row.ProductCategoryName.Should()
            .Be(categoryCommand.Name);
        row.IsActive.Should()
            .BeTrue();
        row.CreatedAt.Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(30));
        row.UpdatedAt.Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(30));
    }
}
