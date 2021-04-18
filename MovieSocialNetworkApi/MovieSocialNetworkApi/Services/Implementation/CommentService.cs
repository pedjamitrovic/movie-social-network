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
    public class CommentService : ICommentService
    {
        private readonly MovieSocialNetworkDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IAuthService _auth;

        public CommentService(
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

        public async Task<PagedList<CommentVM>> GetList(Paging paging, Sorting sorting, string q, int? creatorId, int? postId)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var comments = _context.Contents.OfType<Comment>().Include(e => e.Creator).Include(e => e.Post).AsQueryable();

                if (!string.IsNullOrEmpty(q))
                {
                    comments = comments.Where((e) => e.Text.ToLower().Contains(q.ToLower()));
                }

                if (creatorId != null)
                {
                    comments = comments.Where((e) => e.Creator.Id == creatorId);
                }

                if (postId != null)
                {
                    comments = comments.Where((e) => e.Post.Id == postId);
                }

                if (string.IsNullOrWhiteSpace(sorting.SortBy))
                {
                    sorting.SortBy = "createdOn";
                }

                if (sorting.SortBy == "createdOn")
                {
                    if (sorting.SortOrder == SortOrder.Desc)
                    {
                        comments = comments.OrderByDescending((e) => e.CreatedOn);
                    }
                    else
                    {
                        comments = comments.OrderBy((e) => e.CreatedOn);
                    }
                }
                else
                {
                    throw new BusinessException($"Sorting by field {sorting.SortBy} is not supported");
                }

                PagedList<CommentVM> result = new PagedList<CommentVM>
                {
                    TotalCount = await comments.CountAsync(),
                    PageSize = paging.PageSize,
                    Page = paging.PageNumber,
                    SortBy = sorting.SortBy,
                    SortOrder = sorting.SortOrder,
                };

                var items = await comments.Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize).ToListAsync();
                result.Items = _mapper.Map<List<Comment>, List<CommentVM>>(items);
                result.TotalPages = (result.TotalCount % result.PageSize > 0) ? (result.TotalCount / result.PageSize + 1) : (result.TotalCount / result.PageSize);

                foreach (var commentVM in result.Items)
                {
                    var existingReaction = await _context.Reactions.SingleOrDefaultAsync(e => e.Owner == authSystemEntity && e.Content.Id == commentVM.Id);
                    if (existingReaction != null) commentVM.ExistingReaction = _mapper.Map<ReactionVM>(existingReaction);
                    
                    commentVM.ReactionStats =
                        _context.Reactions.Where(e => e.Content.Id == commentVM.Id)
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

        public async Task<CommentVM> GetById(int id)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var comment = await _context.Contents.OfType<Comment>().Include(e => e.Creator).Include(e => e.Post).SingleOrDefaultAsync(e => e.Id == id);
                var commentVM = _mapper.Map<CommentVM>(comment);

                var existingReaction = await _context.Reactions.SingleOrDefaultAsync(e => e.Owner == authSystemEntity && e.Content == comment);
                if (existingReaction != null) commentVM.ExistingReaction = _mapper.Map<ReactionVM>(existingReaction);

                commentVM.ReactionStats = _context.Reactions.Where(e => e.Content == comment)
                    .GroupBy(e => e.Value)
                    .Select(g => new ReactionStats { Value = g.Key, Count = g.Count() })
                    .OrderByDescending(e => e.Count)
                    .ToList();

                return commentVM;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<CommentVM> Create(CreateCommentCommand command)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var post = await _context.Contents.OfType<Post>().SingleOrDefaultAsync(e => e.Id == command.PostId);
                if (post == null) throw new BusinessException($"Post with {command.PostId} not found");

                var comment = new Comment
                {
                    Text = command.Text,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Post = post,
                    Creator = authSystemEntity
                };

                _context.Contents.Add(comment);
                await _context.SaveChangesAsync();

                return _mapper.Map<Comment, CommentVM>(comment);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
    }
}
