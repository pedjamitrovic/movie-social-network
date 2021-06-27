using Microsoft.Extensions.Logging;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using MovieSocialNetworkApi.Database;
using MovieSocialNetworkApi.Recommender;
using System;
using System.IO;
using System.Linq;

namespace MovieSocialNetworkApi.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly ILogger _logger;
        private readonly MovieSocialNetworkDbContext _context;

        private MLContext _mlContext;
        private ITransformer _transformer;
        private PredictionEngine<MovieRating, MovieRatingPrediction> _engine;

        public RecommendationService(
            ILogger<RecommendationService> logger,
            MovieSocialNetworkDbContext context
        )
        {
            _logger = logger;
            _context = context;
            _mlContext = new MLContext();
            LoadModel();
        }

        public MovieRatingPrediction Predict(int userId, int movieId)
        {
            return _engine.Predict(new MovieRating { OwnerId = userId, MovieId = movieId });
        }

        public void CreateModel()
        {
            try
            {
                var movieRatings = _context.MovieRatings.Select(e => new MovieRating { OwnerId = e.OwnerId, MovieId = e.MovieId, Rating = e.Rating }).ToList();

                var data = _mlContext.Data.LoadFromEnumerable(movieRatings);

                var trainTestData = _mlContext.Data.TrainTestSplit(data, testFraction: 0.2);

                var options = new MatrixFactorizationTrainer.Options
                {
                    MatrixColumnIndexColumnName = "OwnerIdEncoded",
                    MatrixRowIndexColumnName = "MovieIdEncoded",
                    LabelColumnName = "Rating",
                    NumberOfIterations = 12,
                    Quiet = true
                };

                _transformer = BuildAndTrainModel(trainTestData.TrainSet, options);

                var metrics = EvaluateModel(trainTestData.TestSet, _transformer);

                Console.WriteLine($"RMSE: {metrics.RootMeanSquaredError:N3}");

                SaveModel(trainTestData.TrainSet.Schema);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        private ITransformer BuildAndTrainModel(IDataView trainingDataView, MatrixFactorizationTrainer.Options options)
        {
            var estimator = _mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "OwnerIdEncoded", inputColumnName: "OwnerId")
                .Append(_mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "MovieIdEncoded", inputColumnName: "MovieId"));

            var trainerEstimator = estimator.Append(_mlContext.Recommendation().Trainers.MatrixFactorization(options));

            return trainerEstimator.Fit(trainingDataView);
        }

        private RegressionMetrics EvaluateModel(IDataView testDataView, ITransformer model)
        {
            var prediction = model.Transform(testDataView);

            return _mlContext.Regression.Evaluate(prediction, labelColumnName: "Rating", scoreColumnName: "Score");
        }

        private void SaveModel(DataViewSchema trainingDataViewSchema)
        {
            var modelPath = Path.Combine(Environment.CurrentDirectory, "MatrixFactorizationModel.zip");
            _mlContext.Model.Save(_transformer, trainingDataViewSchema, modelPath);
        }

        private void LoadModel()
        {
            try
            {
                var modelPath = Path.Combine(Environment.CurrentDirectory, "MatrixFactorizationModel.zip");
                if (File.Exists(modelPath))
                {
                    _transformer = _mlContext.Model.Load(modelPath, out _);
                    _engine = _mlContext.Model.CreatePredictionEngine<MovieRating, MovieRatingPrediction>(_transformer);
                }
            }
            catch { }
        }
    }
}
