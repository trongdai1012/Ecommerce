using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Models.Feedbacks
{
    public class RateCountFeedback
    {
        public int Id { get; set; }
        public int OneStar { get; set; }
        public int TwoStar { get; set; }
        public int ThreeStar { get; set; }
        public int FourStar { get; set; }
        public int FiveStar { get; set; }
    }
}
