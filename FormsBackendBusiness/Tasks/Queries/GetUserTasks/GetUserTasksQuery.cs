using MediatR;

namespace FormsBackendBusiness.Tasks.Queries.GetUserTasks;

public class GetUserTasksQuery : IRequest<GetUserTasksQueryResult>
{
    public int UserId { get; init; }
}
