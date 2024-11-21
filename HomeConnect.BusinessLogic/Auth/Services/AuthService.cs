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
        ValidateCreateTokenArgs(args);
        User user = GetUserByEmail(args.Email);
        EnsurePasswordIsValid(args.Password, user);
        Token session = CreateSessionToken(user);
        _tokenRepository.Add(session);
        return session.Id.ToString();
    }

    public User GetUserFromToken(string token)
    {
        Token session = GetValidatedSession(token);
        return session.User;
    }

    public bool IsTokenExpired(string token)
    {
        Token session = GetValidatedSession(token);
        return session.IsExpired();
    }

    public bool Exists(string token)
    {
        EnsureIsValidGuid(token);
        return _tokenRepository.Exists(Guid.Parse(token));
    }

    private void ValidateCreateTokenArgs(CreateTokenArgs args)
    {
        EnsureEmailIsNotEmpty(args);
        EnsurePasswordIsNotEmpty(args);
        EnsureUserExists(args.Email);
    }

    private static void EnsurePasswordIsNotEmpty(CreateTokenArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.Password))
        {
            throw new ArgumentException("Password is required.");
        }
    }

    private static void EnsureEmailIsNotEmpty(CreateTokenArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.Email))
        {
            throw new ArgumentException("Email is required.");
        }
    }

    private User GetUserByEmail(string email)
    {
        return _userRepository.GetByEmail(email);
    }

    private static void EnsurePasswordIsValid(string password, User user)
    {
        if (!user.Password.Equals(password))
        {
            throw new AuthException("Invalid email or password.");
        }
    }

    private static Token CreateSessionToken(User user)
    {
        return new Token(user);
    }

    private Token GetValidatedSession(string token)
    {
        EnsureIsValidGuid(token);
        return _tokenRepository.Get(Guid.Parse(token));
    }

    private void EnsureUserExists(string email)
    {
        if (!_userRepository.ExistsByEmail(email))
        {
            throw new AuthException("Invalid email or password.");
        }
    }

    private static void EnsureIsValidGuid(string id)
    {
        if (!Guid.TryParse(id, out _))
        {
            throw new ArgumentException("Token is not a valid GUID.");
        }
    }
}
