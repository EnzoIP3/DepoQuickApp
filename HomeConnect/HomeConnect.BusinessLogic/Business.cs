using System.ComponentModel.DataAnnotations;

namespace BusinessLogic;

public class Business
{
    [Key]
    public string Rut { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public User Owner { get; set; } = null!;

    public Business()
    {
    }

    public Business(string rut, string name, User owner)
    {
        Rut = rut;
        Name = name;
        Owner = owner;
    }
}
