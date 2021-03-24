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

        public async Task<PagedList<PostVM>> GetList(Paging paging, Sorting sorting, string q)
        {
            try
            {
                var posts = _context.Contents.OfType<Post>().AsQueryable();

                if (!string.IsNullOrEmpty(q))
                {
                    posts = posts.Where((e) => e.Text.ToLower().Contains(q.ToLower()));
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
                var post = await _context.Contents.OfType<Post>().SingleOrDefaultAsync(e => e.Id == id);
                return _mapper.Map<PostVM>(post);
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

                var post = new Post
                {
                    Text = command.Text,
                    CreatedOn = DateTime.UtcNow,
                    FilePath = filePath,
                    Creator = authUser
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
