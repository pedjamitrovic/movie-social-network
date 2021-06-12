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

    }
}
