namespace HomeConnect.WebApi.Test.Filters;

internal class FilterTestsUtils
{
    internal static string GetInnerCode(object? value)
    {
        return value?.GetType().GetProperty("InnerCode")?.GetValue(value)?.ToString() ?? string.Empty;
    }

    internal static string GetMessage(object? value)
    {
        return value?.GetType().GetProperty("Message")?.GetValue(value)?.ToString() ?? string.Empty;
    }
}
