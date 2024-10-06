using System.ComponentModel.DataAnnotations;
using BusinessLogic.Users.Entities;

namespace BusinessLogic.BusinessOwners.Entities;

public class Business
{
    [Key]
    public string Rut { get; set; } = string.Empty;

    private string _logo = string.Empty;

    public string Logo
    {
        get => _logo;
        set
        {
            EnsureLogoIsValidUrl(value);

            _logo = value;
        }
    }

    private static void EnsureLogoIsValidUrl(string value)
    {
        if (!Uri.TryCreate(value, UriKind.Absolute, out _))
        {
            throw new ArgumentException("Logo must be a valid URI");
        }
    }

    public string Name { get; set; } = string.Empty;
    public User Owner { get; set; } = null!;

    public Business()
    {
    }

    public Business(string rut, string name, string logo, User owner)
    {
        Rut = rut;
        Name = name;
        Logo = logo;
        Owner = owner;
    }
}
