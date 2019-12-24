using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Models.Recommenders
{
    // <SnippetMovieRatingClass>
    public class ProductRating
    {
        //[LoadColumn(1)]
        public float UserId;
        //[LoadColumn(2)]
        public float ProductId;
        //[LoadColumn(3)]
        public float Label;
    }
    // </SnippetMovieRatingClass>

    // <SnippetPredictionClass>
    public class ProductRatingPrediction
    {
        public float Label;
        public float Score;
    }
    // </SnippetPredictionClass>
}
