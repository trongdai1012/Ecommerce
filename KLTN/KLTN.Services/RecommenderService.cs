using KLTN.DataModels.Models.Recommenders;
using KLTN.Services.Repositories;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using System.Linq;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using KLTN.DataModels.Models.Products;
using KLTN.Common.Infrastructure;
using Microsoft.AspNetCore.Http;
using KLTN.DataAccess.Models;

namespace KLTN.Services
{
    public class RecommenderService : IRecommenderService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly HttpContext _httpContext;

        public RecommenderService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<LaptopViewModel> GetAllLaptop()
        {
            var isAuthen = (_httpContext.User != null) && _httpContext.User.Identity.IsAuthenticated;
            if (isAuthen)
            {
                // Create MLContext to be shared across the model creation workflow objects 
                // <SnippetMLContext>
                MLContext mlContext = new MLContext();
                // </SnippetMLContext>

                // Load data
                // <SnippetLoadDataMain>
                (IDataView trainingDataView, IDataView testDataView) = LoadData(mlContext);
                // </SnippetLoadDataMain>

                // Build & train model
                // <SnippetBuildTrainModelMain>
                ITransformer model = BuildAndTrainModel(mlContext, trainingDataView);
                // </SnippetBuildTrainModelMain>

                // Evaluate quality of model
                // <SnippetEvaluateModelMain>
                EvaluateModel(mlContext, testDataView, model);
                // </SnippetEvaluateModelMain>

                var listRate = _unitOfWork.FeedbackRepository.GetMany(x => x.UserId == GetClaimUserId() && x.Rate>0).ToList();

                var listProduct = _unitOfWork.ProductRepository.GetMany(x => x.CategoryId == (int)EnumCategory.Laptop).ToList();

                var movies = listProduct.SkipWhile(x=>listRate.Contains(GetFeedback(x.Id))).ToList();

                // Use model to try a single prediction (one row of data)
                // <SnippetUseModelMain>
                UseModelForSinglePrediction(mlContext, model, 1, 99);
                // </SnippetUseModelMain>
            }


            var listLaptop = (from pro in _unitOfWork.ProductRepository.ObjectContext
                              join usc in _unitOfWork.UserRepository.ObjectContext on pro.CreateBy equals usc.Id
                              join usu in _unitOfWork.UserRepository.ObjectContext on pro.UpdateBy equals usu.Id
                              join bra in _unitOfWork.BrandRepository.ObjectContext on pro.BrandId equals bra.Id
                              join lap in _unitOfWork.LaptopRepository.ObjectContext on pro.Id equals lap.ProductId
                              where pro.Status
                              select new LaptopViewModel
                              {
                                  Id = pro.Id,
                                  Name = pro.Name,
                                  Category = Enum.GetName(typeof(EnumCategory), pro.CategoryId),
                                  Brand = bra.Name,
                                  InitialPrice = pro.InitialPrice,
                                  CurrentPrice = pro.CurrentPrice,
                                  Description = pro.Description,
                                  Screen = lap.Screen,
                                  OperatingSystem = lap.OperatingSystem,
                                  Camera = lap.Camera,
                                  CPU = lap.CPU,
                                  RAM = lap.RAM,
                                  ROM = lap.ROM,
                                  Card = lap.Card,
                                  Design = lap.Design,
                                  Size = lap.Size,
                                  PortSupport = lap.PortSupport,
                                  Pin = lap.Pin,
                                  Color = lap.Color,
                                  Weight = lap.Weight,
                                  CreateAt = pro.CreateAt,
                                  CreateBy = usc.Email,
                                  UpdateAt = pro.UpdateAt,
                                  UpdateBy = usu.Email,
                                  ViewCount = pro.ViewCount,
                                  LikeCount = pro.LikeCount,
                                  TotalSold = pro.TotalSold,
                                  Status = pro.Status,
                                  Images = (from img in _unitOfWork.ImageRepository.ObjectContext
                                            where img.ProductId == pro.Id
                                            select img).ToList(),
                                  ImageDefault = (from img in _unitOfWork.ImageRepository.ObjectContext
                                                  where img.ProductId == pro.Id
                                                  select img.Url
                                      ).FirstOrDefault()
                              }).ToList();

            return listLaptop;
        }

