using AutoMapper;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.AutoMapper
{
    public class CategoryProfiles : Profile
    {
        public CategoryProfiles()
        {
            CreateMap<Category, CategoryViewModel>();
        }
    }
}
