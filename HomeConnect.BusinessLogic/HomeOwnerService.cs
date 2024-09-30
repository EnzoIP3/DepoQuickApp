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
        EnsureCreateHomeModelIsValid(model);
        var user = _userRepository.Get(model.HomeOwnerEmail);
        var home = new Home(user, model.Address, model.Latitude, model.Longitude, model.MaxMembers);
        _homeRepository.Add(home);
    }

    private static void EnsureCreateHomeModelIsValid(CreateHomeModel model)
    {
        if (string.IsNullOrWhiteSpace(model.HomeOwnerEmail) || string.IsNullOrWhiteSpace(model.Address))
        {
            throw new ArgumentException("All arguments are required");
        }
    }

    public void AddMemberToHome(AddMemberModel model)
    {
        EnsureAddMemberModelIsValid(model);
        EnsureHomeIdIsValidGuid(model);
        var user = _userRepository.Get(model.HomeOwnerEmail);
        var home = _homeRepository.Get(Guid.Parse(model.HomeId));
        var permissions = new List<HomePermission>();

        if (model.CanAddDevices)
        {
            permissions.Add(new HomePermission("canAddDevices"));
        }

        if (model.CanListDevices)
        {
            permissions.Add(new HomePermission("canListDevices"));
        }

        var member = new Member(user, permissions);
        home.AddMember(member);
    }

    private static void EnsureHomeIdIsValidGuid(AddMemberModel model)
    {
        if (!Guid.TryParse(model.HomeId, out _))
        {
            throw new ArgumentException("HomeId must be a valid guid");
        }
    }

    private static void EnsureAddMemberModelIsValid(AddMemberModel model)
    {
        if (string.IsNullOrWhiteSpace(model.HomeId) || string.IsNullOrWhiteSpace(model.HomeOwnerEmail))
        {
            throw new ArgumentException("All arguments are required");
        }
    }
}
