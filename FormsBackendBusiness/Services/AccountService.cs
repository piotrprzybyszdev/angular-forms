using AutoMapper;
using FormsBackendBusiness.Exceptions;
using FormsBackendBusiness.Validation;
using FormsBackendCommon.Dtos.Account;
using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using Microsoft.AspNetCore.Identity;

namespace FormsBackendBusiness.Services;

public class AccountService(UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager, IMapper mapper,
    AccountRegisterValidator accountRegisterValidator) : IAccountService
{
    public async Task RegisterAsync(AccountRegister accountRegister)
    {
        var validatorResult = await accountRegisterValidator.ValidateAsync(accountRegister);
        if (!validatorResult.IsValid) throw new ValidationFailedException(validatorResult.Errors);

        var user = mapper.Map<ApplicationUser>(accountRegister);
        var result = await userManager.CreateAsync(user, accountRegister.Password);
        if (!result.Succeeded) throw new ValidationFailedException(result.Errors);
    }

    public async Task<AccountLogInResponse> LogInAsync(AccountLogIn accountLogIn)
    {
        var result = await signInManager.PasswordSignInAsync(accountLogIn.Email, accountLogIn.Password, false, false);
        if (!result.Succeeded) throw new LogInFailedException();
        return new AccountLogInResponse((await userManager.FindByEmailAsync(accountLogIn.Email))?.Id);
    }

    public async Task LogOutAsync()
    {
        await signInManager.SignOutAsync();
    }
}
