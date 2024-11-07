using BusinessLogic.BusinessOwners.Helpers;
using BusinessLogic.Devices.Services;
using Moq;

namespace HomeConnect.BusinessLogic.Test.Devices.Services;

[TestClass]
public class ImporterServiceTests
{
    private Mock<IAssemblyInterfaceLoader<IDeviceImporter>> _mockAssemblyInterfaceLoader = null!;
    private ImporterService _importerService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _mockAssemblyInterfaceLoader = new Mock<IAssemblyInterfaceLoader<IDeviceImporter>>();
        _importerService = new ImporterService(_mockAssemblyInterfaceLoader.Object);
    }

    [TestMethod]
    public void GetImporters_WhenCalled_ShouldReturnListOfImporters()
    {
        // Arrange
        var importerName = "ImporterName";
        _mockAssemblyInterfaceLoader
            .Setup(x => x.GetImplementationsList(It.IsAny<string>()))
            .Returns(new List<string> { importerName });

        // Act
        var result = _importerService.GetImporters();

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(importerName, result[0]);
    }
}
