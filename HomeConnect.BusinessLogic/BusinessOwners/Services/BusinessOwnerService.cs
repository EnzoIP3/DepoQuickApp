using BusinessLogic.Admins.Services;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Repositories;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
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
        Guid? validatorId = GetValidatorId(args.Validator);
        User owner = GetUserById(ownerId);
        var business = new Business(args.Rut, args.Name, args.Logo, owner, validatorId);
        BusinessRepository.Add(business);
        return business;
    }

    private Guid? GetValidatorId(string? argsValidator)
    {
        if (!string.IsNullOrWhiteSpace(argsValidator))
        {
            return ValidatorService.GetValidatorIdByName(argsValidator);
        }

        return null;
    }

    private void EnsureValidatorExists(string? argsValidator)
    {
        if (!string.IsNullOrWhiteSpace(argsValidator) && !ValidatorService.Exists(argsValidator))
        {
            throw new ArgumentException("The specified validator does not exist.");
        }
    }

    public Device CreateDevice(CreateDeviceArgs args)
    {
        Business business = GetValidatedBusiness(args.Owner.Id);
        Device device = CreateDevice(args, business);
        DeviceRepository.Add(device);
        return device;
    }

    public Camera CreateCamera(CreateCameraArgs args)
    {
        Business business = GetValidatedBusiness(args.Owner.Id);
        Camera camera = CreateCamera(args, business);
        DeviceRepository.Add(camera);
        return camera;
    }

    public void UpdateValidator(UpdateValidatorArgs args)
    {
        EnsureBusinessExistsFromRut(args.BusinessRut);
        EnsureBusinessIsFromOwner(args.BusinessRut, args.OwnerId);
        EnsureValidatorExists(args.Validator);
        Guid? validatorId = GetValidatorId(args.Validator);
        BusinessRepository.UpdateValidator(args.BusinessRut, validatorId);
    }

    public PagedData<Business> GetBusinesses(string ownerIdFilter, int currentPage, int pageSize)
    {
        Guid ownerId = ParseAndValidateOwnerId(ownerIdFilter);
        var filterArgs = new FilterArgs { OwnerIdFilter = ownerId };
        PagedData<Business> businesses =
            BusinessRepository.GetPaged(filterArgs);
        return businesses;
    }

    public PagedData<Device> GetDevices(string businessId, User user)
    {
        EnsureBusinessIsFromOwner(businessId, user.Id.ToString());
        return DeviceRepository.GetPaged(new GetDevicesArgs { RutFilter = businessId });
    }

    private void EnsureBusinessExistsFromRut(string argsBusinessRut)
    {
        if (!BusinessRepository.Exists(argsBusinessRut))
        {
            throw new ArgumentException("The business does not exist.");
        }
    }

    private void EnsureBusinessIsFromOwner(string argsBusinessRut, string argsOwnerId)
    {
        Guid ownerId = ParseAndValidateOwnerId(argsOwnerId);
        Business business = BusinessRepository.Get(argsBusinessRut);
        if (business.Owner.Id != ownerId)
        {
            throw new InvalidOperationException("The business does not belong to the specified owner.");
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

    private void EnsureModelNumberIsValid(string? modelNumber, Guid? validatorName)
    {
        if (validatorName != null && modelNumber != null)
        {
            IModeloValidador validator = ValidatorService.GetValidator(validatorName);
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
