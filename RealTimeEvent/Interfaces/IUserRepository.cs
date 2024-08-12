using RealTimeEvent.Models;

namespace RealTimeEvent.Interfaces;

public interface IUserRepository
{
    Task RegisterAsync(User user, CancellationToken cancellationToken);

    Task SaveMesageAsync(Message message);

    Task<User> FindUserNameAsync(string name, CancellationToken cancellationToken);

    Task<List<Message>> GetMessageAsync(DateTime? lastMessage, CancellationToken cancellationToken);
}