using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.HomeOwners.Entities;

public class HomePermission
{
    [Key]
    public string Value { get; init; }

    public HomePermission(string value)
    {
        Value = value;
    }
}
