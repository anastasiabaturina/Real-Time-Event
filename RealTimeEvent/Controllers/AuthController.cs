using Microsoft.AspNetCore.Mvc;
using RealTimeEvent.Interfaces;
using RealTimeEvent.Models.DTOs;
using RealTimeEvent.Models.Requests;
using RealTimeEvent.Models.Responses;

namespace RealTimeEvent.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<Response<AuthResponse>>> Register(RegisterUserRequest registerUserRequest, CancellationToken cancellationToken)
    {
        var userDto = new RegisterUserDto
        {
            UserName = registerUserRequest.UserName,
            Password = registerUserRequest.Password,
        };

        var result = await _userService.RegisterAsync(userDto, cancellationToken);

        var reposnse = new Response<AuthResponse>
        {
            Data = result,
        };

        return Ok(reposnse);
    }

    [HttpPost("login")]
    public async Task<ActionResult<Response<AuthResponse>>> Login(LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        var userDto = new LoginUserDto
        {
            UserName = loginRequest.UserName,
            Password = loginRequest.Password,
        };

        var result = await _userService.LoginAsync(userDto, cancellationToken);

        var response = new Response<AuthResponse>
        {
            Data = result,
        };

        return Ok(response);
    }
}