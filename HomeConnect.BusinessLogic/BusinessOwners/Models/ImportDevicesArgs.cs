using BusinessLogic.Users.Entities;

namespace BusinessLogic.BusinessOwners.Models;

public struct ImportDevicesArgs
{
    public string ImporterName { get; set; }
    public string Route { get; set; }
    public User User { get; set; }
}
