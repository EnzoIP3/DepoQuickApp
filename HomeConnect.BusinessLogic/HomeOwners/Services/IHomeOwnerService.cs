using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Models;
using BusinessLogic.Users.Entities;

namespace HomeConnect.WebApi.Test.Controllers;

public interface IHomeOwnerService
{
    public Guid CreateHome(CreateHomeArgs createHomeArgs);
    public Guid AddMemberToHome(AddMemberArgs args);
    List<Member> GetHomeMembers(string homeId);
    public IEnumerable<OwnedDevice> GetHomeDevices(string homeId);
    void UpdateMemberNotifications(Guid membersId, bool requestShouldBeNotified);
}
