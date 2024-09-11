using RealTimeEvent.Models;

namespace RealTimeEvent.Interfaces;

public interface IUserRepository
{
    Task RegisterAsync(User user, CancellationToken cancellationToken);

    Task<User> FindAsync(string name, CancellationToken cancellationToken);
}