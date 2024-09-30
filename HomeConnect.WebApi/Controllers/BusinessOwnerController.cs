using BusinessLogic;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers;

[ApiController]
[Route("business_owners")]
[AuthorizationFilter]
public class BusinessOwnerController() : ControllerBase
{
    public CreateBusinessOwnerResponse CreateBusinessOwner(CreateBusinessOwnerRequest request, string s)
    {
        throw new NotImplementedException();
    }
}
