using BusinessLogic.Roles.Entities;
using BusinessLogic.Roles.Repositories;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
using BusinessLogic.Users.Repositories;

namespace BusinessLogic.Users.Services;

public class UserService : IUserService
{
    private readonly IRoleRepository _roleRepository;

    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public User CreateUser(CreateUserArgs args)
    {
        EnsureRoleExists(args);
        EnsureUserDoesNotExist(args);
        Role role = _roleRepository.Get(args.Role);
        ValidateHomeOwner(args, role);
        var user = new User(args.Name, args.Surname, args.Email, args.Password, role, args.ProfilePicture);
        _userRepository.Add(user);
        return user;
    }

    public bool Exists(string requestUserId)
    {
        return UserIdIsValid(requestUserId) && _userRepository.Exists(Guid.Parse(requestUserId));
    }

    public User AddRoleToUser(AddRoleToUserArgs args)
    {
        User user = _userRepository.Get(Guid.Parse(args.UserId));
        Role role = _roleRepository.Get(args.Role);
        user.AddRole(role);
        _userRepository.Update(user);
        return user;
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
        if (_userRepository.ExistsByEmail(args.Email))
        {
            throw new InvalidOperationException("User already exists.");
        }
    }

    private void EnsureRoleExists(CreateUserArgs args)
    {
        if (!_roleRepository.Exists(args.Role))
        {
            throw new ArgumentException("Invalid role.");
        }
    }
}
