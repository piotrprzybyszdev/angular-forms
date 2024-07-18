using FormsBackendBusiness.Exceptions;
using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using MediatR;

namespace FormsBackendBusiness.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(IGenericRepository<UserModel> userRepository,
    UpdateUserRequestValidator updateUserRequestValidator)
    : IRequestHandler<UpdateUserCommand, UpdateUserCommandResult>
{
    public async Task<UpdateUserCommandResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await updateUserRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid) throw new ValidationFailedException(validationResult.Errors);

        var user = await userRepository.GetByIdAsync(request.Id) ??
            throw new UserNotFoundException(request.Id);

        user.Email = request.Email;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        await userRepository.UpdateAsync(user);
        await userRepository.SaveChangesAsync();
        return new UpdateUserCommandResult();
    }
}
