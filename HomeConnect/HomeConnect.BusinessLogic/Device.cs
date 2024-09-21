namespace BusinessLogic;

public class Device
{
    private string _name = string.Empty;
    private string _description = string.Empty;
    private string _mainPhoto = string.Empty;
    private List<string> _secondaryPhotos = [];
    private string _type = string.Empty;

    public Guid Id { get; init; } = Guid.NewGuid();

    public string Name
    {
        get => _name;
        set
        {
            if (value == string.Empty)
            {
                throw new ArgumentException("Name is missing.");
            }

            _name = value;
        }
    }

    public int ModelNumber { get; init; }

    public string Description
    {
        get => _description;
        set
        {
            if (value == string.Empty)
            {
                throw new ArgumentException("Description is missing.");
            }

            _description = value;
        }
    }

    public string MainPhoto
    {
        get => _mainPhoto;
        set
        {
            if (value == string.Empty)
            {
                throw new ArgumentException("Main photo is missing.");
            }

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

    public string Type
    {
        get => _type;
        set
        {
            if (value == string.Empty)
            {
                throw new ArgumentException("Type is missing.");
            }

            _type = value;
        }
    }

    public Device(string name, int modelNumber, string description, string mainPhoto, string[] secondaryPhotos,
        string type)
    {
        Name = name;
        ModelNumber = modelNumber;
        Description = description;
        MainPhoto = mainPhoto;
        SecondaryPhotos = secondaryPhotos.ToList();
        Type = type;
    }

    private static void EnsurePhotoUrlIsValid(string photoUrl)
    {
        if (!Uri.IsWellFormedUriString(photoUrl, UriKind.Absolute))
        {
            throw new ArgumentException($"{photoUrl} is not a valid image URL");
        }
    }
}
