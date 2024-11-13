namespace BusinessLogic.BusinessOwners.Models;

public struct UpdateValidatorArgs
{
    public string BusinessRut { get; set; }
    public string Validator { get; set; }
    public string OwnerId { get; set; }
}
