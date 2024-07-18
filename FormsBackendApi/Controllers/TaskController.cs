using FormsBackendBusiness.Tasks.Commands.AddTask;
using FormsBackendBusiness.Tasks.Commands.DeleteTask;
using FormsBackendBusiness.Tasks.Commands.UpdateTask;
using FormsBackendBusiness.Tasks.Queries.GetUserTasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormsBackendApi.Controllers;

[Authorize]
[Route("/task")]
[ApiController]
public class TaskController(IMediator mediator) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateTaskAsync([FromBody] AddTaskCommand addTaskCommand)
    {
        return Ok(await mediator.Send(addTaskCommand));
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteTaskAsync(int id)
    {
        return Ok(await mediator.Send(new DeleteTaskCommand() { Id = id }));
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateTaskAsync([FromBody] UpdateTaskCommand updateTaskCommand)
    {
        return Ok(await mediator.Send(updateTaskCommand));
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetUserTasksAsync(int id)
    {
        return Ok((await mediator.Send(new GetUserTasksQuery() { UserId = id })).Tasks);
    }
}
