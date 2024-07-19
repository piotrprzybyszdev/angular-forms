using Dapper;
using FormsBackendBusiness.Exceptions;
using FormsBackendBusiness.Users.Queries.GetUsers;
using FormsBackendCommon.Model;
using FormsBackendInfrastructure;
using MediatR;

namespace FormsBackendBusiness.Users.Commands.LogInUser;

public class ValidateLogInHandler(ApplicationDbContext dbContext)
    : IRequestHandler<ValidateLogInCommand, ValidateLogInResult>
{
    public async Task<ValidateLogInResult> Handle(ValidateLogInCommand request, CancellationToken cancellationToken)
    {
        var user = await GetUserByEmail(request.Email);

        var hash = BCrypt.Net.BCrypt.HashPassword(request.Password, user.PasswordSalt);

        if (BCrypt.Net.BCrypt.Verify(user.HashedPassword, hash))
            throw new LogInFailedException();

        return new ValidateLogInResult() { User = new UserGet() { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email } };
    }

    public async Task<UserModel> GetUserByEmail(string Email)
    {
        var sql = @"
SELECT Id, FirstName, LastName, Email, HashedPassword, PasswordSalt FROM Users
WHERE Email=@Email";

        return await dbContext.Connection.QuerySingleOrDefaultAsync<UserModel?>(sql, new { Email })
            ?? throw new LogInFailedException();
    }
}
