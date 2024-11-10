using BusinessLogic.Devices.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.DeviceImportFiles;
using HomeConnect.WebApi.Controllers.Devices.Models;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class DeviceImportFilesControllerTests
{
    private DeviceImportFilesController _controller = null!;
    private Mock<IImporterService> _importerService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _importerService = new Mock<IImporterService>();
        _controller = new DeviceImportFilesController(_importerService.Object);
    }

    #region GetImportFiles
    [TestMethod]
    public void GetImportFiles_WhenCalled_ReturnsGetImportFilesResponse()
    {
        // Arrange
        var files = new List<string>
        {
            "file1.csv",
            "file2.json"
        };
        var expectedResponse = new GetImportFilesResponse
        {
            ImportFiles = files
        };
        _importerService.Setup(x => x.GetImportFiles()).Returns(files);

        // Act
        GetImportFilesResponse response = _controller.GetImportFiles();

        // Assert
        _importerService.Verify(x => x.GetImportFiles(), Times.Once);
        response.Should().BeEquivalentTo(expectedResponse, options => options
            .ComparingByMembers<GetImportFilesResponse>());
    }
    #endregion
}
