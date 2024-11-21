using BusinessLogic.Users.Entities;

namespace BusinessLogic.Users.Repositories;

public interface IUserRepository
{
    void Add(User user);
    User Get(Guid id);
    User GetByEmail(string email);
    bool Exists(Guid id);
    bool ExistsByEmail(string email);
    void Delete(Guid id);
    void Update(User user);

    PagedData<User> GetPaged(int currentPage, int pageSize, string? fullNameFilter = null,
        string? roleFilter = null);
}
