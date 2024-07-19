using Dapper;
using FormsBackendInfrastructure;
using MediatR;

namespace FormsBackendBusiness.Tasks.Queries.GetUserTasks;

public class GetUserTasksQueryHandler(ApplicationDbContext dbContext)
    : IRequestHandler<GetUserTasksQuery, GetUserTasksQueryResult>
{
    public async Task<GetUserTasksQueryResult> Handle(GetUserTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = await GetTasks();
        return new GetUserTasksQueryResult() { Tasks = tasks };
    }
    
    public async Task<List<TaskGet>> GetTasks()
    {
        var sql = "SELECT Id, Title, Description, CreationDate, ModificationDate, DueDate FROM Tasks;";
        return (await dbContext.Connection.QueryAsync<TaskGet>(sql)).ToList();
    }
}
