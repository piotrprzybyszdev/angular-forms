using AutoMapper;
using FormsBackendBusiness.Exceptions;
using FormsBackendBusiness.Users.Queries.GetUsers;
using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using MediatR;

namespace FormsBackendBusiness.Users.Commands.LogInUser;

public class ValidateLogInHandler(IGenericRepository<UserModel> userRepository, IMapper mapper)
    : IRequestHandler<ValidateLogInCommand, ValidateLogInResult>
{
    public async Task<ValidateLogInResult> Handle(ValidateLogInCommand request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAsync();
        var user = users.Find(user => user.Email == request.Email)
            ?? throw new LogInFailedException();

        var hash = BCrypt.Net.BCrypt.HashPassword(request.Password, user.PasswordSalt);

        if (BCrypt.Net.BCrypt.Verify(user.HashedPassword, hash))
            throw new LogInFailedException();

        return new ValidateLogInResult() { User = mapper.Map<UserGet>(user) };
    }
}
