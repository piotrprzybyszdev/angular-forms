using FormsBackendBusiness.Users.Commands.AddUser;
using FormsBackendBusiness.Users.Commands.DeleteUser;
using FormsBackendBusiness.Users.Queries.GetUsers;
using FormsBackendBusiness.Users.Commands.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FormsBackendApi.Controllers;

[EnableCors]
[Authorize]
[Route("/user")]
[ApiController]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateUserAsync([FromBody] AddUserCommand addUserCommand)
    {
        return Ok(await mediator.Send(addUserCommand));
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUserAsync(int id)
    {
        return Ok(await mediator.Send(
            new DeleteUserCommand()
            {
                Id = id
            })
        );
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserCommand updateUserCommand)
    {
        return Ok(await mediator.Send(updateUserCommand));
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetUsersAsync()
    {
        return Ok((await mediator.Send(
            new GetUsersQuery()
        )).Users);
    }
}
