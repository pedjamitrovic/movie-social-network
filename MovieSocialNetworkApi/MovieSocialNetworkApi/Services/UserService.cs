using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MovieSocialNetworkApi.Database;
using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Exceptions;
using MovieSocialNetworkApi.Helpers;
using MovieSocialNetworkApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace MovieSocialNetworkApi.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly MovieSocialNetworkDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IAuthService _auth;

        public UserService(
            IOptions<AppSettings> appSettings,
            IMapper mapper,
            ILogger<UserService> logger,
            MovieSocialNetworkDbContext context,
            IAuthService auth
        )
        {
            _appSettings = appSettings.Value;
            _mapper = mapper;
            _logger = logger;
            _context = context;
            _auth = auth;
        }

        public async Task<AbstractUserVM> GetById(int id)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(e => e.Id == id);
                return _mapper.Map<AbstractUserVM>(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task Ban(BanCommand command)
        {
            throw new NotImplementedException();
        }

        public async Task ChangeDescription(ChangeAboutCommand command)
        {
            throw new NotImplementedException();
        }

        public async Task ChangeImage(ChangeImageCommand command)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Follow(int id)
        {
            try
            {
                var authUser = await _auth.GetAuthenticatedUser();
                if (authUser == null) throw new BusinessException($"Authenticated user not found");

                var user = await _context.AbstractUsers.SingleOrDefaultAsync(e => e.Id == id);
                if (user == null) throw new BusinessException($"User with {id} not found");

                var relation = new Relation
                {
                    FollowerId = authUser.Id,
                    FollowingId = user.Id,
                };

                var existingRelation = await _context.Relations.FindAsync(relation.FollowingId, relation.FollowerId);

                if (existingRelation == null)
                {
                    _context.Relations.Add(relation);

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<AbstractUserVM>> GetFollowers(int id)
        {
            try
            {
                var user = await _context.Users.Include(e => e.Followers).SingleOrDefaultAsync(e => e.Id == id);
                if (user == null) throw new BusinessException($"User with id {id} not found");

                foreach (var relation in user.Followers)
                {
                    await _context.Entry(relation).Reference(e => e.Follower).LoadAsync();
                }
                var followers = user.Followers.Select(e => e.Follower).ToList();

                return _mapper.Map<List<AbstractUser>, List<AbstractUserVM>>(followers);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<AbstractUserVM>> GetFollowing(int id)
        {
            try
            {
                var user = await _context.Users.Include(e => e.Following).SingleOrDefaultAsync(e => e.Id == id);
                if (user == null) throw new BusinessException($"User with id {id} not found");

                foreach (var relation in user.Followers)
                {
                    await _context.Entry(relation).Reference(e => e.Following).LoadAsync();
                }
                var following = user.Followers.Select(e => e.Following).ToList();

                return _mapper.Map<List<AbstractUser>, List<AbstractUserVM>>(following);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<PagedList<AbstractUserVM>> GetList(Paging paging, Sorting sorting, string q)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthenticatedUser> Login(LoginCommand command)
        {
            try
            {
                var hashedPassword = PasswordHelper.SHA256(command.Password, _appSettings.PwSecret);

                var user = await _context.Users.SingleOrDefaultAsync(
                    e => e.Username == command.Username && e.Password == hashedPassword
                );

                if (user == null) throw new BusinessException("Invalid username or password");

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.JwtSecret);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.Name, user.Id.ToString()),
                            new Claim(ClaimTypes.Role, user.Role)
                        }
                    ),
                    Expires = DateTime.UtcNow.AddDays(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                var authenticatedUser = _mapper.Map<AuthenticatedUser>(user);
                authenticatedUser.Token = tokenHandler.WriteToken(token);

                return authenticatedUser;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<AuthenticatedUser> Register(RegisterCommand command)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(e => e.Username == command.Username);
                if (user != null) throw new BusinessException("User with provided username already exists");

                user = await _context.Users.SingleOrDefaultAsync(e => e.Email == command.Email);
                if (user != null) throw new BusinessException("User with provided email already exists");

                user = new User
                {
                    Email = command.Email,
                    Username = command.Username,
                    Password = PasswordHelper.SHA256(command.Password, _appSettings.PwSecret),
                    Role = Role.User,
                    Description = string.Empty
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var authenticatedUser = _mapper.Map<AuthenticatedUser>(user);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.JwtSecret);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.Name, user.Id.ToString()),
                            new Claim(ClaimTypes.Role, user.Role)
                        }
                    ),
                    Expires = DateTime.UtcNow.AddDays(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                authenticatedUser.Token = tokenHandler.WriteToken(token);

                return authenticatedUser;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task Report(ReportCommand command)
        {
            throw new NotImplementedException();
        }

        public async Task Unfollow(int id)
        {
            try
            {
                var authUser = await _auth.GetAuthenticatedUser();
                if (authUser == null) throw new BusinessException($"Authenticated user not found");

                var user = await _context.AbstractUsers.SingleOrDefaultAsync(e => e.Id == id);
                if (user == null) throw new BusinessException($"User with {id} not found");

                var relation = new Relation
                {
                    FollowerId = authUser.Id,
                    FollowingId = user.Id,
                };

                var existingRelation = await _context.Relations.FindAsync(relation.FollowingId, relation.FollowerId);

                if (existingRelation != null)
                {
                    _context.Relations.Remove(existingRelation);

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
    }
}
