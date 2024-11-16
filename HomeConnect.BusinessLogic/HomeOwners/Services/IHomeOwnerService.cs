using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Models;

namespace BusinessLogic.HomeOwners.Services;

public interface IHomeOwnerService
{
    public Guid CreateHome(CreateHomeArgs createHomeArgs);
    public Guid AddMemberToHome(AddMemberArgs args);
    List<Member> GetHomeMembers(string homeId);
    public IEnumerable<OwnedDevice> GetHomeDevices(string homeId, string? roomId);
    public Member GetMemberById(Guid memberId);
    void UpdateMemberNotifications(Guid membersId, bool? requestShouldBeNotified);
    public void AddDeviceToHome(AddDevicesArgs addDevicesArgs);
    Home GetHome(Guid homeId);
    public List<Home> GetHomesByOwnerId(Guid userId);
    public void NameHome(NameHomeArgs args);
    public List<HomePermission> GetHomePermissions(Guid homeId, Guid userId);
    public void NameDevice(NameDeviceArgs args);
    public OwnedDevice GetOwnedDeviceByHardwareId(string hardwareId);
    Room CreateRoom(string homeId, string name);
    Room GetRoom(string roomId);
    Guid AddOwnedDeviceToRoom(string roomId, string ownedDeviceId);
}
