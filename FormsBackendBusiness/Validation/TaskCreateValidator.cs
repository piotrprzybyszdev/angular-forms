using FluentValidation;
using FormsBackendCommon.Dtos.Task;

namespace FormsBackendBusiness.Validation;

public class TaskCreateValidator : AbstractValidator<TaskCreate>
{
    public TaskCreateValidator()
    {
        RuleFor(taskCreate => taskCreate.Title).NotEmpty().MaximumLength(100);
        RuleFor(taskCreate => taskCreate.Description).MaximumLength(1000);
        RuleFor(taskCreate => taskCreate.DueDate).GreaterThanOrEqualTo(DateTime.Now);
    }
}
