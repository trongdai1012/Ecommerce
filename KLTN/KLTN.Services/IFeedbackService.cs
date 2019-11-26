using KLTN.DataModels.Models.Feedbacks;
using System;
using System.Collections.Generic;
using KLTN.DataAccess.Models;
using KLTN.Common.Datatables;

namespace KLTN.Services
{
    public interface IFeedbackService
    {
        int Rating(int productId, string comment, byte rate);

        Tuple<IEnumerable<FeedbackViewModel>, RateCountFeedback> GetFeedbackByProducId(int id);

        bool LikeProduct(int productId);

        Tuple<IEnumerable<FeedbackViewModel>, int, int> LoadFeedback(DTParameters dtParameters);

        Tuple<FeedbackViewModel, int> GetFeedbackById(int? id);

        bool ChangeStatus(int id);
    }
}