using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FormsBackendBusiness.Exceptions;

public class UserNotFoundException(string guid) : ApiException
{
    public override ProblemDetails GetProblemDetails(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        return new ProblemDetails()
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "User not found",
            Detail = $"User with guid: '{guid}' not found"
        };
    }
}