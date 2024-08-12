using Microsoft.EntityFrameworkCore;
using RealTimeEvent.Interfaces;
using RealTimeEvent.Models;

namespace RealTimeEvent.Repositories;

public class UserRepository : IUserRepository
{ 
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task RegisterAsync(User user, CancellationToken cancellationToken)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<User> FindUserNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _context.Users
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.Name == name, cancellationToken);
    }

    public async Task SaveMesageAsync(Message message)
    {
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Message>> GetMessageAsync(DateTime? lastIdMesssge, CancellationToken cancellationToken)
    {
        if (lastIdMesssge != null)
        {
            return await _context.Messages                            
                .Where(u => u.TimeSending < lastIdMesssge)
                .OrderByDescending(u => u.TimeSending)
                .Take(10)
                .ToListAsync(cancellationToken);
        }
        
        return await  _context.Messages
            .OrderByDescending(u => u.TimeSending)
            .Take(10)
            .ToListAsync(cancellationToken);
    }
}