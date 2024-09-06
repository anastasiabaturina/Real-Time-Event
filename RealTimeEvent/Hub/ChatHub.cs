﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RealTimeEvent.Extension;
using RealTimeEvent.Interfaces;
using RealTimeEvent.Models.DTOs;
using System.Security.Claims;

namespace SignalRApp;

[Authorize]
public class ChatHub : Hub
{
    private readonly IUserService _userService;

    public ChatHub(IUserService userService)
    {
        _userService = userService; 
    }

    public async Task SendMessage(string message)
    {
        var userName = Context.GetUserName;
        await Clients.All.SendAsync("Receive", message, userName);

        var saveMessageDto = new MessageDto
        {
            Text = message,
            UserId = Guid.Parse(Context.User.FindFirstValue(ClaimTypes.NameIdentifier)),
            UserName = userName.ToString(),
        };

        await _userService.SaveMessageAsync(saveMessageDto);
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("Notify", $"{Context.User.FindFirstValue(ClaimTypes.Name)} logged into the chat");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.All.SendAsync("Notify", $"{Context.User.FindFirstValue(ClaimTypes.Name)} left the chat");
        await base.OnDisconnectedAsync(exception);
    }
}