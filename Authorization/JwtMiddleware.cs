namespace TechnicalTest.Authorization;

using TechnicalTest.Services;

public class JwtMiddleware
{
    private readonly RequestDelegate _requestDelegate;

    public JwtMiddleware(RequestDelegate requestDelegate)
    {
        _requestDelegate = requestDelegate;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtUtils.ValidateToken(token);
        if (userId != null) context.Items["User"] = userService.GetById(userId.Value);

        await _requestDelegate(context);
    }
}