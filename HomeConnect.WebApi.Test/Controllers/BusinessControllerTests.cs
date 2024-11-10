using BusinessLogic;
using BusinessLogic.Admins.Services;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Businesses;
using HomeConnect.WebApi.Controllers.Businesses.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class BusinessControllerTests
{
    private Mock<IAdminService> _adminService = null!;
    private CreateBusinessArgs _businessArgs = null!;
    private List<Business> _businesses = null!;
    private Mock<IBusinessOwnerService> _businessOwnerService = null!;
    private CreateBusinessRequest _businessRequest = null!;
    private BusinessController _controller = null!;
    private Pagination _expectedPagination = null!;
    private Mock<HttpContext> _httpContextMock = null!;
    private User _otherUser = null!;
    private PagedData<Business> _pagedList = null!;
    private Role _role = null!;
    private User _user = null!;
    private Guid _validatorId = Guid.NewGuid();

    [TestInitialize]
    public void Initialize()
    {
        _adminService = new Mock<IAdminService>(MockBehavior.Strict);
        _businessOwnerService = new Mock<IBusinessOwnerService>(MockBehavior.Strict);
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        _controller = new BusinessController(_adminService.Object, _businessOwnerService.Object);
        _controller.ControllerContext = new ControllerContext { HttpContext = _httpContextMock.Object };

        _role = new Role("BusinessOwner", []);
        _user = new User("Name", "Surname", "email@email.com", "Password@100", _role);
        _otherUser = new User("Name1", "Surname1", "email1@email.com", "Password@100", _role);
        _businesses =
        [
            new Business("123456789123", "Business 1", "https://example.com/image.png", _user, _validatorId),
            new Business("123456789124", "Business 2", "https://example.com/image.png", _otherUser),
        ];
        _expectedPagination = new Pagination { Page = 1, PageSize = 10, TotalPages = 1 };
        _pagedList = new PagedData<Business>
        {
            Data = _businesses,
            Page = _expectedPagination.Page,
            PageSize = _expectedPagination.PageSize,
            TotalPages = _expectedPagination.TotalPages
        };
        _businessArgs =
            new CreateBusinessArgs
            {
                Name = "Business 1",
                OwnerId = _user.Id.ToString(),
                Rut = "123456789123",
                Logo = "https://example.com/image.png",
                Validator = "Validator"
            };
        _businessRequest = new CreateBusinessRequest
        {
            Name = "Business 1",
            Rut = _businesses[0].Rut,
            Logo = "https://example.com/image.png",
            Validator = "Validator"
        };
    }

    #region CreateBusiness

    [TestMethod]
    public void CreateBusiness_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        _businessOwnerService.Setup(x => x.CreateBusiness(_businessArgs)).Returns(_businesses[0]);
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);

        var expectedResponse = new CreateBusinessResponse { Rut = _businesses[0].Rut };

        // Act
        CreateBusinessResponse response = _controller.CreateBusiness(_businessRequest);

        // Assert
        _businessOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeEquivalentTo(expectedResponse);
    }

    #endregion

    #region GetBusinesses

    [TestMethod]
    public void GetBusinesses_WhenCalledWithNoFiltersOrPagination_ReturnsExpectedResponse()
    {
        // Arrange
        _adminService.Setup(x => x.GetBusinesses(null, null, null, null)).Returns(_pagedList);

        var expectedBusinesses = _businesses.Select(b => new ListBusinessInfo
        {
            Name = b.Name,
            OwnerEmail = b.Owner.Email,
            OwnerName = b.Owner.Name,
            OwnerSurname = b.Owner.Surname,
            Rut = b.Rut
        }).ToList();

        var expectedResponse = new GetBusinessesResponse
        {
            Businesses = expectedBusinesses,
            Pagination = _expectedPagination
        };

        // Act
        GetBusinessesResponse response = _controller.GetBusinesses(new GetBusinessesRequest());

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeEquivalentTo(expectedResponse);
    }

    [TestMethod]
    public void GetBusinesses_WhenCalledWithNameFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        _adminService.Setup(x => x.GetBusinesses(null, null, _businesses.First().Name, null)).Returns(_pagedList);

        var expectedBusinesses = _businesses.Select(b => new ListBusinessInfo
        {
            Name = b.Name,
            OwnerEmail = b.Owner.Email,
            OwnerName = b.Owner.Name,
            OwnerSurname = b.Owner.Surname,
            Rut = b.Rut
        }).ToList();

        var expectedResponse = new GetBusinessesResponse
        {
            Businesses = expectedBusinesses,
            Pagination = _expectedPagination
        };

        // Act
        GetBusinessesResponse response =
            _controller.GetBusinesses(new GetBusinessesRequest { Name = _businesses.First().Name });

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeEquivalentTo(expectedResponse);
    }

    [TestMethod]
    public void GetBusinesses_WhenCalledWithPagination_ReturnsPagedExpectedResponse()
    {
        // Arrange
        _adminService.Setup(x => x.GetBusinesses(1, 1, null, null)).Returns(_pagedList);

        var expectedBusinesses = _businesses.Select(b => new ListBusinessInfo
        {
            Name = b.Name,
            OwnerEmail = b.Owner.Email,
            OwnerName = b.Owner.Name,
            OwnerSurname = b.Owner.Surname,
            Rut = b.Rut
        }).ToList();

        var expectedResponse = new GetBusinessesResponse
        {
            Businesses = expectedBusinesses,
            Pagination = _expectedPagination
        };

        // Act
        GetBusinessesResponse response =
            _controller.GetBusinesses(new GetBusinessesRequest { CurrentPage = 1, PageSize = 1 });

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeEquivalentTo(expectedResponse);
    }

    #endregion
}
