using AutoMapper;
using KLTN.Common.Infrastructure;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Users;

namespace KLTN.DataModels.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, AuthenticationViewModel>();
            CreateMap<User, AdminViewModel>()
                .ForMember(vm => vm.Role, m => m.MapFrom(u => (EnumRole)u.Role));
            CreateMap<User, EmployeeViewModel>();
            CreateMap<User, CustomerViewModel>();
        }
    }
}
