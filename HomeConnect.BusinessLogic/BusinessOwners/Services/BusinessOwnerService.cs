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
        _userRepository = userRepository;
        _businessRepository = businessRepository;
        _deviceRepository = deviceRepository;
        _validatorService = validatorService;
    }

    private readonly IUserRepository _userRepository;
    private readonly IBusinessRepository _businessRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IValidatorService _validatorService;

    public Business CreateBusiness(CreateBusinessArgs args)
    {
        Guid ownerId = ParseAndValidateOwnerId(args.OwnerId);
        EnsureBusinessPrerequisites(ownerId, args.Rut);
        EnsureValidatorExists(args.Validator);
        Guid? validatorId = GetValidatorId(args.Validator);
        User owner = GetUserById(ownerId);
        var business = new Business(args.Rut, args.Name, args.Logo, owner, validatorId);
        _businessRepository.Add(business);
        return business;
    }

    private Guid? GetValidatorId(string? argsValidator)
    {
        if (!string.IsNullOrWhiteSpace(argsValidator))
        {
            return _validatorService.GetValidatorIdByName(argsValidator);
        }

        return null;
    }

    private void EnsureValidatorExists(string? argsValidator)
    {
        if (!string.IsNullOrWhiteSpace(argsValidator) && !_validatorService.Exists(argsValidator))
        {
            throw new ArgumentException("The specified validator does not exist.");
        }
    }

    public Device CreateDevice(CreateDeviceArgs args)
    {
        Business business = GetValidatedBusiness(args.Owner.Id);
        Device device = CreateDevice(args, business);
        _deviceRepository.Add(device);
        return device;
    }

    public Camera CreateCamera(CreateCameraArgs args)
    {
        Business business = GetValidatedBusiness(args.Owner.Id);
        Camera camera = CreateCamera(args, business);
        _deviceRepository.Add(camera);
        return camera;
    }

    public void UpdateValidator(UpdateValidatorArgs args)
    {
        EnsureBusinessExistsFromRut(args.BusinessRut);
        EnsureBusinessIsFromOwner(args.BusinessRut, args.OwnerId);
        EnsureValidatorExists(args.Validator);
        Guid? validatorId = GetValidatorId(args.Validator);
        _businessRepository.UpdateValidator(args.BusinessRut, validatorId);
    }

    public PagedData<Business> GetBusinesses(string ownerIdFilter, int currentPage, int pageSize)
    {
        Guid ownerId = ParseAndValidateOwnerId(ownerIdFilter);
        var filterArgs = new FilterArgs { OwnerIdFilter = ownerId, CurrentPage = currentPage, PageSize = pageSize };
        PagedData<Business> businesses =
            _businessRepository.GetPaged(filterArgs);
        return businesses;
    }

    public PagedData<Device> GetDevices(GetBusinessDevicesArgs args)
    {
        EnsureBusinessIsFromOwner(args.Rut, args.User.Id.ToString());
        return _deviceRepository.GetPaged(new GetDevicesArgs
        {
            RutFilter = args.Rut, PageSize = args.PageSize, Page = args.CurrentPage
        });
    }

    private void EnsureBusinessExistsFromRut(string argsBusinessRut)
    {
        if (!_businessRepository.Exists(argsBusinessRut))
        {
            throw new ArgumentException("The business does not exist.");
        }
    }

    private void EnsureBusinessIsFromOwner(string argsBusinessRut, string argsOwnerId)
    {
        Guid ownerId = ParseAndValidateOwnerId(argsOwnerId);
        Business business = _businessRepository.Get(argsBusinessRut);
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
        return _userRepository.Get(ownerId);
    }

    private Business GetValidatedBusiness(Guid ownerId)
    {
        EnsureBusinessExists(ownerId);
        return _businessRepository.GetByOwnerId(ownerId);
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
            IModeloValidador validator = _validatorService.GetValidator(validatorName);
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
        if (!_businessRepository.ExistsByOwnerId(ownerId))
        {
            throw new ArgumentException("That business does not exist.");
        }
    }

    private void EnsureOwnerExists(Guid ownerId)
    {
        if (!_userRepository.Exists(ownerId))
        {
            throw new ArgumentException("That business owner does not exist.");
        }
    }

    private void EnsureOwnerDoesNotHaveBusiness(Guid ownerId)
    {
        if (_businessRepository.ExistsByOwnerId(ownerId))
        {
            throw new InvalidOperationException("Owner already has a business.");
        }
    }

    private void EnsureBusinessRutDoesNotExist(string businessRut)
    {
        if (_businessRepository.Exists(businessRut))
        {
            throw new InvalidOperationException("There is already a business with this RUT.");
        }
    }
}
