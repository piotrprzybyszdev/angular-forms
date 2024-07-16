using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FormsBackendBusiness.Exceptions;

public abstract class ApiException : Exception
{
    public ApiException()
    {
    }

    public ApiException(string? message) : base(message)
    {
    }

    public ApiException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public abstract ProblemDetails GetProblemDetails(HttpContext context);
}
