using FluentValidation;
using FormsBackendCommon.Dtos.Account;

namespace FormsBackendBusiness.Validation;

public class AccountRegisterValidator : AbstractValidator<AccountRegister>
{
    public AccountRegisterValidator()
    {
        RuleFor(accountRegister => accountRegister.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(accountRegister => accountRegister.LastName).NotEmpty().MaximumLength(100);
        RuleFor(accountRegister => accountRegister.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(accountRegister => accountRegister.Password).NotEmpty().MaximumLength(100);
    }
}
