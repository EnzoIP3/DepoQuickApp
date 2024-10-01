using BusinessLogic;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.User;
using HomeConnect.WebApi.Test.Filters;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;
[TestClass]
public class UserControllerTests
{
    private Mock<IAdminService> _adminService = null!;
    private UserController _controller = null!;
    private int _defaultPageSize = 10;
    private int _defaultPage = 1;

    [TestInitialize]
    public void Initialize()
    {
        _adminService = new Mock<IAdminService>();
        _controller = new UserController(_adminService.Object);
    }

    [TestMethod]
    public void GetUsers_WhenCalledWithValidRequestAndNoFiltersOrPagination_ReturnsExpectedResponse()
    {
        // Arrange
        var user = new User("Name", "Surname", "email@email.com", "Password@100",
            new Role("Admin", new List<SystemPermission>()));
        var otherUser = new User("Name1", "Surname1", "email1@email.com", "Password@100",
            new Role("BusinessOwner", new List<SystemPermission>()));
        var expectedUsers = new List<ListUserModel>
        {
            new ListUserModel()
            {
                Name = user.Name,
                Surname = user.Surname,
                FullName = $"{user.Name} {user.Surname}",
                Role = user.Role.Name,
                CreatedAt = user.CreatedAt
            },
            new ListUserModel()
            {
                Name = otherUser.Name,
                Surname = otherUser.Surname,
                FullName = $"{otherUser.Name} {otherUser.Surname}",
                Role = otherUser.Role.Name,
                CreatedAt = otherUser.CreatedAt
            }
        };
        var expectedPagination = new Pagination { Page = _defaultPage, PageSize = _defaultPageSize, TotalPages = 1 };
        var expectedResponse = new { Data = expectedUsers, Pagination = expectedPagination };
        var pagedList = new PagedData<ListUserModel>
        {
            Data = expectedUsers,
            Page = expectedPagination.Page,
            PageSize = expectedPagination.PageSize,
            TotalPages = expectedPagination.TotalPages
        };

        _adminService.Setup(x => x.GetUsers(null, null, null, null)).Returns(pagedList);

        // Act
        var response = _controller.GetUsers();

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [TestMethod]
    public void GetUsers_WhenCalledWithValidRequestAndFullNameFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        var user = new User("Name", "Surname", "email@email.com", "Password@100",
            new Role("Admin", new List<SystemPermission>()));
        var expectedUsers = new List<ListUserModel>
        {
            new ListUserModel()
            {
                Name = user.Name,
                Surname = user.Surname,
                FullName = $"{user.Name} {user.Surname}",
                Role = user.Role.Name,
                CreatedAt = user.CreatedAt
            }
        };
        var expectedPagination = new Pagination { Page = _defaultPage, PageSize = _defaultPageSize, TotalPages = 1 };
        var expectedResponse = new { Data = expectedUsers, Pagination = expectedPagination };
        var pagedList = new PagedData<ListUserModel>
        {
            Data = expectedUsers,
            Page = expectedPagination.Page,
            PageSize = expectedPagination.PageSize,
            TotalPages = expectedPagination.TotalPages
        };

        _adminService.Setup(x => x.GetUsers(null, null, expectedUsers.First().FullName, null)).Returns(pagedList);

        // Act
        var response = _controller.GetUsers(fullNameFilter: expectedUsers.First().FullName);

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [TestMethod]
    public void GetUsers_WhenCalledWithValidRequestAndRoleFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        var user = new User("Name", "Surname", "email@email.com", "Password@100",
            new Role("Admin", new List<SystemPermission>()));
        var expectedUsers = new List<ListUserModel>
        {
            new ListUserModel()
            {
                Name = user.Name,
                Surname = user.Surname,
                FullName = $"{user.Name} {user.Surname}",
                Role = user.Role.Name,
                CreatedAt = user.CreatedAt
            }
        };
        var expectedPagination = new Pagination { Page = _defaultPage, PageSize = _defaultPageSize, TotalPages = 1 };
        var expectedResponse = new { Data = expectedUsers, Pagination = expectedPagination };
        var pagedList = new PagedData<ListUserModel>
        {
            Data = expectedUsers,
            Page = expectedPagination.Page,
            PageSize = expectedPagination.PageSize,
            TotalPages = expectedPagination.TotalPages
        };

        _adminService.Setup(x => x.GetUsers(null, null, null, expectedUsers.First().Role)).Returns(pagedList);

        // Act
        var response = _controller.GetUsers(roleFilter: expectedUsers.First().Role);

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [TestMethod]
    public void GetUsers_WhenCalledWithPagination_ReturnsPagedExpectedResponse()
    {
        // Arrange
        var user = new User("Name", "Surname", "email@email.com", "Password@100",
            new Role("Admin", new List<SystemPermission>()));
        var expectedUsers = new List<ListUserModel>
        {
            new ListUserModel()
            {
                Name = user.Name,
                Surname = user.Surname,
                FullName = $"{user.Name} {user.Surname}",
                Role = user.Role.Name,
                CreatedAt = user.CreatedAt
            }
        };
        var expectedPagination = new Pagination { Page = 1, PageSize = 1, TotalPages = 1 };
        var expectedResponse = new { Data = expectedUsers, Pagination = expectedPagination };
        var pagedList = new PagedData<ListUserModel>
        {
            Data = expectedUsers,
            Page = expectedPagination.Page,
            PageSize = expectedPagination.PageSize,
            TotalPages = expectedPagination.TotalPages
        };

        _adminService.Setup(x => x.GetUsers(expectedPagination.Page, expectedPagination.PageSize, null, null)).Returns(pagedList);

        // Act
        var response = _controller.GetUsers(expectedPagination.Page, expectedPagination.PageSize);

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }
}
