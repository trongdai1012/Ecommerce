using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Feedbacks;
using KLTN.Services.Repositories;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace KLTN.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly HttpContext _httpContext;

        private readonly IMapper _mapper;

        public FeedbackService(IUnitOfWork unitOfWork, HttpContextAccessor contextAccessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _httpContext = contextAccessor.HttpContext;
            _mapper = mapper;
        }

        public IEnumerable<FeedbackViewModel> GetFeedbackByProducId(int id)
        {
            var listFeedback = _unitOfWork.FeedbackRepository.GetMany(x=>x.ProductId==id);
            var listFeedbackModel = _mapper.Map<IEnumerable<FeedbackViewModel>>(listFeedback);

            return listFeedbackModel;
        }

        public bool Rating(int productId, string comment, byte rate)
        {
            try
            {
                var feedback = _unitOfWork.FeedbackRepository.Get(x => x.ProductId == productId && x.CustomerId == GetUserId());
                if (feedback == null)
                {
                    var newFeedback = new Feedback
                    {
                        ProductId = productId,
                        CustomerId = GetUserId(),
                        Rate = rate,
                        IsBought = true,
                        Comment = comment
                    };
                    return true;
                }
                feedback.Comment = comment;
                feedback.Rate = rate;
                return true;
            }catch(Exception e)
            {
                Log.Error("Have an error when rating", e);
                return false;
            }
        }

        private int GetUserId()
        {
            return Convert.ToInt32(_httpContext.User.FindFirst(x => x.Type == "Id").Value);
        }
    }
}
