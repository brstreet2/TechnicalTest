namespace TechnicalTest.Authorization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TechnicalTest.Entities;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext filterContext)
    {
        var allowAnonymous = filterContext.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous) return;

        var user = (User)filterContext.HttpContext.Items["User"];
        if (user == null) filterContext.Result = new JsonResult(new { message = "You're not authorized!" }) { StatusCode = StatusCodes.Status401Unauthorized };
    }
}