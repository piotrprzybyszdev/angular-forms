using FormsBackendCommon.Dtos.Account;
using FormsBackendCommon.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        var user = await accountService.LogInAsync(accountLogIn);

        var claims = new List<Claim>()
        {
            new(ClaimTypes.Name, user.Email),
            new("Email", user.Email),
            new("FirstName", user.FirstName),
            new("LastName", user.LastName),
            new(ClaimTypes.Role, "User")
        };

        var claimsIdentity = new ClaimsIdentity(claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties());
        return Ok(new AccountLogInResponse(user.Id));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        await accountService.LogOutAsync();
        await HttpContext.SignOutAsync();
        return Ok();
    }
}
