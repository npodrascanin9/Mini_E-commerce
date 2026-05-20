using ClosedXML.Excel;

namespace Api.IntegrationTests.Features.Products;

using static Testing;

[TestFixture]
public class ExportExcelProductsTests :
    BaseIntegrationTest
{
    [Test]
    public async Task ShouldExportProductsWithCorrectHeaders()
    {
        // Arrange
        var categoryCommand = new CreateProductCategory.Command
        {
            Name = "Excel Category",
            Description = "Excel desc"
        };

        var categoryResult = await SendAsync(categoryCommand);

        var productCommand = new CreateProduct.Command
        {
            Name = "Excel Product",
            Description = "Excel desc",
            Price = 99.99m,
            ProductCategoryId = categoryResult.Value.Id
        };

        var productResult = await SendAsync(productCommand);

        var query = new ExportExcelProducts.Query();

        // Act
        var result = await SendAsync(query);

        // Assert
        result.Should()
            .NotBeNull();
        result.IsSuccess.Should()
            .BeTrue();
        result.Value.Should()
            .NotBeNull();

        using var stream = new MemoryStream(
            result.Value.FileContents);
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheet(1);

        // Header row
        worksheet.Cell(1, 1).Value.ToString().Should()
            .Be("Id");
        worksheet.Cell(1, 2).Value.ToString().Should()
            .Be("Product name");
        worksheet.Cell(1, 3).Value.ToString().Should()
            .Be("Product Description");
        worksheet.Cell(1, 4).Value.ToString().Should()
            .Be("Category name");
        worksheet.Cell(1, 5).Value.ToString().Should()
            .Be("Price");

        // First data row
        worksheet.Cell(2, 1).Value.ToString().Should()
            .Be(productResult.Value.Id.ToString());
        worksheet.Cell(2, 2).Value.ToString().Should()
            .Be(productCommand.Name);
        worksheet.Cell(2, 3).Value.ToString().Should()
            .Be(productCommand.Description);
        worksheet.Cell(2, 4).Value.ToString().Should()
            .Be(categoryCommand.Name);
        worksheet.Cell(2, 5).Value.ToString().Should()
            .Be(productCommand.Price.ToString());
    }
}
