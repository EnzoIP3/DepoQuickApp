namespace BusinessLogic.Admins.Models;

public struct GetBusinessesArgs
{
    public string Name { get; init; }
    public string OwnerFullName { get; init; }
    public string OwnerEmail { get; init; }
    public string Rut { get; init; }
}
