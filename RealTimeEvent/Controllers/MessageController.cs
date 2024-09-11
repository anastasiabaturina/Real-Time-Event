using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeEvent.Extensions;
using RealTimeEvent.Interfaces;
using RealTimeEvent.Models.DTOs;
using RealTimeEvent.Models.Requests;
using RealTimeEvent.Models.Responses;

namespace RealTimeEvent.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/messages")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet]
    public async Task<ActionResult<Response<GetMessageResponse>>> GetMessage([FromQuery] DateTime? lastMessage, CancellationToken cancellationToken)
    {
        var getMesssgeDto = new GetMessageDto
        {
            LastMessage = lastMessage,
        };

        var result = await _messageService.GetAsync(getMesssgeDto, cancellationToken);

        var response = new Response<GetMessageResponse>
        {
            Data = result
        };

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Response<object>>> SendMessage(SendMessageRequest messageRequest)
    {
        var userName = HttpContext.GetUserName();
        var userId = HttpContext.GetUserId();

        var message = new MessageDto
        {
            Text = messageRequest.Message,
            UserName = userName,
            UserId = userId
        };

        await _messageService.SendAsync(message);

        var response = new Response<object>();

        return Ok(response);
    }
}