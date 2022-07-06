namespace WebApplication1.Services.Exceptions;

public class ApiResponseException : Exception
{
    public ApiResponseException(string message) : base(message)
    {
    }
}