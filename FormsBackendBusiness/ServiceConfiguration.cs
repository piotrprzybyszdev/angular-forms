using FormsBackendBusiness.Services;
using FormsBackendBusiness.Validation;
using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using FormsBackendInfrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace FormsBackendBusiness;

public static class ServiceConfiguration
{
    public static void Configure(IServiceCollection services)
    {
        services.AddScoped<IGenericRepository<UserModel>, GenericRepository<UserModel>>();
        services.AddScoped<IGenericRepository<TaskModel>, GenericRepository<TaskModel>>();

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
