using KLTN.DataModels.Models.Feedbacks;
using System;
using System.Collections.Generic;

namespace KLTN.Services
{
    public interface IFeedbackService
    {
        bool Rating(int productId, string comment, byte rate);

        Tuple<IEnumerable<FeedbackViewModel>, RateCountFeedback> GetFeedbackByProducId(int id);
    }
}