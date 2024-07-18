using FluentValidation;

namespace FormsBackendBusiness.Tasks.Commands.AddTask;

public class AddTaskRequestValidator : AbstractValidator<AddTaskCommand>
{
    public AddTaskRequestValidator()
    {
        RuleFor(taskCreate => taskCreate.Title).NotEmpty().MaximumLength(100);
        RuleFor(taskCreate => taskCreate.Description).MaximumLength(1000);
        RuleFor(taskCreate => taskCreate.DueDate).GreaterThanOrEqualTo(DateTime.Now);
    }
}
