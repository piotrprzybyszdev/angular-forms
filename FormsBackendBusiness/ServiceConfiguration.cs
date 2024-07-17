using FormsBackendBusiness.Services;
using FormsBackendBusiness.Validation;
using FormsBackendCommon.Interface;
using FormsBackendInfrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace FormsBackendBusiness;

public static class ServiceConfiguration
{
    public static void Configure(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();

        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITaskService, TaskService>();

        services.AddScoped<AccountRegisterValidator>();
        services.AddScoped<UserCreateValidator>();
        services.AddScoped<UserUpdateValidator>();
        services.AddScoped<TaskCreateValidator>();
        services.AddScoped<TaskUpdateValidator>();
    }
}
