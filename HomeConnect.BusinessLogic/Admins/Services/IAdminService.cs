using BusinessLogic.Admins.Models;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Users.Entities;

namespace BusinessLogic.Admins.Services;

public interface IAdminService
{
    void DeleteAdmin(string id);
    PagedData<User> GetUsers(GetUsersArgs args);
    PagedData<Business> GetBusinesses(GetBusinessesArgs args);
}
