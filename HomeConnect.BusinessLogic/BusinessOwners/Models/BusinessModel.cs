using BusinessLogic.Users.Entities;

namespace BusinessLogic.BusinessOwners.Models;

public struct BusinessModel
{
    public string Rut { get; set; }
    public string Name { get; set; }
    public User Owner { get; set; }
}
