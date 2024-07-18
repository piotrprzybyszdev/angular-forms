using MediatR;

namespace FormsBackendBusiness.Tasks.Commands.UpdateTask;

public class UpdateTaskCommand : IRequest<UpdateTaskCommandResult>
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public string Description { get; init; } = string.Empty;
    public DateTime DueDate { get; init; }
}
