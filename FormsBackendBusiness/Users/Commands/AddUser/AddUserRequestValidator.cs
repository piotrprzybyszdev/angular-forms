using FluentValidation;

namespace FormsBackendBusiness.Users.Commands.AddUser;

public class AddUserRequestValidator : AbstractValidator<AddUserCommand>
{
    public AddUserRequestValidator()
    {
        RuleFor(addUserRequest => addUserRequest.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(addUserRequest => addUserRequest.LastName).NotEmpty().MaximumLength(100);
        RuleFor(addUserRequest => addUserRequest.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(addUserRequest => addUserRequest.Password).NotEmpty().MaximumLength(100);
    }
}
