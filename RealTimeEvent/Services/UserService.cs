using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealTimeEvent.Configuration;
using RealTimeEvent.Exceptions;
using RealTimeEvent.Interfaces;
using RealTimeEvent.Models;
using RealTimeEvent.Models.DTOs;
using RealTimeEvent.Models.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealTimeEvent.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IOptions<JwtSettings> _jwt;
    public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IOptions<JwtSettings> jwt)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwt = jwt;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterUserDto registerUserDto, CancellationToken cancellationToken)
    {
        var newUser = new User
        {
            Name = registerUserDto.UserName,  
        };

        var userExist = await _userRepository.FindUserNameAsync(registerUserDto.UserName, cancellationToken);

        if (userExist != null)
        {
            throw new UserAlreadyExistsException();
        }

        newUser.Password = _passwordHasher.HashPassword(newUser, registerUserDto.Password);

        await _userRepository.RegisterAsync(newUser, cancellationToken);

        var response =  new AuthResponse
        {
            Token = GenerateToken(newUser),
            UserName = newUser.Name,
        };

        return response;
    }

    public async Task<AuthResponse> LoginAsync(LoginUserDto loginUserDto, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserNameAsync(loginUserDto.UserName, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, loginUserDto.Password);

        if (verificationResult != PasswordVerificationResult.Success)
        {
            throw new ArgumentException("Invalid password");
        }

        var tokens = GenerateToken(user);

        var loginResponse = new AuthResponse
        {
            Token = tokens,
            UserName = user.Name,
        };

        return loginResponse;
    }

    public async Task SaveMessageAsync(MessageDto message)
    {
        var saveMessage = new Message
        {
            Text = message.Text,
            UserId = message.UserId,
            TimeSending = DateTime.UtcNow,
            UserName = message.UserName
        };

        await _userRepository.SaveMesageAsync(saveMessage);
    }

    public async Task<GetMessageResponse> GetMessageAsync(GetMessageDto getMessageDto, CancellationToken cancellationToken)
    {
        var messages = await _userRepository.GetMessageAsync(getMessageDto.LastMessage, cancellationToken);

        var lastMessage = messages.Last().TimeSending;

        var messageResult = messages.Select(message => new GetMessageResult
        {
            UserName = message.UserName, 
            Text = message.Text, 
            TimeSending = message.TimeSending,
        }).ToList();

        var response = new GetMessageResponse
        {
            Result = messageResult,
            LastMessage = lastMessage
        };

        return response;
    }

    private string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Value.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
        };

        var token = new JwtSecurityToken(
            issuer: _jwt.Value.Issuer,
            audience: _jwt.Value.Audience,
            claims: claims,
            signingCredentials: credentials,
            expires: DateTime.UtcNow.AddHours(_jwt.Value.TokenLifetime)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}