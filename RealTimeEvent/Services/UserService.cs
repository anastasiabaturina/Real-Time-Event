﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealTimeEvent.Configuration;
using RealTimeEvent.Exceptions;
using RealTimeEvent.Interfaces;
using RealTimeEvent.Models;
using RealTimeEvent.Models.DTOs;
using RealTimeEvent.Models.Responses;
using SignalRApp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
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
            throw new BadRequestException("User with this name already exists");
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
        var user = await _userRepository.FindAsync(loginUserDto.UserName, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, loginUserDto.Password);

        if (verificationResult != PasswordVerificationResult.Success)
        {
            throw new AuthenticationException("Invalid password");
        }

        var tokens = GenerateToken(user);

        var loginResponse = new AuthResponse
        {
            Token = tokens,
            UserName = user.Name,
        };

        return loginResponse;
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