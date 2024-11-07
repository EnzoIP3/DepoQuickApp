using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Repositories;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Importer;
using BusinessLogic.Devices.Models;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.Devices.Services;
using BusinessLogic.Helpers;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;
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
        _mockAssemblyInterfaceLoader
            .Setup(x => x.GetImplementationsList(It.IsAny<string>()))
            .Returns(new List<string> { importerName });

        // Act
        var result = _importerService.GetImporters();

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(importerName, result[0]);
    }
    #endregion

    #region ImportDevices

    [TestMethod]
    public void ImportDevices_WhenCalled_ShouldReturnListOfDeviceNames()
    {
        // Arrange
        var importerName = "ImporterName";
        var route = "Route";
        var importDevicesArgs = new ImportDevicesArgs
        {
            ImporterName = importerName,
            Route = route,
            User = new User("John", "Doe", "email@email.com", "Password123!", new Role())
        };

        var deviceArgs = new List<DeviceArgs>
        {
            new DeviceArgs
            {
                Name = "DeviceName",
                ModelNumber = "ModelNumber",
                Description = "Description",
                MainPhoto = "MainPhoto",
                SecondaryPhotos = new List<string> { "SecondaryPhoto" },
                Type = "Sensor",
            },
            new DeviceArgs
            {
                Name = "DeviceName2",
                ModelNumber = "ModelNumber2",
                Description = "Description2",
                MainPhoto = "MainPhoto2",
                SecondaryPhotos = new List<string>(),
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
            .Setup(x => x.ImportDevices(route))
            .Returns(deviceArgs);
        _mockAssemblyInterfaceLoader
            .Setup(x => x.GetImplementation(importerName, It.IsAny<string>()))
            .Returns(_mockDeviceImporter.Object);
        _mockBusinessOwnerService
            .Setup(x => x.CreateDevice(It.IsAny<CreateDeviceArgs>())).Returns(It.IsAny<Device>());
        _mockBusinessOwnerService
            .Setup(x => x.CreateCamera(It.IsAny<CreateCameraArgs>())).Returns(It.IsAny<Camera>());

        // Act
        var result = _importerService.ImportDevices(importDevicesArgs);

        // Assert
        Assert.AreEqual(deviceNames.Count, result.Count);
        Assert.AreEqual(deviceNames[0], result[0]);
        Assert.AreEqual(deviceNames[1], result[1]);

        _mockAssemblyInterfaceLoader.Verify(x => x.GetImplementation(importerName, It.IsAny<string>()), Times.Once);
        _mockDeviceImporter.Verify(x => x.ImportDevices(route), Times.Once);
        _mockBusinessOwnerService.Verify(x => x.CreateDevice(It.Is<CreateDeviceArgs>(args =>
            args.SecondaryPhotos != null &&
            args.Owner == sensorArgs.Owner &&
            args.ModelNumber == sensorArgs.ModelNumber &&
            args.Name == sensorArgs.Name &&
            args.Description == sensorArgs.Description &&
            args.MainPhoto == sensorArgs.MainPhoto &&
            args.SecondaryPhotos.SequenceEqual(sensorArgs.SecondaryPhotos) &&
            args.Type == sensorArgs.Type)), Times.Once);
        _mockBusinessOwnerService.Verify(x => x.CreateCamera(It.Is<CreateCameraArgs>(args =>
            args.SecondaryPhotos != null &&
            args.Owner == cameraArgs.Owner &&
            args.ModelNumber == cameraArgs.ModelNumber &&
            args.Name == cameraArgs.Name &&
            args.Description == cameraArgs.Description &&
            args.MainPhoto == cameraArgs.MainPhoto &&
            args.SecondaryPhotos.SequenceEqual(cameraArgs.SecondaryPhotos) &&
            args.MotionDetection == cameraArgs.MotionDetection &&
            args.PersonDetection == cameraArgs.PersonDetection &&
            args.Exterior == cameraArgs.Exterior &&
            args.Interior == cameraArgs.Interior)), Times.Once);
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
        CollectionAssert.AreEqual(fileNames, result);

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
        Assert.IsTrue(Directory.Exists(_importFilesPath));
        Assert.AreEqual(0, result.Count);

        // Cleanup
        Directory.Delete(_importFilesPath, true);
    }
    #endregion
}
