namespace Application.DTOs.Common;

public interface IResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
}

public class Result<T> : IResult
{
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }

    public static Result<T> Suc(T data, string message = null)
    {
        return new Result<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message,
            Errors = null
        };
    }

    public static Result<T> Failure(List<string> errors, string message = null)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Data = default,
            Message = message,
            Errors = errors
        };
    }

    public static Result<T> Failure(string error, string message = null)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Data = default,
            Message = message,
            Errors = new List<string> { error }
        };
    }
}

public sealed class Result : IResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }

    public static Result Success(string message = null)
    {
        return new Result
        {
            IsSuccess = true,
            Message = message,
            Errors = null
        };
    }

    public static Result Failure(List<string> errors, string message = null)
    {
        return new Result
        {
            IsSuccess = false,
            Message = message,
            Errors = errors
        };
    }

    public static Result Failure(string error, string message = null)
    {
        return new Result
        {
            IsSuccess = false,
            Message = message,
            Errors = new List<string> { error }
        };
    }
}



