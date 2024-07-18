using AutoMapper;
using FormsBackendBusiness.Exceptions;
using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using MediatR;

namespace FormsBackendBusiness.Users.Commands.AddUser;

public class AddUserCommandHandler(AddUserRequestValidator addUserRequestValidator,
    IGenericRepository<UserModel> userRepository, IMapper mapper)
    : IRequestHandler<AddUserCommand, AddUserCommandResult>
{
    public async Task<AddUserCommandResult> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await addUserRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid) throw new ValidationFailedException(validationResult.Errors);

        if ((await userRepository.GetFilteredAsync([user => user.Email == request.Email])).Count > 0)
            throw new ValidationFailedException([new ValidationError("Email validation failed", "User with this email already exists")]);
        var user = mapper.Map<UserModel>(request);

        user.PasswordSalt = BCrypt.Net.BCrypt.GenerateSalt();
        user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, user.PasswordSalt);

        var id = await userRepository.InsertAsync(user);
        await userRepository.SaveChangesAsync();
        return new AddUserCommandResult() { UserId = id };
    }
}
