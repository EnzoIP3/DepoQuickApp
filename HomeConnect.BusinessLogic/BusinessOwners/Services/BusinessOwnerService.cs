using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Repositories;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;

namespace BusinessLogic.BusinessOwners.Services;

public class BusinessOwnerService : IBusinessOwnerService
{
    public BusinessOwnerService(
        IUserRepository userRepository,
        IBusinessRepository businessRepository,
        IDeviceRepository deviceRepository)
    {
        UserRepository = userRepository;
        BusinessRepository = businessRepository;
        DeviceRepository = deviceRepository;
    }

    private IUserRepository UserRepository { get; }
    private IBusinessRepository BusinessRepository { get; }
    private IDeviceRepository DeviceRepository { get; }

    public Business CreateBusiness(CreateBusinessArgs args)
    {
        var ownerId = ParseAndValidateOwnerId(args.OwnerId);
        EnsureBusinessPrerequisites(ownerId, args.Rut);
        User owner = GetUserById(ownerId);
        var business = new Business(args.Rut, args.Name, args.Logo, owner);
        BusinessRepository.Add(business);
        return business;
    }

    public Device CreateDevice(CreateDeviceArgs args)
    {
        var business = GetValidatedBusiness(args.Owner.Id);
        var device = CreateDevice(args, business);
        DeviceRepository.EnsureDeviceDoesNotExist(device);
        DeviceRepository.Add(device);
        return device;
    }

    public Camera CreateCamera(CreateCameraArgs args)
    {
        var business = GetValidatedBusiness(args.Owner.Id);
        var camera = CreateCamera(args, business);
        DeviceRepository.EnsureDeviceDoesNotExist(camera);
        DeviceRepository.Add(camera);
        return camera;
    }

    private static Guid ParseAndValidateOwnerId(string ownerId)
    {
        if (!Guid.TryParse(ownerId, out var parsedOwnerId))
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

    private static Device CreateDevice(CreateDeviceArgs args, Business business)
    {
        return new Device(
            args.Name,
            args.ModelNumber,
            args.Description,
            args.MainPhoto,
            args.SecondaryPhotos,
            args.Type,
            business);
    }

    private static Camera CreateCamera(CreateCameraArgs args, Business business)
    {
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
