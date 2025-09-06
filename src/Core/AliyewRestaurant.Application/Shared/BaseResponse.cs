using System.Net;

namespace AliyewRestaurant.Application.Shared;

public class BaseResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public T? Data { get; set; }

    public BaseResponse(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
        Success = statusCode == HttpStatusCode.OK || statusCode == HttpStatusCode.Created;
    }

    public BaseResponse(string message, HttpStatusCode statusCode)
    {
        Message = message;
        StatusCode = statusCode;
        Success = statusCode == HttpStatusCode.OK || statusCode == HttpStatusCode.Created;
    }

    public BaseResponse(string message, bool isSuccess, HttpStatusCode statusCode)
    {
        Message = message;
        Success = isSuccess;
        StatusCode = statusCode;
    }

    public BaseResponse(string message, T? data, HttpStatusCode statusCode)
    {
        Message = message;
        Data = data;
        StatusCode = statusCode;
        Success = statusCode == HttpStatusCode.OK || statusCode == HttpStatusCode.Created;
    }
}

