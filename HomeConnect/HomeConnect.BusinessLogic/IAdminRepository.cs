namespace BusinessLogic;

public interface IAdminRepository
{
    bool Exists(string email);
    void Add(User user);
    void Delete(string email);
}
