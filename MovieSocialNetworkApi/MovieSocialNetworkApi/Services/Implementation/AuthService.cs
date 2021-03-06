﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieSocialNetworkApi.Database;
using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public class AuthService: IAuthService
    {
        private string UserId { get { return _httpContextAccessor.HttpContext?.User.Identity.Name; } }

        private readonly MovieSocialNetworkDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        public AuthService(
            MovieSocialNetworkDbContext context,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AuthService> logger
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _logger = logger;
        }

        public async Task<SystemEntity> GetAuthenticatedSystemEntity()
        {
            try
            {
                if (UserId == null) { return null; }
                var systemEntity = await _context.SystemEntities.Include(se => se.Bans).SingleOrDefaultAsync(e => e.Id == long.Parse(UserId));

                var activeBan = systemEntity.Bans.Any((b) => b.BannedUntil > DateTimeOffset.UtcNow);
                if (activeBan) throw new ForbiddenException();

                return systemEntity;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
    }
}
