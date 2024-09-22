namespace BusinessLogic;

public interface IBusinessOwnerRepository
{
    bool Exists(string email);
    void Add(BusinessOwner businessOwner);
}
