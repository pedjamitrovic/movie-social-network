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
    public class GroupService : IGroupService
    {
        private readonly MovieSocialNetworkDbContext _context;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IAuthService _auth;

        public GroupService(
            MovieSocialNetworkDbContext context,
            IOptions<AppSettings> appSettings,
            IMapper mapper,
            ILogger<GroupService> logger,
            IAuthService auth
        )
        {
            _mapper = mapper;
            _logger = logger;
            _context = context;
            _auth = auth;
            _appSettings = appSettings.Value;
        }

        public async Task<GroupVM> GetById(int id)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var group = await _context.SystemEntities.OfType<Group>().Include(e => e.GroupAdmin).SingleOrDefaultAsync(e => e.Id == id);
                var groupVM = _mapper.Map<GroupVM>(group);
                groupVM.IsAuthUserAdmin = group.GroupAdmin.Any(e => e.AdminId == authSystemEntity.Id);
                return groupVM;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<PagedList<GroupVM>> GetList(Paging paging, Sorting sorting, string q, int? followerId, int? adminId)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var groups = _context.SystemEntities.OfType<Group>().Include(e => e.GroupAdmin).AsQueryable();

                if (!string.IsNullOrEmpty(q))
                {
                    groups = groups.Where(
                        (e) =>
                        e.Title.ToLower().Contains(q.ToLower()) ||
                        e.Subtitle.ToLower().Contains(q.ToLower()) ||
                        e.Description.ToLower().Contains(q.ToLower())
                    );
                }

                if (followerId != null)
                {
                    groups = groups.Include((e) => e.Followers).Where(e => e.Followers.Any(f => f.FollowerId == followerId));
                }

                if (adminId != null)
                {
                    groups = groups.Include((e) => e.GroupAdmin).Where(e => e.GroupAdmin.Any(ga => ga.AdminId == adminId));
                }

                if (string.IsNullOrWhiteSpace(sorting.SortBy))
                {
                    sorting.SortBy = "followers";
                }

                if (sorting.SortBy == "followers")
                {
                    if (sorting.SortOrder == SortOrder.Desc)
                    {
                        groups = groups.Include(e => e.Followers).OrderByDescending((e) => e.Followers.Count);
                    }
                    else
                    {
                        groups = groups.Include(e => e.Followers).OrderBy((e) => e.Followers.Count);
                    }
                }
                else
                {
                    throw new BusinessException($"Sorting by field {sorting.SortBy} is not supported");
                }

                PagedList<GroupVM> result = new PagedList<GroupVM>
                {
                    TotalCount = await groups.CountAsync(),
                    PageSize = paging.PageSize,
                    Page = paging.PageNumber,
                    SortBy = sorting.SortBy,
                    SortOrder = sorting.SortOrder,
                };

                var items = await groups.Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize).ToListAsync();
                result.Items = _mapper.Map<List<Group>, List<GroupVM>>(items);
                result.TotalPages = (result.TotalCount % result.PageSize > 0) ? (result.TotalCount / result.PageSize + 1) : (result.TotalCount / result.PageSize);

                for (int i = 0; i < items.Count; i++)
                {
                    result.Items[i].IsAuthUserAdmin = items[i].GroupAdmin.Any(e => e.AdminId == authSystemEntity.Id);
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<GroupVM> Create(CreateGroupCommand command)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var group = await _context.SystemEntities.OfType<Group>().SingleOrDefaultAsync(e => e.Title == command.Title);
                if (group != null) throw new BusinessException("Group with provided title already exists", BusinessErrorCode.TitleAlreadyExists);

                group = new Group
                {
                    Title = command.Title,
                    Subtitle = command.Subtitle,
                    Description = string.Empty,

                };

                _context.SystemEntities.Add(group);

                var groupAdmin = new GroupAdmin
                {
                    Group = group,
                    Admin = authSystemEntity as User
                };

                _context.GroupAdmins.Add(groupAdmin);

                await _context.SaveChangesAsync();

                return _mapper.Map<GroupVM>(group);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<AuthenticationInfo> Login(int id)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var groupAdmin = await _context.GroupAdmins.Include(e => e.Group)
                    .Where(e => e.AdminId == authSystemEntity.Id && e.GroupId == id)
                    .SingleOrDefaultAsync();
                if (groupAdmin == null) throw new BusinessException($"Authenticated user is not admin of group with {id}", BusinessErrorCode.NotAdmin);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.JwtSecret);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.Name, groupAdmin.Group.Id.ToString()),
                            new Claim(ClaimTypes.NameIdentifier, groupAdmin.Group.Id.ToString()),
                            new Claim(ClaimTypes.Role, groupAdmin.Group.Role)
                        }
                    ),
                    Expires = DateTime.UtcNow.AddDays(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                var authenticatedUser = _mapper.Map<AuthenticationInfo>(groupAdmin.Group);
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
