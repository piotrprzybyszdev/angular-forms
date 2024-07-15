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
        CreateMap<AccountRegister, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        CreateMap<UserCreate, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UserUpdate, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        CreateMap<ApplicationUser, UserGet>();
        CreateMap<TaskCreate, TaskModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<TaskUpdate, TaskModel>();
        CreateMap<TaskModel, TaskGet>();
    }
}
