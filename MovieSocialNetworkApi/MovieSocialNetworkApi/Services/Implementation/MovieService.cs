using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MovieSocialNetworkApi.Database;
using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Exceptions;
using MovieSocialNetworkApi.Helpers;
using MovieSocialNetworkApi.Models;
using MovieSocialNetworkApi.Models.Response;
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

        public MovieService(
            MovieSocialNetworkDbContext context,
            IMapper mapper,
            ILogger<MovieService> logger,
            IAuthService auth,
            IOptions<AppSettings> appSettings
        )
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _auth = auth;
            _appSettings = appSettings.Value;
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

        public async Task<object> GetPopularMovies()
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                using var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(_appSettings.TmdbBaseUrl)
                };

                using var response = await httpClient.GetAsync(new Uri($"movie/popular?api_key={_appSettings.TmdbApiKey}", UriKind.Relative));

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

                if (command.Rating.HasValue && (command.Rating < 1 || command.Rating > 10)) {
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
    }
}
