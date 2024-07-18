using AutoMapper;
using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using MediatR;

namespace FormsBackendBusiness.Tasks.Queries.GetUserTasks;

public class GetUserTasksQueryHandler(IMapper mapper,
    IGenericRepository<TaskModel> taskRepository)
    : IRequestHandler<GetUserTasksQuery, GetUserTasksQueryResult>
{
    public async Task<GetUserTasksQueryResult> Handle(GetUserTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = mapper.Map<List<TaskGet>>(await taskRepository.GetFilteredAsync(
            [task => task.User.Id == request.UserId]
        ));

        return new GetUserTasksQueryResult() { Tasks = tasks };
    }
}
