namespace RealTimeEvent.Models;

public class Message
{
    public Guid Id { get; set; }

    public string? Text { get; set; }

    public DateTime? TimeSending { get; set; }

    public Guid UserId { get; set; }

    public string? UserName { get; set; }

    public User? User { get; set; }
}