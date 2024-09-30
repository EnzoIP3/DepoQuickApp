namespace BusinessLogic;

public interface IAdminService
{
    public Guid Create(UserModel userModel);
    void Delete(Guid id);
    public Guid CreateBusinessOwner(UserModel userModel);
    PagedData<ListUserModel> GetUsers(int? currentPage, int? pageSize, string? fullNameFilter, string? roleFilter);
}
