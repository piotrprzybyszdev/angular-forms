using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FormsBackendBusiness.Exceptions;

public record ValidationError(string Title, string Details);

public class ValidationFailedException : ApiException
{
    readonly IEnumerable<ValidationError> errors;

    public ValidationFailedException(IEnumerable<IdentityError> errors)
    {
        this.errors = errors.Select(error => new ValidationError(error.Code, error.Description));
    }

    public ValidationFailedException(IEnumerable<ValidationFailure> errors)
    {
        this.errors = errors.Select(error => new ValidationError(error.ErrorCode, error.ErrorMessage));
    }

    public override ProblemDetails GetProblemDetails(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        return new ProblemDetails()
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation exception",
            Extensions =
            {
                ["validationFailures"] = errors
            }
        };
    }
}