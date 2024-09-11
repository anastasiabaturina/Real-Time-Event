using System.Security.Claims;

namespace RealTimeEvent.Extensions;

public static class HttpContextExtension
{
    public static string GetUserName(this HttpContext httpContext)
    {
        return httpContext.User.FindFirstValue(ClaimTypes.Name);
    }
    public static Guid GetUserId(this HttpContext context)
    {
        var userId = context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        return userId != null ? Guid.Parse(userId) : Guid.Empty;
    }
}