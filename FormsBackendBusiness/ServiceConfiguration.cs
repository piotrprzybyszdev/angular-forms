using FormsBackendBusiness.Services;
using FormsBackendCommon.Interface;
using FormsBackendInfrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace FormsBackendBusiness;

public static class ServiceConfiguration
{
    public static void Configure(IServiceCollection services)
    {
        services.AddScoped<ITaskRepository, TaskRepository>();

        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITaskService, TaskService>();
    }
}
