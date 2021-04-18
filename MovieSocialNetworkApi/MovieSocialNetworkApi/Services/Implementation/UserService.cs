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

namespace MovieSocialNetworkApi.Services
{
    public class UserService : IUserService
    {
        private readonly MovieSocialNetworkDbContext _context;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UserService(
            MovieSocialNetworkDbContext context,
            IOptions<AppSettings> appSettings,
            IMapper mapper,
            ILogger<UserService> logger
        )
        {
            _appSettings = appSettings.Value;
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        public async Task<UserVM> GetById(int id)
        {
            try
            {
                var user = await _context.SystemEntities.OfType<User>().SingleOrDefaultAsync(e => e.Id == id);
                return _mapper.Map<UserVM>(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<PagedList<UserVM>> GetList(Paging paging, Sorting sorting, string q)
        {
            try
            {
                var users = _context.SystemEntities.OfType<User>().AsQueryable();

                if (!string.IsNullOrEmpty(q))
                {
                    users = users.Where((e) => e.Username.ToLower().Contains(q.ToLower()) || e.Description.ToLower().Contains(q.ToLower()));
                }

                if (string.IsNullOrWhiteSpace(sorting.SortBy))
                {
                    sorting.SortBy = "followers";
                }

                if (sorting.SortBy == "followers")
                {
                    if (sorting.SortOrder == SortOrder.Desc)
                    {
                        users = users.Include(e => e.Followers).OrderByDescending((e) => e.Followers.Count);
                    }
                    else
                    {
                        users = users.Include(e => e.Followers).OrderBy((e) => e.Followers.Count);
                    }
                }
                else
                {
                    throw new BusinessException($"Sorting by field {sorting.SortBy} is not supported");
                }

                PagedList<UserVM> result = new PagedList<UserVM>
                {
                    TotalCount = await users.CountAsync(),
                    PageSize = paging.PageSize,
                    Page = paging.PageNumber,
                    SortBy = sorting.SortBy,
                    SortOrder = sorting.SortOrder,
                };

                var items = await users.Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize).ToListAsync();
                result.Items = _mapper.Map<List<User>, List<UserVM>>(items);
                result.TotalPages = (result.TotalCount % result.PageSize > 0) ? (result.TotalCount / result.PageSize + 1) : (result.TotalCount / result.PageSize);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<AuthenticationInfo> Register(RegisterCommand command)
        {
            try
            {
                var user = await _context.SystemEntities.OfType<User>().SingleOrDefaultAsync(e => e.Username == command.Username);
                if (user != null) throw new BusinessException("User with provided username already exists", BusinessErrorCode.UsernameAlreadyExists);

                user = await _context.SystemEntities.OfType<User>().SingleOrDefaultAsync(e => e.Email == command.Email);
                if (user != null) throw new BusinessException("User with provided email already exists", BusinessErrorCode.EmailAlreadyExists);

                user = new User
                {
                    Email = command.Email,
                    Username = command.Username,
                    Password = PasswordHelper.SHA256(command.Password, _appSettings.PwSecret),
                    Role = Role.User,
                    Description = string.Empty
                };

                _context.SystemEntities.Add(user);
                await _context.SaveChangesAsync();

                var authenticatedUser = _mapper.Map<AuthenticationInfo>(user);

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

        public async Task<AuthenticationInfo> Login(LoginCommand command)
        {
            try
            {
                var hashedPassword = PasswordHelper.SHA256(command.Password, _appSettings.PwSecret);

                var user = await _context.SystemEntities.OfType<User>().SingleOrDefaultAsync(e => e.Username == command.Username);

                if (user == null) throw new BusinessException("Invalid username", BusinessErrorCode.InvalidUsername);
                if (user.Password != hashedPassword) throw new BusinessException("Invalid password", BusinessErrorCode.InvalidPassword);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.JwtSecret);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.Name, user.Id.ToString())
                        }
                    ),
                    Expires = DateTime.UtcNow.AddDays(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                var authenticatedUser = _mapper.Map<AuthenticationInfo>(user);
                authenticatedUser.Token = tokenHandler.WriteToken(token);

                return authenticatedUser;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
    }
}
