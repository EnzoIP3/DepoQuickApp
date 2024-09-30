using BusinessLogic;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public sealed class AdminServiceTests
{
    private Mock<IUserRepository> _userRepository = null!;
    private Mock<IBusinessRepository> _businessRepository = null!;
    private Mock<IRoleRepository> _roleRepository = null!;
    private AdminService _adminService = null!;
    private readonly int _defaultPageSize = 10;
    private readonly int _defaultCurrentPage = 1;

    private UserModel _validUserModel = new UserModel
    {
        Id = Guid.NewGuid().ToString(),
        Name = "name",
        Surname = "surname",
        Email = "email@email.com",
        Password = "Password#100",
        Role = "Admin"
    };

    private User _validUser = null!;
    private User _owner = null!;
    private User _otherOwner = null!;

    [TestInitialize]
    public void Initialize()
    {
        _userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
        _businessRepository = new Mock<IBusinessRepository>(MockBehavior.Strict);
        _roleRepository = new Mock<IRoleRepository>(MockBehavior.Strict);
        _adminService = new AdminService(_userRepository.Object, _businessRepository.Object, _roleRepository.Object);
        var adminRole = new Role("Admin", []);
        var businessOwnerRole = new Role("Business Owner", []);
        _validUser = new User(_validUserModel.Name, _validUserModel.Surname, _validUserModel.Email,
            _validUserModel.Password, adminRole);
        _owner = new User("name", "surname", "email@email.com", "Password#100", businessOwnerRole);
        _otherOwner = new User("name2", "surname2", "email2@email.com", "Password2#100", businessOwnerRole);
    }

    #region Create

    #region Error

    [TestMethod]
    public void Create_WhenAlreadyExists_ThrowsException()
    {
        // Arrange
        _userRepository.Setup(x => x.Exists(It.IsAny<Guid>())).Returns(true);

        // Act
        var act = () => _adminService.Create(_validUserModel);

        // Assert
        act.Should().Throw<Exception>().WithMessage("User already exists.");
    }

    [TestMethod]
    public void Create_WhenArgumentsHaveEmptyFields_ThrowsException()
    {
        // Arrange
        var invalidUserModel = new UserModel
        {
            Name = string.Empty,
            Surname = string.Empty,
            Email = string.Empty,
            Password = string.Empty,
            Role = string.Empty
        };

        // Act
        var act = () => _adminService.Create(invalidUserModel);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid input data.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void Create_WhenArgumentsAreValid_CreatesAdmin()
    {
        // Arrange
        _userRepository.Setup(x => x.Exists(It.IsAny<Guid>())).Returns(false);
        _userRepository.Setup(x => x.Add(It.IsAny<User>()));
        _roleRepository.Setup(x => x.GetRole(It.IsAny<string>())).Returns(new Role("Admin", []));

        // Act
        _adminService.Create(_validUserModel);

        // Assert
        _userRepository.Verify(x => x.Add(It.Is<User>(a =>
            a.Name == _validUserModel.Name &&
            a.Surname == _validUserModel.Surname &&
            a.Email == _validUserModel.Email &&
            a.Password == _validUserModel.Password &&
            a.Role.Name == _validUserModel.Role)));
    }

    #endregion

    #endregion

    #region Delete

    #region Error

    [TestMethod]
    public void Delete_WhenDoesNotExist_ThrowsException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _userRepository.Setup(x => x.Exists(It.IsAny<Guid>())).Returns(false);

        // Act
        var act = () => _adminService.Delete(id);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Admin does not exist.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void Delete_WhenArgumentsAreValid_DeletesAdmin()
    {
        // Arrange
        var id = Guid.NewGuid();
        _userRepository.Setup(x => x.Exists(It.IsAny<Guid>())).Returns(true);
        _userRepository.Setup(x => x.Delete(It.IsAny<Guid>()));

        // Act
        _adminService.Delete(id);

        // Assert
        _userRepository.Verify(x => x.Delete(It.Is<Guid>(a => a == id)));
    }

    #endregion

    #endregion

    #region CreateBusinessOwner

    #region Error

    [TestMethod]
    public void CreateBusinessOwner_WhenAlreadyExists_ThrowsException()
    {
        // Arrange
        var businessOwnerModel = new UserModel
        {
            Id = Guid.NewGuid().ToString(),
            Name = "name",
            Surname = "surname",
            Email = "email",
            Password = "password",
            Role = "BusinessOwner"
        };
        _userRepository.Setup(x => x.Exists(It.IsAny<Guid>())).Returns(true);

        // Act
        var act = () => _adminService.CreateBusinessOwner(businessOwnerModel);

        // Assert
        act.Should().Throw<Exception>().WithMessage("User already exists.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateBusinessOwner_WhenArgumentsAreValid_CreatesBusinessOwner()
    {
        // Arrange
        var businessOwnerModel = new UserModel
        {
            Id = Guid.NewGuid().ToString(),
            Name = "name",
            Surname = "surname",
            Email = "email@email.com",
            Password = "Password#100",
            Role = "Business Owner"
        };
        _userRepository.Setup(x => x.Exists(It.IsAny<Guid>())).Returns(false);
        _userRepository.Setup(x => x.Add(It.IsAny<User>()));
        _roleRepository.Setup(x => x.GetRole(It.IsAny<string>())).Returns(new Role(businessOwnerModel.Role, []));

        // Act
        _adminService.CreateBusinessOwner(businessOwnerModel);

        // Assert
        _userRepository.Verify(x => x.Add(It.Is<User>(a =>
            a.Name == businessOwnerModel.Name &&
            a.Surname == businessOwnerModel.Surname &&
            a.Email == businessOwnerModel.Email &&
            a.Password == businessOwnerModel.Password &&
            a.Role.Name == businessOwnerModel.Role)));
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
        var userList = new List<ListUserModel>
        {
            new ListUserModel
            {
                Id = _validUser.Id.ToString(),
                Name = _validUser.Name,
                Surname = _validUser.Surname,
                FullName = $"{_validUser.Name} {_validUser.Surname}",
                Role = "Admin",
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            },
            new ListUserModel
            {
                Id = _otherOwner.Id.ToString(),
                Name = _otherOwner.Name,
                Surname = _otherOwner.Surname,
                FullName = $"{_otherOwner.Name} {_otherOwner.Surname}",
                Role = "Business Owner",
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            }
        };
        var pagedList = new PagedData<User>
        {
            Data = users,
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        var pagedResponse = new PagedData<ListUserModel>
        {
            Data = userList,
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        _userRepository.Setup(x => x.GetUsers(_defaultCurrentPage, _defaultPageSize, null, null)).Returns(pagedList);

        // Act
        var result = _adminService.GetUsers(_defaultCurrentPage, _defaultPageSize);

        // Assert
        result.Should().BeEquivalentTo(pagedResponse, options => options.ComparingByMembers<PagedData<ListUserModel>>());
        _userRepository.Verify(x => x.GetUsers(
            It.Is<int>(a => a == _defaultCurrentPage),
            It.Is<int>(a => a == _defaultPageSize),
            It.Is<string>(a => true),
            It.Is<string>(a => true)));
    }

    [TestMethod]
    public void GetUsers_WhenCalledWithoutCurrentPageOrPageSize_ReturnsUserListWithDefaultValues()
    {
        // Arrange
        var users = new List<User> { _validUser, _otherOwner };
        var userList = new List<ListUserModel>
        {
            new ListUserModel
            {
                Id = _validUser.Id.ToString(),
                Name = _validUser.Name,
                Surname = _validUser.Surname,
                FullName = $"{_validUser.Name} {_validUser.Surname}",
                Role = "Admin",
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            },
            new ListUserModel
            {
                Id = _otherOwner.Id.ToString(),
                Name = _otherOwner.Name,
                Surname = _otherOwner.Surname,
                FullName = $"{_otherOwner.Name} {_otherOwner.Surname}",
                Role = "Business Owner",
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            }
        };
        var pagedList = new PagedData<User>
        {
            Data = users,
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        var pagedResponse = new PagedData<ListUserModel>
        {
            Data = userList,
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        _userRepository.Setup(x => x.GetUsers(_defaultCurrentPage, _defaultPageSize, null, null)).Returns(pagedList);

        // Act
        var result = _adminService.GetUsers();

        // Assert
        result.Should().BeEquivalentTo(pagedResponse, options => options.ComparingByMembers<PagedData<ListUserModel>>());
        _userRepository.Verify(x => x.GetUsers(
            It.Is<int>(a => a == _defaultCurrentPage),
            It.Is<int>(a => a == _defaultPageSize),
            It.Is<string>(a => true),
            It.Is<string>(a => true)));
    }

    [TestMethod]
    public void GetUsers_WhenCalledWithFullNameFilter_ReturnsFilteredUserList()
    {
        // Arrange
        var users = new List<User> { _validUser, _otherOwner };
        var userList = new List<ListUserModel>
        {
            new ListUserModel
            {
                Id = _validUser.Id.ToString(),
                Name = _validUser.Name,
                Surname = _validUser.Surname,
                FullName = $"{_validUser.Name} {_validUser.Surname}",
                Role = "Admin",
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            }
        };
        var filter = "name surname";
        var pagedList = new PagedData<User>
        {
            Data = [users[0]],
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        var pagedResponse = new PagedData<ListUserModel>
        {
            Data = userList,
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        _userRepository.Setup(x => x.GetUsers(_defaultCurrentPage, _defaultPageSize, filter, null))
            .Returns(pagedList);

        // Act
        var result = _adminService.GetUsers(fullNameFilter: filter);

        // Assert
        result.Should().BeEquivalentTo(pagedResponse, options => options.ComparingByMembers<PagedData<ListUserModel>>());
        _userRepository.Verify(x => x.GetUsers(
            It.Is<int>(a => a == _defaultCurrentPage),
            It.Is<int>(a => a == _defaultPageSize),
            It.Is<string>(a => a == filter),
            It.Is<string>(a => true)));
    }

    [TestMethod]
    public void GetUsers_WhenCalledWithRoleFilter_ReturnsFilteredUserList()
    {
        // Arrange
        var users = new List<User> { _validUser, _otherOwner };
        var userList = new List<ListUserModel>
        {
            new ListUserModel
            {
                Id = _otherOwner.Id.ToString(),
                Name = _otherOwner.Name,
                Surname = _otherOwner.Surname,
                FullName = $"{_otherOwner.Name} {_otherOwner.Surname}",
                Role = "Business Owner",
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            }
        };
        var filter = "BusinessOwner";
        var pagedList = new PagedData<User>
        {
            Data = [users[1]],
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        var pagedResponse = new PagedData<ListUserModel>
        {
            Data = userList,
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        _userRepository.Setup(x => x.GetUsers(_defaultCurrentPage, _defaultPageSize, null, filter))
            .Returns(pagedList);

        // Act
        var result = _adminService.GetUsers(roleFilter: filter);

        // Assert
        result.Should().BeEquivalentTo(pagedResponse, options => options.ComparingByMembers<PagedData<ListUserModel>>());
        _userRepository.Verify(x => x.GetUsers(
            It.Is<int>(a => a == _defaultCurrentPage),
            It.Is<int>(a => a == _defaultPageSize),
            It.Is<string>(a => true),
            It.Is<string>(a => a == filter)));
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
            new Business("123456789123", "name", _owner), new Business("123456789456", "name2", _otherOwner)
        };
        var businessList = new List<ListBusinessModel>
        {
            new ListBusinessModel
            {
                Name = "name",
                OwnerEmail = _owner.Email,
                OwnerFullName = $"{_owner.Name} {_owner.Surname}",
                Rut = "123456789123"
            },
            new ListBusinessModel
            {
                Name = "name2",
                OwnerEmail = _otherOwner.Email,
                OwnerFullName = $"{_otherOwner.Name} {_otherOwner.Surname}",
                Rut = "123456789456"
            }
        };
        var pagedList = new PagedData<Business>
        {
            Data = businesses,
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        var pagedResponse = new PagedData<ListBusinessModel>
        {
            Data = businessList,
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        _businessRepository.Setup(x => x.GetBusinesses(_defaultCurrentPage, _defaultPageSize, null, null))
            .Returns(pagedList);

        // Act
        var result = _adminService.GetBusiness(_defaultCurrentPage, _defaultPageSize);

        // Assert
        result.Should().BeEquivalentTo(pagedResponse, options => options.ComparingByMembers<PagedData<ListBusinessModel>>());
        _businessRepository.Verify(x => x.GetBusinesses(
            It.Is<int>(a => a == _defaultCurrentPage),
            It.Is<int>(a => a == _defaultPageSize),
            It.Is<string>(a => true),
            It.Is<string>(a => true)));
    }

    [TestMethod]
    public void GetBusiness_WhenCalledWithoutCurrentPageOrPageSize_ReturnsBusinessListWithDefaultValues()
    {
        // Arrange
        var businesses = new List<Business>
        {
            new Business("123456789123", "name", _owner), new Business("123456789456", "name2", _otherOwner)
        };
        var businessList = new List<ListBusinessModel>
        {
            new ListBusinessModel
            {
                Name = "name",
                OwnerEmail = _owner.Email,
                OwnerFullName = $"{_owner.Name} {_owner.Surname}",
                Rut = "123456789123"
            },
            new ListBusinessModel
            {
                Name = "name2",
                OwnerEmail = _otherOwner.Email,
                OwnerFullName = $"{_otherOwner.Name} {_otherOwner.Surname}",
                Rut = "123456789456"
            }
        };
        var pagedList = new PagedData<Business>
        {
            Data = businesses,
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        var pagedResponse = new PagedData<ListBusinessModel>
        {
            Data = businessList,
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        _businessRepository.Setup(x => x.GetBusinesses(_defaultCurrentPage, _defaultPageSize, null, null))
            .Returns(pagedList);

        // Act
        var result = _adminService.GetBusiness();

        // Assert
        result.Should().BeEquivalentTo(pagedResponse, options => options.ComparingByMembers<PagedData<ListBusinessModel>>());
        _businessRepository.Verify(x => x.GetBusinesses(
            It.Is<int>(a => a == _defaultCurrentPage),
            It.Is<int>(a => a == _defaultPageSize),
            It.Is<string>(a => true),
            It.Is<string>(a => true)));
    }

    [TestMethod]
    public void GetBusiness_WhenCalledWithFullNameFilter_ReturnsFilteredBusinessList()
    {
        // Arrange
        var businesses = new List<Business>
        {
            new Business("123456789123", "name", _owner), new Business("123456789456", "name2", _otherOwner)
        };
        var businessList = new List<ListBusinessModel>
        {
            new ListBusinessModel
            {
                Name = "name",
                OwnerEmail = _owner.Email,
                OwnerFullName = $"{_owner.Name} {_owner.Surname}",
                Rut = "123456789123"
            }
        };
        var filter = $"{_owner.Name} {_owner.Surname}";
        var pagedList = new PagedData<Business>
        {
            Data = [businesses[0]],
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        var pagedResponse = new PagedData<ListBusinessModel>
        {
            Data = businessList,
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        _businessRepository.Setup(x => x.GetBusinesses(_defaultCurrentPage, _defaultPageSize, filter, null))
            .Returns(pagedList);

        // Act
        var result = _adminService.GetBusiness(fullNameFilter: filter);

        // Assert
        result.Should().BeEquivalentTo(pagedResponse, options => options.ComparingByMembers<PagedData<ListBusinessModel>>());
        _businessRepository.Verify(x => x.GetBusinesses(
            It.Is<int>(a => a == _defaultCurrentPage),
            It.Is<int>(a => a == _defaultPageSize),
            It.Is<string>(a => a == filter),
            It.Is<string>(a => true)));
    }

    [TestMethod]
    public void GetBusiness_WhenCalledWithNameFilter_ReturnsFilteredBusinessList()
    {
        // Arrange
        var businesses = new List<Business>
        {
            new Business("123456789123", "name", _owner), new Business("123456789456", "name2", _otherOwner)
        };
        var businessList = new List<ListBusinessModel>
        {
            new ListBusinessModel
            {
                Name = "name2",
                OwnerEmail = _otherOwner.Email,
                OwnerFullName = $"{_otherOwner.Name} {_otherOwner.Surname}",
                Rut = "123456789456"
            }
        };
        var filter = "name2";
        var pagedList = new PagedData<Business>
        {
            Data = [businesses[1]],
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        var pagedResponse = new PagedData<ListBusinessModel>
        {
            Data = businessList,
            Page = _defaultCurrentPage,
            PageSize = _defaultPageSize,
            TotalPages = 1
        };
        _businessRepository.Setup(x => x.GetBusinesses(_defaultCurrentPage, _defaultPageSize, null, filter))
            .Returns(pagedList);

        // Act
        var result = _adminService.GetBusiness(nameFilter: filter);

        // Assert
        result.Should().BeEquivalentTo(pagedResponse, options => options.ComparingByMembers<PagedData<ListBusinessModel>>());
        _businessRepository.Verify(x => x.GetBusinesses(
            It.Is<int>(a => a == _defaultCurrentPage),
            It.Is<int>(a => a == _defaultPageSize),
            It.Is<string>(a => true),
            It.Is<string>(a => a == filter)));
    }

    #endregion
}
