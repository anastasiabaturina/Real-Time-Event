using Microsoft.EntityFrameworkCore;
using RealTimeEvent.Interfaces;
using RealTimeEvent.Models;

namespace RealTimeEvent.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly UserDbContext _context;

    public MessageRepository(UserDbContext context)
    {
        _context = context;
    }
    public async Task SaveAsync(Message message)
    {
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Message>> GetAsync(DateTime? lastMesssge, CancellationToken cancellationToken)
    {
        if (lastMesssge != null)
        {
            return await _context.Messages
                .Where(u => u.TimeSending < lastMesssge)
                .OrderByDescending(u => u.TimeSending)
                .Take(10)
                .ToListAsync(cancellationToken);
        }

        return await _context.Messages
            .OrderByDescending(u => u.TimeSending)
            .Take(10)
            .ToListAsync(cancellationToken);
    }
}