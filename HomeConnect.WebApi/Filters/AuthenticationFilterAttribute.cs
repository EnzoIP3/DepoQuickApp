using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeConnect.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthenticationFilterAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        throw new NotImplementedException();
    }
}
