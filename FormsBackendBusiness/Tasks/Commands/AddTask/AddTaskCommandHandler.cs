using AutoMapper;
using FormsBackendBusiness.Exceptions;
using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using MediatR;

namespace FormsBackendBusiness.Tasks.Commands.AddTask;

public class AddTaskCommandHandler(IMapper mapper, IGenericRepository<UserModel> userRepository,
    IGenericRepository<TaskModel> taskRepository, AddTaskRequestValidator addTaskRequestValidator)
    : IRequestHandler<AddTaskCommand, AddTaskCommandResult>
{
    public async Task<AddTaskCommandResult> Handle(AddTaskCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await addTaskRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid) throw new ValidationFailedException(validationResult.Errors);
        var user = await userRepository.GetByIdAsync(request.UserId) ??
            throw new UserNotFoundException(request.UserId);

        var task = mapper.Map<TaskModel>(request);
        task.User = user;
        task.CreationDate = DateTime.Now;
        task.ModificationDate = DateTime.Now;

        await taskRepository.InsertAsync(task);
        await taskRepository.SaveChangesAsync();
        return new AddTaskCommandResult() { Id = task.Id };
    }
}
