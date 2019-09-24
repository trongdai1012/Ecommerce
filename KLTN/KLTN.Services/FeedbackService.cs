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
                var listFeedback = (from fb in _unitOfWork.FeedbackRepository.ObjectContext
                                    join user in _unitOfWork.UserRepository.ObjectContext on fb.UserId equals user.Id
                                    where fb.ProductId == id
                                    select new FeedbackViewModel
                                    {
                                        Id = fb.Id,
                                        Comment = fb.Comment,
                                        CustomerId = fb.Id,
                                        CustomerName = user.FirstName + " " + user.LastName,
                                        IsBought = fb.IsBought,
                                        IsLike = fb.IsLike,
                                        ProductId = fb.ProductId,
                                        Rate = fb.Rate
                                    });
                var rateCount = new RateCountFeedback
                {
                    OneStar = listFeedback.Where(x => x.Rate == 1).Count(),
                    TwoStar = listFeedback.Where(x => x.Rate == 2).Count(),
                    ThreeStar = listFeedback.Where(x => x.Rate == 3).Count(),
                    FourStar = listFeedback.Where(x => x.Rate == 4).Count(),
                    FiveStar = listFeedback.Where(x => x.Rate == 5).Count(),
                    Id = id
                };
                return new Tuple<IEnumerable<FeedbackViewModel>, RateCountFeedback>(listFeedback,rateCount);
            }
            catch (Exception e)
            {
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
                        Comment = comment
                    };
                    _unitOfWork.FeedbackRepository.Create(newFeedback);
                    _unitOfWork.Save();
                    return true;
                }
                feedback.Comment = comment;
                feedback.Rate = rate;
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
                _unitOfWork.FeedbackRepository.Create(newFeedback);
                _unitOfWork.Save();
                return true;
            }

            feedback.IsLike = !feedback.IsLike;
            _unitOfWork.Save();
            return feedback.IsLike;
        }

        private int GetUserId()
        {
            return Convert.ToInt32(_httpContext.User.FindFirst(x => x.Type == "Id").Value);
        }
    }
}
