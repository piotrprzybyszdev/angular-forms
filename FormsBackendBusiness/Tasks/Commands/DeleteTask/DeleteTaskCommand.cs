using MediatR;

namespace FormsBackendBusiness.Tasks.Commands.DeleteTask;

public class DeleteTaskCommand : IRequest<DeleteTaskCommandResult>
{
    public int Id { get; init; }
}
