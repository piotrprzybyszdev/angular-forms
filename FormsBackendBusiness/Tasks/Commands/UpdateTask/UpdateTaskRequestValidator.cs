using FluentValidation;

namespace FormsBackendBusiness.Tasks.Commands.UpdateTask;

public class UpdateTaskRequestValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskRequestValidator()
    {
        RuleFor(taskUpdate => taskUpdate.Title).NotEmpty().MaximumLength(100);
        RuleFor(taskUpdate => taskUpdate.Description).MaximumLength(1000);
        RuleFor(taskUpdate => taskUpdate.DueDate).GreaterThanOrEqualTo(DateTime.Today);
    }
}
