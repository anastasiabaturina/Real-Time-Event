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

    public async Task<User> FindAsync(string name, CancellationToken cancellationToken)
    {
        return await _context.Users
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.Name == name, cancellationToken);
    }
}