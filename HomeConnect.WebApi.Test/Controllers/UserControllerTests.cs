using BusinessLogic;
using BusinessLogic.Admins.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.User;
using HomeConnect.WebApi.Controllers.User.Models;
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
    private List<User> _expectedUsers = null!;
    private Pagination _expectedPagination = null!;
    private PagedData<User> _pagedList = null!;

    [TestInitialize]
    public void Initialize()
    {
        _adminService = new Mock<IAdminService>();
        _controller = new UserController(_adminService.Object);

        _user = new User("Name", "Surname", "email@email.com", "Password@100", new Role("Admin", []))
        {
            RoleName = "Admin"
        };
        _otherUser = new User("Name1", "Surname1", "email1@email.com", "Password@100", new Role("BusinessOwner", []))
        {
            RoleName = "BusinessOwner"
        };
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
        _adminService.Setup(x => x.GetUsers(parameters.CurrentPage, parameters.PageSize, parameters.FullNameFilter,
            parameters.RoleFilter)).Returns(_pagedList);

        // Act
        var response = _controller.GetUsers(parameters);

        // Assert
        _adminService.VerifyAll();
        response.Should().BeEquivalentTo(new GetUsersResponse
        {
            Users = _expectedUsers.Select(user => new ListUserInfo
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Surname = user.Surname,
                Role = user.Role.Name,
                CreatedAt = user.CreatedAt.ToString()
            }).ToList(),
            Pagination = _expectedPagination
        });
    }

    [TestMethod]
    public void GetUsers_WhenCalledWithValidRequestAndFullNameFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        var parameters = new GetUsersRequest { FullNameFilter = _expectedUsers.First().Name };
        _adminService.Setup(x => x.GetUsers(parameters.CurrentPage, parameters.PageSize, parameters.FullNameFilter,
            parameters.RoleFilter)).Returns(_pagedList);

        // Act
        var response = _controller.GetUsers(parameters);

        // Assert
        _adminService.VerifyAll();
        response.Should().BeEquivalentTo(new GetUsersResponse
        {
            Users = _expectedUsers.Select(user => new ListUserInfo
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Surname = user.Surname,
                Role = user.Role.Name,
                CreatedAt = user.CreatedAt.ToString()
            }).ToList(),
            Pagination = _expectedPagination
        });
    }

    [TestMethod]
    public void GetUsers_WhenCalledWithValidRequestAndRoleFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        var parameters = new GetUsersRequest { RoleFilter = _expectedUsers.First().Role.Name };
        _adminService.Setup(x => x.GetUsers(parameters.CurrentPage, parameters.PageSize, parameters.FullNameFilter,
            parameters.RoleFilter)).Returns(_pagedList);

        // Act
        var response = _controller.GetUsers(parameters);

        // Assert
        _adminService.VerifyAll();
        response.Should().BeEquivalentTo(new GetUsersResponse
        {
            Users = _expectedUsers.Select(user => new ListUserInfo
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Surname = user.Surname,
                Role = user.Role.Name,
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
        _adminService.Setup(x => x.GetUsers(parameters.CurrentPage, parameters.PageSize, parameters.FullNameFilter,
            parameters.RoleFilter)).Returns(_pagedList);

        // Act
        var response = _controller.GetUsers(parameters);

        // Assert
        _adminService.VerifyAll();
        response.Should().BeEquivalentTo(new GetUsersResponse
        {
            Users = _expectedUsers.Select(user => new ListUserInfo
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Surname = user.Surname,
                Role = user.Role.Name,
                CreatedAt = user.CreatedAt.ToString()
            }).ToList(),
            Pagination = _expectedPagination
        });
    }

    #endregion
}
