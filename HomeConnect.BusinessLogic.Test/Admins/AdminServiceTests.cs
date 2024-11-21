using BusinessLogic;
using BusinessLogic.Admins.Models;
using BusinessLogic.Admins.Services;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Repositories;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
using BusinessLogic.Users.Repositories;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test.Admins;

[TestClass]
public sealed class AdminServiceTests
{
    private readonly int _defaultCurrentPage = 1;
    private readonly int _defaultPageSize = 10;

    private readonly CreateUserArgs _validCreateUserArgs = new()
    {
        Name = "name",
        Surname = "surname",
        Email = "email@email.com",
        Password = "Password#100",
        Role = "Admin"
    };

    private AdminService _adminService = null!;
    private Mock<IBusinessRepository> _businessRepository = null!;
    private User _otherOwner = null!;
    private User _owner = null!;
    private Mock<IUserRepository> _userRepository = null!;

    private User _validUser = null!;

    [TestInitialize]
    public void Initialize()
    {
        _userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
        _businessRepository = new Mock<IBusinessRepository>(MockBehavior.Strict);
        _adminService = new AdminService(_userRepository.Object, _businessRepository.Object);
        var adminRole = new Role("Admin", []);
        var businessOwnerRole = new Role("Business Owner", []);
        _validUser = new User(_validCreateUserArgs.Name,
            _validCreateUserArgs.Surname, _validCreateUserArgs.Email,
            _validCreateUserArgs.Password, adminRole);
        _owner = new User("name", "surname", "email@email.com", "Password#100",
            businessOwnerRole);
        _otherOwner = new User("name2", "surname2", "email2@email.com",
            "Password2#100", businessOwnerRole);
    }

    #region Delete

    #region Error

    [TestMethod]
    public void Delete_WhenDoesNotExist_ThrowsException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _userRepository.Setup(x => x.Exists(It.IsAny<Guid>())).Returns(false);

        // Act
        Action act = () => _adminService.DeleteAdmin(id.ToString());

