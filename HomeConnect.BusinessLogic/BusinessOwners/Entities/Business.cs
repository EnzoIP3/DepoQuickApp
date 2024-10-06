using System.ComponentModel.DataAnnotations;
using BusinessLogic.Users.Entities;

namespace BusinessLogic.BusinessOwners.Entities;

public class Business
{
    private string _rut = string.Empty;

    [Key]
    public string Rut
    {
        get => _rut;
        set
        {
            EnsureIsNotEmpty(value, nameof(Rut));
            _rut = value;
        }
    }

    private string _logo = string.Empty;

    public string Logo
    {
        get => _logo;
        set
        {
            EnsureIsNotEmpty(value, nameof(Logo));
            EnsureLogoIsValidUrl(value);

            _logo = value;
        }
    }

    private static void EnsureIsNotEmpty(string value, string name)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{name} must not be blank");
        }
    }

    private static void EnsureLogoIsValidUrl(string value)
    {
        if (!Uri.TryCreate(value, UriKind.Absolute, out _))
        {
            throw new ArgumentException("Logo must be a valid URI");
        }
    }

    private string _name = string.Empty;

    public string Name
    {
        get => _name;
        set
        {
            EnsureIsNotEmpty(value, nameof(Name));
            _name = value;
        }
    }

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
