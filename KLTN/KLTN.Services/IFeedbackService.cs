using KLTN.DataModels.Models.Feedbacks;
using System.Collections.Generic;

namespace KLTN.Services
{
    public interface IFeedbackService
    {
        bool Rating(int productId, string comment, byte rate);

        IEnumerable<FeedbackViewModel> GetFeedbackByProducId(int id);
    }
}