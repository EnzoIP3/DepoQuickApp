namespace BusinessLogic;

public interface IUserRepository
{
    bool Exists(string email);
    void Add(User user);
    void Delete(string email);
    List<User> GetUsers(int currentPage, int pageSize);
}
