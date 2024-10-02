using BusinessLogic.HomeOwners.Models;

namespace HomeConnect.WebApi.Test.Controllers;

public interface IHomeOwnerService
{
    public Guid CreateHome(CreateHomeArgs createHomeArgs);
    public Guid AddMemberToHome(AddMemberArgs args);
}
