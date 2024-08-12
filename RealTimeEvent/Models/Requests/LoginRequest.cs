using System.ComponentModel.DataAnnotations;

namespace RealTimeEvent.Models.Requests;

public class LoginRequest
{
    [Required(ErrorMessage = "Name is required.")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(30, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string? Password { get; set; }
}