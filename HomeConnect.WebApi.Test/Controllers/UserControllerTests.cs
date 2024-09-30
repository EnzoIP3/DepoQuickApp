using BusinessLogic;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.User;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

public class UserControllerTests
{
    private Mock<IAdminService> _adminService = null!;
    private UserController _controller = null!;

    [TestInitialize]
    public void Initialize()
    {
        _adminService = new Mock<IAdminService>();
        _controller = new UserController();
    }

    [TestMethod]
public void GetUsers_WhenCalledWithValidRequest_ReturnsExpectedResponse()
{
    // Arrange
    var user = new User("Name", "Surname", "email@email.com", "password@100",
        new Role("Admin", new List<SystemPermission>()));
    var otherUser = new User("Name1", "Surname1", "email1@email.com", "password@100",
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
    var expectedPagination = new Pagination { Page = 1, PageSize = 10, TotalPages = 1 };
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
}

public struct Pagination
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}
