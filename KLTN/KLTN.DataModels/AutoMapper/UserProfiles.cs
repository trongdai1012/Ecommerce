using AutoMapper;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Users;

namespace KLTN.DataModels.AutoMapper
{
    public class UserProfiles : Profile
    {
        public UserProfiles()
        {
            CreateMap<AuthenticationViewModel, User>();
            CreateMap<AdminViewModel, User>();
            CreateMap<EmployeeViewModel, User>();
            CreateMap<CustomerViewModel, User>();
            CreateMap<EditUserViewModel, User>();
        }
    }
}
