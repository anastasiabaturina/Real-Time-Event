using Microsoft.AspNetCore.SignalR;
using RealTimeEvent.Interfaces;
using RealTimeEvent.Models.DTOs;
using RealTimeEvent.Models.Responses;
using RealTimeEvent.Models;
using SignalRApp;

namespace RealTimeEvent.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IHubContext<ChatHub> _hubContext;

    public MessageService(IMessageRepository messageRepository, IHubContext<ChatHub> hubContext)
    {
        _messageRepository = messageRepository;
        _hubContext = hubContext;
    }

    public async Task SendAsync(MessageDto message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message.UserName, message.Text);

        var saveMessage = new Message
        {
            Text = message.Text,
            UserId = message.UserId,
            TimeSending = DateTime.UtcNow,
            UserName = message.UserName
        };

        await _messageRepository.SaveAsync(saveMessage);
    }

    public async Task<GetMessageResponse> GetAsync(GetMessageDto getMessageDto, CancellationToken cancellationToken)
    {
        var messages = await _messageRepository.GetAsync(getMessageDto.LastMessage, cancellationToken);

        var lastMessage = messages.Last().TimeSending;

        var messageResult = messages.Select(message => new GetMessageResult
        {
            UserName = message.UserName,
            Text = message.Text,
            TimeSending = message.TimeSending,
        }).ToList();

        var response = new GetMessageResponse
        {
            Result = messageResult,
            LastMessage = lastMessage
        };

        return response;
    }
}