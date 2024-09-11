using RealTimeEvent.Models;

namespace RealTimeEvent.Interfaces;

public interface IMessageRepository
{
    Task SaveAsync(Message message);

    Task<List<Message>> GetAsync(DateTime? lastMessage, CancellationToken cancellationToken);
}