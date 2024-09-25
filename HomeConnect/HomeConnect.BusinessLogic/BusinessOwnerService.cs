namespace BusinessLogic
{
    public class BusinessOwnerService
    {
        public IUserRepository UserRepository { get; init; }
        public IBusinessRepository BusinessRepository { get; init; }
        public IRoleRepository RoleRepository { get; init; }

        public BusinessOwnerService(IUserRepository userRepository, IBusinessRepository businessRepository, IRoleRepository roleRepository)
        {
            throw new Exception("Not implemented");
        }
    }
}
