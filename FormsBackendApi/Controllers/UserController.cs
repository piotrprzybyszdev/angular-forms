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

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUserAsync(int id)
    {
        await userService.DeleteUserAsync(id);
        return Ok();
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UserUpdate userUpdate)
    {
        await userService.UpdateUserAsync(userUpdate);
        return Ok();
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetUserAsync(int id)
    {
        return Ok(await userService.GetUserById(id));
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetUsersAsync()
    {
        return Ok(await userService.GetUsersAsync());
    }
}
