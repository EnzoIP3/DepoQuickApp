namespace BusinessLogic;

public interface IUserRepository
{
    void Add(User user);
    bool Exists(Guid id);
    void Delete(Guid id);
    PagedData<User> GetUsers(int currentPage, int pageSize, string? fullNameFilter = null, string? roleFilter = null);
}
