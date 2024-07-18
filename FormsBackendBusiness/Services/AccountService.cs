using AutoMapper;
using FormsBackendBusiness.Exceptions;
using FormsBackendBusiness.Validation;
using FormsBackendCommon.Dtos.Account;
using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;

namespace FormsBackendBusiness.Services;

public class AccountService(IGenericRepository<UserModel> userRepository, IMapper mapper,
    AccountRegisterValidator accountRegisterValidator) : IAccountService
{
    public async Task RegisterAsync(AccountRegister accountRegister)
    {
        var validatorResult = await accountRegisterValidator.ValidateAsync(accountRegister);
        if (!validatorResult.IsValid) throw new ValidationFailedException(validatorResult.Errors);

        if ((await userRepository.GetAsync()).Any(user => user.Email == accountRegister.Email))
            throw new ValidationFailedException([new ValidationError("Email validation failed", "User with this email already exists")]);

        var user = mapper.Map<UserModel>(accountRegister);

        user.PasswordSalt = BCrypt.Net.BCrypt.GenerateSalt();
        user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(accountRegister.Password, user.PasswordSalt);

        await userRepository.InsertAsync(user);
        await userRepository.SaveChangesAsync();
    }

    public async Task<UserModel> LogInAsync(AccountLogIn accountLogIn)
    {
        var users = await userRepository.GetAsync();
        var user = users.Find(user => user.Email == accountLogIn.Email)
            ?? throw new LogInFailedException();

        var hash = BCrypt.Net.BCrypt.HashPassword(accountLogIn.Password, user.PasswordSalt);

        if (BCrypt.Net.BCrypt.Verify(user.HashedPassword, hash))
            throw new LogInFailedException();
        
        return user;
    }

    public async Task LogOutAsync()
    {
        await Task.CompletedTask;
    }
}
