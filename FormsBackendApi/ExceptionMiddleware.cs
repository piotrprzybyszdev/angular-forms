using FormsBackendBusiness.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FormsBackendApi;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        ProblemDetails problemDetails;

        try
        {
            await next(context);
            return;
        }
        catch (ApiException ex)
        {
            problemDetails = ex.GetProblemDetails(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status500InternalServerError,
                Detail = ex.Message,
                Title = "Internal Server Error - something went wrong.",
            };
        }

        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }
}
