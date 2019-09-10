using System;
using System.Collections.Generic;
using System.Text;
using KLTN.Common;
using KLTN.Common.Datatables;
using KLTN.DataModels.Models.Brands;
using KLTN.DataModels.Models.Categories;
using KLTN.Services.Repositories;
using System.Linq;
using AutoMapper;

namespace KLTN.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public CategoryViewModel GetCategoryById(int? id)*/
        {
            var product = (from cate in _unitOfWork.CategoryRepository.ObjectContext
                           join usc in _unitOfWork.UserRepository.ObjectContext on cate.CreateBy equals usc.Id
                           join usu in _unitOfWork.UserRepository.ObjectContext on cate.UpdateBy equals usu.Id
                           where cate.Id == id
                           select new CategoryViewModel
                           {
                               Id = cate.Id,
                               Name = cate.Name,
                               CreateBy = usc.Email,
                               UpdateBy = usu.Email,
                               CreateAt = cate.CreateAt,
                               UpdateAt = cate.UpdateAt,
                               Status = cate.Status
                           }).FirstOrDefault();

            return product;
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

            var listCate = (from cate in _unitOfWork.CategoryRepository.ObjectContext
                              join usc in _unitOfWork.UserRepository.ObjectContext on cate.CreateBy equals usc.Id
                              join usu in _unitOfWork.UserRepository.ObjectContext on cate.UpdateBy equals usu.Id
                              select new CategoryViewModel
                              {
                                  Id = cate.Id,
                                  Name = cate.Name,
                                  CreateAt = cate.CreateAt,
                                  CreateBy = usc.Email,
                                  UpdateAt = cate.UpdateAt,
                                  UpdateBy = usu.Email,
                                  Status = cate.Status
                              });

            if (!string.IsNullOrEmpty(searchBy))
            {
                listCate = listCate.Where(r =>
                        r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Name.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.CreateBy.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.UpdateBy.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Status.ToString().ToUpper().Equals(searchBy.ToUpper()));
            }

            listCate = orderAscendingDirection
               ? listCate.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
               : listCate.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var viewModels = listCate.OrderBy(x => x.Id).ToArray();
            var filteredResultsCount = viewModels.ToArray().Length;
            var totalResultsCount = viewModels.Count();

            var tuple = new Tuple<IEnumerable<CategoryViewModel>, int, int>(viewModels, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }
    }
}
