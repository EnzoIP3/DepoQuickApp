using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
using BusinessLogic.Devices.Services;
using BusinessLogic.Helpers;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using DeviceImporter;
using DeviceImporter.Models;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test.Devices.Services;

[TestClass]
public class ImporterServiceTests
{
    private Mock<IAssemblyInterfaceLoader<IDeviceImporter>> _mockAssemblyInterfaceLoader = null!;
    private Mock<IDeviceImporter> _mockDeviceImporter = null!;
    private Mock<IBusinessOwnerService> _mockBusinessOwnerService = null!;
    private ImporterService _importerService = null!;
    private readonly string _importFilesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImportFiles");

    [TestInitialize]
    public void Initialize()
    {
        _mockAssemblyInterfaceLoader = new Mock<IAssemblyInterfaceLoader<IDeviceImporter>>(MockBehavior.Strict);
        _mockDeviceImporter = new Mock<IDeviceImporter>(MockBehavior.Strict);
        _mockBusinessOwnerService = new Mock<IBusinessOwnerService>(MockBehavior.Strict);

        _importerService = new ImporterService(_mockAssemblyInterfaceLoader.Object, _mockBusinessOwnerService.Object);
    }

    #region GetImporters
    [TestMethod]
    public void GetImporters_WhenCalled_ShouldReturnListOfImporters()
    {
        // Arrange
        var importerName = "ImporterName";
        var paramsList = new List<string> { "param1", "param2" };
        var expectedResponse = new List<ImporterData>
        {
            new ImporterData { Name = importerName, Parameters = paramsList }
        };
        _mockAssemblyInterfaceLoader
            .Setup(x => x.GetImplementationsList(It.IsAny<string>()))
            .Returns([importerName]);
        _mockDeviceImporter
            .Setup(x => x.GetParams())
            .Returns(paramsList);
        _mockAssemblyInterfaceLoader
            .Setup(x => x.GetImplementationByName(importerName, It.IsAny<string>()))
            .Returns(_mockDeviceImporter.Object);

        // Act
        var result = _importerService.GetImporters();

        // Assert
        result.Should().HaveCount(1);
        result.Should().BeEquivalentTo(expectedResponse);
    }
    #endregion

    #region ImportDevices

