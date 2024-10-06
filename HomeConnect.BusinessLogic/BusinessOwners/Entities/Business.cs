using System.ComponentModel.DataAnnotations;
using BusinessLogic.Users.Entities;

namespace BusinessLogic.BusinessOwners.Entities;

public class Business
{
    [Key]
    public string Rut { get; set; } = string.Empty;

    public string Logo { get; set; } = string.Empty;
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
