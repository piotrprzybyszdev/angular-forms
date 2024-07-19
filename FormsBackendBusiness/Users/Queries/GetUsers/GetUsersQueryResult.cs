namespace FormsBackendBusiness.Users.Queries.GetUsers;

public class UserGet
{
    public int Id { get; init; }
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Email { get; init; } = default!;
}

public class GetUsersQueryResult
{
    public List<UserGet> Users { get; init; } = default!;
}
