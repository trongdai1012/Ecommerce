using AutoMapper;
using KLTN.Common;
using KLTN.Common.Datatables;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Category;
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
    public class CategoryService : ICategoryService
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
        public CategoryService(IMapper mapper, IHttpContextAccessor httpContext, IUnitOfWork unitOfWork)
        {
            _httpContext = httpContext.HttpContext;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all list categories 
        /// </summary>
        /// <returns></returns>
        public Tuple<IEnumerable<CategoryViewModel>, int, int> LoadCategory(DTParameters dtParameters)
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

            var listCategory = (from cate in _unitOfWork.CategoryRepository.ObjectContext
                                join usc in _unitOfWork.UserRepository.ObjectContext on cate.CreateBy equals usc.Id
                                join usu in _unitOfWork.UserRepository.ObjectContext on cate.UpdateBy equals usu.Id
                                select new CategoryViewModel
                                {
                                    Id = cate.Id,
                                    Name = cate.Name,
                                    ParentCategoryId = cate.ParrentCategoryId.Value,
                                    CreateAt = cate.CreateAt,
                                    CreateBy = usc.Email,
                                    UpdateAt = cate.UpdateAt,
                                    UpdateBy = usu.Email,
                                    Status = cate.Status
                                });

            var categoryViewModels = _mapper.Map<IEnumerable<CategoryViewModel>>(listCategory);

            if (!string.IsNullOrEmpty(searchBy))
            {
                categoryViewModels = categoryViewModels.Where(r =>
                        r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Name.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.ParentCategoryId.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Status.ToString().ToUpper().Equals(searchBy.ToUpper()));
            }

            categoryViewModels = orderAscendingDirection
               ? categoryViewModels.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
               : categoryViewModels.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var viewModels = categoryViewModels.OrderBy(x => x.Id).ToArray();
            var filteredResultsCount = viewModels.ToArray().Length;
            var totalResultsCount = viewModels.Count();

            var tuple = new Tuple<IEnumerable<CategoryViewModel>, int, int>(viewModels, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        /// <summary>
        /// Get a category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CategoryViewModel GetCategoryById(int? id)
        {
            var category = _unitOfWork.CategoryRepository.GetById(id);
            if (category == null)
            {
                return null;
            }

            var categoryViewModel = _mapper.Map<CategoryViewModel>(category);
            return categoryViewModel;
        }

        /// <summary>
        /// Create category
        /// </summary>
        /// <param name="model"></param>
        public int CreateCategory(CreateCategoryModel model)
        {
            var checkName = CheckNameCategoryExisted(model.Name);
            if(checkName) return 0;
            try
            {
                var category = new Category
                {
                    Name = model.Name,
                    ParrentCategoryId = model.ParrentCategoryID,
                    CreateAt = DateTime.UtcNow,
                    CreateBy = GetUserId(),
                    Status = true

                };
                _unitOfWork.CategoryRepository.Create(category);
                _unitOfWork.CategoryRepository.Save();
                return 1;
            }catch(Exception e)
            {
                Log.Error("Have an error when create category in service",e);
                return -1;
            }
        }

        private bool CheckNameCategoryExisted(string name)
        {
            var result = _unitOfWork.CategoryRepository.ObjectContext.Any(x => x.Name == name);
            return result;
        }

        /// <summary>
        /// Update category
        /// </summary>
        /// <param name="model"></param>
        public int UpdateCategory(CategoryViewModel model)
        {
            try
            {
                var category = _unitOfWork.CategoryRepository.GetById(model.Id);
                if (category == null) return 0;
                category.Name = model.Name;
                category.ParrentCategoryId = model.ParentCategoryId;
                category.UpdateAt = DateTime.Now;
                category.UpdateBy = GetUserId();
                _unitOfWork.CategoryRepository.Save();
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
            _unitOfWork.CategoryRepository.Delete(id);
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
