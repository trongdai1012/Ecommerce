using AutoMapper;
using KLTN.Common;
using KLTN.Common.Datatables;
using KLTN.Common.Infrastructure;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Brands;
using KLTN.DataModels.Models.Products;
using KLTN.Services.Repositories;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KLTN.Services
{
    public class ProductService : IProductService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        private readonly HttpContext _httpContext;
        /// <summary>
        /// 
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor category service
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="httpContext"></param>
        /// <param name="genericRepository"></param>
        /// <param name="genericRepositoryUser"></param>
        public ProductService(IMapper mapper, IHttpContextAccessor httpContext, IUnitOfWork unitOfWork)
        {
            _httpContext = httpContext.HttpContext;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tuple<LaptopViewModel, int> GetLaptopById(int? id)
        {
            try
            {
                var laptop = (from pro in _unitOfWork.ProductRepository.ObjectContext
                                  join usc in _unitOfWork.UserRepository.ObjectContext on pro.CreateBy equals usc.Id
                                  join usu in _unitOfWork.UserRepository.ObjectContext on pro.UpdateBy equals usu.Id
                                  join bra in _unitOfWork.BrandRepository.ObjectContext on pro.BrandId equals bra.Id
                                  where pro.CategoryId == (int)EnumCategory.Laptop && pro.Id ==  id
                                  select new LaptopViewModel
                                  {
                                      Id = pro.Id,
                                      Name = pro.Name,
                                      ProductCode = pro.ProductCode,
                                      Category = Enum.GetName(typeof(EnumCategory), pro.CategoryId),
                                      Brand = bra.Name,
                                      InitialPrice = pro.InitialPrice,
                                      CurrentPrice = pro.CurrentPrice,
                                      CreateAt = pro.CreateAt,
                                      CreateBy = usc.Email,
                                      UpdateAt = pro.UpdateAt,
                                      UpdateBy = usu.Email,
                                      Status = pro.Status
                                  }).FirstOrDefault();
                if (laptop == null) return new Tuple<LaptopViewModel, int>(null, 0);
                return new Tuple<LaptopViewModel, int>(laptop, 1);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when get brand by id in brand service", e);
                return new Tuple<LaptopViewModel, int>(null, -1);
            }
        }

        /// <summary>
        /// Get all list categories
        /// </summary>
        /// <returns></returns>
        public Tuple<IEnumerable<LaptopViewModel>, int, int> LoadLaptop(DTParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;
            string orderCriteria;
            bool orderAscendingDirection;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == ParamConstants.Asc;
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = ParamConstants.Id;
                orderAscendingDirection = true;
            }

            var listLaptop = (from pro in _unitOfWork.ProductRepository.ObjectContext
                              join usc in _unitOfWork.UserRepository.ObjectContext on pro.CreateBy equals usc.Id
                              join usu in _unitOfWork.UserRepository.ObjectContext on pro.UpdateBy equals usu.Id
                              join bra in _unitOfWork.BrandRepository.ObjectContext on pro.BrandId equals bra.Id
                              where pro.CategoryId == (int)EnumCategory.Laptop
                              select new LaptopViewModel
                              {
                                  Id = pro.Id,
                                  Name = pro.Name,
                                  ProductCode = pro.ProductCode,
                                  Category = Enum.GetName(typeof(EnumCategory), pro.CategoryId),
                                  Brand = bra.Name,
                                  InitialPrice = pro.InitialPrice,
                                  CurrentPrice = pro.CurrentPrice,
                                  CreateAt = pro.CreateAt,
                                  CreateBy = usc.Email,
                                  UpdateAt = pro.UpdateAt,
                                  UpdateBy = usu.Email,
                                  Status = pro.Status
                              });

            if (!string.IsNullOrEmpty(searchBy))
            {
                listLaptop = listLaptop.Where(r =>
                        r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Name.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Brand.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Status.ToString().ToUpper().Equals(searchBy.ToUpper()));
            }

            listLaptop = orderAscendingDirection
               ? listLaptop.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
               : listLaptop.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var viewModels = listLaptop.OrderBy(x => x.Id).ToArray();
            var filteredResultsCount = viewModels.ToArray().Length;
            var totalResultsCount = viewModels.Count();

            var tuple = new Tuple<IEnumerable<LaptopViewModel>, int, int>(viewModels, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        public int CreateLaptop(CreateLaptopViewModel laptopModel)
        {
            try
            {
                var product = new Product
                {
                    ProductCode = laptopModel.ProductCode,
                    Name = laptopModel.Name,
                    CategoryId = (int)EnumCategory.Laptop,
                    BrandId = laptopModel.BrandId,
                    InitialPrice = laptopModel.InitialPrice,
                    CurrentPrice = laptopModel.CurrentPrice,
                    PromotionPrice = laptopModel.PromotionPrice,
                    DurationWarranty = laptopModel.DurationWarranty,
                    MetaTitle = laptopModel.MetaTitle,
                    Description = laptopModel.Description,
                    Rate = 0,
                    ViewCount = 0,
                    LikeCount = 0,
                    TotalSold = 0,
                    Amount = laptopModel.Amount
                };

                var productCreate = _unitOfWork.ProductRepository.Create(product);

                var laptop = new Laptop
                {
                    ProductId = productCreate.Id,
                    Screen = laptopModel.Screen,
                    OperatingSystem = laptopModel.OperatingSystem,
                    Camera = laptopModel.Camera,
                    CPU = laptopModel.CPU,
                    RAM = laptopModel.RAM,
                    ROM = laptopModel.ROM,
                    Card = laptopModel.Card,
                    Design = laptopModel.Design,
                    Size = laptopModel.Size,
                    PortSupport = laptopModel.PortSupport,
                    Pin = laptopModel.Pin,
                    Color = laptopModel.Color,
                    Weight = laptopModel.Weight
                };

                _unitOfWork.LaptopRepository.Create(laptop);
                _unitOfWork.Save();
                return 1;
            }
            catch(Exception e)
            {
                Log.Error("Have an error when create laptop in service",e);
                return 0;
            }
        }
    }
}
