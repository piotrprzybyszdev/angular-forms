using FormsBackendCommon.Dtos.Task;
using FormsBackendCommon.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormsBackendApi.Controllers;

[Authorize]
[Route("/task")]
[ApiController]
public class TaskController(ITaskService taskService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateTaskAsync([FromBody] TaskCreate taskCreate)
    {
        return Ok(await taskService.CreateTaskAsync(taskCreate));
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteTaskAsync(int id)
    {
        await taskService.DeleteTaskAsync(id);
        return Ok();
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateTaskAsync([FromBody] TaskUpdate taskUpdate)
    {
        await taskService.UpdateTaskAsync(taskUpdate);
        return Ok();
    }

    [HttpGet("get/{guid}")]
    public async Task<IActionResult> GetUserTasksAsync(string guid)
    {
        return Ok(await taskService.GetTasksByUserIdAsync(guid));
    }
}
