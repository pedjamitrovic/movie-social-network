using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MovieSocialNetworkApi.Database;
using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Exceptions;
using MovieSocialNetworkApi.Helpers;
using MovieSocialNetworkApi.Models;

namespace MovieSocialNetworkApi.Services
{
    public class PostService : IPostService
    {
        private readonly AppSettings _appSettings;
        private readonly MovieSocialNetworkDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PostService(
            IOptions<AppSettings> appSettings,
            IMapper mapper,
            ILogger<PostService> logger,
            MovieSocialNetworkDbContext context
        )
        {
            _appSettings = appSettings.Value;
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        public async Task<PagedList<Post>> GetList(Paging paging, Sorting sorting, string q)
        {
            try
            {
                var posts = _context.Posts.AsQueryable();

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
                
                PagedList<Post> result = new PagedList<Post>
                {
                    TotalCount = await posts.CountAsync(),
                    PageSize = paging.PageSize,
                    Page = paging.PageNumber,
                    SortBy = sorting.SortBy,
                    SortOrder = sorting.SortOrder,
                };

                result.Items = await posts.Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize).ToListAsync();
                result.TotalPages = (result.TotalCount % result.PageSize > 0) ? (result.TotalCount / result.PageSize + 1) : (result.TotalCount / result.PageSize);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<Post> GetById(int id)
        {
            try
            {
                return await _context.Posts.SingleOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        Task<Post> IPostService.Create(CreatePostCommand command)
        {
            throw new NotImplementedException();
        }

        Task<bool> IPostService.React(CreateReactionCommand command)
        {
            throw new NotImplementedException();
        }

        Task<bool> IPostService.Report(ReportCommand command)
        {
            throw new NotImplementedException();
        }

        Task<bool> IPostService.Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
