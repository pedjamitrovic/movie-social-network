using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MovieSocialNetworkApi.Database;
using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Exceptions;
using MovieSocialNetworkApi.Helpers;
using MovieSocialNetworkApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public class MovieService : IMovieService
    {
        private readonly MovieSocialNetworkDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IAuthService _auth;
        private readonly AppSettings _appSettings;
        private readonly IRecommendationService _recommendationService;

        public MovieService(
            MovieSocialNetworkDbContext context,
            IMapper mapper,
            ILogger<MovieService> logger,
            IAuthService auth,
            IOptions<AppSettings> appSettings,
            IRecommendationService recommendationService
        )
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _auth = auth;
            _appSettings = appSettings.Value;
            _recommendationService = recommendationService;
        }

        public async Task<object> GetConfiguration()
        {
            try
            {
                using var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(_appSettings.TmdbBaseUrl)
                };

                using var response = await httpClient.GetAsync(new Uri($"configuration?api_key={_appSettings.TmdbApiKey}", UriKind.Relative));

                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject(responseContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<object> GetMovieDetails(int id)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                using var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(_appSettings.TmdbBaseUrl)
                };

                using var response = await httpClient.GetAsync(new Uri($"movie/{id}?api_key={_appSettings.TmdbApiKey}", UriKind.Relative));

                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject(responseContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<object> SearchMovies(string query)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                using var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(_appSettings.TmdbBaseUrl)
                };

                using var response = await httpClient.GetAsync(new Uri($"search/movie?api_key={_appSettings.TmdbApiKey}&query=${query}", UriKind.Relative));

                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject(responseContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<object> GetTrendingMovies(string timeWindow)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                using var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(_appSettings.TmdbBaseUrl)
                };

                using var response = await httpClient.GetAsync(new Uri($"trending/movie/{timeWindow}?api_key={_appSettings.TmdbApiKey}", UriKind.Relative));

                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject(responseContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<object> GetPopularMovies(Paging paging)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                using var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(_appSettings.TmdbBaseUrl)
                };

                using var response = await httpClient.GetAsync(new Uri($"movie/popular?api_key={_appSettings.TmdbApiKey}&page={paging.PageNumber}", UriKind.Relative));

                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject(responseContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
        public async Task<object> GetMovieKeywords(int id)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                using var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(_appSettings.TmdbBaseUrl)
                };

                using var response = await httpClient.GetAsync(new Uri($"movie/{id}/keywords?api_key={_appSettings.TmdbApiKey}", UriKind.Relative));

                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject(responseContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<object> GetMovieRecommendations(int id)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                using var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(_appSettings.TmdbBaseUrl)
                };

                using var response = await httpClient.GetAsync(new Uri($"movie/{id}/recommendations?api_key={_appSettings.TmdbApiKey}", UriKind.Relative));

                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject(responseContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<object> GetMovieCredits(int id)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                using var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(_appSettings.TmdbBaseUrl)
                };

                using var response = await httpClient.GetAsync(new Uri($"movie/{id}/credits?api_key={_appSettings.TmdbApiKey}", UriKind.Relative));

                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject(responseContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<object> GetSimilarMovies(int id)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                using var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(_appSettings.TmdbBaseUrl)
                };

                using var response = await httpClient.GetAsync(new Uri($"movie/{id}/similar?api_key={_appSettings.TmdbApiKey}", UriKind.Relative));

                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject(responseContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<object> GetMovieVideos(int id)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                using var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(_appSettings.TmdbBaseUrl)
                };

                using var response = await httpClient.GetAsync(new Uri($"movie/{id}/videos?api_key={_appSettings.TmdbApiKey}", UriKind.Relative));

                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject(responseContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<MovieRatingVM> GetMyMovieRating(int movieId)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var movieRating = await _context.MovieRatings
                    .Include(e => e.Owner)
                    .SingleOrDefaultAsync(e => e.MovieId == movieId && e.Owner == authSystemEntity);

                var movieRatingVM = _mapper.Map<MovieRatingVM>(movieRating);

                return movieRatingVM;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<MovieRatingVM> RateMovie(int movieId, RateMovieCommand command)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                if (command.Rating.HasValue && (command.Rating < 1 || command.Rating > 10))
                {
                    throw new BusinessException($"Rating must be between 1 and 10");
                }

                // Check if movie exists
                var movie = await GetMovieDetails(movieId);

                var movieRating = await _context.MovieRatings
                    .Include((mr) => mr.Owner)
                    .SingleOrDefaultAsync((mr) => mr.MovieId == movieId && mr.Owner == authSystemEntity);

                if (movieRating != null)
                {
                    if (command.Rating.HasValue)
                    {
                        movieRating.Rating = command.Rating.Value;

                        _context.MovieRatings.Update(movieRating);
                    }
                    else
                    {
                        _context.MovieRatings.Remove(movieRating);
                    }
                }
                else
                {
                    if (command.Rating.HasValue)
                    {
                        movieRating = new MovieRating
                        {
                            MovieId = movieId,
                            Rating = command.Rating.Value,
                            Owner = authSystemEntity,
                        };

                        _context.MovieRatings.Add(movieRating);
                    }
                }

                await _context.SaveChangesAsync();

                return _mapper.Map<MovieRatingVM>(movieRating);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task CalculateTempRecommendations()
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var sysEntities = await _context.SystemEntities
                    .Include(e => e.MovieRatings)
                    .Where(e => e.MovieRatings.Count >= _appSettings.MinRatingsCount)
                    .ToListAsync();

                var movieIds = await _context.MovieRatings
                    .GroupBy(e => e.MovieId)
                    .Select(g => new { MovieId = g.Key, Count = g.Count() })
                    .Where(e => e.Count >= _appSettings.MinRatingsCount)
                    .Select(e => e.MovieId)
                    .ToListAsync();

                await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE TempRecommendations");

                var tempRecommendations = new List<TempRecommendation>();

                for (int i = 0; i < sysEntities.Count; i++)
                {
                    Console.WriteLine($"Predicting ratings for user with id {sysEntities[i].Id}");
                    for (int j = 0; j < movieIds.Count; j++)
                    {
                        tempRecommendations.Add(
                            new TempRecommendation
                            {
                                MovieId = movieIds[j],
                                OwnerId = sysEntities[i].Id,
                                Rating = (int)Math.Round(_recommendationService.Predict(sysEntities[i].Id, movieIds[j]).Score)
                            }
                        );
                    }
                }

                await _context.BulkInsertAsync(tempRecommendations);

                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();

                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task ActivateTempRecommendations()
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Recommendations");
                await _context.Database.ExecuteSqlRawAsync("INSERT INTO Recommendations SELECT MovieId, OwnerId, Rating FROM TempRecommendations");

                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();

                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task CalculateRecommendations()
        {
            await CalculateTempRecommendations();
            await ActivateTempRecommendations();
        }

        public async Task<object> GetRecommendations(Paging paging)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                await _context.Entry(authSystemEntity).Collection(e => e.MovieRatings).LoadAsync();

                if (authSystemEntity.MovieRatings.Count < _appSettings.MinRatingsCount)
                {
                    return await GetPopularMovies(paging);
                }

                var recommendations = from r in _context.Recommendations
                                      where r.OwnerId == authSystemEntity.Id
                                      join mr in _context.MovieRatings on
                                      new { r.MovieId, r.OwnerId } equals new { mr.MovieId, mr.OwnerId }
                                      into rmrs
                                      from rmr in rmrs.DefaultIfEmpty()
                                      where rmr == null
                                      select r;

                recommendations = recommendations.OrderByDescending(e => e.Rating).AsQueryable();

                var totalResults = await recommendations.CountAsync();

                var pagedRecommendations = await recommendations.Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize).ToListAsync();

                var results = new List<object>();

                foreach (var recommendation in pagedRecommendations)
                {
                    try
                    {
                        var movie = await GetMovieDetails(recommendation.MovieId);
                        results.Add(movie);
                    }
                    catch
                    {
                        // We ignore exceptions
                    }
                }

                var response = new Dictionary<string, object>
                {
                    ["page"] = paging.PageNumber,
                    ["total_results"] = totalResults,
                    ["total_pages"] = totalResults / paging.PageSize + 1,
                    ["results"] = results
                };

                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
    }
}
