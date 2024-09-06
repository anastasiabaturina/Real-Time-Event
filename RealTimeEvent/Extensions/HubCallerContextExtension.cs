using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace RealTimeEvent.Extension;

public static class HubCallerContextExtension
{
    public static string GetUserName(this HubCallerContext context)
    {
        return context.User.FindFirstValue(ClaimTypes.Name);
    }
}
