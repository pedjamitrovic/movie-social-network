using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieSocialNetworkApi.Database;
using MovieSocialNetworkApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public class AuthService: IAuthService
    {
        private string UserId { get { return _httpContextAccessor.HttpContext?.User.Identity.Name; } }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MovieSocialNetworkDbContext _context;
        private readonly ILogger _logger;

        public AuthService(
            IHttpContextAccessor httpContextAccessor,
            MovieSocialNetworkDbContext context,
            ILogger<AuthService> logger
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _logger = logger;
        }

        public async Task<AbstractUser> GetAuthenticatedUser()
        {
            try
            {
                if (UserId == null) { return null; }
                var user = await _context.Users.SingleOrDefaultAsync(e => e.Id == long.Parse(UserId));
                return user;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
    }
}
