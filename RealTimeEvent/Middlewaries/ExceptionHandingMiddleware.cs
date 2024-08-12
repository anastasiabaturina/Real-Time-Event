using RealTimeEvent.Exceptions;
using RealTimeEvent.Models.Responses;
using System.Net;

namespace RealTimeEvent.Middlewaries;

public class ExceptionHandingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ArgumentException ex)
        {
            var code = HttpStatusCode.BadRequest;
            var errorMessage = ex.Message;
            await HandleExceptionAsync(context, code, errorMessage);
        }
        catch (NotFoundException ex)
        {
            var code = HttpStatusCode.NotFound;
            var errorMessage = ex.Message;
            await HandleExceptionAsync(context, code, errorMessage);
        }
        catch (UserAlreadyExistsException)
        {
            var code = HttpStatusCode.BadRequest;
            var errorMessage = "User with this name already exists";
            await HandleExceptionAsync(context, code, errorMessage);
        }
        catch (Exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var errorMessage = "Internal server error.";
            await HandleExceptionAsync(context, code, errorMessage);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, HttpStatusCode code, string errorMessage)
    {
        context.Response.StatusCode = (int)code;
        var response = new Response<object>
        {
            Data = null,
            Error = new Error
            {
                Message = errorMessage,
                ErrorCode = code
            }
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}