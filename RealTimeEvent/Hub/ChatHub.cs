using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RealTimeEvent.Extension;

namespace SignalRApp;

[Authorize]
public class ChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("Notify", $"{Context.GetUserName()} logged into the chat");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.All.SendAsync("Notify", $"{Context.GetUserName()} left the chat");
        await base.OnDisconnectedAsync(exception);
    }
}