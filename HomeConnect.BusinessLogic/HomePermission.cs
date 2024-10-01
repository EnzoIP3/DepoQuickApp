using System.ComponentModel.DataAnnotations;

namespace BusinessLogic;

public class HomePermission
{
    [Key]
    public string Value { get; init; }

    public HomePermission(string value)
    {
        Value = value;
    }
}
