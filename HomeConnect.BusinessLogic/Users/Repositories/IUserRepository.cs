using BusinessLogic.Users.Entities;

namespace BusinessLogic.Users.Repositories;

public interface IUserRepository
{
    void Add(User user);
    User Get(Guid id);
    User Get(string email);
    bool Exists(Guid id);
    bool Exists(string email);
    void Delete(Guid id);

    PagedData<User> GetAllPaged(int currentPage, int pageSize, string? fullNameFilter = null,
        string? roleFilter = null);
}
