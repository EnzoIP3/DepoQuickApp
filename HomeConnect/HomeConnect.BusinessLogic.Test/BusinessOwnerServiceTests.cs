using BusinessLogic;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class BusinessOwnerServiceTests
{
    private Mock<IUserRepository> _userRepository = null!;
    private Mock<IBusinessRepository> _businessRepository = null!;
    private Mock<IRoleRepository> _roleRepository = null!;
    private BusinessOwnerService _businessOwnerService = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
        _businessRepository = new Mock<IBusinessRepository>(MockBehavior.Strict);
        _roleRepository = new Mock<IRoleRepository>(MockBehavior.Strict);
        _businessOwnerService = new BusinessOwnerService(_userRepository.Object, _businessRepository.Object, _roleRepository.Object);
    }

    #region CreateBusiness

    #region Success

    [TestMethod]
    public void CreateBusiness_WhenOwnerExists_CreatesBusiness()
    {
        // Arrange
        var ownerEmail = "owner@example.com";
        var businessRut = "123456789";
        var businessName = "Test Business";

        _userRepository.Setup(x => x.Exists(ownerEmail)).Returns(true);
        _businessRepository.Setup(x => x.Add(It.IsAny<Business>()));

        // Act
        _businessOwnerService.CreateBusiness(ownerEmail, businessRut, businessName);

        // Assert
        _businessRepository.Verify(x => x.Add(It.Is<Business>(b =>
            b.Rut == businessRut &&
            b.Name == businessName &&
            b.Owner.Email == ownerEmail)));
    }

    #endregion

    #endregion
}
