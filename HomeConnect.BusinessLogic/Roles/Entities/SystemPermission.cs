using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Roles.Entities;

public record SystemPermission
{
    [Key]
    public string Value { get; init; }

    public SystemPermission(string value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
}
