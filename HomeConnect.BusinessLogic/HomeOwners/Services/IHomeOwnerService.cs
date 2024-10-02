using BusinessLogic.HomeOwners.Models;

namespace HomeConnect.WebApi.Test.Controllers;

public interface IHomeOwnerService
{
    public Guid Create(CreateHomeArgs createHomeArgs);
}
