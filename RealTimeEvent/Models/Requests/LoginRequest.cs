using System.Reflection.Metadata;

namespace RealTimeEvent.Models.Request;

public class LoginRequest
{
    public string? UserName { get; set; }

    public string? Password { get; set; }
}
