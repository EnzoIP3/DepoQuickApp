namespace BusinessLogic.Users.Repositories;

public interface IUserRepository
{
    Entities.User? GetUser(string email);
    void Add(Entities.User user);
    Entities.User Get(Guid id);
    bool Exists(Guid id);
    void Delete(Guid id);
    PagedData<Entities.User> GetUsers(int currentPage, int pageSize, string? fullNameFilter = null, string? roleFilter = null);
}
