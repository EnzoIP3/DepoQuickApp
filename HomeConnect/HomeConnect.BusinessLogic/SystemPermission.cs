namespace BusinessLogic;

public record SystemPermission
{
    private string Value { get; init; }

    public SystemPermission(string value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
}
