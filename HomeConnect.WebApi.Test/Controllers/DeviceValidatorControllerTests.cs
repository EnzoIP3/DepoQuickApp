using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.DeviceValidators;
using HomeConnect.WebApi.Controllers.DeviceValidators.Models;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class DeviceValidatorControllerTests
{
    private DeviceValidatorController _controller = null!;
    private Mock<IValidatorService> _validatorService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _validatorService = new Mock<IValidatorService>();
        _controller = new DeviceValidatorController(_validatorService.Object);
    }

    #region GetValidators

    [TestMethod]
    public void GetValidators_WhenCalled_ReturnsGetValidatorsResponse()
    {
        // Arrange
        var validators = new List<ValidatorInfo> { new() { Name = "Validator1" }, new() { Name = "Validator2" } };
        var expectedResponse = new GetValidatorsResponse { Validators = validators.Select(v => v.Name).ToList() };
        _validatorService.Setup(x => x.GetValidators()).Returns(validators);

        // Act
        GetValidatorsResponse response = _controller.GetValidators();

        // Assert
        _validatorService.Verify(x => x.GetValidators(), Times.Once);
        response.Should().BeEquivalentTo(expectedResponse, options => options
            .ComparingByMembers<GetValidatorsResponse>());
    }

    #endregion
}
