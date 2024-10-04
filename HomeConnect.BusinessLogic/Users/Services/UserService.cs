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
        if (!_roleRepository.Exists(args.Role))
        {
            throw new ArgumentException("Invalid role");
        }

        if (_userRepository.Exists(args.Email))
        {
            throw new ArgumentException("User already exists");
        }

        var role = _roleRepository.Get(args.Role);
        var user = new User(args.Name, args.Surname, args.Email, args.Password, role);
        _userRepository.Add(user);
        return user;
    }
}
