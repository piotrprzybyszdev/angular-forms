using FormsBackendCommon.Dtos.Account;

namespace FormsBackendCommon.Interface;

public interface IAccountService
{
    Task<object> RegisterAsync(AccountRegister accountRegister);
    Task<AccountLogInResponse> LogInAsync(AccountLogIn accountLogIn);
    Task LogOutAsync();
}
