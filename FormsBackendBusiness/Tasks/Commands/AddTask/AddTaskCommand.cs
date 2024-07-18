using MediatR;

namespace FormsBackendBusiness.Tasks.Commands.AddTask;

public class AddTaskCommand : IRequest<AddTaskCommandResult>
{
    public int UserId { get; init; }
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public DateTime DueDate { get; init; } = default!;
}
