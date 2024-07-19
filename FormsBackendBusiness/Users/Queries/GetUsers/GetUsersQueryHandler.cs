using Dapper;
using FormsBackendInfrastructure;
using MediatR;

namespace FormsBackendBusiness.Users.Queries.GetUsers;

public class GetUsersQueryHandler(ApplicationDbContext dbContext)
    : IRequestHandler<GetUsersQuery, GetUsersQueryResult>
{
    public async Task<GetUsersQueryResult> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return new GetUsersQueryResult() { 
            Users = await GetUsers()
        };
    }

    public async Task<List<UserGet>> GetUsers()
    {
        var sql = "SELECT Id, FirstName, LastName, Email FROM Users";
        return (await dbContext.Connection.QueryAsync<UserGet>(sql)).ToList();
    }
}
