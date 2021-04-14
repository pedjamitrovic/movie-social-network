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
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IAuthService _auth;

        public GroupService(
            MovieSocialNetworkDbContext context,
            IMapper mapper,
            ILogger<UserService> logger,
            IAuthService auth
        )
        {
            _mapper = mapper;
            _logger = logger;
            _context = context;
            _auth = auth;
        }

        public async Task<GroupVM> GetById(int id)
        {
            try
            {
                var group = await _context.SystemEntities.OfType<Group>().SingleOrDefaultAsync(e => e.Id == id);
                return _mapper.Map<GroupVM>(group);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<PagedList<GroupVM>> GetList(Paging paging, Sorting sorting, string q)
        {
            try
            {
                var groups = _context.SystemEntities.OfType<Group>().AsQueryable();

                if (!string.IsNullOrEmpty(q))
                {
                    groups = groups.Where(
                        (e) => 
                        e.Title.ToLower().Contains(q.ToLower()) ||
                        e.Subtitle.ToLower().Contains(q.ToLower()) || 
                        e.Description.ToLower().Contains(q.ToLower())
                    );
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
                var authUser = await _auth.GetAuthenticatedUser();
                if (authUser == null) throw new BusinessException($"Authenticated user not found");

                var group = await _context.SystemEntities.OfType<Group>().SingleOrDefaultAsync(e => e.Title == command.Title);
                if (group != null) throw new BusinessException("Group with provided title already exists");

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
                    Admin = authUser
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
    }
}
