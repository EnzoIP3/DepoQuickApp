namespace HomeConnect.WebApi.Controllers.BusinessOwner;

public struct CreateBusinessRequest
{
    public string Name { get; set; }
    public BusinessLogic.Users.Entities.User Owner { get; set; }
    public string Rut { get; set; }
}
