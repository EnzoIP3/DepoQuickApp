namespace BusinessLogic;

public class HomeOwnerService
{
    private readonly IHomeRepository _homeRepository;

    public HomeOwnerService(IHomeRepository homeRepository)
    {
        _homeRepository = homeRepository;
    }

    public void CreateHome(Home home)
    {
        _homeRepository.Add(home);
    }
}
