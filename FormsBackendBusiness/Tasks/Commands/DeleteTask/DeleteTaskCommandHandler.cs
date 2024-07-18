using FormsBackendBusiness.Exceptions;
using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using MediatR;

namespace FormsBackendBusiness.Tasks.Commands.DeleteTask;

public class DeleteTaskCommandHandler(IGenericRepository<TaskModel> taskRepository)
    : IRequestHandler<DeleteTaskCommand, DeleteTaskCommandResult>
{
    public async Task<DeleteTaskCommandResult> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetByIdAsync(request.Id) ??
            throw new TaskNotFoundException(request.Id);

        await taskRepository.DeleteAsync(task);
        await taskRepository.SaveChangesAsync();

        return new DeleteTaskCommandResult();
    }
}
