using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieSocialNetworkApi.Database;
using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Exceptions;
using MovieSocialNetworkApi.Helpers;
using MovieSocialNetworkApi.Models;
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

        public async Task<PagedList<CommentVM>> GetList(Paging paging, Sorting sorting, string q)
        {
            try
            {
                var comments = _context.Contents.OfType<Comment>().AsQueryable();

                if (!string.IsNullOrEmpty(q))
                {
                    comments = comments.Where((e) => e.Text.ToLower().Contains(q.ToLower()));
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
                var comment = await _context.Contents.OfType<Comment>().SingleOrDefaultAsync(e => e.Id == id);
                return _mapper.Map<CommentVM>(comment);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<CommentVM> Create(string filePath, CreateCommentCommand command)
        {
            try
            {
                var authUser = await _auth.GetAuthenticatedUser();
                if (authUser == null) throw new BusinessException($"Authenticated user not found");

                var post = await _context.Contents.OfType<Post>().SingleOrDefaultAsync(e => e.Id == command.PostId);
                if (post == null) throw new BusinessException($"Post with {command.PostId} not found");

                var comment = new Comment
                {
                    Text = command.Text,
                    CreatedOn = DateTime.UtcNow,
                    Post = post,
                    Creator = authUser
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
