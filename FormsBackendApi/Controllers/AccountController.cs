using FormsBackendCommon.Dtos.Account;
using FormsBackendCommon.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FormsBackendApi.Controllers;

[Route("/")]
[ApiController]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] AccountRegister accountRegister)
    {
        await accountService.RegisterAsync(accountRegister);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] AccountLogIn accountLogIn)
    {
        return Ok(await accountService.LogInAsync(accountLogIn));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        await accountService.LogOutAsync();
        return Ok();
    }
}
