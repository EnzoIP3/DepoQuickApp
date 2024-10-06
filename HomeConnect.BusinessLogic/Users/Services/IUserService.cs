using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;

namespace BusinessLogic.Users.Services;

public interface IUserService
{
    User CreateUser(CreateUserArgs args);
    bool Exists(string requestUserId);
}
