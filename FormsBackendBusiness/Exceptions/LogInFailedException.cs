using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FormsBackendBusiness.Exceptions;

public class LogInFailedException : ApiException
{
    public override ProblemDetails GetProblemDetails(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;

        return new ProblemDetails()
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Login Failed",
            Detail = "Incorrect username or password"
        };
    }
}