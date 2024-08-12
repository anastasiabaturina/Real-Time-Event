namespace RealTimeEvent.Models.DTOs;

public class MessageDto
{
    public Guid UserId { get; set; }

    public string? UserName { get; set; }

    public string? Text { get; set; }
}