using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
    public class ContentService : IContentService
    {
        private readonly MovieSocialNetworkDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IAuthService _auth;
        private readonly AppSettings _appSettings;

        public ContentService(
            MovieSocialNetworkDbContext context,
            IMapper mapper,
            ILogger<PostService> logger,
            IAuthService auth,
            IOptions<AppSettings> appSettings
        )
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _auth = auth;
            _appSettings = appSettings.Value;
        }

        public async Task React(int id, CreateReactionCommand command)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var content = await _context.Contents.Include(e => e.Reactions).ThenInclude(e => e.Owner).SingleOrDefaultAsync(e => e.Id == id);
                if (content == null) throw new BusinessException($"Content with {id} not found");

                var existingReaction = content.Reactions.ToList().Find(e => e.Owner.Id == authSystemEntity.Id);

                var reaction = new Reaction
                {
                    Value = command.Value,
                    Owner = authSystemEntity,
                    Content = content
                };

                _context.Reactions.Add(reaction);

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }

        public async Task Report(int id, ReportCommand command)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");
                if (authSystemEntity.Id == id) throw new ForbiddenException($"User has no permission to report his posts");

                var post = await _context.Contents.OfType<Post>().Include(e => e.ReportedReports).SingleOrDefaultAsync(e => e.Id == id);
                if (post == null) throw new BusinessException($"Post with {id} not found");

                var existingReport = post.ReportedReports.ToList().Find(e => e.ReporterId == authSystemEntity.Id);

                if (existingReport != null)
                {
                    throw new BusinessException($"Post {post.Id} already has active report by user {authSystemEntity.Id}");
                }

                var report = new Report
                {
                    Reason = command.Reason,
                    Reporter = authSystemEntity,
                    ReportedContent = post,
                };

                _context.Reports.Add(report);

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var post = await _context.Contents.OfType<Post>().Include(e => e.Creator).Include(e => e.ReportedReports).SingleOrDefaultAsync(e => e.Id == id);
                if (post == null) throw new BusinessException($"Post with {id} not found");

                if (authSystemEntity.Id != post.Creator.Id && authSystemEntity.Role != Role.Admin) throw new ForbiddenException($"User has no permission to delete post with id {id}");
                if (post.ReportedReports.Count >= _appSettings.MinReportsCount) throw new ForbiddenException($"Post can't be deleted as it's currently being reviewed because users have reported it multiple times");

                _context.Contents.Remove(post);

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<PagedList<ReportedDetails>> GetBannable(Paging paging, Sorting sorting)
        {
            try
            {
                var contents = _context.Contents.Include(e => e.ReportedReports).Where(e => e.ReportedReports.Count > _appSettings.MinReportsCount).AsQueryable();

                if (string.IsNullOrWhiteSpace(sorting.SortBy))
                {
                    sorting.SortBy = "reportedReports";
                }

                if (sorting.SortBy == "reportedReports")
                {
                    if (sorting.SortOrder == SortOrder.Desc)
                    {
                        contents = contents.OrderByDescending((e) => e.ReportedReports.Count);
                    }
                    else
                    {
                        contents = contents.OrderBy((e) => e.ReportedReports.Count);
                    }
                }
                else
                {
                    throw new BusinessException($"Sorting by field {sorting.SortBy} is not supported");
                }

                PagedList<ReportedDetails> result = new PagedList<ReportedDetails>
                {
                    TotalCount = await contents.CountAsync(),
                    PageSize = paging.PageSize,
                    Page = paging.PageNumber,
                    SortBy = sorting.SortBy,
                    SortOrder = sorting.SortOrder,
                };

                var bannableContents = await contents.Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize).ToListAsync();

                var items = new List<ReportedDetails>();

                foreach (var content in bannableContents)
                {
                    var rd = content.GetReportedDetails();
                    items.Add(rd);
                }

                result.Items = items;
                result.TotalPages = (result.TotalCount % result.PageSize > 0) ? (result.TotalCount / result.PageSize + 1) : (result.TotalCount / result.PageSize);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
    }
}
