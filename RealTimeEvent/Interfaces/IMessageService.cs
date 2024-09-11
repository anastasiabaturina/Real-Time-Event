using RealTimeEvent.Models.DTOs;
using RealTimeEvent.Models.Responses;

namespace RealTimeEvent.Interfaces;

public interface IMessageService
{
    Task SendAsync(MessageDto message);

    Task<GetMessageResponse> GetAsync(GetMessageDto getMessageDto, CancellationToken cancellationToken);
}