using KLTN.Common;
using KLTN.Common.Datatables;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.News;
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
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly HttpContext _httpContext;

        public NewsService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _httpContext = contextAccessor.HttpContext;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get all list categories
        /// </summary>
        /// <returns></returns>
        public Tuple<IEnumerable<NewsViewModel>, int, int> LoadNews(DTParameters dtParameters)
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

            var listNews = (from news in _unitOfWork.NewsRepository.ObjectContext
                             join usc in _unitOfWork.UserRepository.ObjectContext on news.CreateBy equals usc.Id
                             join usu in _unitOfWork.UserRepository.ObjectContext on news.UpdateBy equals usu.Id
                             select new NewsViewModel
                             {
                                 Id = news.Id,
                                 Title = news.Title,
                                 Content = news.Content,
                                 CreateAt = news.CreateAt,
                                 CreateBy = usc.Email,
                                 UpdateAt = news.UpdateAt,
                                 UpdateBy = usu.Email,
                                 Status = news.Status
                             });

            if (!string.IsNullOrEmpty(searchBy))
            {
                listNews = listNews.Where(r =>
                        searchBy != null && (r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                                             r.Title.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                                             r.Status.ToString().ToUpper().Equals(searchBy.ToUpper())));
            }

            listNews = orderAscendingDirection
               ? listNews.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
               : listNews.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var viewModels = listNews.OrderBy(x => x.Id).ToArray();
            var filteredResultsCount = viewModels.ToArray().Length;
            var totalResultsCount = viewModels.Count();

            var tuple = new Tuple<IEnumerable<NewsViewModel>, int, int>(viewModels, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        /// <summary>
        /// Get a category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tuple<NewsViewModel, int> GetNewsById(int? id)
        {
            try
            {
                var news = (from newss in _unitOfWork.NewsRepository.ObjectContext
                           join usc in _unitOfWork.UserRepository.ObjectContext on newss.CreateBy equals usc.Id
                           join usu in _unitOfWork.UserRepository.ObjectContext on newss.UpdateBy equals usu.Id
                           where newss.Id == id
                           select new NewsViewModel
                           {
                               Id = newss.Id,
                               Title = newss.Title,
                               Content = newss.Content,
                               CreateAt = newss.CreateAt,
                               CreateBy = usc.Email,
                               UpdateBy = usu.Email,
                               UpdateAt = newss.UpdateAt,
                               Status = newss.Status,
                               Image = newss.Image
                           }).FirstOrDefault();
                if (news == null)
                {
                    return new Tuple<NewsViewModel, int>(null, 0);
                }

                return new Tuple<NewsViewModel, int>(news, 1);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when get news by id in news service", e);
                return new Tuple<NewsViewModel, int>(null, -1);
            }
        }

        /// <summary>
        /// Create category
        /// </summary>
        /// <param name="model"></param>
        public async Task<int> Create(NewsViewModel model, IFormFile image)
        {
            var extImg = Path.GetExtension(image.FileName);
            var imgName = Guid.NewGuid().ToString() + extImg;
            var fileName = Path.Combine(Directory.GetCurrentDirectory(), RedirectConfig.DataImages,
                imgName);

            var checkImage = Path.GetExtension(image.FileName).ToUpper();
            if (checkImage != ".JPEG" && checkImage != ".JPG" && checkImage != ".PNG" && checkImage != ".GIF" && checkImage != ".TIFF" &&
                checkImage != ".PSD" && checkImage != ".PDF" && checkImage != ".EPS" && checkImage != ".AI" && checkImage != ".INDD" && checkImage != ".RAW")
            {
                return 2;
            }

            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            var checkName = CheckTitleExisted(model.Title);
            if (checkName) return 0;
            try
            {
                var news = new News
                {
                    Title = model.Title,
                    Content = model.Content,
                    CreateAt = DateTime.UtcNow,
                    CreateBy = GetUserId(),
                    UpdateAt = DateTime.UtcNow,
                    UpdateBy = GetUserId(),
                    Status = true,
                    Image = imgName
                };
                _unitOfWork.NewsRepository.Create(news);
                _unitOfWork.Save();
                return 1;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when create news in service", e);
                return -1;
            }
        }

        private bool CheckTitleExisted(string title)
        {
            var result = _unitOfWork.NewsRepository.ObjectContext.Any(x => x.Title == title);
            return result;
        }

        public bool Update(NewsViewModel viewModel)
        {
            try
            {
                if (CheckTitleExisted(viewModel.Title, viewModel.Id)) return false;

                var model = _unitOfWork.NewsRepository.GetById(viewModel.Id);
                model.Title = viewModel.Title;
                model.Content = viewModel.Content;

                _unitOfWork.Save();

                return true;
            }
            catch(Exception e)
            {
                Log.Error("Have an error when update News in Service", e);
                return false;
            }
        }

        private bool CheckTitleExisted(string title, int id)
        {
            var result = _unitOfWork.NewsRepository.ObjectContext.Any(x => x.Title == title && x.Id == id);
            return result;
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

        /// <summary>
        /// Method ChangeStatus News
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ChangeStatus(int id)
        {
            var news = _unitOfWork.NewsRepository.GetById(id);
            news.Status = !news.Status;
            _unitOfWork.Save();
            return news.Status;
        }

        public IEnumerable<NewsViewModel> GetAll()
        {
            var listNews = (from news in _unitOfWork.NewsRepository.ObjectContext
                            join usc in _unitOfWork.UserRepository.ObjectContext on news.CreateBy equals usc.Id
                            join usu in _unitOfWork.UserRepository.ObjectContext on news.UpdateBy equals usu.Id
                            select new NewsViewModel
                            {
                                Id = news.Id,
                                Title = news.Title,
                                Content = news.Content,
                                CreateAt = news.CreateAt,
                                CreateBy = usc.Email,
                                UpdateAt = news.UpdateAt,
                                UpdateBy = usu.Email,
                                Status = news.Status,
                                Image = news.Image
                            });

            return listNews;
        }

        public IEnumerable<NewsViewModel> GetSixNews(int id)
        {
            var listNews = (from news in _unitOfWork.NewsRepository.ObjectContext
                            join usc in _unitOfWork.UserRepository.ObjectContext on news.CreateBy equals usc.Id
                            join usu in _unitOfWork.UserRepository.ObjectContext on news.UpdateBy equals usu.Id
                            select new NewsViewModel
                            {
                                Id = news.Id,
                                Title = news.Title,
                                Content = news.Content,
                                CreateAt = news.CreateAt,
                                CreateBy = usc.Email,
                                UpdateAt = news.UpdateAt,
                                UpdateBy = usu.Email,
                                Status = news.Status,
                                Image = news.Image
                            });

            return listNews.Where(x=>x.Id!=id).OrderBy(x=>x.CreateAt).Take(6);
        }
    }
}
