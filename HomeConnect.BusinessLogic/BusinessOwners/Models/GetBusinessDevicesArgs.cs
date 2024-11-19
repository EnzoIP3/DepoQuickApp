using BusinessLogic.Users.Entities;

namespace BusinessLogic.BusinessOwners.Models;

public struct GetBusinessDevicesArgs
{
    public string Rut { get; set; }
    public User User { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
}
