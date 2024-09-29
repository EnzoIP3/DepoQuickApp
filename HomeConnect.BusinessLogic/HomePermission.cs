using System.ComponentModel.DataAnnotations;

namespace BusinessLogic;

public class HomePermission
{
    [Key]
    private string Value { get; set; }

    public HomePermission(string value)
    {
        Value = value;
    }
}
