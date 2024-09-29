using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeConnect.WebApi.Filters;

public class ExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        throw new NotImplementedException();
    }
}
