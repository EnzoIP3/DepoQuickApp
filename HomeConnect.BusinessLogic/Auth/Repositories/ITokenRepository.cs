using BusinessLogic.Auth.Entities;

namespace BusinessLogic.Auth.Repositories;

public interface ITokenRepository
{
    Token Get(Guid token);
    void Add(Token token);
    bool Exists(Guid token);
}
