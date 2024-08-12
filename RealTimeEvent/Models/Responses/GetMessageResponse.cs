namespace RealTimeEvent.Models.Responses;

public class GetMessageResponse
{
    public List<GetMessageResult> Result { get; set; }

    public DateTime? LastMessage { get; set; }
}