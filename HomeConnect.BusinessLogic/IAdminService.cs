namespace BusinessLogic;

public interface IAdminService
{
    public Guid Create(UserModel userModel);
    void Delete(Guid id);
}
