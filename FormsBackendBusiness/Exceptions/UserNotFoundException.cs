using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FormsBackendBusiness.Exceptions;

public class UserNotFoundException(string id) : ApiException
{
    public override ProblemDetails GetProblemDetails(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        return new ProblemDetails()
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "User not found",
            Detail = $"User with guid: '{id}' not found"
        };
    }
}