using AutoMapper;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Users;

namespace KLTN.DataModels.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, AuthenticationViewModel>();
            CreateMap<User, AdminViewModel>();
            CreateMap<User, EmployeeViewModel>();
            CreateMap<User, CustomerViewModel>();
            CreateMap<User ,EditUserViewModel>();
        }
    }
}
