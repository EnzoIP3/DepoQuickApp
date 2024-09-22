namespace BusinessLogic;

public interface IBusinessOwnerRepository
{
    bool Exists(string email);
}
