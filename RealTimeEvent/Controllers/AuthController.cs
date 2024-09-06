using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeEvent.Filters;
using RealTimeEvent.Interfaces;
using RealTimeEvent.Models.DTOs;
using RealTimeEvent.Models.Requests;
using RealTimeEvent.Models.Responses;

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

    [ValidatorFilter]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest registerUserRequest, CancellationToken cancellationToken)
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

    [ValidatorFilter]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest, CancellationToken cancellationToken)
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

    [Authorize]
    [HttpGet("messages")]
    public async Task<IActionResult> GetMessage([FromQuery] DateTime? lastMessage, CancellationToken cancellationToken)
    {
        var getMesssgeDto = new GetMessageDto
        {
            LastMessage = lastMessage,
        };

        var result = await _userService.GetMessageAsync(getMesssgeDto, cancellationToken);

        var response = new Response<GetMessageResponse>
        {
            Data = result
        };

        return Ok(response);
    }
}