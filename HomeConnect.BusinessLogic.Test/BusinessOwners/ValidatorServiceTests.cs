using BusinessLogic.BusinessOwners.Helpers;
using BusinessLogic.BusinessOwners.Services;
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
            .Returns(new List<string> { validatorName });

        // Act
        var result = _validatorService.GetValidators();

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
            .Setup(x => x.GetImplementation(validatorName, It.IsAny<string>()))
            .Returns(validator.Object);

        // Act
        var result = _validatorService.GetValidatorByName(validatorName);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(validator.Object, result);
    }
}
