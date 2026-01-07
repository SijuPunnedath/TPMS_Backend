namespace TPMS.Application.Common.Models;

public class ApiResponse<T>
{
    public int IRet { get; set; }              // 1 = success, -1 = failure
    public string Message { get; set; } = string.Empty;
    public T? Result { get; set; }

    public static ApiResponse<T> Success(T result, string message)
        => new()
        {
            IRet = 1,
            Message = message,
            Result = result
        };

    public static ApiResponse<T> Failure(string message)
        => new()
        {
            IRet = -1,
            Message = message,
            Result = default
        };
}
