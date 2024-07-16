using FluentValidation;
using FormsBackendCommon.Dtos.User;

namespace FormsBackendBusiness.Validation;

public class UserCreateValidator : AbstractValidator<UserCreate>
{
    public UserCreateValidator()
    {
        RuleFor(userCreate => userCreate.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(userCreate => userCreate.LastName).NotEmpty().MaximumLength(100);
        RuleFor(userCreate => userCreate.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(userCreate => userCreate.Password).NotEmpty().MaximumLength(100);
    }
}
