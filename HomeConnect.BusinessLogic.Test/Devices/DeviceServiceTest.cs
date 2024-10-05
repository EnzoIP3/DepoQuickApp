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
    var user1 = new global::BusinessLogic.Users.Entities.User("name", "surname", "email1@email.com", "Password#100", new global::BusinessLogic.Roles.Entities.Role());
    var user2 = new global::BusinessLogic.Users.Entities.User("name", "surname", "email2@email.com", "Password#100", new global::BusinessLogic.Roles.Entities.Role());
    var _validDevice = new global::BusinessLogic.Devices.Entities.Device("Device1", 12345, "Device description1", "https://example1.com/image.png",
        new List<string>(), "Sensor", new Business("Rut1", "Business", user1));
    var _otherDevice = new global::BusinessLogic.Devices.Entities.Device("Device2", 12345, "Device description2", "https://example2.com/image.png",
        new List<string>(), "Sensor", new Business("Rut2", "Business", user2));

    var devices = new List<Device> { _validDevice, _otherDevice };

    var _defaultCurrentPage = 1;
    var _defaultPageSize = 10;

    var pagedDeviceList = new PagedData<Device>
    {
        Data = devices,
        Page = _defaultCurrentPage,
        PageSize = _defaultPageSize,
        TotalPages = 1
    };

    var _deviceRepository = new Mock<IDeviceRepository>();
    _deviceRepository.Setup(x => x.GetDevices(_defaultCurrentPage, _defaultPageSize, null, null, null, null)).Returns(pagedDeviceList);

    var _deviceService = new DeviceService(_deviceRepository.Object);

    // Act
    var result = _deviceService.GetDevices(_defaultCurrentPage, _defaultPageSize, null, null, null, null);

    // Assert
    var expectedPagedDeviceList = new PagedData<Device>
    {
        Data = devices,
        Page = _defaultCurrentPage,
        PageSize = _defaultPageSize,
        TotalPages = 1
    };

    result.Should().BeEquivalentTo(expectedPagedDeviceList, options => options.ComparingByMembers<PagedData<Device>>());
    _deviceRepository.Verify(x => x.GetDevices(
        It.Is<int>(a => a == _defaultCurrentPage),
        It.Is<int>(a => a == _defaultPageSize),
        It.Is<string>(a => a == null),
        It.Is<int?>(a => a == null),
        It.Is<string>(a => a == null),
        It.Is<string>(a => a == null)), Times.Once);
}
}
