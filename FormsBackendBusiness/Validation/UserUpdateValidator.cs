using FluentValidation;
using FormsBackendCommon.Dtos.User;

namespace FormsBackendBusiness.Validation;

public class UserUpdateValidator : AbstractValidator<UserUpdate>
{
    public UserUpdateValidator()
    {
        RuleFor(userUpdate => userUpdate.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(userUpdate => userUpdate.LastName).NotEmpty().MaximumLength(100);
        RuleFor(userUpdate => userUpdate.Email).NotEmpty().EmailAddress().MaximumLength(100);
    }
}
