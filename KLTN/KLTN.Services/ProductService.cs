using AutoMapper;
using KLTN.Common;
using KLTN.Common.Datatables;
using KLTN.Common.Infrastructure;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Products;
using KLTN.Services.Repositories;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

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
                              join lap in _unitOfWork.LaptopRepository.ObjectContext on pro.Id equals lap.ProductId
                              where pro.CategoryId == (int)EnumCategory.Laptop && pro.Id == id
                              select new LaptopViewModel
                              {
                                  Id = pro.Id,
                                  Name = pro.Name,
                                  ProductCode = pro.ProductCode,
                                  Category = Enum.GetName(typeof(EnumCategory), pro.CategoryId),
                                  Brand = bra.Name,
                                  InitialPrice = pro.InitialPrice,
                                  CurrentPrice = pro.CurrentPrice,
                                  Screen = lap.Screen,
                                  OperatingSystem = lap.OperatingSystem,
                                  Camera = lap.Camera,
                                  CPU = lap.CPU,
                                  RAM = lap.RAM,
                                  ROM = lap.ROM,
                                  Card = lap.Card,
                                  Design = lap.Design,
                                  Size = lap.Size,
                                  PortSupport = lap.PortSupport,
                                  Pin = lap.Pin,
                                  Color = lap.Color,
                                  Weight = lap.Weight,
                                  CreateAt = pro.CreateAt,
                                  CreateBy = usc.Email,
                                  UpdateAt = pro.UpdateAt,
                                  UpdateBy = usu.Email,
                                  ViewCount = pro.ViewCount,
                                  LikeCount = pro.LikeCount,
                                  TotalSold = pro.TotalSold,
                                  Status = pro.Status,
                                  Images = (from img in _unitOfWork.ImageRepository.ObjectContext
                                            where img.ProductId == pro.Id
                                            select img).ToList(),
                                  ImageDefault = (from img in _unitOfWork.ImageRepository.ObjectContext
                                                  where img.ProductId == pro.Id
                                                  orderby img.Order
                                                  select img.Url
                                                  ).FirstOrDefault()
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
                              join lap in _unitOfWork.LaptopRepository.ObjectContext on pro.Id equals lap.ProductId
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
                                  Screen = lap.Screen,
                                  OperatingSystem = lap.OperatingSystem,
                                  Camera = lap.Camera,
                                  CPU = lap.CPU,
                                  RAM = lap.RAM,
                                  ROM = lap.ROM,
                                  Card = lap.Card,
                                  Design = lap.Design,
                                  Size = lap.Size,
                                  PortSupport = lap.PortSupport,
                                  Pin = lap.Pin,
                                  Color = lap.Color,
                                  Weight = lap.Weight,
                                  CreateAt = pro.CreateAt,
                                  CreateBy = usc.Email,
                                  UpdateAt = pro.UpdateAt,
                                  UpdateBy = usu.Email,
                                  ViewCount = pro.ViewCount,
                                  LikeCount = pro.LikeCount,
                                  TotalSold = pro.TotalSold,
                                  Status = pro.Status,
                                  Images = (from img in _unitOfWork.ImageRepository.ObjectContext
                                            where img.ProductId == pro.Id
                                            select img).ToList(),
                                  ImageDefault = (from img in _unitOfWork.ImageRepository.ObjectContext
                                                  where img.ProductId == pro.Id
                                                  orderby img.Order
                                                  select img.Url
                                                  ).FirstOrDefault()
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

        /// <summary>
        /// Get a product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tuple<IEnumerable<LaptopViewModel>, int> GetLaptopTopView()
        {
            try
            {
                var laptop = (from pro in _unitOfWork.ProductRepository.ObjectContext
                              join usc in _unitOfWork.UserRepository.ObjectContext on pro.CreateBy equals usc.Id
                              join usu in _unitOfWork.UserRepository.ObjectContext on pro.UpdateBy equals usu.Id
                              join bra in _unitOfWork.BrandRepository.ObjectContext on pro.BrandId equals bra.Id
                              join lap in _unitOfWork.LaptopRepository.ObjectContext on pro.Id equals lap.ProductId
                              where pro.CategoryId == (int)EnumCategory.Laptop
                              orderby pro.ViewCount descending
                              select new LaptopViewModel
                              {
                                  Id = pro.Id,
                                  Name = pro.Name,
                                  ProductCode = pro.ProductCode,
                                  Category = Enum.GetName(typeof(EnumCategory), pro.CategoryId),
                                  Brand = bra.Name,
                                  InitialPrice = pro.InitialPrice,
                                  CurrentPrice = pro.CurrentPrice,
                                  Screen = lap.Screen,
                                  OperatingSystem = lap.OperatingSystem,
                                  Camera = lap.Camera,
                                  CPU = lap.CPU,
                                  RAM = lap.RAM,
                                  ROM = lap.ROM,
                                  Card = lap.Card,
                                  Design = lap.Design,
                                  Size = lap.Size,
                                  PortSupport = lap.PortSupport,
                                  Pin = lap.Pin,
                                  Color = lap.Color,
                                  Weight = lap.Weight,
                                  CreateAt = pro.CreateAt,
                                  CreateBy = usc.Email,
                                  UpdateAt = pro.UpdateAt,
                                  UpdateBy = usu.Email,
                                  ViewCount = pro.ViewCount,
                                  LikeCount = pro.LikeCount,
                                  TotalSold = pro.TotalSold,
                                  Status = pro.Status,
                                  Images = (from img in _unitOfWork.ImageRepository.ObjectContext
                                            where img.ProductId == pro.Id
                                            select img).ToList(),
                                  ImageDefault = (from img in _unitOfWork.ImageRepository.ObjectContext
                                                  where img.ProductId == pro.Id
                                                  orderby img.Order
                                                  select img.Url
                                                  ).FirstOrDefault()
                              }).Take(8);
                if (laptop == null) return new Tuple<IEnumerable<LaptopViewModel>, int>(null, 0);
                return new Tuple<IEnumerable<LaptopViewModel>,int>(laptop, 1);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when get brand by id in brand service", e);
                return new Tuple<IEnumerable<LaptopViewModel>, int>(null, -1);
            }
        }

        /// <summary>
        /// Get a product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tuple<IEnumerable<LaptopViewModel>, int> GetLaptopTopLike()
        {
            try
            {
                var laptop = (from pro in _unitOfWork.ProductRepository.ObjectContext
                              join usc in _unitOfWork.UserRepository.ObjectContext on pro.CreateBy equals usc.Id
                              join usu in _unitOfWork.UserRepository.ObjectContext on pro.UpdateBy equals usu.Id
                              join bra in _unitOfWork.BrandRepository.ObjectContext on pro.BrandId equals bra.Id
                              join lap in _unitOfWork.LaptopRepository.ObjectContext on pro.Id equals lap.ProductId
                              where pro.CategoryId == (int)EnumCategory.Laptop
                              orderby pro.LikeCount descending
                              select new LaptopViewModel
                              {
                                  Id = pro.Id,
                                  Name = pro.Name,
                                  ProductCode = pro.ProductCode,
                                  Category = Enum.GetName(typeof(EnumCategory), pro.CategoryId),
                                  Brand = bra.Name,
                                  InitialPrice = pro.InitialPrice,
                                  CurrentPrice = pro.CurrentPrice,
                                  Screen = lap.Screen,
                                  OperatingSystem = lap.OperatingSystem,
                                  Camera = lap.Camera,
                                  CPU = lap.CPU,
                                  RAM = lap.RAM,
                                  ROM = lap.ROM,
                                  Card = lap.Card,
                                  Design = lap.Design,
                                  Size = lap.Size,
                                  PortSupport = lap.PortSupport,
                                  Pin = lap.Pin,
                                  Color = lap.Color,
                                  Weight = lap.Weight,
                                  CreateAt = pro.CreateAt,
                                  CreateBy = usc.Email,
                                  UpdateAt = pro.UpdateAt,
                                  UpdateBy = usu.Email,
                                  ViewCount = pro.ViewCount,
                                  LikeCount = pro.LikeCount,
                                  TotalSold = pro.TotalSold,
                                  Status = pro.Status,
                                  Images = (from img in _unitOfWork.ImageRepository.ObjectContext
                                            where img.ProductId == pro.Id
                                            select img).ToList(),
                                  ImageDefault = (from img in _unitOfWork.ImageRepository.ObjectContext
                                                  where img.ProductId == pro.Id
                                                  orderby img.Order
                                                  select img.Url
                                                  ).FirstOrDefault()
                              }).Take(8);
                if (laptop == null) return new Tuple<IEnumerable<LaptopViewModel>, int>(null, 0);
                return new Tuple<IEnumerable<LaptopViewModel>, int>(laptop, 1);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when get brand by id in brand service", e);
                return new Tuple<IEnumerable<LaptopViewModel>, int>(null, -1);
            }
        }

        /// <summary>
        /// Get a product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tuple<IEnumerable<LaptopViewModel>, int> GetLaptopTopSold()
        {
            try
            {
                var laptop = (from pro in _unitOfWork.ProductRepository.ObjectContext
                              join usc in _unitOfWork.UserRepository.ObjectContext on pro.CreateBy equals usc.Id
                              join usu in _unitOfWork.UserRepository.ObjectContext on pro.UpdateBy equals usu.Id
                              join bra in _unitOfWork.BrandRepository.ObjectContext on pro.BrandId equals bra.Id
                              join lap in _unitOfWork.LaptopRepository.ObjectContext on pro.Id equals lap.ProductId
                              where pro.CategoryId == (int)EnumCategory.Laptop
                              orderby pro.TotalSold descending
                              select new LaptopViewModel
                              {
                                  Id = pro.Id,
                                  Name = pro.Name,
                                  ProductCode = pro.ProductCode,
                                  Category = Enum.GetName(typeof(EnumCategory), pro.CategoryId),
                                  Brand = bra.Name,
                                  InitialPrice = pro.InitialPrice,
                                  CurrentPrice = pro.CurrentPrice,
                                  Screen = lap.Screen,
                                  OperatingSystem = lap.OperatingSystem,
                                  Camera = lap.Camera,
                                  CPU = lap.CPU,
                                  RAM = lap.RAM,
                                  ROM = lap.ROM,
                                  Card = lap.Card,
                                  Design = lap.Design,
                                  Size = lap.Size,
                                  PortSupport = lap.PortSupport,
                                  Pin = lap.Pin,
                                  Color = lap.Color,
                                  Weight = lap.Weight,
                                  CreateAt = pro.CreateAt,
                                  CreateBy = usc.Email,
                                  UpdateAt = pro.UpdateAt,
                                  UpdateBy = usu.Email,
                                  ViewCount = pro.ViewCount,
                                  LikeCount = pro.LikeCount,
                                  TotalSold = pro.TotalSold,
                                  Status = pro.Status,
                                  Images = (from img in _unitOfWork.ImageRepository.ObjectContext
                                            where img.ProductId == pro.Id
                                            select img).ToList(),
                                  ImageDefault = (from img in _unitOfWork.ImageRepository.ObjectContext
                                                  where img.ProductId == pro.Id
                                                  orderby img.Order
                                                  select img.Url
                                                  ).FirstOrDefault()
                              }).Take(8);
                if (laptop == null) return new Tuple<IEnumerable<LaptopViewModel>, int>(null, 0);
                return new Tuple<IEnumerable<LaptopViewModel>, int>(laptop, 1);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when get brand by id in brand service", e);
                return new Tuple<IEnumerable<LaptopViewModel>, int>(null, -1);
            }
        }

        public int CreateLaptop(CreateLaptopViewModel laptopModel)
        {
            try
            {
                if (CheckNameExisted(laptopModel.Name)) return 2;
                if (CheckProductCodeExisted(laptopModel.ProductCode)) return 3;
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
                    Amount = laptopModel.Amount,
                    Status = true,
                    CreateAt = DateTime.UtcNow,
                    CreateBy = GetClaimUserId(),
                    UpdateAt = DateTime.UtcNow,
                    UpdateBy = GetClaimUserId()
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
            catch (Exception e)
            {
                Log.Error("Have an error when create laptop in service", e);
                return -1;
            }
        }

        public int UpdateLaptop(UpdateLaptopViewModel laptopModel)
        {
            try
            {
                var product = _unitOfWork.ProductRepository.GetById(laptopModel.Id);
                var laptop = _unitOfWork.LaptopRepository.Get(x => x.ProductId == laptopModel.Id);

                product.ProductCode = laptopModel.ProductCode;
                product.Name = laptopModel.Name;
                product.BrandId = laptopModel.BrandId;
                product.InitialPrice = laptopModel.InitialPrice;
                product.CurrentPrice = laptopModel.CurrentPrice;
                product.PromotionPrice = laptopModel.PromotionPrice;
                product.DurationWarranty = laptopModel.DurationWarranty;
                product.MetaTitle = laptopModel.MetaTitle;
                product.Description = laptopModel.Description;
                product.UpdateAt = DateTime.UtcNow;
                product.UpdateBy = GetClaimUserId();

                laptop.Screen = laptopModel.Screen;
                laptop.OperatingSystem = laptopModel.OperatingSystem;
                laptop.Camera = laptopModel.Camera;
                laptop.CPU = laptopModel.CPU;
                laptop.RAM = laptopModel.RAM;
                laptop.ROM = laptopModel.ROM;
                laptop.Card = laptopModel.Card;
                laptop.Design = laptopModel.Design;
                laptop.Size = laptopModel.Size;
                laptop.PortSupport = laptopModel.PortSupport;
                laptop.Pin = laptopModel.Pin;
                laptop.Color = laptopModel.Color;
                laptop.Weight = laptopModel.Weight;

                _unitOfWork.Save();
                return 1;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when update laptop in service", e);
                return -1;
            }
        }

        /// <summary>
        /// Get a product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tuple<MobileViewModel, int> GetMobileById(int? id)
        {
            try
            {
                var mobile = (from pro in _unitOfWork.ProductRepository.ObjectContext
                              join usc in _unitOfWork.UserRepository.ObjectContext on pro.CreateBy equals usc.Id
                              join usu in _unitOfWork.UserRepository.ObjectContext on pro.UpdateBy equals usu.Id
                              join bra in _unitOfWork.BrandRepository.ObjectContext on pro.BrandId equals bra.Id
                              join mobi in _unitOfWork.MobileRepository.ObjectContext on pro.Id equals mobi.ProductId
                              where pro.CategoryId == (int)EnumCategory.Mobile && pro.Id == id
                              select new MobileViewModel
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
                                  Status = pro.Status,
                                  Screen = mobi.Screen,
                                  OperatingSystem = mobi.OperatingSystem,
                                  RearCamera = mobi.RearCamera,
                                  FrontCamera = mobi.FrontCamera,
                                  CPU = mobi.CPU,
                                  RAM = mobi.RAM,
                                  ROM = mobi.ROM,
                                  SIM = mobi.SIM,
                                  Pin = mobi.Pin,
                                  Color = mobi.Color
                              }).FirstOrDefault();
                if (mobile == null) return new Tuple<MobileViewModel, int>(null, 0);
                return new Tuple<MobileViewModel, int>(mobile, 1);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when get brand by id in brand service", e);
                return new Tuple<MobileViewModel, int>(null, -1);
            }
        }

        /// <summary>
        /// Get all list categories
        /// </summary>
        /// <returns></returns>
        public Tuple<IEnumerable<MobileViewModel>, int, int> LoadMobile(DTParameters dtParameters)
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

            var listMobile = (from pro in _unitOfWork.ProductRepository.ObjectContext
                              join usc in _unitOfWork.UserRepository.ObjectContext on pro.CreateBy equals usc.Id
                              join usu in _unitOfWork.UserRepository.ObjectContext on pro.UpdateBy equals usu.Id
                              join bra in _unitOfWork.BrandRepository.ObjectContext on pro.BrandId equals bra.Id
                              join mobi in _unitOfWork.MobileRepository.ObjectContext on pro.Id equals mobi.ProductId
                              where pro.CategoryId == (int)EnumCategory.Mobile
                              select new MobileViewModel
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
                                  Status = pro.Status,
                                  Screen = mobi.Screen,
                                  OperatingSystem = mobi.OperatingSystem,
                                  RearCamera = mobi.RearCamera,
                                  FrontCamera = mobi.FrontCamera,
                                  CPU = mobi.CPU,
                                  RAM = mobi.RAM,
                                  ROM = mobi.ROM,
                                  SIM = mobi.SIM,
                                  Pin = mobi.Pin,
                                  Color = mobi.Color
                              });

            if (!string.IsNullOrEmpty(searchBy))
            {
                listMobile = listMobile.Where(r =>
                        r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Name.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Brand.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Status.ToString().ToUpper().Equals(searchBy.ToUpper()));
            }

            listMobile = orderAscendingDirection
               ? listMobile.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
               : listMobile.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var viewModels = listMobile.OrderBy(x => x.Id).ToArray();
            var filteredResultsCount = viewModels.ToArray().Length;
            var totalResultsCount = viewModels.Count();

            var tuple = new Tuple<IEnumerable<MobileViewModel>, int, int>(viewModels, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        public int CreateMobile(CreateMoblieViewModel mobileModel)
        {
            try
            {
                if (CheckNameExisted(mobileModel.Name)) return 2;
                if (CheckProductCodeExisted(mobileModel.ProductCode)) return 3;
                var product = new Product
                {
                    ProductCode = mobileModel.ProductCode,
                    Name = mobileModel.Name,
                    CategoryId = (int)EnumCategory.Laptop,
                    BrandId = mobileModel.BrandId,
                    InitialPrice = mobileModel.InitialPrice,
                    CurrentPrice = mobileModel.CurrentPrice,
                    PromotionPrice = mobileModel.PromotionPrice,
                    DurationWarranty = mobileModel.DurationWarranty,
                    MetaTitle = mobileModel.MetaTitle,
                    Description = mobileModel.Description,
                    Rate = 0,
                    ViewCount = 0,
                    LikeCount = 0,
                    TotalSold = 0,
                    Amount = mobileModel.Amount,
                    Status = true
                };

                var productCreate = _unitOfWork.ProductRepository.Create(product);

                var mobile = new Mobile
                {
                    ProductId = productCreate.Id,
                    Screen = mobileModel.Screen,
                    OperatingSystem = mobileModel.OperatingSystem,
                    RearCamera = mobileModel.RearCamera,
                    FrontCamera = mobileModel.FrontCamera,
                    CPU = mobileModel.CPU,
                    RAM = mobileModel.RAM,
                    ROM = mobileModel.ROM,
                    SIM = mobileModel.SIM,
                    Pin = mobileModel.Pin,
                    Color = mobileModel.Color
                };

                _unitOfWork.MobileRepository.Create(mobile);
                _unitOfWork.Save();
                return 1;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when create mobile in service", e);
                return -1;
            }
        }

        public int UpdateMobile(UpdateMoblieViewModel mobileModel)
        {
            try
            {
                var product = _unitOfWork.ProductRepository.GetById(mobileModel.Id);
                var mobile = _unitOfWork.MobileRepository.Get(x => x.ProductId == mobileModel.Id);

                product.ProductCode = mobileModel.ProductCode;
                product.Name = mobileModel.Name;
                product.BrandId = mobileModel.BrandId;
                product.InitialPrice = mobileModel.InitialPrice;
                product.CurrentPrice = mobileModel.CurrentPrice;
                product.PromotionPrice = mobileModel.PromotionPrice;
                product.DurationWarranty = mobileModel.DurationWarranty;
                product.MetaTitle = mobileModel.MetaTitle;
                product.Description = mobileModel.Description;

                mobile.Screen = mobileModel.Screen;
                mobile.OperatingSystem = mobileModel.OperatingSystem;
                mobile.RearCamera = mobileModel.RearCamera;
                mobile.FrontCamera = mobileModel.FrontCamera;
                mobile.CPU = mobileModel.CPU;
                mobile.RAM = mobileModel.RAM;
                mobile.ROM = mobileModel.ROM;
                mobile.SIM = mobileModel.SIM;
                mobile.Pin = mobileModel.Pin;
                mobile.Color = mobileModel.Color;

                _unitOfWork.Save();
                return 1;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when update laptop in service", e);
                return -1;
            }
        }

        public string GetClaimUserMail()
        {
            var claimEmail = _httpContext.User.FindFirst(c => c.Type == "Email").Value;
            return claimEmail;
        }

        public int GetClaimUserId()
        {
            var claimId = Convert.ToInt32(_httpContext.User.Identities);
            return claimId;
        }

        private bool CheckNameExisted(string name)
        {
            var result = _unitOfWork.ProductRepository.Get(x => x.Name == name);
            return result != null ? true : false;
        }

        private bool CheckProductCodeExisted(string code)
        {
            var result = _unitOfWork.ProductRepository.Get(x => x.ProductCode == code);
            return result != null ? true : false;
        }

        /// <summary>
        /// Method ChangeStatus User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ChangeStatus(int id)
        {
            var product = _unitOfWork.ProductRepository.GetById(id);
            product.Status = !product.Status;
            _unitOfWork.Save();
            return product.Status;
        }
    }
}
