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

        public Tuple<IEnumerable<FeedbackViewModel>,RateCountFeedback> GetFeedbackByProducId(int id)
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
                                        RatedAt = fb.RatedAt
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
                return new Tuple<IEnumerable<FeedbackViewModel>, RateCountFeedback>(listFeedback,rateCount);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when get feedback by productId", e);
                return null;
            }
        }

        public bool Rating(int productId, string comment, byte rate)
        {
            try
            {
                var feedback = _unitOfWork.FeedbackRepository.Get(x => x.ProductId == productId && x.UserId == GetUserId());
                if (feedback == null)
                {
                    var newFeedback = new Feedback
                    {
                        ProductId = productId,
                        UserId = GetUserId(),
                        Rate = rate,
                        IsBought = true,
                        Comment = comment,
                        RatedAt = DateTime.UtcNow
                    };
                    _unitOfWork.FeedbackRepository.Create(newFeedback);
                    _unitOfWork.Save();
                    return true;
                }
                feedback.Comment = comment;
                feedback.Rate = rate;
                feedback.RatedAt = DateTime.UtcNow;
                _unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when rating", e);
                return false;
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
                    IsLike = true
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
    }
}
