using Api.Shared.ExcelFileExporting;

namespace Api.UnitTests.Shared.ExcelFileExporting;


[TestFixture]
public class ExcelFileNameSetterTests
{
    [Test]
    public void ShouldThrowExceptionWhenNameIsNullOrEmpty()
    {
        // Arrange
        string? name = null;

        // Act
        Action act = () => ExcelFileNameSetter.SetExcelFileName(name!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void ShouldReturnSameNameIfAlreadyEndsWithXlsx()
    {
        // Arrange
        var name = "Report.xlsx";

        // Act
        var result = ExcelFileNameSetter.SetExcelFileName(name);

        // Assert
        result.Should().Be("Report.xlsx");
    }

    [Test]
    public void ShouldAppendXlsxExtensionIfMissing()
    {
        // Arrange
        var name = "Report";

        // Act
        var result = ExcelFileNameSetter.SetExcelFileName(name);

        // Assert
        result.Should().Be("Report.xlsx");
    }
}
