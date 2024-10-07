using BusinessLogic.BusinessOwners.Entities;

namespace BusinessLogic.Devices.Entities;

public class Device
{
    private string _description = string.Empty;
    private string _mainPhoto = string.Empty;
    private string _name = string.Empty;
    private List<string> _secondaryPhotos = [];

    public Device(string name, int? modelNumber, string description, string mainPhoto, List<string>? secondaryPhotos,
        string type, Business business)
    {
        Name = name;
        EnsureModelNumberIsNotNull(modelNumber);
        ModelNumber = modelNumber!.Value;
        Description = description;
        MainPhoto = mainPhoto;
        SecondaryPhotos = secondaryPhotos ?? [];
        Type = ParseDeviceType(type);
        Business = business;
    }

    public Device()
    {
    }

    public bool ConnectionState { get; set; }
    public Guid Id { get; init; } = Guid.NewGuid();

    public Business Business { get; set; } = null!;

    public string Name
    {
        get => _name;
        set
        {
            EnsureFieldIsNotEmpty("Name", value);
            _name = value;
        }
    }

    public int ModelNumber { get; init; }

    public string Description
    {
        get => _description;
        set
        {
            EnsureFieldIsNotEmpty("Description", value);
            _description = value;
        }
    }

    public string MainPhoto
    {
        get => _mainPhoto;
        set
        {
            EnsureFieldIsNotEmpty("MainPhoto", value);
            EnsurePhotoUrlIsValid(value);
            _mainPhoto = value;
        }
    }

    public List<string> SecondaryPhotos
    {
        get => _secondaryPhotos;
        set
        {
            value.ForEach(EnsurePhotoUrlIsValid);
            _secondaryPhotos = value;
        }
    }

    public DeviceType Type { get; set; }

    private static void EnsurePhotoUrlIsValid(string photoUrl)
    {
        if (!Uri.IsWellFormedUriString(photoUrl, UriKind.Absolute))
        {
            throw new ArgumentException($"{photoUrl} is not a valid image URL.");
        }
    }

    private static void EnsureFieldIsNotEmpty(string field, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException($"{field} is missing");
        }
    }

    private static void EnsureModelNumberIsNotNull(int? modelNumber)
    {
        if (modelNumber == null)
        {
            throw new ArgumentException("Model number is missing");
        }
    }

    private static DeviceType ParseDeviceType(string type)
    {
        if (Enum.TryParse(type, true, out DeviceType parsedType))
        {
            return parsedType;
        }

        throw new ArgumentException("Invalid device type");
    }
}
