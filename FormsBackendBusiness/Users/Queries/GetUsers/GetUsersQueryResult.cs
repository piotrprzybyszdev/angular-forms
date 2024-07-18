namespace FormsBackendBusiness.Users.Queries.GetUsers;

public record UserGet(int Id, string FirstName, string LastName, string Email);

public class GetUsersQueryResult
{
    public List<UserGet> Users { get; init; } = default!;
}
