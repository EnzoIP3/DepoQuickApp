namespace BusinessLogic;

public class HomeOwnerService
{
    private readonly IHomeRepository _homeRepository;
    private readonly IUserRepository _userRepository;

    public HomeOwnerService(IHomeRepository homeRepository, IUserRepository userRepository)
    {
        _homeRepository = homeRepository;
        _userRepository = userRepository;
    }

    public void CreateHome(CreateHomeModel model)
    {
        EnsureArgumentsAreValid(model);
        var user = _userRepository.Get(model.HomeOwnerEmail);
        var home = new Home(user, model.Address, model.Latitude, model.Longitude, model.MaxMembers);
        _homeRepository.Add(home);
    }

    private static void EnsureArgumentsAreValid(CreateHomeModel model)
    {
        if (string.IsNullOrWhiteSpace(model.HomeOwnerEmail) || string.IsNullOrWhiteSpace(model.Address))
        {
            throw new ArgumentException("All arguments are required");
        }
    }
}
