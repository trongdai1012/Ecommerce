using System;
using System.Collections.Generic;
using System.Text;
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

        public FeedbackService(IUnitOfWork unitOfWork, HttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContext = contextAccessor.HttpContext;
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
