namespace BusinessLogic;

public interface IHomeRepository
{
    void Add(Home home);
    Home Get(Guid homeId);
}
