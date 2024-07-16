using FluentValidation;
using FormsBackendCommon.Dtos.Task;

namespace FormsBackendBusiness.Validation;

public class TaskUpdateValidator : AbstractValidator<TaskUpdate>
{
    public TaskUpdateValidator()
    {
        RuleFor(taskUpdate => taskUpdate.Title).NotEmpty().MaximumLength(100);
        RuleFor(taskUpdate => taskUpdate.Description).MaximumLength(1000);
        RuleFor(taskUpdate => taskUpdate.DueDate).GreaterThanOrEqualTo(DateTime.Today);
    }
}
