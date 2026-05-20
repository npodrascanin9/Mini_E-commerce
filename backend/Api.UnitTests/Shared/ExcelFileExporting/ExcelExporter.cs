using Api.Shared.ExcelFileExporting;
using ClosedXML.Excel;

namespace Api.UnitTests.Shared.ExcelFileExporting;

[TestFixture]
public class ExcelExporterTests
{
    private IExcelExporter _excelExporter;

    [SetUp]
    public void Setup()
    {
        _excelExporter = new ExcelExporter();
    }

    class TestRow
    {
        [ExcelColumn("Id")]
        public int Id { get; set; }
        [ExcelColumn("Custom Name")]
        public string Name { get; set; }
        [ExcelColumn("Custom Description")]
        public string Description { get; set; }
        [IgnoreExcelColumn]
        public string Ignored { get; set; }
    }

    [Test]
    public void ShouldGenerateExcelFileWithHeadersAndRows()
    {
        // Arrange
        var rows = new[]
        {
            new TestRow 
            { 
                Id = 1, 
                Name = "Row1", 
                Description = "Desc1", 
                Ignored = "Ignore1" 
            },
            new TestRow 
            { 
                Id = 2, 
                Name = "Row2", 
                Description = "Desc2", 
                Ignored = "Ignore2" 
            }
        };

        // Act
        var bytes = _excelExporter.ExportExcelFile(rows, "TestSheet");

        // Assert
        bytes.Should().NotBeNull();
        bytes.Length.Should().BeGreaterThan(0);

        using var stream = new MemoryStream(bytes);
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheet("TestSheet");

        // Header row
        worksheet.Cell(1, 1).Value.ToString().Should()
            .Be("Id");
        worksheet.Cell(1, 2).Value.ToString().Should()
            .Be("Custom Name");
        worksheet.Cell(1, 3).Value.ToString().Should()
            .Be("Custom Description");
        worksheet.Cell(1, 4).Value.ToString().Should()
            .Be("");

        // Ignored property should not appear
        worksheet.Cell(1, 4).Value.ToString().Should()
            .BeEmpty();

        // Data rows
        worksheet.Cell(2, 1).Value.ToString().Should()
            .Be("1");
        worksheet.Cell(2, 2).Value.ToString().Should()
            .Be("Row1");
        worksheet.Cell(2, 3).Value.ToString().Should()
            .Be("Desc1");
        worksheet.Cell(2, 4).Value.ToString().Should()
            .Be("");

        worksheet.Cell(3, 1).Value.ToString().Should()
            .Be("2");
        worksheet.Cell(3, 2).Value.ToString().Should()
            .Be("Row2");
        worksheet.Cell(3, 3).Value.ToString().Should()
            .Be("Desc2");
        worksheet.Cell(3, 4).Value.ToString().Should()
            .Be("");
    }

    [Test]
    public void ShouldUseDefaultSheetNameIfNotProvided()
    {
        // Arrange
        var rows = new[]
        {
            new TestRow { Id = 1, Name = "Row1", Description = "Desc1" }
        };

        // Act
        var bytes = _excelExporter.ExportExcelFile(rows);

        // Assert
        using var stream = new MemoryStream(bytes);
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheet("Export");

        worksheet.Should().NotBeNull();
    }
}
