using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Repositories;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;
using ModeloValidador.Abstracciones;

namespace BusinessLogic.BusinessOwners.Services;

public class BusinessOwnerService : IBusinessOwnerService
{
    public BusinessOwnerService(
        IUserRepository userRepository,
        IBusinessRepository businessRepository,
        IDeviceRepository deviceRepository,
        IValidatorService validatorService)
    {
        UserRepository = userRepository;
        BusinessRepository = businessRepository;
        DeviceRepository = deviceRepository;
        ValidatorService = validatorService;
    }

    private IUserRepository UserRepository { get; }
    private IBusinessRepository BusinessRepository { get; }
    private IDeviceRepository DeviceRepository { get; }
    private IValidatorService ValidatorService { get; }

    public Business CreateBusiness(CreateBusinessArgs args)
    {
        Guid ownerId = ParseAndValidateOwnerId(args.OwnerId);
        EnsureBusinessPrerequisites(ownerId, args.Rut);
        EnsureValidatorExists(args.Validator);
        User owner = GetUserById(ownerId);
        var business = new Business(args.Rut, args.Name, args.Logo, owner);
        BusinessRepository.Add(business);
        return business;
    }

    private void EnsureValidatorExists(string? argsValidator)
    {
        if (argsValidator != null && !ValidatorService.Exists(argsValidator))
        {
            throw new ArgumentException("The specified validator does not exist.");
        }
    }

    public Device CreateDevice(CreateDeviceArgs args)
    {
        Business business = GetValidatedBusiness(args.Owner.Id);
        Device device = CreateDevice(args, business);
        EnsureDeviceDoesNotExist(args.ModelNumber);
        DeviceRepository.Add(device);
        return device;
    }

    public Camera CreateCamera(CreateCameraArgs args)
    {
        Business business = GetValidatedBusiness(args.Owner.Id);
        Camera camera = CreateCamera(args, business);
        EnsureDeviceDoesNotExist(args.ModelNumber);
        DeviceRepository.Add(camera);
        return camera;
    }

    private void EnsureDeviceDoesNotExist(string? modelNumber)
    {
        if (DeviceRepository.ExistsByModelNumber(modelNumber!))
        {
            throw new InvalidOperationException("Device already exists");
        }
    }

    private static Guid ParseAndValidateOwnerId(string ownerId)
    {
        if (!Guid.TryParse(ownerId, out Guid parsedOwnerId))
        {
            throw new ArgumentException("The business owner ID is not a valid GUID.");
        }

        return parsedOwnerId;
    }

    private void EnsureBusinessPrerequisites(Guid ownerId, string businessRut)
    {
        EnsureOwnerExists(ownerId);
        EnsureOwnerDoesNotHaveBusiness(ownerId);
        EnsureBusinessRutDoesNotExist(businessRut);
    }

    private User GetUserById(Guid ownerId)
    {
        return UserRepository.Get(ownerId);
    }

    private Business GetValidatedBusiness(Guid ownerId)
    {
        EnsureBusinessExists(ownerId);
        return BusinessRepository.GetByOwnerId(ownerId);
    }

    private Device CreateDevice(CreateDeviceArgs args, Business business)
    {
        EnsureModelNumberIsValid(args.ModelNumber, business.Validator);
        return new Device(
            args.Name,
            args.ModelNumber,
            args.Description,
            args.MainPhoto,
            args.SecondaryPhotos,
            args.Type,
            business);
    }

    private void EnsureModelNumberIsValid(string? modelNumber, string? validatorName)
    {
        if (validatorName != null && modelNumber != null)
        {
            IModeloValidador validator = ValidatorService.GetValidatorByName(validatorName);
            if (!validator.EsValido(new Modelo(modelNumber)))
            {
                throw new ArgumentException("The model number is not valid according to the specified validator.");
            }
        }
    }

    private Camera CreateCamera(CreateCameraArgs args, Business business)
    {
        EnsureModelNumberIsValid(args.ModelNumber, business.Validator);
        return new Camera(
            args.Name,
            args.ModelNumber,
            args.Description,
            args.MainPhoto,
            args.SecondaryPhotos,
            business,
            args.MotionDetection,
            args.PersonDetection,
            args.Exterior,
            args.Interior);
    }

    private void EnsureBusinessExists(Guid ownerId)
    {
        if (!BusinessRepository.ExistsByOwnerId(ownerId))
        {
            throw new ArgumentException("That business does not exist.");
        }
    }

    private void EnsureOwnerExists(Guid ownerId)
    {
        if (!UserRepository.Exists(ownerId))
        {
            throw new ArgumentException("That business owner does not exist.");
        }
    }

    private void EnsureOwnerDoesNotHaveBusiness(Guid ownerId)
    {
        if (BusinessRepository.ExistsByOwnerId(ownerId))
        {
            throw new InvalidOperationException("Owner already has a business.");
        }
    }

    private void EnsureBusinessRutDoesNotExist(string businessRut)
    {
        if (BusinessRepository.Exists(businessRut))
        {
            throw new InvalidOperationException("There is already a business with this RUT.");
        }
    }
}
