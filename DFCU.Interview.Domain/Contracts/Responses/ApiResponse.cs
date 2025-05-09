using System.Net;

namespace DFCU.Interview.Domain.Contracts.Responses;

public class ApiResponse<T> where T : class
{
    public bool Success { get; }
    public string Message { get; } = string.Empty;
    public T? Data { get; }
    public HttpStatusCode StatusCode { get; }
    public ApiResponse(bool success, string message, T? data = null, HttpStatusCode statusCode = HttpStatusCode.Continue)
    {
        Success = success;
        Message = message;
        Data = data;
        StatusCode = statusCode;
    }
}

public class ApiResponse
{
    public bool Success { get; }
    public string Message { get; } = string.Empty;
    public HttpStatusCode StatusCode { get; }
    public ApiResponse(bool success, string message, HttpStatusCode statusCode = HttpStatusCode.Continue)
    {
        Success = success;
        Message = message;
        StatusCode = statusCode;
    }
}
