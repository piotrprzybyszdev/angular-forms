using Dapper;
using FormsBackendBusiness.Exceptions;
using FormsBackendInfrastructure;
using MediatR;

namespace FormsBackendBusiness.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(ApplicationDbContext dbContext,
    UpdateUserRequestValidator updateUserRequestValidator)
    : IRequestHandler<UpdateUserCommand, UpdateUserCommandResult>
{
    public async Task<UpdateUserCommandResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await updateUserRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid) throw new ValidationFailedException(validationResult.Errors);

        await UpdateUser(request.Id, request.FirstName, request.LastName, request.Email);

        return new UpdateUserCommandResult();
    }

    public async Task UpdateUser(int Id, string FirstName, string LastName, string Email)
    {
        var sql = @"
UPDATE Users SET
FirstName=@FirstName, LastName=@LastName, Email=@Email
WHERE Id=@Id";

        int affectedRows = await dbContext.Connection.ExecuteAsync(sql, new { Id, FirstName, LastName, Email });
        if (affectedRows == 0)
            throw new UserNotFoundException(Id);
    }
}
