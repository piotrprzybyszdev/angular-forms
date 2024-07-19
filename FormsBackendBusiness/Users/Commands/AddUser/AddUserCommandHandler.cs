using Dapper;
using FormsBackendBusiness.Exceptions;
using FormsBackendInfrastructure;
using MediatR;

namespace FormsBackendBusiness.Users.Commands.AddUser;

public class AddUserCommandHandler(AddUserRequestValidator addUserRequestValidator,
    ApplicationDbContext dbContext) : IRequestHandler<AddUserCommand, AddUserCommandResult>
{
    public async Task<AddUserCommandResult> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await addUserRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid) throw new ValidationFailedException(validationResult.Errors);

        await CheckEmailExists(request.Email);

        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

        var id = await InsertUser(request.FirstName, request.LastName, request.Email, hashedPassword, salt);
        return new AddUserCommandResult() { UserId = id };
    }

    public async Task CheckEmailExists(string Email)
    {
        string sql = @"
SELECT Id FROM Users
WHERE Email = @Email";

        var id = await dbContext.Connection.QuerySingleOrDefaultAsync<int?>(sql, new { Email });
        if (id != null)
            throw new ValidationFailedException([new ValidationError("Email validation failed", "User with this email already exists")]);
    }

    public async Task<int> InsertUser(string FirstName, string LastName, string Email, string HashedPassword, string PasswordSalt)
    {
        string sql = @"
INSERT INTO Users (FirstName, LastName, Email, HashedPassword, PasswordSalt)
VALUES (@FirstName, @LastName, @Email, @HashedPassword, @PasswordSalt);
SELECT last_insert_rowid()";

        return await dbContext.Connection.QuerySingleOrDefaultAsync<int>(sql, new { FirstName, LastName, Email, HashedPassword, PasswordSalt });
    }
}
