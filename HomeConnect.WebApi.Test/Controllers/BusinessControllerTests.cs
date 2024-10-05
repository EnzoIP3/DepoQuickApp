using BusinessLogic;
using BusinessLogic.Admins.Models;
using BusinessLogic.Admins.Services;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Business;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class BusinessControllerTests
{
    private BusinessController _controller = null!;
    private Mock<IAdminService> _adminService = null!;
    private Role _role = null!;
    private User _user = null!;
    private User _otherUser = null!;
    private List<Business> _businesses = null!;
    private Pagination _expectedPagination;
    private PagedData<Business> _pagedList;

    [TestInitialize]
    public void Initialize()
    {
        _adminService = new Mock<IAdminService>();
        _controller = new BusinessController(_adminService.Object);

        _role = new Role("BusinessOwner", []);
        _user = new User("Name", "Surname", "email@email.com", "Password@100", _role);
        _otherUser = new User("Name1", "Surname1", "email1@email.com", "Password@100", _role);
        _businesses =
        [
            new Business("123456789123", "Business 1", _user),
            new Business("123456789124", "Business 2", _otherUser),
        ];
        _expectedPagination = new Pagination { Page = 1, PageSize = 10, TotalPages = 1 };
        _pagedList = new PagedData<Business>
        {
            Data = _businesses,
            Page = _expectedPagination.Page,
            PageSize = _expectedPagination.PageSize,
            TotalPages = _expectedPagination.TotalPages
        };
    }

    #region GetBusinesses
    [TestMethod]
    public void GetBusinesses_WhenCalledWithNoFiltersOrPagination_ReturnsExpectedResponse()
    {
        // Arrange
        _adminService.Setup(x => x.GetBusinesses(null, null, null, null)).Returns(_pagedList);

        // Act
        var response = _controller.GetBusinesses();

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _businesses, Pagination = _expectedPagination });
    }

    [TestMethod]
    public void GetBusinesses_WhenCalledWithNameFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        _adminService.Setup(x => x.GetBusinesses(null, null, _businesses.First().Name, null)).Returns(_pagedList);

        // Act
        var response = _controller.GetBusinesses(nameFilter: _businesses.First().Name);

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _businesses, Pagination = _expectedPagination });
    }

    [TestMethod]
    public void GetBusinesses_WhenCalledWithFullNameFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        _adminService.Setup(x => x.GetBusinesses(null, null, _businesses.First().Name, null)).Returns(_pagedList);

        // Act
        var response = _controller.GetBusinesses(nameFilter: _businesses.First().Name);

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _businesses, Pagination = _expectedPagination });
    }

    [TestMethod]
    public void GetBusinesses_WhenCalledWithPagination_ReturnsPagedExpectedResponse()
    {
        // Arrange
        _adminService.Setup(x => x.GetBusinesses(1, 1, null, null)).Returns(_pagedList);

        // Act
        var response = _controller.GetBusinesses(1, 1);

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _businesses, Pagination = _expectedPagination });
    }
    #endregion
}
