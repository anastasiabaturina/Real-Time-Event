using RealTimeEvent.Models.DTOs;
using RealTimeEvent.Models.Responses;

namespace RealTimeEvent.Interfaces;

public interface IUserService
{
    Task<AuthResponse> RegisterAsync(RegisterUserDto registerUserDto, CancellationToken cancellationToken);

    Task<AuthResponse> LoginAsync(LoginUserDto loginUserDto, CancellationToken cancellationToken);

    Task SaveMessageAsync(MessageDto message);

    Task<GetMessageResponse> GetMessageAsync(GetMessageDto getMessageDto, CancellationToken cancellationToken);
}
