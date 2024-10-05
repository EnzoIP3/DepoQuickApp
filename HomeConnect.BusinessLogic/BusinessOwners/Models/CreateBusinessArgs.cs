namespace BusinessLogic.BusinessOwners.Models;

public struct CreateBusinessArgs
{
    public string Name { get; set; }
    public string Rut { get; set; }
    public string OwnerId { get; set; }
}
