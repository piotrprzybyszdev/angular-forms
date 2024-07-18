using AutoMapper;
using FormsBackendBusiness.Tasks.Commands.AddTask;
using FormsBackendBusiness.Tasks.Commands.UpdateTask;
using FormsBackendBusiness.Tasks.Queries.GetUserTasks;
using FormsBackendBusiness.Users.Commands.AddUser;
using FormsBackendBusiness.Users.Commands.UpdateUser;
using FormsBackendBusiness.Users.Queries.GetUsers;
using FormsBackendCommon.Model;

namespace FormsBackendBusiness;

public class DtoEntityMapperProfile : Profile
{
    public DtoEntityMapperProfile()
    {
        GetConfiguration().CompileMappings();
    }

    public static MapperConfiguration GetConfiguration()
    {
        return new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AddUserCommand, UserModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            cfg.CreateMap<UpdateUserCommand, UserModel>();
            cfg.CreateMap<UserModel, UserGet>();
            cfg.CreateMap<AddTaskCommand, TaskModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            cfg.CreateMap<UpdateTaskCommand, TaskModel>();
            cfg.CreateMap<TaskModel, TaskGet>(); 
        });
    }
}
