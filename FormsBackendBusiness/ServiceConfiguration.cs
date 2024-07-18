using FormsBackendBusiness.Tasks.Commands.AddTask;
using FormsBackendBusiness.Tasks.Commands.UpdateTask;
using FormsBackendBusiness.Users.Commands.AddUser;
using FormsBackendBusiness.Users.Commands.UpdateUser;
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

        services.AddScoped<AddUserRequestValidator>();
        services.AddScoped<UpdateUserRequestValidator>();
        services.AddScoped<AddTaskRequestValidator>();
        services.AddScoped<UpdateTaskRequestValidator>();
    }
}
