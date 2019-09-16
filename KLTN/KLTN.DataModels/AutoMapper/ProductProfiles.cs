using AutoMapper;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.AutoMapper
{
    public class ProductProfiles : Profile
    {
        public ProductProfiles()
        {
            CreateMap<Product, ProductViewModel>();
        }
    }
}
