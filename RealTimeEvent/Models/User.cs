namespace RealTimeEvent.Models;

public class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public IEnumerable<Message> Messages { get; set; }
}