    [TestMethod]
    public void ImportDevices_WhenCalled_ShouldReturnListOfDeviceNames()
    {
        // Arrange
        var importerName = "ImporterName";
        var route = "Route";
        var importerParams = new Dictionary<string, string> { { "route", Path.Combine(_importFilesPath, route) }, { "param2", "value2" } };
        var importDevicesArgs = new ImportDevicesArgs
        {
            ImporterName = importerName,
            User = new User("John", "Doe", "email@email.com", "Password123!", new Role()),
            Parameters = importerParams
        };

        var deviceArgs = new List<DeviceArgs>
        {
            new DeviceArgs
            {
                Name = "DeviceName",
                ModelNumber = "ModelNumber",
                Description = "Description",
                MainPhoto = "MainPhoto",
                SecondaryPhotos = ["SecondaryPhoto"],
                Type = "Sensor",
            },
            new DeviceArgs
            {
                Name = "DeviceName2",
                ModelNumber = "ModelNumber2",
                Description = "Description2",
                MainPhoto = "MainPhoto2",
                SecondaryPhotos = [],
                Type = "Camera",
                MotionDetection = true,
                PersonDetection = false,
                IsExterior = true,
                IsInterior = false
            }
        };
        var sensorArgs = new CreateDeviceArgs
        {
            Owner = importDevicesArgs.User,
            ModelNumber = deviceArgs[0].ModelNumber,
            Name = deviceArgs[0].Name,
            Description = deviceArgs[0].Description,
            MainPhoto = deviceArgs[0].MainPhoto,
            SecondaryPhotos = deviceArgs[0].SecondaryPhotos,
            Type = deviceArgs[0].Type
        };
        var cameraArgs = new CreateCameraArgs
        {
            Owner = importDevicesArgs.User,
            ModelNumber = deviceArgs[1].ModelNumber,
            Name = deviceArgs[1].Name,
            Description = deviceArgs[1].Description,
            MainPhoto = deviceArgs[1].MainPhoto,
            SecondaryPhotos = deviceArgs[1].SecondaryPhotos,
            MotionDetection = deviceArgs[1].MotionDetection,
            PersonDetection = deviceArgs[1].PersonDetection,
            Exterior = deviceArgs[1].IsExterior,
            Interior = deviceArgs[1].IsInterior
        };
        var deviceNames = deviceArgs.Select(deviceArg => deviceArg.Name).ToList();
        _mockDeviceImporter
            .Setup(x => x.ImportDevices(importerParams))
            .Returns(deviceArgs);
        _mockAssemblyInterfaceLoader
            .Setup(x => x.GetImplementationByName(importerName, It.IsAny<string>()))
            .Returns(_mockDeviceImporter.Object);
        _mockBusinessOwnerService
            .Setup(x => x.CreateDevice(It.IsAny<CreateDeviceArgs>())).Returns(It.IsAny<Device>());
        _mockBusinessOwnerService
            .Setup(x => x.CreateCamera(It.IsAny<CreateCameraArgs>())).Returns(It.IsAny<Camera>());

        // Act
        var result = _importerService.ImportDevices(importDevicesArgs);

        // Assert
        result.Should().HaveCount(deviceNames.Count);
        result[0].Should().Be(deviceNames[0]);
        result[1].Should().Be(deviceNames[1]);

        _mockAssemblyInterfaceLoader.Verify(x => x.GetImplementationByName(importerName, It.IsAny<string>()), Times.Once);
        _mockDeviceImporter.Verify(x => x.ImportDevices(importerParams), Times.Once);
        _mockBusinessOwnerService.Verify(x => x.CreateDevice(It.Is<CreateDeviceArgs>(device =>
            device.SecondaryPhotos != null &&
            device.Owner == sensorArgs.Owner &&
            device.ModelNumber == sensorArgs.ModelNumber &&
            device.Name == sensorArgs.Name &&
            device.Description == sensorArgs.Description &&
            device.MainPhoto == sensorArgs.MainPhoto &&
            device.SecondaryPhotos.SequenceEqual(sensorArgs.SecondaryPhotos) &&
            device.Type == sensorArgs.Type)), Times.Once);
        _mockBusinessOwnerService.Verify(x => x.CreateCamera(It.Is<CreateCameraArgs>(camera =>
            camera.SecondaryPhotos != null &&
            camera.Owner == cameraArgs.Owner &&
            camera.ModelNumber == cameraArgs.ModelNumber &&
            camera.Name == cameraArgs.Name &&
            camera.Description == cameraArgs.Description &&
            camera.MainPhoto == cameraArgs.MainPhoto &&
            camera.SecondaryPhotos.SequenceEqual(cameraArgs.SecondaryPhotos) &&
            camera.MotionDetection == cameraArgs.MotionDetection &&
            camera.PersonDetection == cameraArgs.PersonDetection &&
            camera.Exterior == cameraArgs.Exterior &&
            camera.Interior == cameraArgs.Interior)), Times.Once);
    }

    #endregion

    #region GetImportFiles
    [TestMethod]
    public void GetImportFiles_WhenDirectoryExists_ShouldReturnListOfFileNames()
    {
        // Arrange
        var fileNames = new List<string> { "file1.txt", "file2.txt" };
        Directory.CreateDirectory(_importFilesPath);
        foreach (var fileName in fileNames)
        {
            File.Create(Path.Combine(_importFilesPath, fileName)).Dispose();
        }

        // Act
        var result = _importerService.GetImportFiles();

        // Assert
        result.Should().BeEquivalentTo(fileNames);

        // Cleanup
        Directory.Delete(_importFilesPath, true);
    }

    [TestMethod]
    public void GetImportFiles_WhenDirectoryDoesNotExist_ShouldCreateDirectoryAndReturnEmptyList()
    {
        // Arrange
        if (Directory.Exists(_importFilesPath))
        {
            Directory.Delete(_importFilesPath, true);
        }

        // Act
        var result = _importerService.GetImportFiles();

        // Assert
        Directory.Exists(_importFilesPath).Should().BeTrue();
        result.Should().BeEmpty();

        // Cleanup
        Directory.Delete(_importFilesPath, true);
    }
    #endregion
}
