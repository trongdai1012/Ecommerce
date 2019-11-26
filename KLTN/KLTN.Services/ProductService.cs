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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        public Tuple<ProductViewModel, int> GetProductById(int? id)
        {
            try
            {
                var product = _unitOfWork.ProductRepository.GetById(id);
                if (product == null) return new Tuple<ProductViewModel, int>(null, 0);
                var productModel = _mapper.Map<ProductViewModel>(product);
                productModel.Image = _unitOfWork.ImageRepository.Get(x => x.ProductId == productModel.Id).Url;
                product.ViewCount += 1;
                _unitOfWork.Save();
                return new Tuple<ProductViewModel, int>(productModel, 1);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when get product by id in product service", e);
                return new Tuple<ProductViewModel, int>(null, -1);
            }
        }

        public IEnumerable<LaptopViewModel> GetAllLaptop(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                var listAllLap = (from pro in _unitOfWork.ProductRepository.ObjectContext
                                  join usc in _unitOfWork.UserRepository.ObjectContext on pro.CreateBy equals usc.Id
                                  join usu in _unitOfWork.UserRepository.ObjectContext on pro.UpdateBy equals usu.Id
                                  join bra in _unitOfWork.BrandRepository.ObjectContext on pro.BrandId equals bra.Id
                                  join lap in _unitOfWork.LaptopRepository.ObjectContext on pro.Id equals lap.ProductId
                                  where pro.CategoryId == (int)EnumCategory.Laptop && pro.Status
                                  select new LaptopViewModel
                                  {
                                      Id = pro.Id,
                                      Name = pro.Name,
                                      Category = Enum.GetName(typeof(EnumCategory), pro.CategoryId),
                                      Brand = bra.Name,
                                      InitialPrice = pro.InitialPrice,
                                      CurrentPrice = pro.CurrentPrice,
                                      Description = pro.Description,
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
                                                      select img.Url
                                          ).FirstOrDefault()
                                  }).ToList();

                return listAllLap;
            }

            var listLaptop = (from pro in _unitOfWork.ProductRepository.ObjectContext
                              join usc in _unitOfWork.UserRepository.ObjectContext on pro.CreateBy equals usc.Id
                              join usu in _unitOfWork.UserRepository.ObjectContext on pro.UpdateBy equals usu.Id
                              join bra in _unitOfWork.BrandRepository.ObjectContext on pro.BrandId equals bra.Id
                              join lap in _unitOfWork.LaptopRepository.ObjectContext on pro.Id equals lap.ProductId
                              where pro.CategoryId == (int)EnumCategory.Laptop && pro.Name == key
                              select new LaptopViewModel
                              {
                                  Id = pro.Id,
                                  Name = pro.Name,
                                  Category = Enum.GetName(typeof(EnumCategory), pro.CategoryId),
                                  Brand = bra.Name,
                                  InitialPrice = pro.InitialPrice,
                                  CurrentPrice = pro.CurrentPrice,
                                  Description = pro.Description,
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
                                                  select img.Url
                                      ).FirstOrDefault()
                              }).ToList();

            return listLaptop;
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
                                  Category = Enum.GetName(typeof(EnumCategory), pro.CategoryId),
                                  Brand = bra.Name,
                                  Description = pro.Description,
                                  InitialPrice = pro.InitialPrice,
                                  CurrentPrice = pro.CurrentPrice,
                                  Rate = pro.Rate,
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
                                                  select img.Url
                                      ).FirstOrDefault(),
                                  Amount = pro.Quantity
                              }).FirstOrDefault();
                if (laptop == null) return new Tuple<LaptopViewModel, int>(null, 0);
                var product = _unitOfWork.ProductRepository.GetById(id);
                product.ViewCount += 1;
                _unitOfWork.Save();
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
                                  Category = Enum.GetName(typeof(EnumCategory), pro.CategoryId),
                                  Brand = bra.Name,
                                  InitialPrice = pro.InitialPrice,
                                  CurrentPrice = pro.CurrentPrice,
                                  Description = pro.Description,
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

            var filteredResultsCount = listLaptop.Count();
            var totalResultsCount = listLaptop.Count();

            var tuple = new Tuple<IEnumerable<LaptopViewModel>, int, int>(listLaptop, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        /// <summary>
        /// Get a product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private IEnumerable<LaptopViewModel> GetLaptopTopView()
        {
            try
            {
                var laptop = (from pro in _unitOfWork.ProductRepository.ObjectContext
                              join usc in _unitOfWork.UserRepository.ObjectContext on pro.CreateBy equals usc.Id
                              join usu in _unitOfWork.UserRepository.ObjectContext on pro.UpdateBy equals usu.Id
                              join bra in _unitOfWork.BrandRepository.ObjectContext on pro.BrandId equals bra.Id
                              join lap in _unitOfWork.LaptopRepository.ObjectContext on pro.Id equals lap.ProductId
                              where pro.CategoryId == (int)EnumCategory.Laptop && pro.Status
                              orderby pro.ViewCount descending
                              select new LaptopViewModel
                              {
                                  Id = pro.Id,
                                  Name = pro.Name,
                                  Category = Enum.GetName(typeof(EnumCategory), pro.CategoryId),
                                  Brand = bra.Name,
                                  InitialPrice = pro.InitialPrice,
                                  CurrentPrice = pro.CurrentPrice,
                                  Rate = pro.Rate,
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
                                                  select img.Url
                                      ).FirstOrDefault()
                              }).Take(8);
                return laptop;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when get brand by id in brand service", e);
                return null;
            }
        }

        private bool GetLike(int productId)
        {
            var feedback =
                _unitOfWork.FeedbackRepository.Get(x => x.ProductId == productId && x.UserId == GetClaimUserId());
            return feedback != null && feedback.IsLike;
        }

        /// <summary>
        /// Get a product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private IEnumerable<LaptopViewModel> GetLaptopTopLike()
        {
            try
            {
                var laptop = (from pro in _unitOfWork.ProductRepository.ObjectContext
                              join usc in _unitOfWork.UserRepository.ObjectContext on pro.CreateBy equals usc.Id
                              join usu in _unitOfWork.UserRepository.ObjectContext on pro.UpdateBy equals usu.Id
                              join bra in _unitOfWork.BrandRepository.ObjectContext on pro.BrandId equals bra.Id
                              join lap in _unitOfWork.LaptopRepository.ObjectContext on pro.Id equals lap.ProductId
                              where pro.CategoryId == (int)EnumCategory.Laptop && pro.Status
                              orderby pro.LikeCount descending
                              select new LaptopViewModel
                              {
                                  Id = pro.Id,
                                  Name = pro.Name,
                                  Category = Enum.GetName(typeof(EnumCategory), pro.CategoryId),
                                  Brand = bra.Name,
                                  InitialPrice = pro.InitialPrice,
                                  CurrentPrice = pro.CurrentPrice,
                                  Rate = pro.Rate,
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
                                                  select img.Url
                                      ).FirstOrDefault()
                              }).Take(8);
                return laptop;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when get brand by id in brand service", e);
                return null;
            }
        }

        /// <summary>
        /// Get a product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private IEnumerable<LaptopViewModel> GetLaptopTopSold()
        {
            try
            {
                var laptop = (from pro in _unitOfWork.ProductRepository.ObjectContext
                              join usc in _unitOfWork.UserRepository.ObjectContext on pro.CreateBy equals usc.Id
                              join usu in _unitOfWork.UserRepository.ObjectContext on pro.UpdateBy equals usu.Id
                              join bra in _unitOfWork.BrandRepository.ObjectContext on pro.BrandId equals bra.Id
                              join lap in _unitOfWork.LaptopRepository.ObjectContext on pro.Id equals lap.ProductId
                              where pro.CategoryId == (int)EnumCategory.Laptop && pro.Status
                              orderby pro.TotalSold descending
                              select new LaptopViewModel
                              {
                                  Id = pro.Id,
                                  Name = pro.Name,
                                  Category = Enum.GetName(typeof(EnumCategory), pro.CategoryId),
                                  Brand = bra.Name,
                                  InitialPrice = pro.InitialPrice,
                                  CurrentPrice = pro.CurrentPrice,
                                  Screen = lap.Screen,
                                  Rate = pro.Rate,
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
                                                  select img.Url
                                      ).FirstOrDefault()
                              }).Take(8);
                return laptop;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when get brand by id in brand service", e);
                return null;
            }
        }

        public Tuple<IEnumerable<LaptopViewModel>, IEnumerable<LaptopViewModel>, IEnumerable<LaptopViewModel>, int>
            GetProductRecommender()
        {
            try
            {
                var topView = GetLaptopTopView();
                var topLike = GetLaptopTopLike();
                var topSold = GetLaptopTopSold();
                return new Tuple<IEnumerable<LaptopViewModel>, IEnumerable<LaptopViewModel>,
                    IEnumerable<LaptopViewModel>, int>(topView, topLike, topSold, 1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<int> UpdateLaptop(UpdateLaptopViewModel laptopModel, IFormFile imageFileMajor, List<IFormFile> imageFile)
        {
            try
            {
                if (imageFileMajor != null)
                {
                    var extImg = Path.GetExtension(imageFileMajor.FileName);
                    var imgName = Guid.NewGuid().ToString() + extImg;
                    var fileName = Path.Combine(Directory.GetCurrentDirectory(), RedirectConfig.DataImages,
                        imgName);

                    var checkImage = Path.GetExtension(imageFileMajor.FileName).ToUpper();
                    if (checkImage != ".JPEG" && checkImage != ".JPG" && checkImage != ".PNG" && checkImage != ".GIF" && checkImage != ".TIFF" &&
                        checkImage != ".PSD" && checkImage != ".PDF" && checkImage != ".EPS" && checkImage != ".AI" && checkImage != ".INDD" && checkImage != ".RAW")
                    {
                        return 0;
                    }

                    using (var stream = new FileStream(fileName, FileMode.Create))
                    {
                        await imageFileMajor.CopyToAsync(stream);
                    }


                    if (CheckNameOtherExisted(laptopModel.Name, laptopModel.Id)) return 2;

                    var productCode = NonUnicode(laptopModel.Name);

                    var product = _unitOfWork.ProductRepository.GetById(laptopModel.Id);
                    var laptop = _unitOfWork.LaptopRepository.Get(x => x.ProductId == laptopModel.Id);
                    var img = _unitOfWork.ImageRepository.Get(x => x.ProductId == laptopModel.Id);

                    var imgDel = img.Url;
                    
                    product.Name = laptopModel.Name;
                    product.CategoryId = (int)EnumCategory.Laptop;
                    product.BrandId = laptopModel.BrandId;
                    product.InitialPrice = laptopModel.InitialPrice;
                    product.CurrentPrice = laptopModel.CurrentPrice;
                    product.DurationWarranty = laptopModel.DurationWarranty;
                    product.Description = laptopModel.Description;
                    product.Quantity = laptopModel.Amount;
                    product.UpdateAt = DateTime.UtcNow;
                    product.UpdateBy = GetClaimUserId();

                    img.Url = imgName;

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

                    var imgOld = Path.Combine(Directory.GetCurrentDirectory(), RedirectConfig.DataImages,
                        img.Url);
                    File.Delete(imgOld);

                    return 1;
                }

                if (CheckNameOtherExisted(laptopModel.Name,laptopModel.Id)) return 2;

                var productCode1 = NonUnicode(laptopModel.Name);

                var product1 = _unitOfWork.ProductRepository.GetById(laptopModel.Id);
                var laptop1 = _unitOfWork.LaptopRepository.Get(x => x.ProductId == laptopModel.Id);
                
                product1.Name = laptopModel.Name;
                product1.CategoryId = (int)EnumCategory.Laptop;
                product1.BrandId = laptopModel.BrandId;
                product1.InitialPrice = laptopModel.InitialPrice;
                product1.CurrentPrice = laptopModel.CurrentPrice;
                product1.DurationWarranty = laptopModel.DurationWarranty;
                product1.Description = laptopModel.Description;
                product1.Quantity = laptopModel.Amount;
                product1.UpdateAt = DateTime.UtcNow;
                product1.UpdateBy = GetClaimUserId();

                laptop1.Screen = laptopModel.Screen;
                laptop1.OperatingSystem = laptopModel.OperatingSystem;
                laptop1.Camera = laptopModel.Camera;
                laptop1.CPU = laptopModel.CPU;
                laptop1.RAM = laptopModel.RAM;
                laptop1.ROM = laptopModel.ROM;
                laptop1.Card = laptopModel.Card;
                laptop1.Design = laptopModel.Design;
                laptop1.Size = laptopModel.Size;
                laptop1.PortSupport = laptopModel.PortSupport;
                laptop1.Pin = laptopModel.Pin;
                laptop1.Color = laptopModel.Color;
                laptop1.Weight = laptopModel.Weight;

                _unitOfWork.Save();

                return 1;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when create laptop in service", e);
                return -1;
            }
        }

        public string NonUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
            "đ",
            "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
            "í","ì","ỉ","ĩ","ị",
            "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
            "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
            "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
            "d",
            "e","e","e","e","e","e","e","e","e","e","e",
            "i","i","i","i","i",
            "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
            "u","u","u","u","u","u","u","u","u","u","u",
            "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }

        public async Task<int> CreateLaptop(CreateLaptopViewModel laptopModel, IFormFile imageFileMajor, List<IFormFile> imageFile)
        {
            try
            {
                var extImg = Path.GetExtension(imageFileMajor.FileName);
                var imgName = Guid.NewGuid().ToString() + extImg;
                var fileName = Path.Combine(Directory.GetCurrentDirectory(), RedirectConfig.DataImages,
                    imgName);

                var checkImage = Path.GetExtension(imageFileMajor.FileName).ToUpper();
                if (checkImage != ".JPEG" && checkImage != ".JPG" && checkImage != ".PNG" && checkImage != ".GIF" && checkImage != ".TIFF" &&
                    checkImage != ".PSD" && checkImage != ".PDF" && checkImage != ".EPS" && checkImage != ".AI" && checkImage != ".INDD" && checkImage != ".RAW")
                {
                    return 0;
                }

                using (var stream = new FileStream(fileName, FileMode.Create))
                {
                    await imageFileMajor.CopyToAsync(stream);
                }

                var listImg = new List<string>();

                foreach (var item in imageFile)
                {
                    var extImg1 = Path.GetExtension(item.FileName);
                    var imgName1 = Guid.NewGuid().ToString() + extImg;
                    var fileName1 = Path.Combine(Directory.GetCurrentDirectory(), RedirectConfig.DataImages,
                        imgName1);

                    var checkImage1 = Path.GetExtension(item.FileName).ToUpper();
                    if (checkImage == ".JPEG" || checkImage == ".JPG" || checkImage == ".PNG" || checkImage == ".GIF" || checkImage == ".TIFF" ||
                        checkImage == ".PSD" || checkImage == ".PDF" || checkImage == ".EPS" || checkImage == ".AI" || checkImage == ".INDD" || checkImage == ".RAW")
                    {
                        using (var stream = File.Create(fileName1))
                        {
                            await item.CopyToAsync(stream);
                        }

                        listImg.Add(imgName1);
                    }
                }

                if (CheckNameExisted(laptopModel.Name)) return 2;

                var productCode = NonUnicode(laptopModel.Name);

                var product = new Product
                {
                    Name = laptopModel.Name,
                    CategoryId = (int)EnumCategory.Laptop,
                    BrandId = laptopModel.BrandId,
                    InitialPrice = laptopModel.InitialPrice,
                    CurrentPrice = laptopModel.CurrentPrice,
                    DurationWarranty = laptopModel.DurationWarranty,
                    Description = laptopModel.Description,
                    Rate = 0,
                    ViewCount = 0,
                    LikeCount = 0,
                    TotalSold = 0,
                    Quantity = laptopModel.Amount,
                    Status = true,
                    CreateAt = DateTime.UtcNow,
                    CreateBy = GetClaimUserId(),
                    UpdateAt = DateTime.UtcNow,
                    UpdateBy = GetClaimUserId()
                };

                var productCreate = _unitOfWork.ProductRepository.Create(product);

                var img = new Image
                {
                    ProductId = productCreate.Id,
                    Url = imgName,
                    IsDefault = true
                };

                var image = _unitOfWork.ImageRepository.Create(img);

                foreach (var item in listImg)
                {
                    var img1 = new Image
                    {
                        ProductId = productCreate.Id,
                        Url = item,
                        IsDefault = false
                    };

                    _unitOfWork.ImageRepository.Create(img1);
                }

                _unitOfWork.Save();

                var newLaptop = new Laptop
                {
                    ProductId = image.ProductId,
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

                 _unitOfWork.LaptopRepository.Create(newLaptop);

                 _unitOfWork.Save();

                return 1;
            }
            catch (Exception e)
             {
                Log.Error("Have an error when create laptop in service", e);
                return -1;
            }
        }

        public UpdateLaptopViewModel GetLaptopUpdateById(int id)
        {
            var laptopModel = new UpdateLaptopViewModel();
            var product = _unitOfWork.ProductRepository.GetById(id);
            var laptop = _unitOfWork.LaptopRepository.Get(x => x.ProductId == id);

            laptopModel.Name = product.Name;
            laptopModel.BrandId = product.BrandId;
            laptopModel.InitialPrice = product.InitialPrice;
            laptopModel.CurrentPrice = product.CurrentPrice;
            laptopModel.DurationWarranty = product.DurationWarranty;
            laptopModel.Description = product.Description;
            laptopModel.Amount = product.Quantity;

            laptopModel.Screen = laptop.Screen;
            laptopModel.OperatingSystem = laptop.OperatingSystem;
            laptopModel.Camera = laptop.Camera;
            laptopModel.CPU = laptop.CPU;
            laptopModel.RAM = laptop.RAM;
            laptopModel.ROM = laptop.ROM;
            laptopModel.Card = laptop.Card;
            laptopModel.Design = laptop.Design;
            laptopModel.Size = laptop.Size;
            laptopModel.PortSupport = laptop.PortSupport;
            laptopModel.Pin = laptop.Pin;
            laptopModel.Color = laptop.Color;
            laptopModel.Weight = laptop.Weight;

            return laptopModel;
        }

        //public int UpdateLaptop(UpdateLaptopViewModel laptopModel)
        //{


        //    try
        //    {
        //        var product = _unitOfWork.ProductRepository.GetById(laptopModel.Id);
        //        var laptop = _unitOfWork.LaptopRepository.Get(x => x.ProductId == laptopModel.Id);

        //        product.ProductCode = laptopModel.ProductCode;
        //        product.Name = laptopModel.Name;
        //        product.BrandId = laptopModel.BrandId;
        //        product.InitialPrice = laptopModel.InitialPrice;
        //        product.CurrentPrice = laptopModel.CurrentPrice;
        //        product.PromotionPrice = laptopModel.PromotionPrice;
        //        product.DurationWarranty = laptopModel.DurationWarranty;
        //        product.MetaTitle = laptopModel.MetaTitle;
        //        product.Description = laptopModel.Description;
        //        product.UpdateAt = DateTime.UtcNow;
        //        product.UpdateBy = GetClaimUserId();

        //        laptop.Screen = laptopModel.Screen;
        //        laptop.OperatingSystem = laptopModel.OperatingSystem;
        //        laptop.Camera = laptopModel.Camera;
        //        laptop.CPU = laptopModel.CPU;
        //        laptop.RAM = laptopModel.RAM;
        //        laptop.ROM = laptopModel.ROM;
        //        laptop.Card = laptopModel.Card;
        //        laptop.Design = laptopModel.Design;
        //        laptop.Size = laptopModel.Size;
        //        laptop.PortSupport = laptopModel.PortSupport;
        //        laptop.Pin = laptopModel.Pin;
        //        laptop.Color = laptopModel.Color;
        //        laptop.Weight = laptopModel.Weight;

        //        _unitOfWork.Save();
        //        return 1;
        //    }
        //    catch (Exception e)
        //    {
        //        Log.Error("Have an error when update laptop in service", e);
        //        return -1;
        //    }
        //}

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
                return mobile == null
                    ? new Tuple<MobileViewModel, int>(null, 0)
                    : new Tuple<MobileViewModel, int>(mobile, 1);
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
                var product = new Product
                {
                    Name = mobileModel.Name,
                    CategoryId = (int)EnumCategory.Laptop,
                    BrandId = mobileModel.BrandId,
                    InitialPrice = mobileModel.InitialPrice,
                    CurrentPrice = mobileModel.CurrentPrice,
                    DurationWarranty = mobileModel.DurationWarranty,
                    Description = mobileModel.Description,
                    Rate = 0,
                    ViewCount = 0,
                    LikeCount = 0,
                    TotalSold = 0,
                    Quantity = mobileModel.Amount,
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
                
                product.Name = mobileModel.Name;
                product.BrandId = mobileModel.BrandId;
                product.InitialPrice = mobileModel.InitialPrice;
                product.CurrentPrice = mobileModel.CurrentPrice;
                product.DurationWarranty = mobileModel.DurationWarranty;
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

        //public UpdateLaptopViewModel

        /// <summary>
        /// check file exist
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string CheckFile(IFormFile path)
        {
            var extImg = Path.GetExtension(path.FileName);
            var imgName = Guid.NewGuid().ToString() + extImg;
            var fileName = Path.Combine(Directory.GetCurrentDirectory(), RedirectConfig.DataImages,
                imgName);
            if (Directory.Exists(fileName)) return fileName;
            if (!File.Exists(fileName))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }
            else
            {
                fileName += Guid.NewGuid().ToString().Substring(0, 6);
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                return fileName;
            }

            return imgName;
        }

        public string GetClaimUserMail()
        {
            var claimEmail = _httpContext.User.FindFirst(c => c.Type == "Email").Value;
            return claimEmail;
        }

        public int GetClaimUserId()
        {
            var claimId = Convert.ToInt32(_httpContext.User.FindFirst(x => x.Type == "Id").Value);
            return claimId;
        }

        private bool CheckNameExisted(string name)
        {
            var result = _unitOfWork.ProductRepository.Get(x => x.Name == name);
            return result != null ? true : false;
        }

        private bool CheckNameOtherExisted(string name, int id)
        {
            var result = _unitOfWork.ProductRepository.Get(x => x.Name == name && x.Id != id);
            return result != null ? true : false;
        }

        /// <summary>
        /// Method ChangeStatus Product
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