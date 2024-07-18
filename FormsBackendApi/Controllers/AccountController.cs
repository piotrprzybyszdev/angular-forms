using FormsBackendBusiness.Users.Commands.AddUser;
using FormsBackendBusiness.Users.Commands.LogInUser;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FormsBackendApi.Controllers;

[Route("/")]
[ApiController]
public class AccountController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] AddUserCommand addUserCommand)
    {
        return Ok(await mediator.Send(addUserCommand));
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] ValidateLogInCommand logInCommand)
    {
        var user = (await mediator.Send(logInCommand)).User;

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
        return Ok(user.Id);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }
}
