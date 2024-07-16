using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FormsBackendBusiness.Exceptions;

public class TaskNotFoundException(int id) : ApiException
{
    public override ProblemDetails GetProblemDetails(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        return new ProblemDetails()
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Task not found",
            Detail = $"Task with id: '{id}' not found"
        };
    }
}