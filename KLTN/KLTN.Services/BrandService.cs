using AutoMapper;
using KLTN.Common;
using KLTN.Common.Datatables;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Brands;
using KLTN.Services.Repositories;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KLTN.Services
{
    /// <summary>
    /// Class category service
    /// </summary>
    public class BrandService : IBrandService
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
        public BrandService(IMapper mapper, IHttpContextAccessor httpContext, IUnitOfWork unitOfWork)
        {
            _httpContext = httpContext.HttpContext;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Method GetAll get all category
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BrandViewModel> GetAll()
        {
            var listBrand = _unitOfWork.BrandRepository.GetAll();
            var listBrandModel = _mapper.Map<IEnumerable<BrandViewModel>>(listBrand);
            return listBrandModel;
        }

        /// <summary>
        /// Get all list categories
        /// </summary>
        /// <returns></returns>
        public Tuple<IEnumerable<BrandViewModel>, int, int> LoadBrand(DTParameters dtParameters)
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

            var listBrand = (from brand in _unitOfWork.BrandRepository.ObjectContext
                                join usc in _unitOfWork.UserRepository.ObjectContext on brand.CreateBy equals usc.Id
                                join usu in _unitOfWork.UserRepository.ObjectContext on brand.UpdateBy equals usu.Id
                                select new BrandViewModel
                                {
                                    Id = brand.Id,
                                    Name = brand.Name,
                                    Address = brand.Address,
                                    CreateAt = brand.CreateAt,
                                    CreateBy = usc.Email,
                                    UpdateAt = brand.UpdateAt,
                                    UpdateBy = usu.Email,
                                    Status = brand.Status
                                });

            var brandViewModels = _mapper.Map<IEnumerable<BrandViewModel>>(listBrand);

            if (!string.IsNullOrEmpty(searchBy))
            {
                brandViewModels = brandViewModels.Where(r =>
                        r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Name.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Address.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Status.ToString().ToUpper().Equals(searchBy.ToUpper()));
            }

            brandViewModels = orderAscendingDirection
               ? brandViewModels.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
               : brandViewModels.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var viewModels = brandViewModels.OrderBy(x => x.Id).ToArray();
            var filteredResultsCount = viewModels.ToArray().Length;
            var totalResultsCount = viewModels.Count();

            var tuple = new Tuple<IEnumerable<BrandViewModel>, int, int>(viewModels, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        /// <summary>
        /// Get a category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tuple<BrandViewModel,int> GetBrandById(int? id)
        {
            try
            {
                var brand = _unitOfWork.BrandRepository.GetById(id);
                if (brand == null)
                {
                    return new Tuple<BrandViewModel,int>(null,0);
                }

                var brandViewModel = _mapper.Map<BrandViewModel>(brand);
                return new Tuple<BrandViewModel, int>(brandViewModel, 1);
            }
            catch(Exception e)
            {
                Log.Error("Have an error when get brand by id in brand service",e);
                return new Tuple<BrandViewModel, int>(null, -1);
            }
        }

        /// <summary>
        /// Create category
        /// </summary>
        /// <param name="model"></param>
        public int CreateBrand(CreateBrandModel model)
        {
            var checkName = CheckNameBrandExisted(model.Name);
            if (checkName) return 0;
            try
            {
                var brand = new Brand();
                brand.Name = model.Name;
                brand.Address = model.Address;
                brand.CreateAt = DateTime.UtcNow;
                brand.CreateBy = GetUserId();
                brand.UpdateAt = DateTime.UtcNow;
                brand.UpdateBy = GetUserId();
                brand.Status = true;
                _unitOfWork.BrandRepository.Create(brand);
                _unitOfWork.BrandRepository.Save();
                return 1;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when create category in service", e);
                return -1;
            }
        }

        private bool CheckNameBrandExisted(string name)
        {
            var result = _unitOfWork.BrandRepository.ObjectContext.Any(x => x.Name == name);
            return result;
        }

        /// <summary>
        /// Update category
        /// </summary>
        /// <param name="model"></param>
        public int UpdateBrand(BrandViewModel model)
        {
            try
            {
                var brand = _unitOfWork.BrandRepository.GetById(model.Id);
                if (brand == null) return 0;
                brand.Name = model.Name;
                brand.Address = model.Address;
                brand.UpdateAt = DateTime.Now;
                brand.UpdateBy = GetUserId();
                _unitOfWork.BrandRepository.Save();
                return 1;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when update category in categoryService", e);
                return -1;
            }
        }

        /// <summary>
        /// Remove category by id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteCategoryById(int? id)
        {
            _unitOfWork.BrandRepository.Delete(id);
            _unitOfWork.Save();
        }

        /// <summary>
        /// Method GetUserMail return Claim Mail
        /// </summary>
        /// <returns></returns>
        private string GetUserMail()
        {
            return _httpContext.User.FindFirst(x => x.Type == Constants.Email).Value;
        }

        /// <summary>
        /// Method GetUserId return Claim Id
        /// </summary>
        /// <returns></returns>
        private int GetUserId()
        {
            var userId = Convert.ToInt32(_httpContext.User.FindFirst(x => x.Type == Constants.Id).Value);
            return userId;
        }
    }
}
