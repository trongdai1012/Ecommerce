using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Feedbacks;
using KLTN.Services.Repositories;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Linq;
using KLTN.Common.Datatables;
using KLTN.Common;

namespace KLTN.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly HttpContext _httpContext;

        private readonly IMapper _mapper;

        public FeedbackService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _httpContext = contextAccessor.HttpContext;
            _mapper = mapper;
        }

        public Tuple<IEnumerable<FeedbackViewModel>,RateCountFeedback, float> GetFeedbackByProducId(int id)
        {
            try
            {
                var listFeedback = from fb in _unitOfWork.FeedbackRepository.ObjectContext
                                   join user in _unitOfWork.UserRepository.ObjectContext on fb.UserId equals user.Id
                                   where fb.ProductId == id
                                   orderby fb.RatedAt descending
                                   select new FeedbackViewModel
                                   {
                                       Id = fb.Id,
                                       Comment = fb.Comment,
                                       CustomerId = fb.Id,
                                       CustomerName = user.FirstName + " " + user.LastName,
                                       IsBought = fb.IsBought,
                                       IsLike = fb.IsLike,
                                       ProductId = fb.ProductId,
                                       Rate = fb.Rate,
                                       RatedAt = fb.RatedAt,
                                       CreateAt = fb.CreateAt
                                    };
                var rateCount = new RateCountFeedback
                {
                    OneStar = listFeedback.Count(x => x.Rate == 1),
                    TwoStar = listFeedback.Count(x => x.Rate == 2),
                    ThreeStar = listFeedback.Count(x => x.Rate == 3),
                    FourStar = listFeedback.Count(x => x.Rate == 4),
                    FiveStar = listFeedback.Count(x => x.Rate == 5),
                    Id = id
                };

                var rateProduct = _unitOfWork.ProductRepository.GetById(id);


                return new Tuple<IEnumerable<FeedbackViewModel>, RateCountFeedback, float>(listFeedback,rateCount, rateProduct.Rate);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when get feedback by productId", e);
                return null;
            }
        }

        public int Rating(int productId, string comment, byte rate)
        {
            try
            {
                var feedback = _unitOfWork.FeedbackRepository.Get(x => x.ProductId == productId && x.UserId == GetUserId());
                if (feedback == null) return 0;
                if (!feedback.IsBought) return 0;
                if (feedback.Status)
                {
                    feedback.Comment = comment;
                    feedback.Rate = rate;
                    feedback.RatedAt = DateTime.UtcNow;
                    feedback.IsRated = true;
                    _unitOfWork.Save();
                    return 1;
                }

                return 2;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when rating", e.Message);
                return -1;
            }
        }

        public bool LikeProduct(int productId)
        {
            var feedback = _unitOfWork.FeedbackRepository.Get(x => x.ProductId == productId && x.UserId == GetUserId());
            if (feedback == null)
            {
                var newFeedback = new Feedback
                {
                    ProductId = productId,
                    UserId = GetUserId(),
                    IsLike = true,
                    Status = true,
                    CreateAt = DateTime.UtcNow
                };

                var product = _unitOfWork.ProductRepository.GetById(productId);
                product.LikeCount += 1;

                _unitOfWork.FeedbackRepository.Create(newFeedback);
                _unitOfWork.Save();
                return true;
            }

            if (!feedback.IsLike)
            {
                var product = _unitOfWork.ProductRepository.GetById(productId);
                product.LikeCount += 1;
            }

            feedback.IsLike = true;
            _unitOfWork.Save();
            return feedback.IsLike;
        }

        private int GetUserId()
        {
            return Convert.ToInt32(_httpContext.User.FindFirst(x => x.Type == "Id").Value);
        }

        /// <summary>
        /// Get all list categories
        /// </summary>
        /// <returns></returns>
        public Tuple<IEnumerable<FeedbackViewModel>, int, int> LoadFeedback(DTParameters dtParameters)
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

            var listFeedback = from feed in _unitOfWork.FeedbackRepository.ObjectContext
                            join usc in _unitOfWork.UserRepository.ObjectContext on feed.UserId equals usc.Id
                            join pro in _unitOfWork.ProductRepository.ObjectContext on feed.ProductId equals pro.Id
                            where feed.IsRated
                            select new FeedbackViewModel
                            {
                                Id = feed.Id,
                                CustomerId = feed.UserId,
                                CustomerName = usc.FirstName + " " + usc.LastName,
                                ProductId = feed.ProductId,
                                ProductName = pro.Name,
                                Comment = feed.Comment,
                                Rate = feed.Rate,
                                RatedAt = feed.RatedAt,
                                Status = feed.Status,
                                CreateAt = feed.CreateAt
                            };

            if (!string.IsNullOrEmpty(searchBy))
            {
                listFeedback = listFeedback.Where(r =>
                    r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.ProductId.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.Comment.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.CustomerName.ToString().ToUpper().Contains(searchBy.ToUpper()));
            }

            listFeedback = orderAscendingDirection
                ? listFeedback.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
                : listFeedback.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var filteredResultsCount = listFeedback.ToArray().Count();
            var totalResultsCount = listFeedback.Count();

            var tuple = new Tuple<IEnumerable<FeedbackViewModel>, int, int>(listFeedback, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        /// <summary>
        /// Get a feedback by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tuple<FeedbackViewModel, int> GetFeedbackById(int? id)
        {
            try
            {
                var feedback = (from feed in _unitOfWork.FeedbackRepository.ObjectContext
                                   join usc in _unitOfWork.UserRepository.ObjectContext on feed.UserId equals usc.Id
                                   join usu in _unitOfWork.UserRepository.ObjectContext on feed.UserId equals usu.Id
                                   where feed.UserId == id.Value
                                   select new FeedbackViewModel
                                   {
                                       Id = feed.Id,
                                       CustomerId = feed.UserId,
                                       CustomerName = usc.FirstName + usc.LastName,
                                       ProductId = feed.ProductId,
                                       Comment = feed.Comment,
                                       Rate = feed.Rate,
                                       RatedAt = feed.RatedAt,
                                       CreateAt = feed.CreateAt
                                   }).FirstOrDefault();

                if (feedback == null)
                {
                    return new Tuple<FeedbackViewModel, int>(null, 0);
                }

                return new Tuple<FeedbackViewModel, int>(feedback, 1);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when get news by id in news service", e);
                return new Tuple<FeedbackViewModel, int>(null, -1);
            }
        }

        /// <summary>
        /// Method ChangeStatus News
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ChangeStatus(int id)
        {
            var feedback = _unitOfWork.NewsRepository.GetById(id);
            feedback.Status = !feedback.Status;
            _unitOfWork.Save();
            return feedback.Status;
        }
    }
}
