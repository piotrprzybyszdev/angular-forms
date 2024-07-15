using AutoMapper;
using FormsBackendCommon.Dtos.Account;
using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using Microsoft.AspNetCore.Identity;

namespace FormsBackendBusiness.Services;

public class AccountService(UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager, IMapper mapper) : IAccountService
{
    public async Task<object> RegisterAsync(AccountRegister accountRegister)
    {
        var user = mapper.Map<ApplicationUser>(accountRegister);
        return await userManager.CreateAsync(user, accountRegister.Password);
    }

    public async Task<AccountLogInResponse> LogInAsync(AccountLogIn accountLogIn)
    {
        var result = await signInManager.PasswordSignInAsync(accountLogIn.Email, accountLogIn.Password, false, false);
        return new AccountLogInResponse(result.Succeeded ? (await userManager.FindByEmailAsync(accountLogIn.Email))?.Id : null);
    }

    public async Task LogOutAsync()
    {
        await signInManager.SignOutAsync();
    }
}
