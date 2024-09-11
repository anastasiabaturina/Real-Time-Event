using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace RealTimeEvent.Extension;

public static class HubCallerContextExtension
{
    public static string GetUserName(this HubCallerContext context)
    {
        return context.User?.FindFirstValue(ClaimTypes.Name);
    }

    public static Guid GetUserId(this HubCallerContext context)
    {
        var userId = context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        return userId != null ? Guid.Parse(userId) : Guid.Empty;
    }
}
