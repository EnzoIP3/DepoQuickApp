namespace BusinessLogic;

public interface IAdminRepository
{
    bool Exists(string username);
    void Add(Admin admin);
}
