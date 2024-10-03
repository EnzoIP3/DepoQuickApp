using BusinessLogic;
using BusinessLogic.Admins.Models;
using BusinessLogic.Admins.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.User;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class UserControllerTests
{
    private UserController _controller = null!;
    private Mock<IAdminService> _adminService = null!;
    private User _user = null!;
    private User _otherUser = null!;
    private List<GetUsersArgs> _expectedUsers = null!;
    private Pagination _expectedPagination;
    private PagedData<GetUsersArgs> _pagedList;

    [TestInitialize]
    public void Initialize()
    {
        _adminService = new Mock<IAdminService>();
        _controller = new UserController(_adminService.Object);

        _user = new User("Name", "Surname", "email@email.com", "Password@100", new Role("Admin", []));
        _otherUser = new User("Name1", "Surname1", "email1@email.com", "Password@100", new Role("BusinessOwner", []));
        _expectedUsers = new List<GetUsersArgs>
        {
            new GetUsersArgs()
            {
                Name = _user.Name,
                Surname = _user.Surname,
                FullName = $"{_user.Name} {_user.Surname}",
                Role = _user.Role.Name,
                CreatedAt = _user.CreatedAt
            },
            new GetUsersArgs()
            {
                Name = _otherUser.Name,
                Surname = _otherUser.Surname,
                FullName = $"{_otherUser.Name} {_otherUser.Surname}",
                Role = _otherUser.Role.Name,
                CreatedAt = _otherUser.CreatedAt
            }
        };
        _expectedPagination = new Pagination { Page = 1, PageSize = 10, TotalPages = 1 };
        _pagedList = new PagedData<GetUsersArgs>
        {
            Data = _expectedUsers,
            Page = _expectedPagination.Page,
            PageSize = _expectedPagination.PageSize,
            TotalPages = _expectedPagination.TotalPages
        };
    }

    #region GetUsers
    [TestMethod]
    public void GetUsers_WhenCalledWithValidRequestAndNoFiltersOrPagination_ReturnsExpectedResponse()
    {
        // Arrange
        _adminService.Setup(x => x.GetUsers(null, null, null, null)).Returns(_pagedList);

        // Act
        var response = _controller.GetUsers();

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _expectedUsers, Pagination = _expectedPagination });
    }

    [TestMethod]
    public void GetUsers_WhenCalledWithValidRequestAndFullNameFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        _adminService.Setup(x => x.GetUsers(null, null, _expectedUsers.First().FullName, null)).Returns(_pagedList);

        // Act
        var response = _controller.GetUsers(fullNameFilter: _expectedUsers.First().FullName);

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _expectedUsers, Pagination = _expectedPagination });
    }

    [TestMethod]
    public void GetUsers_WhenCalledWithValidRequestAndRoleFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        _adminService.Setup(x => x.GetUsers(null, null, null, _expectedUsers.First().Role)).Returns(_pagedList);

        // Act
        var response = _controller.GetUsers(roleFilter: _expectedUsers.First().Role);

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _expectedUsers, Pagination = _expectedPagination });
    }

    [TestMethod]
    public void GetUsers_WhenCalledWithPagination_ReturnsPagedExpectedResponse()
    {
        // Arrange
        _adminService.Setup(x => x.GetUsers(_expectedPagination.Page, _expectedPagination.PageSize, null, null)).Returns(_pagedList);

        // Act
        var response = _controller.GetUsers(_expectedPagination.Page, _expectedPagination.PageSize);

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _expectedUsers, Pagination = _expectedPagination });
    }
    #endregion
}
