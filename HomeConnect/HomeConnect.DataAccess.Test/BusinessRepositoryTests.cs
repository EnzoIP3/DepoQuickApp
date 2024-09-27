using BusinessLogic;
using FluentAssertions;

namespace HomeConnect.DataAccess.Test;

[TestClass]
public class BusinessRepositoryTests
{
    private readonly Context _context = DbContextBuilder.BuildTestDbContext();
    private BusinessRepository _businessRepository = null!;
    private readonly User _validUser = new User("John", "Doe", "test@example.com", "password1M@", new Role("Role", []));

    private readonly User _otherUser =
        new User("Jane", "Doe", "test2@example.com", "password1M@", new Role("Role 2", []));

    private Business _validBusiness = null!;
    private Business _otherBusiness = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
        _businessRepository = new BusinessRepository(_context);
        _validBusiness = new Business("1", "Business", _validUser);
        _otherBusiness = new Business("2", "Other Business", _otherUser);
        _context.Businesses.Add(_validBusiness);
        _context.Businesses.Add(_otherBusiness);
        _context.SaveChanges();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    #region GetBusinesses

    [TestMethod]
    public void GetBusinesses_WithNoFilters_ReturnsAllBusinesses()
    {
        // Arrange
        var expected = new List<Business> { _validBusiness, _otherBusiness };

        // Act
        var result = _businessRepository.GetBusinesses(1, 2);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetBusinesses_WithPageAndPageSize_ReturnsBusinesses()
    {
        // Arrange
        var expected = new List<Business> { _validBusiness };

        // Act
        var result = _businessRepository.GetBusinesses(1, 1);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetBusinesses_WithFullNameFilter_ReturnsFilteredBusinesses()
    {
        // Arrange
        var expected = new List<Business> { _validBusiness };

        // Act
        var result = _businessRepository.GetBusinesses(1, 2, "John Doe");

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetBusinesses_WithNameFilter_ReturnsFilteredBusinesses()
    {
        // Arrange
        var expected = new List<Business> { _otherBusiness };

        // Act
        var result = _businessRepository.GetBusinesses(1, 2, null, "Other");

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetBusinesses_WithFullNameFilterAndNameFilter_ReturnsFilteredBusinesses()
    {
        // Arrange
        var expected = new List<Business> { _otherBusiness };

        // Act
        var result = _businessRepository.GetBusinesses(1, 2, "J", "Other");

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    #endregion

    #region Add

    #region Success

    [TestMethod]
    public void Add_WhenBusinessDoesNotExist_AddsBusiness()
    {
        // Arrange
        var business = new Business("3", "New Business", _validUser);

        // Act
        _businessRepository.Add(business);

        // Assert
        _context.Businesses.Should().Contain(business);
    }

    #endregion
    [TestMethod]
    public void Add_WhenBusinessAlreadyExists_ThrowsException()
    {
        // Arrange
        var business = new Business("1", "Business", _validUser);

        // Act
        var act = () => _businessRepository.Add(business);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #region Error

    #endregion

    #endregion

    #region GestBusinessByOwner

    #region Success

    [TestMethod]
    public void GetBusinessByOwner_WhenBusinessExists_ReturnsBusiness()
    {
        // Arrange
        var expected = _validBusiness;

        // Act
        var result = _businessRepository.GetBusinessByOwner(_validUser.Email);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
    #endregion

    #region Error

    #endregion

    #endregion
}
