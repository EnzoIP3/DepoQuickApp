using BusinessLogic.BusinessOwners.Entities;

namespace BusinessLogic.Devices.Entities;

public class Device
{
    private string _name = string.Empty;
    private string _description = string.Empty;
    private string _mainPhoto = string.Empty;
    private List<string> _secondaryPhotos = [];
    private DeviceType _type;
    private Business _business = null!;

    public bool ConnectionState { get; set; } = false;
    public Guid Id { get; init; } = Guid.NewGuid();

    public Business Business
    {
        get => _business;
        set
        {
            _business = value;
        }
    }

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

    public DeviceType Type
    {
        get => _type;
        set
        {
            _type = value;
        }
    }

    public Device(string name, int? modelNumber, string description, string mainPhoto, List<string>? secondaryPhotos,
        string type, Business business)
    {
        Name = name;
        ModelNumber = modelNumber ?? 0;
        Description = description;
        MainPhoto = mainPhoto;
        SecondaryPhotos = secondaryPhotos ?? [];

        if (Enum.TryParse(type, true, out DeviceType parsedType))
        {
            Type = parsedType;
        }
        else
        {
            throw new ArgumentException("Invalid device type");
        }

        Business = business;
    }

    public Device()
    {
    }

    private static void EnsurePhotoUrlIsValid(string photoUrl)
    {
        if (!Uri.IsWellFormedUriString(photoUrl, UriKind.Absolute))
        {
            throw new ArgumentException($"{photoUrl} is not a valid image URL.");
        }
    }

    private static void EnsureFieldIsNotEmpty(string field, string value)
    {
        if (value == string.Empty)
        {
            throw new ArgumentException($"{field} is missing");
        }
    }
}
