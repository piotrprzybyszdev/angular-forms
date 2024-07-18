using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using MediatR;

namespace FormsBackendBusiness.Tasks.Commands.DeleteUserTasks;

public class DeleteUserTasksCommandHandler(IGenericRepository<TaskModel> taskRepository)
    : IRequestHandler<DeleteUserTasksCommand, DeleteUserTasksCommandResult>
{
    public async Task<DeleteUserTasksCommandResult> Handle(DeleteUserTasksCommand request, CancellationToken cancellationToken)
    {
        var tasks = await taskRepository.GetFilteredAsync(
            [task => task.User.Id == request.UserId]
        );

        foreach (var task in tasks)
            await taskRepository.DeleteAsync(task);

        return new DeleteUserTasksCommandResult();
    }
}
