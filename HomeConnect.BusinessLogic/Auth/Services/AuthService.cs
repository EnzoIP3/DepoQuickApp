using BusinessLogic.Auth.Entities;
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
        var user = _userRepository.Get(args.Email);
        if (user == null || !user.Password.Equals(args.Password))
        {
            throw new ArgumentException("Invalid email or password");
        }

        var session = new Token(user);
        _tokenRepository.Add(session);
        return session.Id.ToString();
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
            throw new ArgumentException("Invalid session id");
        }
    }

    public bool IsTokenExpired(string token)
    {
        EnsureIsValidGuid(token);
        var session = _tokenRepository.Get(Guid.Parse(token));
        return session.IsExpired();
    }
}
