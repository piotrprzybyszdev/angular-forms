using FormsBackendCommon.Dtos.User;
using FormsBackendCommon.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FormsBackendApi.Controllers;

[EnableCors]
[Authorize]
[Route("/user")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserCreate userCreate)
    {
        return Ok(await userService.CreateUserAsync(userCreate));
    }

    [HttpDelete("delete/{guid}")]
    public async Task<IActionResult> DeleteUserAsync(string guid)
    {
        await userService.DeleteUserAsync(guid);
        return Ok();
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UserUpdate userUpdate)
    {
        await userService.UpdateUserAsync(userUpdate);
        return Ok();
    }

    [HttpGet("get/{guid}")]
    public async Task<IActionResult> GetUserAsync(string guid)
    {
        return Ok(await userService.GetUserById(guid));
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetUsersAsync()
    {
        return Ok(await userService.GetUsersAsync());
    }
}
