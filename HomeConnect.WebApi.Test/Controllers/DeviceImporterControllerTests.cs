using BusinessLogic.Devices.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.DeviceImporters;
using HomeConnect.WebApi.Controllers.DeviceImporters.Models;
using HomeConnect.WebApi.Controllers.Devices.Models;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;
[TestClass]
public class DeviceImporterControllerTests
{
    private DeviceImporterController _controller = null!;
    private Mock<IImporterService> _importerService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _importerService = new Mock<IImporterService>();
        _controller = new DeviceImporterController(_importerService.Object);
    }
    #region GetImporters
    [TestMethod]
    public void GetImporters_WhenCalled_ReturnsGetImportersResponse()
    {
        // Arrange
        var importers = new List<string>
        {
            "Importer1",
            "Importer2"
        };
        var expectedResponse = new GetImportersResponse
        {
            Importers = importers
        };
        _importerService.Setup(x => x.GetImporters()).Returns(importers);

        // Act
        GetImportersResponse response = _controller.GetImporters();

        // Assert
        _importerService.Verify(x => x.GetImporters(), Times.Once);
        response.Should().BeEquivalentTo(expectedResponse, options => options
            .ComparingByMembers<GetImportersResponse>());
    }
    #endregion
}
