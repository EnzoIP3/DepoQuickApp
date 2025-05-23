using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Roles.Entities;

public record SystemPermission
{
    public const string CreateAdministrator = "create-administrator";
    public const string DeleteAdministrator = "delete-administrator";
    public const string CreateBusinessOwner = "create-business-owner";
    public const string GetAllUsers = "get-all-users";
    public const string GetAllBusinesses = "get-all-businesses";
    public const string CreateHome = "create-home";
    public const string AddMember = "add-member";
    public const string AddDevice = "add-device";
    public const string GetDevices = "get-devices";
    public const string UpdateMember = "update-member";
    public const string GetMembers = "get-members";
    public const string CreateBusiness = "create-business";
    public const string CreateCamera = "create-camera";
    public const string CreateSensor = "create-sensor";
    public const string CreateMotionSensor = "create-motion-sensor";
    public const string CreateLamp = "create-lamp";
    public const string GetNotifications = "get-notifications";
    public const string GetHomes = "get-homes";
    public const string NameHome = "name-home";
    public const string UpdateBusinessValidator = "update-business-validator";
    public const string GetDeviceValidators = "get-device-validators";
    public const string ImportDevices = "import-devices";
    public const string GetDeviceImportFiles = "get-device-import-files";
    public const string GetDeviceImporters = "get-device-importers";
    public const string NameDevice = "name-device";
    public const string GetBusinesses = "get-businesses";
    public const string GetBusinessDevices = "get-business-devices";
    public const string GetCamera = "get-camera";
    public const string AddDeviceToRoom = "add-device-to-room";
    public const string MoveDevice = "move-device";
    public const string CreateRoom = "create-room";

    public SystemPermission()
    {
    }

    public SystemPermission(string value, List<Role>? roles = null)
    {
        Value = value;
        Roles = roles;
    }

    [Key]
    public string Value { get; init; } = null!;

    public List<Role>? Roles { get; set; }

    public override string ToString()
    {
        return Value;
    }
}