        // Assert
        act.Should().Throw<KeyNotFoundException>().WithMessage("Admin does not exist.");
    }

    [TestMethod]
    public void Delete_WhenIdIsNotAGuid_ThrowsException()
    {
        // Arrange
        var id = "not-a-guid";

        // Act
        Action act = () => _adminService.DeleteAdmin(id);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The id is not a valid GUID.");
    }

    [TestMethod]
    public void Delete_WhenOnlyOneAdminExists_ThrowsException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var args = new FilterArgs { RoleFilter = "Admin" };
        _userRepository.Setup(x => x.Exists(id)).Returns(true);
        _userRepository.Setup(x => x.GetPaged(It.IsAny<FilterArgs>())).Returns(
            new PagedData<User>
            {
                Data = [_validUser],
                Page = _defaultCurrentPage,
                PageSize = _defaultPageSize,
                TotalPages = 1
            });

        // Act
        Action act = () => _adminService.DeleteAdmin(id.ToString());

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("The last admin cannot be deleted");
        _userRepository.Verify(x => x.GetPaged(It.Is<FilterArgs>(a => a.RoleFilter == "Admin")));
    }

    #endregion

    #region Success

    [TestMethod]
    public void Delete_WhenArgumentsAreValid_DeletesAdmin()
    {
        // Arrange
        var id = Guid.NewGuid();
        var args = new FilterArgs { CurrentPage = 1, PageSize = 1, RoleFilter = "Admin" };
        _userRepository.Setup(x => x.Exists(id)).Returns(true);
        _userRepository.Setup(x => x.Delete(id));
        _userRepository.Setup(x => x.GetPaged(It.IsAny<FilterArgs>())).Returns(
            new PagedData<User>
            {
                Data = [_validUser, _otherOwner],
                Page = _defaultCurrentPage,
                PageSize = _defaultPageSize,
                TotalPages = 2
            });

        // Act
        _adminService.DeleteAdmin(id.ToString());

        // Assert
        _userRepository.Verify(x => x.Delete(It.Is<Guid>(a => a == id)));
        _userRepository.Verify(x =>
            x.GetPaged(It.Is<FilterArgs>(a => a.PageSize == 1 && a.CurrentPage == 1 && a.RoleFilter == "Admin")));
    }

    #endregion

    #endregion

    #region GetUsers

    #region Success

    [TestMethod]
    public void GetUsers_WhenCalled_ReturnsUserList()
    {
        // Arrange
        var users = new List<User> { _validUser, _otherOwner };
        var pagedList = new PagedData<User>
        {
            Data = users,
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        var args = new FilterArgs { CurrentPage = _defaultCurrentPage, PageSize = _defaultPageSize };
        _userRepository.Setup(x => x.GetPaged(args)).Returns(pagedList);

        // Act
        PagedData<User> result =
            _adminService.GetUsers(new GetUsersArgs { CurrentPage = _defaultCurrentPage, PageSize = _defaultPageSize });

        // Assert
        result.Should().BeEquivalentTo(pagedList, options => options.ComparingByMembers<PagedData<User>>());
        _userRepository.Verify(x => x.GetPaged(
            It.Is<FilterArgs>(a => a.FullNameFilter == null && a.RoleFilter == null)));
    }

    [TestMethod]
    public void GetUsers_WhenCalledWithoutCurrentPageOrPageSize_ReturnsUserListWithDefaultValues()
    {
        // Arrange
        var users = new List<User> { _validUser, _otherOwner };
        var pagedList = new PagedData<User>
        {
            Data = users,
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        var args = new FilterArgs();
        _userRepository.Setup(x => x.GetPaged(args)).Returns(pagedList);

        // Act
        PagedData<User> result = _adminService.GetUsers(new GetUsersArgs());

        // Assert
        result.Should().BeEquivalentTo(pagedList, options => options.ComparingByMembers<PagedData<User>>());
        _userRepository.Verify(x => x.GetPaged(
            It.Is<FilterArgs>(a => a.FullNameFilter == null && a.RoleFilter == null)));
    }

    [TestMethod]
    public void GetUsers_WhenCalledWithFullNameFilter_ReturnsFilteredUserList()
    {
        // Arrange
        var users = new List<User> { _validUser, _otherOwner };
        var filter = "name surname";
        var pagedList = new PagedData<User>
        {
            Data = [users[0]],
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        var args = new FilterArgs { CurrentPage = _defaultCurrentPage, PageSize = _defaultPageSize, FullNameFilter = filter };
        _userRepository.Setup(x => x.GetPaged(args))
            .Returns(pagedList);

        // Act
        PagedData<User> result = _adminService.GetUsers(new GetUsersArgs { FullNameFilter = filter });

        // Assert
        result.Should().BeEquivalentTo(pagedList, options => options.ComparingByMembers<PagedData<User>>());
        _userRepository.Verify(x => x.GetPaged(
            It.Is<FilterArgs>(a => a.FullNameFilter == filter && a.RoleFilter == null)));
    }

    [TestMethod]
    public void GetUsers_WhenCalledWithRoleFilter_ReturnsFilteredUserList()
    {
        // Arrange
        var users = new List<User> { _validUser, _otherOwner };
        var filter = "BusinessOwner";
        var pagedList = new PagedData<User>
        {
            Data = [users[1]],
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        var args = new FilterArgs { CurrentPage = _defaultCurrentPage, PageSize = _defaultPageSize, RoleFilter = filter };
        _userRepository.Setup(x => x.GetPaged(args))
            .Returns(pagedList);

        // Act
        PagedData<User> result = _adminService.GetUsers(new GetUsersArgs { RoleFilter = filter });

        // Assert
        result.Should().BeEquivalentTo(pagedList, options => options.ComparingByMembers<PagedData<User>>());
        _userRepository.Verify(x => x.GetPaged(
            It.Is<FilterArgs>(a => a.FullNameFilter == null && a.RoleFilter == filter)));
    }

    #endregion

    #endregion

    #region GetBusiness

    [TestMethod]
    public void GetBusiness_WhenCalled_ReturnsBusinessList()
    {
        // Arrange
        var businesses = new List<Business>
        {
            new("123456789123", "name", "https://example.com/image.png", _owner),
            new("123456789456", "name2", "https://example.com/image.png", _otherOwner)
        };
        var pagedList = new PagedData<Business>
        {
            Data = businesses,
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        var filterArgs = new FilterArgs { CurrentPage = _defaultCurrentPage, PageSize = _defaultPageSize };
        _businessRepository.Setup(x => x.GetPaged(filterArgs))
            .Returns(pagedList);

        // Act
        PagedData<Business> result =
            _adminService.GetBusinesses(new GetBusinessesArgs
            {
                CurrentPage = _defaultCurrentPage,
                PageSize = _defaultPageSize
            });

        // Assert
        result.Should().BeEquivalentTo(pagedList, options => options.ComparingByMembers<PagedData<Business>>());
        _businessRepository.Verify(x => x.GetPaged(
            It.Is<FilterArgs>(a => a.FullNameFilter == null && a.NameFilter == null)));
    }

    [TestMethod]
    public void GetBusiness_WhenCalledWithoutCurrentPageOrPageSize_ReturnsBusinessListWithDefaultValues()
    {
        // Arrange
        var businesses = new List<Business>
        {
            new("123456789123", "name", "https://example.com/image.png", _owner),
            new("123456789456", "name2", "https://example.com/image.png", _otherOwner)
        };
        var pagedList = new PagedData<Business>
        {
            Data = businesses,
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        var filterArgs = new FilterArgs { CurrentPage = _defaultCurrentPage, PageSize = _defaultPageSize };
        _businessRepository.Setup(x => x.GetPaged(filterArgs))
            .Returns(pagedList);

        // Act
        PagedData<Business> result = _adminService.GetBusinesses(new GetBusinessesArgs());

        // Assert
        result.Should().BeEquivalentTo(pagedList, options => options.ComparingByMembers<PagedData<Business>>());
        _businessRepository.Verify(x => x.GetPaged(
            It.Is<FilterArgs>(a => a.FullNameFilter == null && a.NameFilter == null)));
    }

    [TestMethod]
    public void GetBusiness_WhenCalledWithFullNameFilter_ReturnsFilteredBusinessList()
    {
        // Arrange
        var businesses = new List<Business>
        {
            new("123456789123", "name", "https://example.com/image.png", _owner),
            new("123456789456", "name2", "https://example.com/image.png", _otherOwner)
        };
        var filter = $"{_owner.Name} {_owner.Surname}";
        var pagedList = new PagedData<Business>
        {
            Data = [businesses[0]],
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        var filterArgs = new FilterArgs
        {
            CurrentPage = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            FullNameFilter = filter
        };
        _businessRepository.Setup(x => x.GetPaged(filterArgs))
            .Returns(pagedList);

        // Act
        PagedData<Business> result = _adminService.GetBusinesses(new GetBusinessesArgs { FullNameFilter = filter });

        // Assert
        result.Should().BeEquivalentTo(pagedList, options => options.ComparingByMembers<PagedData<Business>>());
        _businessRepository.Verify(x => x.GetPaged(
            It.Is<FilterArgs>(a => a.FullNameFilter == filter && a.NameFilter == null)));
    }

    [TestMethod]
    public void GetBusiness_WhenCalledWithNameFilter_ReturnsFilteredBusinessList()
    {
        // Arrange
        var businesses = new List<Business>
        {
            new("123456789123", "name", "https://example.com/image.png", _owner),
            new("123456789456", "name2", "https://example.com/image.png", _otherOwner)
        };
        var filter = "name2";
        var pagedList = new PagedData<Business>
        {
            Data = [businesses[1]],
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        var filterArgs = new FilterArgs
        {
            CurrentPage = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            NameFilter = filter
        };
        _businessRepository.Setup(x => x.GetPaged(filterArgs))
            .Returns(pagedList);

        // Act
        PagedData<Business> result = _adminService.GetBusinesses(new GetBusinessesArgs { NameFilter = filter });

        // Assert
        result.Should().BeEquivalentTo(pagedList, options => options.ComparingByMembers<PagedData<Business>>());
        _businessRepository.Verify(x => x.GetPaged(
            It.Is<FilterArgs>(a => a.FullNameFilter == null && a.NameFilter == filter)));
    }

    #endregion
}
