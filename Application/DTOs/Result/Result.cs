using System.Net;
using System.Runtime.CompilerServices;

namespace Application.DTOs.Result;

public record struct ErrorDesc(string Title, string Desc);

public record struct Error(HttpStatusCode StatusCode, string Title, string Desc)
{
    public static Error NotFound(ErrorDesc desc) => new Error(HttpStatusCode.NotFound, desc.Title, desc.Desc);
    public static Error BadRequest(ErrorDesc desc) => new Error(HttpStatusCode.BadRequest, desc.Title, desc.Desc);
    public static Error InvalidRequest(ErrorDesc desc) => new Error(HttpStatusCode.Forbidden, desc.Title, desc.Desc);
    public static Error Custom(HttpStatusCode statusCode, ErrorDesc desc) => new Error(statusCode, desc.Title, desc.Desc);
    public static Error None => new Error(HttpStatusCode.OK, string.Empty, string.Empty);
};

public struct Result
{
    public bool IsSuccess { get; }
    public Error Error { get; }
    public bool IsFailure => !IsSuccess;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException("Success result cannot have an error.");
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException("Failure result must have an error.");
        }

        IsSuccess = isSuccess;
        Error = error;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result Success() => new Result(true, Error.None);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result Failure(Error error) => new Result(false, error);
}
public struct Result<T>
{
    private readonly T? _value;
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Result(T value, bool isSuccess, Error error)
    {
        _value = value;
        IsSuccess = isSuccess;
        Error = error;
    }

    public T Value
    {
        get
        {
            if (!IsSuccess)
            {
                throw new InvalidOperationException("Cannot access value of a failed result.");
            }

            return _value;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T> Success(T value) => new Result<T>(value, true, Error.None);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T> Failure(Error error) => new Result<T>(default!, false, error);
}