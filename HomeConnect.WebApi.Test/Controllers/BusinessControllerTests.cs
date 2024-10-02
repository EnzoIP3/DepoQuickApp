using BusinessLogic;
using BusinessLogic.Admins.Models;
using BusinessLogic.Admins.Services;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class BusinessControllerTests
{
    private BusinessController _controller = null!;
    private Mock<IAdminService> _adminService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _adminService = new Mock<IAdminService>();
        _controller = new BusinessController(_adminService.Object);
    }

    [TestMethod]
    public void GetBusinesses_WhenCalledWithNoFiltersOrPagination_ReturnsExpectedResponse()
    {
        // Arrange
        var role = new Role("BusinessOwner", []);
        var user = new User("Name", "Surname", "email@email.com", "Password@100", role);
        var otherUser = new User("Name1", "Surname1", "email1@email.com", "Password@100", role);
        var businesses = new List<Business>
        {
            new Business("123456789123", "Business 1", user), new Business("123456789124", "Business 2", otherUser),
        };
        var expectedBusinesses = new List<GetBusinessesArgs>
        {
            new GetBusinessesArgs()
            {
                Name = businesses[0].Name,
                OwnerFullName = $"{user.Name} {user.Surname}",
                OwnerEmail = user.Email,
                Rut = businesses[0].Rut,
            },
            new GetBusinessesArgs()
            {
                Name = businesses[1].Name,
                OwnerFullName = $"{otherUser.Name} {otherUser.Surname}",
                OwnerEmail = otherUser.Email,
                Rut = businesses[1].Rut,
            }
        };
        var expectedPagination = new Pagination { Page = 1, PageSize = 10, TotalPages = 1 };
        var expectedResponse = new { Data = expectedBusinesses, Pagination = expectedPagination };
        var pagedList = new PagedData<GetBusinessesArgs>
        {
            Data = expectedBusinesses,
            Page = expectedPagination.Page,
            PageSize = expectedPagination.PageSize,
            TotalPages = expectedPagination.TotalPages
        };

        _adminService.Setup(x => x.GetBusinesses(null, null, null, null)).Returns(pagedList);

        // Act
        var response = _controller.GetBusinesses();

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [TestMethod]
    public void GetBusinesses_WhenCalledWithNameFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        var role = new Role("BusinessOwner", []);
        var otherUser = new User("Name1", "Surname1", "email1@email.com", "Password@100", role);
        var businesses = new List<Business>
        {
            new Business("123456789124", "Business 2", otherUser)
        };
        var expectedBusinesses = new List<GetBusinessesArgs>
        {
            new GetBusinessesArgs()
            {
                Name = businesses[0].Name,
                OwnerFullName = $"{otherUser.Name} {otherUser.Surname}",
                OwnerEmail = otherUser.Email,
                Rut = businesses[0].Rut,
            }
        };
        var expectedPagination = new Pagination { Page = 1, PageSize = 10, TotalPages = 1 };
        var expectedResponse = new { Data = expectedBusinesses, Pagination = expectedPagination };
        var pagedList = new PagedData<GetBusinessesArgs>
        {
            Data = expectedBusinesses,
            Page = expectedPagination.Page,
            PageSize = expectedPagination.PageSize,
            TotalPages = expectedPagination.TotalPages
        };

        _adminService.Setup(x => x.GetBusinesses(null, null, expectedBusinesses.First().Name, null)).Returns(pagedList);

        // Act
        var response = _controller.GetBusinesses(nameFilter: expectedBusinesses.First().Name);

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [TestMethod]
    public void GetBusinesses_WhenCalledWithFullNameFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        var role = new Role("BusinessOwner", []);
        var otherUser = new User("Name1", "Surname1", "email1@email.com", "Password@100", role);
        var businesses = new List<Business>
        {
            new Business("123456789124", "Business 2", otherUser),
        };
        var expectedBusinesses = new List<GetBusinessesArgs>
        {
            new GetBusinessesArgs()
            {
                Name = businesses[0].Name,
                OwnerFullName = $"{otherUser.Name} {otherUser.Surname}",
                OwnerEmail = otherUser.Email,
                Rut = businesses[0].Rut,
            }
        };
        var expectedPagination = new Pagination { Page = 1, PageSize = 10, TotalPages = 1 };
        var expectedResponse = new { Data = expectedBusinesses, Pagination = expectedPagination };
        var pagedList = new PagedData<GetBusinessesArgs>
        {
            Data = expectedBusinesses,
            Page = expectedPagination.Page,
            PageSize = expectedPagination.PageSize,
            TotalPages = expectedPagination.TotalPages
        };

        _adminService.Setup(x => x.GetBusinesses(null, null, expectedBusinesses.First().OwnerFullName, null)).Returns(pagedList);

        // Act
        var response = _controller.GetBusinesses(nameFilter: expectedBusinesses.First().OwnerFullName);

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [TestMethod]
    public void GetBusinesses_WhenCalledWithPagination_ReturnsPagedExpectedResponse()
    {
        // Arrange
        var role = new Role("BusinessOwner", []);
        var otherUser = new User("Name1", "Surname1", "email1@email.com", "Password@100", role);
        var businesses = new List<Business>
        {
            new Business("123456789124", "Business 2", otherUser),
        };
        var expectedBusinesses = new List<GetBusinessesArgs>
        {
            new GetBusinessesArgs()
            {
                Name = businesses[0].Name,
                OwnerFullName = $"{otherUser.Name} {otherUser.Surname}",
                OwnerEmail = otherUser.Email,
                Rut = businesses[0].Rut,
            }
        };
        var expectedPagination = new Pagination { Page = 1, PageSize = 1, TotalPages = 1 };
        var expectedResponse = new { Data = expectedBusinesses, Pagination = expectedPagination };
        var pagedList = new PagedData<GetBusinessesArgs>
        {
            Data = expectedBusinesses,
            Page = expectedPagination.Page,
            PageSize = expectedPagination.PageSize,
            TotalPages = expectedPagination.TotalPages
        };

        _adminService.Setup(x => x.GetBusinesses(1, 1, null, null)).Returns(pagedList);

        // Act
        var response = _controller.GetBusinesses(1, 1);

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }
}
