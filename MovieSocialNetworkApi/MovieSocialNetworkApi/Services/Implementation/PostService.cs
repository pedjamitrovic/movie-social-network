using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieSocialNetworkApi.Database;
using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Exceptions;
using MovieSocialNetworkApi.Helpers;
using MovieSocialNetworkApi.Models;
using MovieSocialNetworkApi.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public class PostService : IPostService
    {
        private readonly MovieSocialNetworkDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IAuthService _auth;

        public PostService(
            MovieSocialNetworkDbContext context,
            IMapper mapper,
            ILogger<PostService> logger,
            IAuthService auth
        )
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _auth = auth;
        }

        public async Task<PagedList<PostVM>> GetList(Paging paging, Sorting sorting, string q, int? creatorId, int? followerId)
        {
            try
            {
                var authUser = await _auth.GetAuthenticatedUser();
                if (authUser == null) throw new BusinessException($"Authenticated user not found");

                var posts = _context.Contents.OfType<Post>().Include(e => e.Creator).ThenInclude(e => e.Followers).AsQueryable();

                if (!string.IsNullOrEmpty(q))
                {
                    posts = posts.Where((e) => e.Text.ToLower().Contains(q.ToLower()));
                }

                if (creatorId != null)
                {
                    posts = posts.Where((e) => e.Creator.Id == creatorId);
                }

                if (followerId != null)
                {
                    posts = posts.Where((e) => e.Creator.Id == followerId || e.Creator.Followers.Any((f) => f.FollowerId == followerId));
                }

                if (string.IsNullOrWhiteSpace(sorting.SortBy))
                {
                    sorting.SortBy = "createdOn";
                }

                if (sorting.SortBy == "createdOn")
                {
                    if (sorting.SortOrder == SortOrder.Desc)
                    {
                        posts = posts.OrderByDescending((e) => e.CreatedOn);
                    }
                    else
                    {
                        posts = posts.OrderBy((e) => e.CreatedOn);
                    }
                }
                else
                {
                    throw new BusinessException($"Sorting by field {sorting.SortBy} is not supported");
                }

                PagedList<PostVM> result = new PagedList<PostVM>
                {
                    TotalCount = await posts.CountAsync(),
                    PageSize = paging.PageSize,
                    Page = paging.PageNumber,
                    SortBy = sorting.SortBy,
                    SortOrder = sorting.SortOrder,
                };

                var items = await posts.Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize).ToListAsync();
                result.Items = _mapper.Map<List<Post>, List<PostVM>>(items);
                result.TotalPages = (result.TotalCount % result.PageSize > 0) ? (result.TotalCount / result.PageSize + 1) : (result.TotalCount / result.PageSize);

                foreach (var postVM in result.Items)
                {
                    var existingReaction = await _context.Reactions.SingleOrDefaultAsync(e => e.Owner == authUser && e.Content.Id == postVM.Id);
                    if (existingReaction != null) postVM.ExistingReaction = _mapper.Map<ReactionVM>(existingReaction);

                    postVM.ReactionStats = 
                        _context.Reactions.Where(e => e.Content.Id == postVM.Id)
                        .GroupBy(e => e.Value)
                        .Select(g => new ReactionStats { Value = g.Key, Count = g.Count() })
                        .OrderByDescending(e => e.Count)
                        .ToList();
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<PostVM> GetById(int id)
        {
            try
            {
                var authUser = await _auth.GetAuthenticatedUser();
                if (authUser == null) throw new BusinessException($"Authenticated user not found");

                var post = await _context.Contents.OfType<Post>().Include(e => e.Creator).SingleOrDefaultAsync(e => e.Id == id);
                var postVM = _mapper.Map<Post, PostVM>(post);

                var existingReaction = await _context.Reactions.SingleOrDefaultAsync(e => e.Owner == authUser && e.Content == post);
                if (existingReaction != null) postVM.ExistingReaction = _mapper.Map<ReactionVM>(existingReaction);

                postVM.ReactionStats = _context.Reactions.Where(e => e.Content == post)
                    .GroupBy(e => e.Value)
                    .Select(g => new ReactionStats{ Value = g.Key, Count = g.Count() })
                    .OrderByDescending(e => e.Count)
                    .ToList();

                return postVM;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<PostVM> Create(string filePath, CreatePostCommand command)
        {
            try
            {
                var authUser = await _auth.GetAuthenticatedUser();
                if (authUser == null) throw new BusinessException($"Authenticated user not found");

                Group forGroup = null;

                if (command.ForGroupId != null)
                {
                    forGroup = await _context.SystemEntities.OfType<Group>().SingleOrDefaultAsync(e => e.Id == command.ForGroupId);
                    if (forGroup == null) throw new BusinessException($"Group with id {command.ForGroupId} not found");
                }

                var post = new Post
                {
                    Text = command.Text,
                    CreatedOn = DateTimeOffset.UtcNow,
                    FilePath = filePath,
                    Creator = authUser,
                    ForGroup = forGroup
                };

                _context.Contents.Add(post);
                await _context.SaveChangesAsync();

                return _mapper.Map<Post, PostVM>(post);

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
    }
}
