using BusinessLogic.Auth.Entities;
using BusinessLogic.Auth.Exceptions;
using BusinessLogic.Auth.Models;
using BusinessLogic.Auth.Repositories;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;

namespace BusinessLogic.Auth.Services;

public class AuthService : IAuthService
{
    private readonly ITokenRepository _tokenRepository;
    private readonly IUserRepository _userRepository;

    public AuthService(ITokenRepository tokenRepository, IUserRepository userRepository)
    {
        _tokenRepository = tokenRepository;
        _userRepository = userRepository;
    }

    public string CreateToken(CreateTokenArgs args)
    {
        EnsureEmailIsNotEmpty(args.Email);
        EnsurePasswordIsNotEmpty(args.Password);
        EnsureUserExists(args.Email);
        var user = _userRepository.GetByEmail(args.Email);
        ValidatePassword(args, user);
        var session = new Token(user);
        _tokenRepository.Add(session);
        return session.Id.ToString();
    }

    private void EnsurePasswordIsNotEmpty(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password is required.");
        }
    }

    private void EnsureEmailIsNotEmpty(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required.");
        }
    }

    private void EnsureUserExists(string email)
    {
        if (!_userRepository.ExistsByEmail(email))
        {
            throw new AuthException("Invalid email or password.");
        }
    }

    private static void ValidatePassword(CreateTokenArgs args, User user)
    {
        if (!user.Password.Equals(args.Password))
        {
            throw new AuthException("Invalid email or password.");
        }
    }

    public User GetUserFromToken(string token)
    {
        EnsureIsValidGuid(token);
        var session = _tokenRepository.Get(Guid.Parse(token));
        return session.User;
    }

    private static void EnsureIsValidGuid(string id)
    {
        if (!Guid.TryParse(id, out _))
        {
            throw new ArgumentException("Token is not a valid GUID.");
        }
    }

    public bool IsTokenExpired(string token)
    {
        EnsureIsValidGuid(token);
        var session = _tokenRepository.Get(Guid.Parse(token));
        return session.IsExpired();
    }
}