        // Load data
        public (IDataView training, IDataView test) LoadData(MLContext mlContext)
        {
            //var trainingData = _unitOfWork.DataTrainRepository.ObjectContext.AsEnumerable();
            //var testData = _unitOfWork.DataTestRepository.GetAll();

            var trainData = from dt in _unitOfWork.DataTrainRepository.GetAll()
                            select new MovieRating
                            {
                                UserId = dt.UserId,
                                ProductId = dt.ProductId,
                                Label = dt.Rating
                            };

            var testData = from dt in _unitOfWork.DataTestRepository.GetAll()
                           select new MovieRating
                           {
                               UserId = dt.UserId,
                               ProductId = dt.ProductId,
                               Label = dt.Rating
                           };

            //IDataView trainingDataView = mlContext.Data.LoadFromTextFile<MovieRating>(trainingDataPath, hasHeader: true, separatorChar: ',');
            //IDataView testDataView = mlContext.Data.LoadFromTextFile<MovieRating>(testDataPath, hasHeader: true, separatorChar: ',');
            IDataView trainingDataView = mlContext.Data.LoadFromEnumerable<MovieRating>(trainData);
            IDataView testDataView = mlContext.Data.LoadFromEnumerable<MovieRating>(testData);

            return (trainingDataView, testDataView);
            // </SnippetLoadData>
        }

        // Build and train model
        public ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainingDataView)
        {
            // Add data transformations
            // <SnippetDataTransformations>
            IEstimator<ITransformer> estimator = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "UserIdEncoded", inputColumnName: "UserId")
                .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "ProductIdEncoded", inputColumnName: "ProductId"));
            // </SnippetDataTransformations>

            // Set algorithm options and append algorithm
            // <SnippetAddAlgorithm>
            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "UserIdEncoded",
                MatrixRowIndexColumnName = "ProductIdEncoded",
                LabelColumnName = "Label",
                NumberOfIterations = 20,
                ApproximationRank = 100
            };

            var trainerEstimator = estimator.Append(mlContext.Recommendation().Trainers.MatrixFactorization(options));
            // </SnippetAddAlgorithm>

            // <SnippetFitModel>
            Console.WriteLine("=============== Training the model ===============");
            ITransformer model = trainerEstimator.Fit(trainingDataView);

            return model;
            // </SnippetFitModel>
        }

        // Evaluate model
        public void EvaluateModel(MLContext mlContext, IDataView testDataView, ITransformer model)
        {
            // Evaluate model on test data & print evaluation metrics
            // <SnippetTransform>
            Console.WriteLine("=============== Evaluating the model ===============");
            var prediction = model.Transform(testDataView);
            // </SnippetTransform>

            // <SnippetEvaluate>
            var metrics = mlContext.Regression.Evaluate(prediction, labelColumnName: "Label", scoreColumnName: "Score");
            // </SnippetEvaluate>

            // <SnippetPrintMetrics>
            Console.WriteLine("Root Mean Squared Error : " + metrics.RootMeanSquaredError.ToString());
            Console.WriteLine("RSquared: " + metrics.RSquared.ToString());
            // </SnippetPrintMetrics>
        }

        // Use model for single prediction
        public double UseModelForSinglePrediction(MLContext mlContext, ITransformer model, int userId, int productId)
        {
            // <SnippetPredictionEngine>
            Console.WriteLine("=============== Making a prediction ===============");
            var predictionEngine = mlContext.Model.CreatePredictionEngine<MovieRating, MovieRatingPrediction>(model);
            // </SnippetPredictionEngine>

            // Create test input & make single prediction
            // <SnippetMakeSinglePrediction>
            var testInput = new MovieRating { UserId = userId, ProductId = productId };

            var movieRatingPrediction = predictionEngine.Predict(testInput);
            // </SnippetMakeSinglePrediction>

            return Math.Round(movieRatingPrediction.Score, 1);
        }

        public int GetClaimUserId()
        {
            var claimId = Convert.ToInt32(_httpContext.User.FindFirst(x=>x.Type=="Id").Value);
            return claimId;
        }

        public Feedback GetFeedback(int id)
        {
            return _unitOfWork.FeedbackRepository.GetById(id);
        }
    }
}
