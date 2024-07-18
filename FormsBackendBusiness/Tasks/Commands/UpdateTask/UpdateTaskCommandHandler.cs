using FormsBackendBusiness.Exceptions;
using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using MediatR;

namespace FormsBackendBusiness.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandHandler(UpdateTaskRequestValidator updateTaskRequestValidator,
    IGenericRepository<TaskModel> taskRepository)
    : IRequestHandler<UpdateTaskCommand, UpdateTaskCommandResult>
{
    public async Task<UpdateTaskCommandResult> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await updateTaskRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid) throw new ValidationFailedException(validationResult.Errors);

        var task = await taskRepository.GetByIdAsync(request.Id) ??
            throw new TaskNotFoundException(request.Id);

        task.Title = request.Title;
        task.Description = request.Description;
        task.DueDate = request.DueDate;
        task.ModificationDate = DateTime.Now;

        await taskRepository.UpdateAsync(task);
        await taskRepository.SaveChangesAsync();

        return new UpdateTaskCommandResult();
    }
}
