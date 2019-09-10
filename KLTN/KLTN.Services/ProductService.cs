using AutoMapper;
using KLTN.Common;
using KLTN.Common.Datatables;
using KLTN.Common.Infrastructure;
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
        /// Method GetAll get all category
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LaptopViewModel> GetAllLaptop()
        {
            var listLaptop = _unitOfWork.ProductRepository.GetMany(x=>x.CategoryId==(int)EnumCategory.Laptop);
            var listLaptopModel = _mapper.Map<IEnumerable<LaptopViewModel>>(listLaptop);
            return listLaptopModel;
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
                                 Category = Enum.GetName(typeof(EnumCategory),pro.CategoryId),
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
    }
}
