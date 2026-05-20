using Api.Shared.ExcelFileExporting;

namespace Api.UnitTests.Shared.ExcelFileExporting;

[TestFixture]
public class ExcelFileConstantsTests
{
    [Test]
    public void ShouldReturnXlsxContentType()
    {
        // Act
        string contentType = ExcelFileConstants.ContentType;

        // Assert
        contentType.Should()
            .Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }
}
