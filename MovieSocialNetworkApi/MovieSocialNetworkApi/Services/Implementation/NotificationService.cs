using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieSocialNetworkApi.Database;
using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Exceptions;
using MovieSocialNetworkApi.Helpers;
using MovieSocialNetworkApi.Hubs;
using MovieSocialNetworkApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public class NotificationService : INotificationService
    {
        private readonly MovieSocialNetworkDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IAuthService _auth;
        private readonly IHubContext<ChatHub, IChatHub> _hubContext;

        public NotificationService(
            MovieSocialNetworkDbContext context,
            IMapper mapper,
            ILogger<UserService> logger,
            IAuthService auth,
            IHubContext<ChatHub, IChatHub> hubContext
        )
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _auth = auth;
            _hubContext = hubContext;
        }

        public async Task<PagedList<NotificationVM>> GetMyNotifications(Paging paging, Sorting sorting)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var notifications = _context.Notifications.Where(e => e.RecepientId == authSystemEntity.Id).Include(e => e.Sender).Include(e => e.Recepient).AsQueryable();

                if (string.IsNullOrWhiteSpace(sorting.SortBy))
                {
                    sorting.SortBy = "createdOn";
                }

                if (sorting.SortBy == "createdOn")
                {
                    if (sorting.SortOrder == SortOrder.Desc)
                    {
                        notifications = notifications.OrderByDescending((e) => e.CreatedOn);
                    }
                    else
                    {
                        notifications = notifications.OrderBy((e) => e.CreatedOn);
                    }
                }
                else
                {
                    throw new BusinessException($"Sorting by field {sorting.SortBy} is not supported");
                }

                PagedList<NotificationVM> result = new PagedList<NotificationVM>
                {
                    TotalCount = await notifications.CountAsync(),
                    PageSize = paging.PageSize,
                    Page = paging.PageNumber,
                    SortBy = sorting.SortBy,
                    SortOrder = sorting.SortOrder,
                };

                var items = await notifications.Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize).ToListAsync();
                result.Items = _mapper.Map<List<NotificationVM>>(items);
                result.TotalPages = (result.TotalCount % result.PageSize > 0) ? (result.TotalCount / result.PageSize + 1) : (result.TotalCount / result.PageSize);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<NotificationVM> CreateNotification(string type, int senderId, int recepientId, Dictionary<string, object> extended = null)
        {
            try
            {
                var notification = new Notification
                {
                    Type = type,
                    SenderId = senderId,
                    RecepientId = recepientId,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Seen = false,
                    Extended = JsonConvert.SerializeObject(extended),
                };

                _context.Notifications.Add(notification);

                await _context.SaveChangesAsync();

                var notificationVM = _mapper.Map<NotificationVM>(notification);

                await _hubContext.Clients.User(recepientId.ToString()).NotifyNewNotification(notificationVM);

                return notificationVM;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<int> GetMyUnseenNotificationCount()
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var count = await _context.Notifications.Where(n => n.RecepientId == authSystemEntity.Id && !n.Seen).CountAsync();

                return count;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
        public async Task<NotificationVM> SetNotificationSeen(int notificationId)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var notification = await _context.Notifications.Include(n => n.Sender).SingleOrDefaultAsync(n => n.Id == notificationId);
                if (notification == null) throw new BusinessException($"Notification with provided id not found");

                notification.Seen = true;

                await _context.SaveChangesAsync();

                return _mapper.Map<NotificationVM>(notification);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
    }
}
