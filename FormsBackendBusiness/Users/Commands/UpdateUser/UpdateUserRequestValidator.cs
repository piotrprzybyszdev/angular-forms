using FluentValidation;

namespace FormsBackendBusiness.Users.Commands.UpdateUser;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(userUpdate => userUpdate.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(userUpdate => userUpdate.LastName).NotEmpty().MaximumLength(100);
        RuleFor(userUpdate => userUpdate.Email).NotEmpty().EmailAddress().MaximumLength(100);
    }
}
