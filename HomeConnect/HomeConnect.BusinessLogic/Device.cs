namespace BusinessLogic;

public class Device
{
    public string Name { get; init; }

    public int ModelNumber { get; init; }

    public string Description { get; init; }

    public string MainPhoto { get; init; }

    public string[] SecondaryPhotos { get; init; }

    public string Type { get; init; }

    public Device(string name, int modelNumber, string description, string mainPhoto, string[] secondaryPhotos,
        string type)
    {
        Name = name;
        ModelNumber = modelNumber;
        Description = description;
        MainPhoto = mainPhoto;
        SecondaryPhotos = secondaryPhotos;
        Type = type;
    }
}
