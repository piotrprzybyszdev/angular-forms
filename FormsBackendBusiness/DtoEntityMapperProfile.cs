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
            cfg.CreateMap<AccountRegister, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            cfg.CreateMap<UserCreate, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            cfg.CreateMap<UserUpdate, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            cfg.CreateMap<ApplicationUser, UserGet>();
            cfg.CreateMap<TaskCreate, TaskModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            cfg.CreateMap<TaskUpdate, TaskModel>();
            cfg.CreateMap<TaskModel, TaskGet>(); 
        });
    }
}
