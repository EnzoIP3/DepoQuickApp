using BusinessLogic;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.Devices.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Devices.Models;
using Moq;

namespace HomeConnect.BusinessLogic.Test.Devices;

[TestClass]
public sealed class DeviceServiceTest
{
    [TestMethod]
    public void GetDevices_WhenCalled_ReturnsDeviceList()
    {
        // Arrange
        var user1 = new User("name", "surname", "email1@email.com", "Password#100", new Role());
        var user2 = new User("name", "surname", "email2@email.com", "Password#100", new Role());
        var validDevice = new Device("Device1", 12345, "Device description1", "https://example1.com/image.png",
            [], "Sensor", new Business("Rut1", "Business", user1));
        var otherDevice = new Device("Device2", 12345, "Device description2", "https://example2.com/image.png",
            [], "Sensor", new Business("Rut2", "Business", user2));

        var devices = new List<Device> { validDevice, otherDevice };

        var parameters = new GetDeviceArgs
        {
            Page = 1,
            PageSize = 10
        };

        var pagedDeviceList = new PagedData<Device>
        {
            Data = devices,
            Page = parameters.Page ?? 1,
            PageSize = parameters.PageSize ?? 10,
            TotalPages = 1
        };

        var deviceRepository = new Mock<IDeviceRepository>();
        var ownedDeviceRepository = new Mock<IOwnedDeviceRepository>();
        deviceRepository.Setup(x => x.GetDevices(parameters.Page ?? 1, parameters.PageSize ?? 10, parameters.DeviceNameFilter, parameters.ModelNumberFilter, parameters.BusinessNameFilter, parameters.DeviceTypeFilter)).Returns(pagedDeviceList);
        var deviceService = new DeviceService(deviceRepository.Object, ownedDeviceRepository.Object);

        // Act
        var result = deviceService.GetDevices(parameters);

        // Assert
        var expectedPagedDeviceList = new PagedData<Device>
        {
            Data = devices,
            Page = parameters.Page ?? 1,
            PageSize = parameters.PageSize ?? 10,
            TotalPages = 1
        };

        result.Should().BeEquivalentTo(expectedPagedDeviceList, options => options.ComparingByMembers<PagedData<Device>>());
        deviceRepository.Verify(x => x.GetDevices(
            It.Is<int>(a => a == parameters.Page),
            It.Is<int>(a => a == parameters.PageSize),
            It.Is<string>(a => a == parameters.DeviceNameFilter),
            It.Is<int?>(a => a == parameters.ModelNumberFilter),
            It.Is<string>(a => a == parameters.BusinessNameFilter),
            It.Is<string>(a => a == parameters.DeviceTypeFilter)), Times.Once);
    }
}
