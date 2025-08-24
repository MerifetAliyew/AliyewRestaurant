using AliyewRestaurant.Application.Shared;
using System.Net;
using System.Text.Json;
using FluentValidation;

namespace AliyewRestaurant.WebApi.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // Növbəti middleware və ya endpoint-i çağır
        }
        catch (ValidationException validationEx)
        {
            var status = HttpStatusCode.BadRequest;

            // PropertyName -> List of error messages
            var errors = validationEx.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToList()
                );

            var response = new BaseResponse<Dictionary<string, List<string>>>(
                message: "Validasiya xətaları baş verdi.",
                data: errors,
                statusCode: status
            );
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new BaseResponse<string>(
            message: "Xəta baş verdi: " + exception.Message,
            isSuccess: false,
            statusCode: HttpStatusCode.InternalServerError
        );

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }
}
