﻿using KLTN.Common.Datatables;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Products;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KLTN.Services
{
    public interface IProductService
    {
        IEnumerable<LaptopViewModel> GetAllLaptop(string key, int brandId);

        IEnumerable<MobileViewModel> GetAllMobile(string key);

        Tuple<ProductViewModel, int> GetProductById(int? id);

        Tuple<IEnumerable<LaptopViewModel>, int, int> LoadLaptop(DTParameters dtParameters);

        Tuple<IEnumerable<MobileViewModel>, int, int> LoadMobile(DTParameters dtParameters);

        Tuple<LaptopViewModel, int> GetLaptopById(int? id);

        Tuple<MobileViewModel, int> GetMobileById(int? id);

        Tuple<IEnumerable<LaptopViewModel>, IEnumerable<LaptopViewModel>, IEnumerable<LaptopViewModel>, int>
            GetProductRecommender();

        Task<int> CreateLaptop(CreateLaptopViewModel laptopModel, IFormFile imageFileMajor, List<IFormFile> imageFile);

        Task<int> CreateMobile(CreateMoblieViewModel mobileModel, IFormFile imageFileMajor, List<IFormFile> imageFile);

        UpdateLaptopViewModel GetLaptopUpdateById(int id);

        Task<int> UpdateLaptop(UpdateLaptopViewModel laptopModel, IFormFile imageFileMajor, List<IFormFile> imageFile);

        bool ChangeStatus(int id);

        IEnumerable<ProductViewModel> GetBanner();
    }
}