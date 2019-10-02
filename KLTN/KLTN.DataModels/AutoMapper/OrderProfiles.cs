using AutoMapper;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Orders;

namespace KLTN.DataModels.AutoMapper
{
    public class OrderProfiles : Profile
    {
        public OrderProfiles()
        {
            CreateMap<Order, OrderViewModel>();
        }
    }
}
