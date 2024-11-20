using BusinessLogic.Roles.Entities;
using BusinessLogic.Roles.Repositories;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
using BusinessLogic.Users.Repositories;

namespace BusinessLogic.Users.Services;

public class UserService : IUserService
{
    public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        UserRepository = userRepository;
        RoleRepository = roleRepository;
    }

    private IUserRepository UserRepository { get; }
    private IRoleRepository RoleRepository { get; }

    public User CreateUser(CreateUserArgs args)
    {
        EnsureRoleExists(args);
        EnsureUserDoesNotExist(args);
        Role role = RoleRepository.Get(args.Role);
        ValidateHomeOwner(args, role);
        var user = new User(args.Name, args.Surname, args.Email, args.Password, role, args.ProfilePicture);
        UserRepository.Add(user);
        return user;
    }

    public bool Exists(string requestUserId)
    {
        return UserIdIsValid(requestUserId) && UserRepository.Exists(Guid.Parse(requestUserId));
    }

    private bool UserIdIsValid(string requestUserId)
    {
        return Guid.TryParse(requestUserId, out _);
    }

    private static void ValidateHomeOwner(CreateUserArgs args, Role role)
    {
        if (role.Name == Role.HomeOwner && args.ProfilePicture == null)
        {
            throw new ArgumentException("Home owners must have a profile picture.");
        }
    }

    private void EnsureUserDoesNotExist(CreateUserArgs args)
    {
        if (UserRepository.ExistsByEmail(args.Email))
        {
            throw new InvalidOperationException("User already exists.");
        }
    }

    private void EnsureRoleExists(CreateUserArgs args)
    {
        if (!RoleRepository.Exists(args.Role))
        {
            throw new ArgumentException("Invalid role.");
        }
    }

    public User AddRoleToUser(AddRoleToUserArgs args)
    {
        var user = UserRepository.Get(Guid.Parse(args.UserId));
        var role = RoleRepository.Get(args.Role);
        user.AddRole(role);
        UserRepository.Update(user);
        return user;
    }
}
