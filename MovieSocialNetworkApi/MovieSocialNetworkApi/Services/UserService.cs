using System;
using System.Collections.Generic;
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

namespace MovieSocialNetworkApi.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly MovieSocialNetworkDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        private List<User> _users = new List<User>
        {
            new User { Id = 1, Username = "admin", PasswordHash = "admin", Role = Role.Admin },
            new User { Id = 2, Username = "user", PasswordHash = "user", Role = Role.User }
        };

        public UserService(
            IOptions<AppSettings> appSettings,
            IMapper mapper, 
            ILogger<UserService> logger, 
            MovieSocialNetworkDbContext context
        ) {
            _appSettings = appSettings.Value;
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        public AuthenticatedUser Authenticate(AuthenticateCommand command)
        {
            try
            {
                // Hash pw before querying

                var user = _users.SingleOrDefault(x => x.Username == command.Username && x.PasswordHash == command.Password);

                if (user == null) throw new BusinessException("Invalid username or password");

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

        public IEnumerable<UserVM> GetAll()
        {
            return _mapper.Map<IEnumerable<User>, IEnumerable<UserVM>>(_users);
        }

        public UserVM GetById(int id)
        {
            var user = _users.FirstOrDefault(x => x.Id == id);
            return _mapper.Map<UserVM>(user);
        }

        Task<bool> IUserService.Ban(BanCommand command)
        {
            throw new NotImplementedException();
        }

        Task<bool> IUserService.ChangeAbout(ChangeAboutCommand command)
        {
            throw new NotImplementedException();
        }

        Task<bool> IUserService.ChangeImage(ChangeImageCommand command)
        {
            throw new NotImplementedException();
        }

        Task<bool> IUserService.Delete(int id)
        {
            throw new NotImplementedException();
        }

        Task<bool> IUserService.Follow(int id)
        {
            throw new NotImplementedException();
        }

        Task<UserVM> IUserService.GetById(int id)
        {
            throw new NotImplementedException();
        }

        Task<User> IUserService.GetFollowers(int id)
        {
            throw new NotImplementedException();
        }

        Task<User> IUserService.GetFollowing(int id)
        {
            throw new NotImplementedException();
        }

        Task<PagedList<UserVM>> IUserService.GetList(Paging paging, Sorting sorting, string q)
        {
            throw new NotImplementedException();
        }

        AuthenticatedUser IUserService.Login(AuthenticateCommand command)
        {
            throw new NotImplementedException();
        }

        Task<User> IUserService.Register()
        {
            throw new NotImplementedException();
        }

        Task<bool> IUserService.Report(ReportCommand command)
        {
            throw new NotImplementedException();
        }

        Task<bool> IUserService.Unfollow(int id)
        {
            throw new NotImplementedException();
        }
    }
}
