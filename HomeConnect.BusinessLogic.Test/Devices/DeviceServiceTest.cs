using BusinessLogic;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.Devices.Services;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test.Devices;

[TestClass]
public sealed class DeviceServiceTest
{
    [TestMethod]
    public void GetDevices_WhenCalled_ReturnsDeviceList()
    {
        // Arrange
        var _validDevice = new global::BusinessLogic.Devices.Entities.Device("Device1", 12345, "Device description1", "https://example1.com/image.png",
            [], "Sensor", new Business("Rut1", "Business1", new User()));

        var _otherDevice = new global::BusinessLogic.Devices.Entities.Device("Device2", 12345, "Device description2", "https://2.com/image.png",
            [], "Sensor", new Business("Rut2", "Business2", new User()));

        var devices = new List<Device> { _validDevice, _otherDevice };

        var _defaultCurrentPage = 1;
        var _defaultPageSize = 10;

        var pagedList = new PagedData<Device>
        {
            Data = devices,
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };

        var _deviceRepository = new Mock<IDeviceRepository>();
        _deviceRepository.Setup(x => x.GetDevices(_defaultCurrentPage, _defaultPageSize, null, null, null, null)).Returns(pagedList);

        var _deviceService = new DeviceService(_deviceRepository.Object);

        // Act
        var result = _deviceService.GetDevices(_defaultCurrentPage, _defaultPageSize, null, null, null, null);

        // Assert
        result.Should().BeEquivalentTo(pagedList, options => options.ComparingByMembers<PagedData<Device>>());
        _deviceRepository.Verify(x => x.GetDevices(
            It.Is<int>(a => a == _defaultCurrentPage),
            It.Is<int>(a => a == _defaultPageSize),
            It.Is<string>(a => true),
            It.Is<int?>(a => true),
            It.Is<string>(a => true),
            It.Is<string>(a => true)));
    }
}
