using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Helpers;
using ModeloValidador.Abstracciones;
using Moq;

namespace HomeConnect.BusinessLogic.Test.BusinessOwners;

[TestClass]
public class ValidatorServiceTests
{
    private Mock<IAssemblyInterfaceLoader<IModeloValidador>> _mockAssemblyInterfaceLoader = null!;
    private ValidatorService _validatorService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _mockAssemblyInterfaceLoader = new Mock<IAssemblyInterfaceLoader<IModeloValidador>>();
        _validatorService = new ValidatorService(_mockAssemblyInterfaceLoader.Object);
    }

    [TestMethod]
    public void GetValidators_WhenCalled_ShouldReturnListOfValidatorInfo()
    {
        // Arrange
        var validatorName = "ValidatorName";
        _mockAssemblyInterfaceLoader
            .Setup(x => x.GetImplementationsList(It.IsAny<string>()))
            .Returns([validatorName]);

        // Act
        List<ValidatorInfo> result = _validatorService.GetValidators();

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(validatorName, result[0].Name);
    }

    [TestMethod]
    public void GetValidatorByName_WhenCalled_ShouldReturnValidator()
    {
        // Arrange
        var validatorName = "ValidatorName";
        var validator = new Mock<IModeloValidador>();
        _mockAssemblyInterfaceLoader
            .Setup(x => x.GetImplementationByName(validatorName, It.IsAny<string>()))
            .Returns(validator.Object);

        // Act
        IModeloValidador result = _validatorService.GetValidatorByName(validatorName);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(validator.Object, result);
    }

    [TestMethod]
    public void Exists_WhenValidatorExists_ShouldReturnTrue()
    {
        // Arrange
        var validatorName = "ValidatorName";
        _mockAssemblyInterfaceLoader
            .Setup(x => x.GetImplementationsList(It.IsAny<string>()))
            .Returns([validatorName]);

        // Act
        var result = _validatorService.Exists(validatorName);

        // Assert
        Assert.IsTrue(result);
    }

    #region GetValidatorIdByName

    [TestMethod]
    public void GetValidatorIdByName_WhenCalled_ShouldReturnValidatorId()
    {
        // Arrange
        var validatorName = "ValidatorName";
        var validatorId = Guid.NewGuid();
        _mockAssemblyInterfaceLoader
            .Setup(x =>
                x.GetImplementationIdByName(validatorName, It.IsAny<string>())).Returns(validatorId);

        // Act
        Guid? result = _validatorService.GetValidatorIdByName(validatorName);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(validatorId, result);
    }

    #endregion

    #region GetValidator

    [TestMethod]
    public void GetValidator_WhenCalled_ShouldReturnValidator()
    {
        // Arrange
        var validatorId = Guid.NewGuid();
        var validator = new Mock<IModeloValidador>();
        _mockAssemblyInterfaceLoader
            .Setup(x => x.GetImplementationById(validatorId, It.IsAny<string>()))
            .Returns(validator.Object);

        // Act
        IModeloValidador result = _validatorService.GetValidator(validatorId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(validator.Object, result);
    }

    #endregion
}
