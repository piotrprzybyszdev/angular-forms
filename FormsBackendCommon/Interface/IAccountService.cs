using FormsBackendCommon.Dtos.Account;
using FormsBackendCommon.Model;

namespace FormsBackendCommon.Interface;

public interface IAccountService
{
    Task RegisterAsync(AccountRegister accountRegister);
    Task<UserModel> LogInAsync(AccountLogIn accountLogIn);
    Task LogOutAsync();
}
