using System.Net;

public class BaseException : Exception
{
    public HttpStatusCode StatusCode { get; }
    protected BaseException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        : base(message)
    {
        StatusCode = statusCode;
    }
}