namespace BusinessLogic;

public interface IAdminRepository
{
    bool Exists(string email);
    void Add(Admin admin);
    void Delete(string email);
}
