using BusinessLogic.Roles.Entities;
using BusinessLogic.Roles.Repositories;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
using BusinessLogic.Users.Repositories;

namespace BusinessLogic.Users.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public User CreateUser(CreateUserArgs args)
    {
        EnsureRoleExists(args);
        EnsureUserDoesNotExist(args);
        var role = _roleRepository.Get(args.Role);
        if (role.Name == Role.HomeOwner.Name && args.ProfilePicture == null)
        {
            throw new ArgumentException("Home owners must have a profile picture");
        }

        var user = new User(args.Name, args.Surname, args.Email, args.Password, role, args.ProfilePicture);
        _userRepository.Add(user);
        return user;
    }

    private void EnsureUserDoesNotExist(CreateUserArgs args)
    {
        if (_userRepository.Exists(args.Email))
        {
            throw new ArgumentException("User already exists");
        }
    }

    private void EnsureRoleExists(CreateUserArgs args)
    {
        if (!_roleRepository.Exists(args.Role))
        {
            throw new ArgumentException("Invalid role");
        }
    }
}
