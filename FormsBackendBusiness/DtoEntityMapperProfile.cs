using AutoMapper;
using FormsBackendCommon.Dtos.Account;
using FormsBackendCommon.Dtos.Task;
using FormsBackendCommon.Dtos.User;
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
            cfg.CreateMap<AccountRegister, UserModel>();
            cfg.CreateMap<UserCreate, UserModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            cfg.CreateMap<UserUpdate, UserModel>();
            cfg.CreateMap<UserModel, UserGet>();
            cfg.CreateMap<TaskCreate, TaskModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            cfg.CreateMap<TaskUpdate, TaskModel>();
            cfg.CreateMap<TaskModel, TaskGet>(); 
        });
    }
}
