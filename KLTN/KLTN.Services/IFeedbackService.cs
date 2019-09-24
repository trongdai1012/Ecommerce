using KLTN.DataModels.Models.Feedbacks;
using System;
using System.Collections.Generic;
using KLTN.DataAccess.Models;

namespace KLTN.Services
{
    public interface IFeedbackService
    {
        bool Rating(int productId, string comment, byte rate);

        Tuple<IEnumerable<FeedbackViewModel>, RateCountFeedback> GetFeedbackByProducId(int id);

        bool LikeProduct(int productId);
    }
}