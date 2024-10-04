using BusinessLogic.Tokens.Entities;

namespace BusinessLogic.Tokens.Repositories;

public interface ITokenRepository
{
    Token Get(Guid token);
    void Add(Token token);
}
