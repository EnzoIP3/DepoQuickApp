using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Models;

namespace BusinessLogic.HomeOwners.Services;

public interface IHomeOwnerService
{
    public Guid CreateHome(CreateHomeArgs createHomeArgs);
    public Guid AddMemberToHome(AddMemberArgs args);
    List<Member> GetHomeMembers(string homeId);
    public IEnumerable<OwnedDevice> GetHomeDevices(string homeId);
    void UpdateMemberNotifications(Guid membersId, bool requestShouldBeNotified);
    public void AddDeviceToHome(AddDevicesArgs addDevicesArgs);
}
