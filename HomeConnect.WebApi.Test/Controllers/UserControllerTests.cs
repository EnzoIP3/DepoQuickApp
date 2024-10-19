using BusinessLogic;
using BusinessLogic.Admins.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
using BusinessLogic.Users.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Users;
using HomeConnect.WebApi.Controllers.Users.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class UserControllerTests
{
    private Mock<HttpContext> _httpContext = null!;
    private Mock<IAdminService> _adminService = null!;
    private Mock<IUserService> _userService = null!;
    private UserController _controller = null!;
    private Pagination _expectedPagination = null!;
    private List<User> _expectedUsers = null!;
    private User _otherUser = null!;
    private PagedData<User> _pagedList = null!;
    private User _user = null!;

    [TestInitialize]
    public void Initialize()
    {
        _httpContext = new Mock<HttpContext>(MockBehavior.Strict);
        _adminService = new Mock<IAdminService>(MockBehavior.Strict);
        _userService = new Mock<IUserService>(MockBehavior.Strict);
        _controller = new UserController(_adminService.Object, _userService.Object)
        {
            ControllerContext = new ControllerContext { HttpContext = _httpContext.Object }
        };

        _user = new User("Name", "Surname", "email@email.com", "Password@100", new Role("Admin", []));
        _otherUser = new User("Name1", "Surname1", "email1@email.com", "Password@100", new Role("BusinessOwner", []));
        _expectedUsers = [_user, _otherUser];
        _expectedPagination = new Pagination { Page = 1, PageSize = 10, TotalPages = 1 };
        _pagedList = new PagedData<User>
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
        var parameters = new GetUsersRequest();
        _adminService.Setup(x => x.GetUsers(parameters.CurrentPage, parameters.PageSize, parameters.FullName,
            parameters.Role)).Returns(_pagedList);

        // Act
        GetUsersResponse response = _controller.GetUsers(parameters);

        // Assert
        _adminService.VerifyAll();
        response.Should().BeEquivalentTo(new GetUsersResponse
        {
            Users = _expectedUsers.Select(user => new ListUserInfo
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Surname = user.Surname,
                Roles = [user.Roles.First().Name],
                CreatedAt = user.CreatedAt.ToString()
            }).ToList(),
            Pagination = _expectedPagination
        });
    }

    [TestMethod]
    public void GetUsers_WhenCalledWithValidRequestAndFullNameFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        var parameters = new GetUsersRequest { FullName = _expectedUsers.First().Name };
        _adminService.Setup(x => x.GetUsers(parameters.CurrentPage, parameters.PageSize, parameters.FullName,
            parameters.Role)).Returns(_pagedList);

        // Act
        GetUsersResponse response = _controller.GetUsers(parameters);

        // Assert
        _adminService.VerifyAll();
        response.Should().BeEquivalentTo(new GetUsersResponse
        {
            Users = _expectedUsers.Select(user => new ListUserInfo
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Surname = user.Surname,
                Roles = [user.Roles.First().Name],
                CreatedAt = user.CreatedAt.ToString()
            }).ToList(),
            Pagination = _expectedPagination
        });
    }

    [TestMethod]
    public void GetUsers_WhenCalledWithValidRequestAndRoleFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        var parameters = new GetUsersRequest { Role = _expectedUsers.First().Roles.First().Name };
        _adminService.Setup(x => x.GetUsers(parameters.CurrentPage, parameters.PageSize, parameters.FullName,
            parameters.Role)).Returns(_pagedList);

        // Act
        GetUsersResponse response = _controller.GetUsers(parameters);

        // Assert
        _adminService.VerifyAll();
        response.Should().BeEquivalentTo(new GetUsersResponse
        {
            Users = _expectedUsers.Select(user => new ListUserInfo
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Surname = user.Surname,
                Roles = [user.Roles.First().Name],
                CreatedAt = user.CreatedAt.ToString()
            }).ToList(),
            Pagination = _expectedPagination
        });
    }

    [TestMethod]
    public void GetUsers_WhenCalledWithPagination_ReturnsPagedExpectedResponse()
    {
        // Arrange
        var parameters = new GetUsersRequest
        {
            CurrentPage = _expectedPagination.Page, PageSize = _expectedPagination.PageSize
        };
        _adminService.Setup(x => x.GetUsers(parameters.CurrentPage, parameters.PageSize, parameters.FullName,
            parameters.Role)).Returns(_pagedList);

        // Act
        GetUsersResponse response = _controller.GetUsers(parameters);

        // Assert
        _adminService.VerifyAll();
        response.Should().BeEquivalentTo(new GetUsersResponse
        {
            Users = _expectedUsers.Select(user => new ListUserInfo
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Surname = user.Surname,
                Roles = [user.Roles.First().Name],
                CreatedAt = user.CreatedAt.ToString()
            }).ToList(),
            Pagination = _expectedPagination
        });
    }

    #endregion

    #region AddHomeOwnerRole

    [TestMethod]
    public void AddHomeOwnerRole_WhenCalledWithValidRequest_ReturnsExpectedResponse()
    {
        // Arrange
        var args = new AddRoleToUserArgs { UserId = _user.Id.ToString(), Role = "HomeOwner" };
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContext.Setup(h => h.Items).Returns(items);
        _userService.Setup(x => x.AddRoleToUser(args)).Verifiable();

        // Act
        var response = _controller.AddHomeOwnerRole();

        // Assert
        _userService.VerifyAll();
        response.Should().BeEquivalentTo(new AddHomeOwnerRoleResponse { Id = args.UserId });
    }

    #endregion
}
