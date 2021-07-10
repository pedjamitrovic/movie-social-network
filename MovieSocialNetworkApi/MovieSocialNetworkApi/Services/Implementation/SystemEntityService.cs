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
    public class SystemEntityService : ISystemEntityService
    {
        private readonly MovieSocialNetworkDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IAuthService _auth;
        private readonly AppSettings _appSettings;
        private readonly INotificationService _notificationService;

        public SystemEntityService(
            MovieSocialNetworkDbContext context,
            IMapper mapper,
            ILogger<SystemEntityService> logger,
            IAuthService auth,
            IOptions<AppSettings> appSettings,
            INotificationService notificationService
        )
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _auth = auth;
            _appSettings = appSettings.Value;
            _notificationService = notificationService;
        }

        public async Task<SystemEntityVM> GetById(int id)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var sysEntity = await _context.SystemEntities.SingleOrDefaultAsync(e => e.Id == id);
                return _mapper.Map<SystemEntityVM>(sysEntity);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<PagedList<SystemEntityVM>> GetList(Paging paging, Sorting sorting, string q)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var sysEntities = _context.SystemEntities.AsQueryable();

                if (!string.IsNullOrEmpty(q))
                {
                    sysEntities = sysEntities.Where((e) => e.Description.ToLower().Contains(q.ToLower()));
                }

                if (string.IsNullOrWhiteSpace(sorting.SortBy))
                {
                    sorting.SortBy = "followers";
                }

                if (sorting.SortBy == "followers")
                {
                    if (sorting.SortOrder == SortOrder.Desc)
                    {
                        sysEntities = sysEntities.Include(e => e.Followers).OrderByDescending((e) => e.Followers.Count);
                    }
                    else
                    {
                        sysEntities = sysEntities.Include(e => e.Followers).OrderBy((e) => e.Followers.Count);
                    }
                }
                else
                {
                    throw new BusinessException($"Sorting by field {sorting.SortBy} is not supported");
                }

                PagedList<SystemEntityVM> result = new PagedList<SystemEntityVM>
                {
                    TotalCount = await sysEntities.CountAsync(),
                    PageSize = paging.PageSize,
                    Page = paging.PageNumber,
                    SortBy = sorting.SortBy,
                    SortOrder = sorting.SortOrder,
                };

                var items = await sysEntities.Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize).ToListAsync();
                result.Items = _mapper.Map<List<SystemEntityVM>>(items);
                result.TotalPages = (result.TotalCount % result.PageSize > 0) ? (result.TotalCount / result.PageSize + 1) : (result.TotalCount / result.PageSize);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task Report(int id, ReportCommand command)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");
                if (authSystemEntity.Id == id) throw new ForbiddenException($"User has no permission to report himself");

                var sysEntity = await _context.SystemEntities.Include(e => e.ReportedReports).SingleOrDefaultAsync(e => e.Id == id);
                if (sysEntity == null) throw new BusinessException($"System entity with {id} not found");

                var existingReport = sysEntity.ReportedReports.Where((e) => e.ReporterId == authSystemEntity.Id && e.ReportedSystemEntityId == id && !e.Reviewed).FirstOrDefault();

                if (existingReport != null)
                {
                    throw new BusinessException($"User {sysEntity.Id} already has active report by user {authSystemEntity.Id}", BusinessErrorCode.AlreadyExists);
                }

                var report = new Report
                {
                    Reason = command.Reason,
                    Reporter = authSystemEntity,
                    ReportedSystemEntity = sysEntity
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

        public async Task ReviewReport(int id, ReviewReportCommand command)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");
                if (authSystemEntity.Role != Role.Admin) throw new ForbiddenException($"User has no permission to ban user with id {id}");

                var sysEntity = await _context.SystemEntities.SingleOrDefaultAsync(e => e.Id == id);
                if (sysEntity == null) throw new BusinessException($"System entity with {id} not found");

                var activeReports = await _context.Reports.Where((r) => r.ReportedSystemEntity == sysEntity && !r.Reviewed).ToListAsync();

                activeReports.ForEach((r) => r.Reviewed = true);

                if (command.IssueBan)
                {
                    var ban = new Ban
                    {
                        BannedFrom = DateTimeOffset.UtcNow,
                        BannedUntil = command.BannedUntil,
                        Reason = command.Reason,
                        BannedEntity = sysEntity
                    };

                    _context.Bans.Add(ban);

                    activeReports.ForEach((r) => r.IssuedBan = ban);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task ChangeImage(int id, string type, string imagePath)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"User not found");
                if (authSystemEntity.Id != id) throw new ForbiddenException($"User has no permission to change image of user with id {id}");

                var sysEntity = await _context.SystemEntities.SingleOrDefaultAsync(e => e.Id == id);
                if (sysEntity == null) throw new BusinessException($"System entity with id {id} not found");

                switch (type)
                {
                    case ImageType.Profile:
                        sysEntity.ProfileImagePath = imagePath;
                        break;
                    case ImageType.Cover:
                        sysEntity.CoverImagePath = imagePath;
                        break;
                    default:
                        throw new BusinessException($"Image type ${type} not supported");
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task ChangeDescription(int id, ChangeDescriptionCommand command)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var sysEntity = await _context.SystemEntities.SingleOrDefaultAsync(e => e.Id == id);
                if (sysEntity == null) throw new BusinessException($"System entity with {id} not found");

                sysEntity.Description = command.Description;

                _context.SystemEntities.Update(sysEntity);

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }


        public async Task Follow(int id)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var sysEntity = await _context.SystemEntities.SingleOrDefaultAsync(e => e.Id == id);
                if (sysEntity == null) throw new BusinessException($"System entity with id {id} not found");

                var relation = new Relation
                {
                    FollowerId = authSystemEntity.Id,
                    FollowingId = sysEntity.Id,
                };

                var existingRelation = await _context.Relations.FindAsync(relation.FollowingId, relation.FollowerId);

                if (existingRelation == null)
                {
                    _context.Relations.Add(relation);

                    await _context.SaveChangesAsync();

                    await _notificationService.CreateNotification(NotificationType.Follow, relation.FollowerId, relation.FollowingId);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task Unfollow(int id)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var sysEntity = await _context.SystemEntities.SingleOrDefaultAsync(e => e.Id == id);
                if (sysEntity == null) throw new BusinessException($"System entity with {id} not found");

                var relation = new Relation
                {
                    FollowerId = authSystemEntity.Id,
                    FollowingId = sysEntity.Id,
                };

                var existingRelation = await _context.Relations.FindAsync(relation.FollowingId, relation.FollowerId);

                if (existingRelation != null)
                {
                    _context.Relations.Remove(existingRelation);

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<PagedList<SystemEntityVM>> GetFollowers(int id, Paging paging, Sorting sorting)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var sysEntity = await _context.SystemEntities.Include(e => e.Followers).SingleOrDefaultAsync(e => e.Id == id);
                if (sysEntity == null) throw new BusinessException($"System entity with id {id} not found");

                foreach (var relation in sysEntity.Followers)
                {
                    await _context.Entry(relation).Reference(e => e.Follower).LoadAsync();
                }

                var followers = sysEntity.Followers.Select(e => e.Follower);

                if (string.IsNullOrWhiteSpace(sorting.SortBy))
                {
                    sorting.SortBy = "followers";
                }

                if (sorting.SortBy == "followers")
                {
                    foreach (var follower in followers)
                    {
                        await _context.Entry(follower).Collection(e => e.Followers).LoadAsync();
                    }
                    if (sorting.SortOrder == SortOrder.Desc)
                    {
                        followers = followers.OrderByDescending((e) => e.Followers.Count);
                    }
                    else
                    {
                        followers = followers.OrderBy((e) => e.Followers.Count);
                    }
                }
                else
                {
                    throw new BusinessException($"Sorting by field {sorting.SortBy} is not supported");
                }

                var result = new PagedList<SystemEntityVM>
                {
                    TotalCount = followers.Count(),
                    PageSize = paging.PageSize,
                    Page = paging.PageNumber,
                    SortBy = sorting.SortBy,
                    SortOrder = sorting.SortOrder,
                };

                var items = followers.Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize).ToList();
                result.Items = _mapper.Map<List<SystemEntity>, List<SystemEntityVM>>(items);
                result.TotalPages = (result.TotalCount % result.PageSize > 0) ? (result.TotalCount / result.PageSize + 1) : (result.TotalCount / result.PageSize);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<PagedList<SystemEntityVM>> GetFollowing(int id, Paging paging, Sorting sorting)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var sysEntity = await _context.SystemEntities.Include(e => e.Following).SingleOrDefaultAsync(e => e.Id == id);
                if (sysEntity == null) throw new BusinessException($"System entity with id {id} not found");

                foreach (var relation in sysEntity.Following)
                {
                    await _context.Entry(relation).Reference(e => e.Following).LoadAsync();
                }

                var following = sysEntity.Following.Select(e => e.Following);

                if (string.IsNullOrWhiteSpace(sorting.SortBy))
                {
                    sorting.SortBy = "followers";
                }

                if (sorting.SortBy == "followers")
                {
                    foreach (var followingSystemEntity in following)
                    {
                        await _context.Entry(followingSystemEntity).Collection(e => e.Followers).LoadAsync();
                    }
                    if (sorting.SortOrder == SortOrder.Desc)
                    {
                        following = following.OrderByDescending((e) => e.Followers.Count);
                    }
                    else
                    {
                        following = following.OrderBy((e) => e.Followers.Count);
                    }
                }
                else
                {
                    throw new BusinessException($"Sorting by field {sorting.SortBy} is not supported");
                }

                var result = new PagedList<SystemEntityVM>
                {
                    TotalCount = following.Count(),
                    PageSize = paging.PageSize,
                    Page = paging.PageNumber,
                    SortBy = sorting.SortBy,
                    SortOrder = sorting.SortOrder,
                };

                var items = following.Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize).ToList();
                result.Items = _mapper.Map<List<SystemEntity>, List<SystemEntityVM>>(items);
                result.TotalPages = (result.TotalCount % result.PageSize > 0) ? (result.TotalCount / result.PageSize + 1) : (result.TotalCount / result.PageSize);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<PagedList<PostVM>> GetPosts(int id, Paging paging, Sorting sorting)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var posts = _context.Contents.OfType<Post>().AsQueryable();

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

                var result = new PagedList<PostVM>
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

        public async Task<PagedList<CommentVM>> GetComments(int id, Paging paging, Sorting sorting)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var comments = _context.Contents.OfType<Comment>().AsQueryable();

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

                var result = new PagedList<CommentVM>
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
        public async Task<PagedList<ReportedDetails>> GetBannable(Paging paging, Sorting sorting)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var sysEntities = _context.SystemEntities.Include(e => e.ReportedReports).Where(e => e.ReportedReports.Where(e => !e.Reviewed).ToList().Count > _appSettings.MinReportsCount).AsQueryable();

                if (string.IsNullOrWhiteSpace(sorting.SortBy))
                {
                    sorting.SortBy = "reportedReports";
                }

                if (sorting.SortBy == "reportedReports")
                {
                    if (sorting.SortOrder == SortOrder.Desc)
                    {
                        sysEntities = sysEntities.OrderByDescending((e) => e.ReportedReports.Count);
                    }
                    else
                    {
                        sysEntities = sysEntities.OrderBy((e) => e.ReportedReports.Count);
                    }
                }
                else
                {
                    throw new BusinessException($"Sorting by field {sorting.SortBy} is not supported");
                }

                PagedList<ReportedDetails> result = new PagedList<ReportedDetails>
                {
                    TotalCount = await sysEntities.CountAsync(),
                    PageSize = paging.PageSize,
                    Page = paging.PageNumber,
                    SortBy = sorting.SortBy,
                    SortOrder = sorting.SortOrder,
                };

                var bannableSysEntites = await sysEntities.Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize).ToListAsync();

                var items = new List<ReportedDetails>();

                foreach (var sysEntity in bannableSysEntites)
                {
                    var rd = sysEntity.GetReportedDetails(_mapper);
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
