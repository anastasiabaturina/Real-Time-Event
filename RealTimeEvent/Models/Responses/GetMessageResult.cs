namespace RealTimeEvent.Models.Responses;

public class GetMessageResult
{
    public string? UserName { get; set; }

    public string? Text { get; set; }

    public DateTime? TimeSending { get; set; }
}