using Microsoft.AspNetCore.Mvc;
using RealTimeEvent.Interfaces;
using RealTimeEvent.Models.DTOs;
using RealTimeEvent.Models.Request;

namespace RealTimeEvent.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest registerUserRequest, CancellationToken cancellationToken)
    {
        var userDto = new RegisterUserDto
        {
            UserName = registerUserRequest.UserName,
            Password = registerUserRequest.Password,
        };

        var response = await _userService.RegisterAsync(userDto, cancellationToken);

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        var userDto = new LoginUserDto
        {
            UserName = loginRequest.UserName,
            Password = loginRequest.Password,
        };

        var response = await _userService.LoginAsync(userDto, cancellationToken);

        return Ok(response);
    }
}
