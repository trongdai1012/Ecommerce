using KLTN.DataModels.Models.Feedbacks;

namespace KLTN.Services
{
    public interface IFeedbackService
    {
        bool Rating(int productId, string comment, byte rate);
    }
}