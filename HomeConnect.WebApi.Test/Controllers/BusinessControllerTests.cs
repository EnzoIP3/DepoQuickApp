using BusinessLogic;
using FluentAssertions;
using HomeConnect.WebApi.Controllers;
using HomeConnect.WebApi.Controllers.User;
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
    public void GetBusinesses_WhenCalledWithNoParameters_ReturnsExpectedResponse()
    {
        // Arrange
        var role = new Role("BusinessOwner", new List<SystemPermission>());
        var user = new User("Name", "Surname", "email@email.com", "Password@100", role);
        var otherUser = new User("Name1", "Surname1", "email1@email.com", "Password@100", role);
        var businesses = new List<Business>
        {
            new Business("123456789123", "Business 1", user),
            new Business("123456789124", "Business 2", otherUser),
        };
        var expectedBusinesses = new List<ListBusinessModel>
        {
            new ListBusinessModel()
            {
                Name = businesses[0].Name,
                OwnerFullName = $"{user.Name} {user.Surname}",
                OwnerEmail = user.Email,
                Rut = businesses[0].Rut,
            },
            new ListBusinessModel()
            {
                Name = businesses[1].Name,
                OwnerFullName = $"{otherUser.Name} {otherUser.Surname}",
                OwnerEmail = otherUser.Email,
                Rut = businesses[1].Rut,
            }
        };
        var expectedPagination = new Pagination { Page = 1, PageSize = 10, TotalPages = 1 };
        var expectedResponse = new { Data = expectedBusinesses, Pagination = expectedPagination };
        var pagedList = new PagedData<ListBusinessModel>
        {
            Data = expectedBusinesses,
            Page = expectedPagination.Page,
            PageSize = expectedPagination.PageSize,
            TotalPages = expectedPagination.TotalPages
        };

        _adminService.Setup(x => x.GetBusiness(null, null, null, null)).Returns(pagedList);

        // Act
        var response = _controller.GetBusinesses();

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }
}
