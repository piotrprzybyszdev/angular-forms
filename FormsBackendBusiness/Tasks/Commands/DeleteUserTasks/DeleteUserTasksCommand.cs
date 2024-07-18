using MediatR;

namespace FormsBackendBusiness.Tasks.Commands.DeleteUserTasks;

public class DeleteUserTasksCommand : IRequest<DeleteUserTasksCommandResult>
{
    public int UserId { get; init; }
}
