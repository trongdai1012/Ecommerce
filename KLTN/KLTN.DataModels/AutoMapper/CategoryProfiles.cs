using AutoMapper;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Brands;

namespace KLTN.DataModels.AutoMapper
{
    public class BrandProfiles : Profile
    {
        public BrandProfiles()
        {
            CreateMap<Brand, BrandViewModel>();
            CreateMap<Brand, CreateBrandModel>();
        }
    }
}
