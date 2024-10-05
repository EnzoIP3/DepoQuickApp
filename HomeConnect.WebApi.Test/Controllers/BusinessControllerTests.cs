using BusinessLogic;
using BusinessLogic.Admins.Models;
using BusinessLogic.Admins.Services;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Business;
using HomeConnect.WebApi.Controllers.Businesses.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class BusinessControllerTests
{
    private BusinessController _controller = null!;
    private Mock<IBusinessOwnerService> _businessOwnerService = null!;
    private Mock<IAdminService> _adminService = null!;
    private Role _role = null!;
    private User _user = null!;
    private User _otherUser = null!;
    private CreateBusinessArgs _businessArgs;
    private CreateBusinessRequest _businessRequest;
    private List<Business> _businesses = null!;
    private List<GetBusinessesArgs> _expectedBusinesses = null!;
    private Pagination _expectedPagination;
    private PagedData<GetBusinessesArgs> _pagedList;

    [TestInitialize]
    public void Initialize()
    {
        _adminService = new Mock<IAdminService>();
        _businessOwnerService = new Mock<IBusinessOwnerService>();
        _controller = new BusinessController(_adminService.Object, _businessOwnerService.Object);

        _role = new Role("BusinessOwner", []);
        _user = new User("Name", "Surname", "email@email.com", "Password@100", _role);
        _otherUser = new User("Name1", "Surname1", "email1@email.com", "Password@100", _role);
        _businesses =
        [
            new Business("123456789123", "Business 1", _user),
            new Business("123456789124", "Business 2", _otherUser),
        ];
        _businessArgs = new CreateBusinessArgs
        {
            Name = _businesses[0].Name,
            Rut = _businesses[0].Rut,
            OwnerId = _user.Id.ToString()
        };
        _businessRequest = new CreateBusinessRequest
        {
            Name = _businesses[0].Name,
            Rut = _businesses[0].Rut,
            OwnerId = _user.Id.ToString()
        };
        _expectedBusinesses =
        [
            new GetBusinessesArgs()
            {
                Name = _businesses[0].Name,
                OwnerFullName = $"{_user.Name} {_user.Surname}",
                OwnerEmail = _user.Email,
                Rut = _businesses[0].Rut,
            },
            new GetBusinessesArgs()
            {
                Name = _businesses[1].Name,
                OwnerFullName = $"{_otherUser.Name} {_otherUser.Surname}",
                OwnerEmail = _otherUser.Email,
                Rut = _businesses[1].Rut,
            }

        ];
        _expectedPagination = new Pagination { Page = 1, PageSize = 10, TotalPages = 1 };
        _pagedList = new PagedData<GetBusinessesArgs>
        {
            Data = _expectedBusinesses,
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
        okResult.Value.Should().BeEquivalentTo(new { Data = _expectedBusinesses, Pagination = _expectedPagination });
    }

    [TestMethod]
    public void GetBusinesses_WhenCalledWithNameFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        _adminService.Setup(x => x.GetBusinesses(null, null, _expectedBusinesses.First().Name, null))
            .Returns(_pagedList);

        // Act
        var response = _controller.GetBusinesses(nameFilter: _expectedBusinesses.First().Name);

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _expectedBusinesses, Pagination = _expectedPagination });
    }

    [TestMethod]
    public void GetBusinesses_WhenCalledWithFullNameFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        _adminService.Setup(x => x.GetBusinesses(null, null, _expectedBusinesses.First().OwnerFullName, null))
            .Returns(_pagedList);

        // Act
        var response = _controller.GetBusinesses(nameFilter: _expectedBusinesses.First().OwnerFullName);

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _expectedBusinesses, Pagination = _expectedPagination });
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
        okResult.Value.Should().BeEquivalentTo(new { Data = _expectedBusinesses, Pagination = _expectedPagination });
    }

    #endregion

    #region CreateBusiness

    [TestMethod]
    public void CreateBusiness_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        _businessOwnerService.Setup(x => x.CreateBusiness(_businessArgs)).Returns(_businesses[0].Rut);

        // Act
        var response = _controller.CreateBusiness(_businessRequest);

        // Assert
        _businessOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.Rut.Should().Be(_businesses[0].Rut);
    }

    #endregion
}
