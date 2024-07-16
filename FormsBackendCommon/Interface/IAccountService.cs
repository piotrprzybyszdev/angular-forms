using FormsBackendCommon.Dtos.Account;

namespace FormsBackendCommon.Interface;

public interface IAccountService
{
    Task RegisterAsync(AccountRegister accountRegister);
    Task<AccountLogInResponse> LogInAsync(AccountLogIn accountLogIn);
    Task LogOutAsync();
}
