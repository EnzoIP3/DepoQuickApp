using BusinessLogic;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public sealed class AdminServiceTests
{
    private Mock<IUserRepository> _userRepository = null!;
    private Mock<IBusinessRepository> _businessRepository = null!;
    private AdminService _adminService = null!;
    private int _defaultPageSize = 10;
    private int _defaultCurrentPage = 1;
    private UserModel _validUserModel = new UserModel
    {
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
        _adminService = new AdminService(_userRepository.Object, _businessRepository.Object);
        _validUser = new User(_validUserModel.Name, _validUserModel.Surname, _validUserModel.Email, _validUserModel.Password, _validUserModel.Role);
        _owner = new User("name", "surname", "email@email.com", "Password#100", "BusinessOwner");
        _otherOwner = new User("name2", "surname2", "email2@email.com", "Password2#100", "BusinessOwner");
    }

    #region Create
    #region Error
    [TestMethod]
    public void Create_WhenAlreadyExists_ThrowsException()
    {
        // Arrange
        _userRepository.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);

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
        _userRepository.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
        _userRepository.Setup(x => x.Add(It.IsAny<User>()));

        // Act
        _adminService.Create(_validUserModel);

        // Assert
        _userRepository.Verify(x => x.Add(It.Is<User>(a =>
            a.Name == _validUserModel.Name &&
            a.Surname == _validUserModel.Surname &&
            a.Email == _validUserModel.Email &&
            a.Password == _validUserModel.Password &&
            a.Role.ToString() == _validUserModel.Role)));
    }
    #endregion
    #endregion

    #region Delete
    #region Error
    [TestMethod]
    public void Delete_WhenDoesNotExist_ThrowsException()
    {
        // Arrange
        var email = "email";
        _userRepository.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);

        // Act
        var act = () => _adminService.Delete(email);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Admin does not exist.");
    }
    #endregion
    #region Success
    [TestMethod]
    public void Delete_WhenArgumentsAreValid_DeletesAdmin()
    {
        // Arrange
        var email = "email";
        _userRepository.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
        _userRepository.Setup(x => x.Delete(It.IsAny<string>()));

        // Act
        _adminService.Delete(email);

        // Assert
        _userRepository.Verify(x => x.Delete(It.Is<string>(a => a == email)));
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
            Name = "name",
            Surname = "surname",
            Email = "email",
            Password = "password",
            Role = "BusinessOwner"
        };
        _userRepository.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);

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
            Name = "name",
            Surname = "surname",
            Email = "email@email.com",
            Password = "Password#100",
            Role = "BusinessOwner"
        };
        _userRepository.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
        _userRepository.Setup(x => x.Add(It.IsAny<User>()));

        // Act
        _adminService.CreateBusinessOwner(businessOwnerModel);

        // Assert
        _userRepository.Verify(x => x.Add(It.Is<User>(a =>
            a.Name == businessOwnerModel.Name &&
            a.Surname == businessOwnerModel.Surname &&
            a.Email == businessOwnerModel.Email &&
            a.Password == businessOwnerModel.Password &&
            a.Role.ToString() == businessOwnerModel.Role)));
    }
    #endregion
    #endregion

    #region GetUsers

    #region Success
    [TestMethod]
    public void GetUsers_WhenCalled_ReturnsUserList()
    {
        // Arrange
        var users = new List<User>
        {
            _validUser,
            _otherOwner
        };
        var userList = new List<ListUserModel>
        {
            new ListUserModel
            {
                Name = "name",
                Surname = "surname",
                FullName = "name surname",
                Role = "Admin",
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            },
            new ListUserModel
            {
                Name = "name2",
                Surname = "surname2",
                FullName = "name2 surname2",
                Role = "BusinessOwner",
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            }
        };
        _userRepository.Setup(x => x.GetUsers(_defaultCurrentPage, _defaultPageSize, null, null)).Returns(users);

        // Act
        var result = _adminService.GetUsers(_defaultCurrentPage, _defaultPageSize);

        // Assert
        result.Should().BeEquivalentTo(userList);
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
        var users = new List<User>
        {
            _validUser,
            _otherOwner
        };
        var userList = new List<ListUserModel>
        {
            new ListUserModel
            {
                Name = "name",
                Surname = "surname",
                FullName = "name surname",
                Role = "Admin",
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            },
            new ListUserModel
            {
                Name = "name2",
                Surname = "surname2",
                FullName = "name2 surname2",
                Role = "BusinessOwner",
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            }
        };
        _userRepository.Setup(x => x.GetUsers(_defaultCurrentPage, _defaultPageSize, null, null)).Returns(users);

        // Act
        var result = _adminService.GetUsers();

        // Assert
        result.Should().BeEquivalentTo(userList);
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
        var users = new List<User>
        {
            _validUser,
            _otherOwner
        };
        var userList = new List<ListUserModel>
        {
            new ListUserModel
            {
                Name = "name",
                Surname = "surname",
                FullName = "name surname",
                Role = "Admin",
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            }
        };
        var filter = "name surname";
        _userRepository.Setup(x => x.GetUsers(_defaultCurrentPage, _defaultPageSize, filter, null)).
            Returns(new List<User> { users[0] });

        // Act
        var result = _adminService.GetUsers(fullNameFilter: filter);

        // Assert
        result.Should().BeEquivalentTo(userList);
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
        var users = new List<User>
        {
            _validUser,
            _otherOwner
        };
        var userList = new List<ListUserModel>
        {
            new ListUserModel
            {
                Name = "name2",
                Surname = "surname2",
                FullName = "name2 surname2",
                Role = "BusinessOwner",
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            }
        };
        var filter = "BusinessOwner";
        _userRepository.Setup(x => x.GetUsers(_defaultCurrentPage, _defaultPageSize, null, filter)).
            Returns(new List<User> { users[1] });

        // Act
        var result = _adminService.GetUsers(roleFilter: filter);

        // Assert
        result.Should().BeEquivalentTo(userList);
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
            new Business("123456789123", "name", _owner),
            new Business("123456789456", "name2", _otherOwner)
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
        _businessRepository.Setup(x => x.GetBusinesses(_defaultCurrentPage, _defaultPageSize, null, null)).Returns(businesses);

        // Act
        var result = _adminService.GetBusiness(_defaultCurrentPage, _defaultPageSize);

        // Assert
        result.Should().BeEquivalentTo(businessList);
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
            new Business("123456789123", "name", _owner),
            new Business("123456789456", "name2", _otherOwner)
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
        _businessRepository.Setup(x => x.GetBusinesses(_defaultCurrentPage, _defaultPageSize, null, null)).Returns(businesses);

        // Act
        var result = _adminService.GetBusiness();

        // Assert
        result.Should().BeEquivalentTo(businessList);
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
            new Business("123456789123", "name", _owner),
            new Business("123456789456", "name2", _otherOwner)
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
        _businessRepository.Setup(x => x.GetBusinesses(_defaultCurrentPage, _defaultPageSize, filter, null)).Returns(new List<Business> { businesses[0] });

        // Act
        var result = _adminService.GetBusiness(fullNameFilter: filter);

        // Assert
        result.Should().BeEquivalentTo(businessList);
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
            new Business("123456789123", "name", _owner),
            new Business("123456789456", "name2", _otherOwner)
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
        _businessRepository.Setup(x => x.GetBusinesses(_defaultCurrentPage, _defaultPageSize, null, filter)).Returns(new List<Business> { businesses[1] });

        // Act
        var result = _adminService.GetBusiness(nameFilter: filter);

        // Assert
        result.Should().BeEquivalentTo(businessList);
        _businessRepository.Verify(x => x.GetBusinesses(
            It.Is<int>(a => a == _defaultCurrentPage),
            It.Is<int>(a => a == _defaultPageSize),
            It.Is<string>(a => true),
            It.Is<string>(a => a == filter)));
    }
    #endregion
}
