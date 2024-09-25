namespace BusinessLogic;

public interface IUserRepository
{
    User? GetUser(string email);
    void Add(User user);
    bool Exists(string email);
    void Delete(string email);
    List<User> GetUsers(int currentPage, int pageSize, string? fullNameFilter = null, string? roleFilter = null);
}
