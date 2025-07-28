namespace TrendFlaunt.Domain.Common;

public class ServiceResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public ErrorCode ErrorCode { get; set; }
    public Dictionary<string, object> ErrorMessage { get; set; } = new();
    public static ServiceResponse<T> SuccessResponse(T data)
    {
        return new ServiceResponse<T> { Success = true, Data = data };
    }
    public static ServiceResponse<T> FailureResponse(string message, ErrorCode errorCode)
    {
        return new ServiceResponse<T> { Success = false, Message = message, ErrorCode = errorCode };
    }
    public static ServiceResponse<T> FailureResponse(Dictionary<string, object> message, ErrorCode errorCode)
    {
        return new ServiceResponse<T> { Success = false, ErrorMessage = message, ErrorCode = errorCode };
    }
}
