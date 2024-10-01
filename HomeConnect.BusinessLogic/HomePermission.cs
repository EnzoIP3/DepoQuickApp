using System.ComponentModel.DataAnnotations;

namespace BusinessLogic;

public class HomePermission
{
    [Key]
    private string Value { get; set; } = null!;

    public HomePermission()
    {
    }

    public HomePermission(string value)
    {
        Value = value;
    }
}
