using BusinessLogic;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.DataAccess.Repositories;

namespace HomeConnect.DataAccess.Test.Repositories;

[TestClass]
public class BusinessRepositoryTests
{
    private readonly Context _context = DbContextBuilder.BuildTestDbContext();

    private readonly User _otherUser = new("Jane", "Doe", "test2@example.com", "password1M@", new Role("Role 2", []));

    private readonly User _validUser = new("John", "Doe", "test@example.com", "password1M@", new Role("Role", []));
    private BusinessRepository _businessRepository = null!;
    private Business _otherBusiness = null!;

    private Business _validBusiness = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
        _businessRepository = new BusinessRepository(_context);
        _validBusiness = new Business("1", "Business", "https://example.com/image.png", _validUser);
        _otherBusiness = new Business("2", "Other Business", "https://example.com/image.png", _otherUser);
        _context.Businesses.Add(_validBusiness);
        _context.Businesses.Add(_otherBusiness);
        _context.SaveChanges();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    #region GestBusinessByRut

    #region Success

    [TestMethod]
    public void GetBusinessByRut_WhenBusinessExists_ReturnsBusiness()
    {
        // Arrange
        Business expected = _validBusiness;

        // Act
        Business result = _businessRepository.Get(_validBusiness.Rut);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    #endregion

    #endregion

    #region Exists

    [TestMethod]
    public void Exists_WhenBusinessExists_ReturnsTrue()
    {
        // Arrange
        var rut = _validBusiness.Rut;

        // Act
        var result = _businessRepository.Exists(rut);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #region GetByOwnerId

    [TestMethod]
    public void GetByOwnerId_WhenBusinessExists_ReturnsBusiness()
    {
        // Arrange
        Business expected = _validBusiness;

        // Act
        Business result = _businessRepository.GetByOwnerId(_validUser.Id);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    #endregion

    #region ExistsByOwnerId

    [TestMethod]
    public void ExistsByOwnerId_WhenBusinessExists_ReturnsTrue()
    {
        // Arrange
        Guid ownerId = _validUser.Id;

        // Act
        var result = _businessRepository.ExistsByOwnerId(ownerId);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #region Add

    #region Success

    [TestMethod]
    public void Add_WhenBusinessDoesNotExist_AddsBusiness()
    {
        // Arrange
        var business = new Business("3", "New Business", "https://example.com/image.png", _validUser);

        // Act
        _businessRepository.Add(business);

        // Assert
        _context.Businesses.Should().Contain(business);
    }

    #endregion

    #endregion

    #region UpdateValidator

    [TestMethod]
    public void UpdateValidator_WhenBusinessExists_UpdatesValidator()
    {
        // Arrange
        Business business = _validBusiness;
        var validatorId = Guid.NewGuid();

        // Act
        _businessRepository.UpdateValidator(business.Rut, validatorId);

        // Assert
        business.Validator.Should().Be(validatorId);
    }

    #endregion

    #region GetBusinesses

    [TestMethod]
    public void GetBusinesses_WithNoFilters_ReturnsAllBusinesses()
    {
        // Arrange
        var expected = new PagedData<Business>
        {
            Data = [_validBusiness, _otherBusiness],
            Page = 1,
            PageSize = 2,
            TotalPages = 1
        };
        var filterArgs = new FilterArgs { CurrentPage = 1, PageSize = 2 };

        // Act
        PagedData<Business> result = _businessRepository.GetPaged(filterArgs);

        // Assert
        result.Data.Should().BeEquivalentTo(expected.Data);
    }

    [TestMethod]
    public void GetBusinesses_WithPageAndPageSize_ReturnsBusinesses()
    {
        // Arrange
        var expected = new List<Business> { _validBusiness };
        var filterArgs = new FilterArgs { CurrentPage = 1, PageSize = 1 };

        // Act
        PagedData<Business> result = _businessRepository.GetPaged(filterArgs);

        // Assert
        result.Data.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetBusinesses_WithFullNameFilter_ReturnsFilteredBusinesses()
    {
        // Arrange
        var expected = new PagedData<Business> { Data = [_validBusiness], Page = 1, PageSize = 2, TotalPages = 1 };
        var filterArgs = new FilterArgs { CurrentPage = 1, PageSize = 2, FullNameFilter = "John Doe" };

        // Act
        PagedData<Business> result = _businessRepository.GetPaged(filterArgs);

        // Assert
        result.Data.Should().BeEquivalentTo(expected.Data);
    }

    [TestMethod]
    public void GetBusinesses_WithNameFilter_ReturnsFilteredBusinesses()
    {
        // Arrange
        var expected = new PagedData<Business> { Data = [_otherBusiness], Page = 1, PageSize = 2, TotalPages = 1 };
        var filterArgs = new FilterArgs { CurrentPage = 1, PageSize = 2, NameFilter = "Other" };

        // Act
        PagedData<Business> result = _businessRepository.GetPaged(filterArgs);

        // Assert
        result.Data.Should().BeEquivalentTo(expected.Data);
    }

    [TestMethod]
    public void GetBusinesses_WithFullNameFilterAndNameFilter_ReturnsFilteredBusinesses()
    {
        // Arrange
        var expected = new PagedData<Business> { Data = [_otherBusiness], Page = 1, PageSize = 2, TotalPages = 1 };
        var filterArgs = new FilterArgs
        {
            CurrentPage = 1,
            PageSize = 2,
            FullNameFilter = "Jane Doe",
            NameFilter = "Other"
        };

        // Act
        PagedData<Business> result = _businessRepository.GetPaged(filterArgs);

        // Assert
        result.Data.Should().BeEquivalentTo(expected.Data);
    }

    [TestMethod]
    public void GetBusinesses_WithOwnerIdFilter_ReturnsFilteredBusinesses()
    {
        // Arrange
        var expected = new PagedData<Business> { Data = [_validBusiness], Page = 1, PageSize = 2, TotalPages = 1 };
        var filterArgs = new FilterArgs { CurrentPage = 1, PageSize = 2, OwnerIdFilter = _validUser.Id };

        // Act
        PagedData<Business> result = _businessRepository.GetPaged(filterArgs);

        // Assert
        result.Data.Should().BeEquivalentTo(expected.Data);
    }

    #endregion

    #region GestBusinessByOwner

    #region Success

    [TestMethod]
    public void GetBusinessByOwner_WhenBusinessExists_ReturnsBusiness()
    {
        // Arrange
        Business expected = _validBusiness;

        // Act
        Business? result = _businessRepository.GetBusinessByOwnerId(_validUser.Email);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetBusinessByOwner_WhenBusinessDoesNotExist_ReturnsNull()
    {
        // Arrange
        var nonExistentOwnerEmail = "nonexistent@example.com";

        // Act
        Business? result = _businessRepository.GetBusinessByOwnerId(nonExistentOwnerEmail);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #endregion
}
