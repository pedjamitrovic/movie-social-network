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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public class ChatRoomService : IChatRoomService
    {
        private readonly MovieSocialNetworkDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IAuthService _auth;
        private readonly IHubContext<ChatHub, IChatHub> _hubContext;

        public ChatRoomService(
            MovieSocialNetworkDbContext context,
            IMapper mapper,
            ILogger<ChatRoomService> logger,
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

        public async Task<PagedList<ChatRoomVM>> GetMyChatRooms(Paging paging, Sorting sorting)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var chatRooms = _context.ChatRooms
                    .Include(e => e.Memberships)
                    .Include(e => e.Messages)
                    .Where(e => e.Memberships.Any(m => m.MemberId == authSystemEntity.Id))
                    .AsQueryable();

                if (string.IsNullOrWhiteSpace(sorting.SortBy))
                {
                    sorting.SortBy = "createdOn";
                }

                if (sorting.SortBy == "createdOn")
                {
                    if (sorting.SortOrder == SortOrder.Desc)
                    {
                        chatRooms = chatRooms.OrderByDescending((e) => e.Messages.OrderByDescending(m => m.CreatedOn).FirstOrDefault().CreatedOn);
                    }
                    else
                    {
                        chatRooms = chatRooms.OrderBy((e) => e.Messages.OrderBy(m => m.CreatedOn).FirstOrDefault().CreatedOn);
                    }
                }
                else
                {
                    throw new BusinessException($"Sorting by field {sorting.SortBy} is not supported");
                }

                PagedList<ChatRoomVM> result = new PagedList<ChatRoomVM>
                {
                    TotalCount = await chatRooms.CountAsync(),
                    PageSize = paging.PageSize,
                    Page = paging.PageNumber,
                    SortBy = sorting.SortBy,
                    SortOrder = sorting.SortOrder,
                };

                var items = await chatRooms.Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize).ToListAsync();
                result.Items = _mapper.Map<List<ChatRoomVM>>(items);
                result.TotalPages = (result.TotalCount % result.PageSize > 0) ? (result.TotalCount / result.PageSize + 1) : (result.TotalCount / result.PageSize);
                
                foreach (var cr in result.Items)
                {
                    var newestMessage = await _context.Messages.Where(m => m.ChatRoomId == cr.Id).OrderByDescending(m => m.CreatedOn).FirstOrDefaultAsync();
                    cr.NewestMessage = _mapper.Map<MessageVM>(newestMessage);
                    var members = await _context.ChatRoomMemberships
                        .Include(crm => crm.Member)
                        .Where(crm => crm.ChatRoomId == cr.Id)
                        .Select(crm => crm.Member)
                        .ToListAsync();
                    cr.Members = _mapper.Map<List<SystemEntityVM>>(members);
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<ChatRoomVM> Create(ICollection<int> memberIds)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var chatRoom = new ChatRoom
                {
                    Memberships = new List<ChatRoomMembership>()
                };

                _context.ChatRooms.Add(chatRoom);

                foreach (var memberId in memberIds)
                {
                    chatRoom.Memberships.Add(new ChatRoomMembership { ChatRoom = chatRoom, MemberId = memberId });
                }

                chatRoom.Memberships.Add(new ChatRoomMembership { ChatRoom = chatRoom, MemberId = authSystemEntity.Id });

                await _context.SaveChangesAsync();

                var chatRoomVM = _mapper.Map<ChatRoomVM>(chatRoom);

                var members = await _context.ChatRoomMemberships
                    .Include(crm => crm.Member)
                    .Where(crm => crm.ChatRoomId == chatRoomVM.Id)
                    .Select(crm => crm.Member)
                    .ToListAsync();
                chatRoomVM.Members = _mapper.Map<List<SystemEntityVM>>(members);

                foreach (var memberId in memberIds)
                {
                    if (memberId == authSystemEntity.Id) continue;
                    await _hubContext.Clients.User(memberId.ToString()).NotifyChatRoomCreated(chatRoomVM);
                }

                return chatRoomVM;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<PagedList<MessageVM>> GetMessageList(Paging paging, Sorting sorting, int chatRoomId)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var chatRoomMembership = await _context.ChatRoomMemberships
                    .SingleOrDefaultAsync(e => e.ChatRoomId == chatRoomId && e.MemberId == authSystemEntity.Id);

                if (chatRoomMembership == null) throw new BusinessException($"Authenticated system entity is not memeber of chat room with id {chatRoomId}");

                var messages = _context.Messages.Where(e => e.ChatRoomId == chatRoomId).AsQueryable();

                if (string.IsNullOrWhiteSpace(sorting.SortBy))
                {
                    sorting.SortBy = "createdOn";
                }

                if (sorting.SortBy == "createdOn")
                {
                    if (sorting.SortOrder == SortOrder.Desc)
                    {
                        messages = messages.OrderByDescending((e) => e.CreatedOn);
                    }
                    else
                    {
                        messages = messages.OrderBy((e) => e.CreatedOn);
                    }
                }
                else
                {
                    throw new BusinessException($"Sorting by field {sorting.SortBy} is not supported");
                }

                PagedList<MessageVM> result = new PagedList<MessageVM>
                {
                    TotalCount = await messages.CountAsync(),
                    PageSize = paging.PageSize,
                    Page = paging.PageNumber,
                    SortBy = sorting.SortBy,
                    SortOrder = sorting.SortOrder,
                };

                var items = await messages.Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize).ToListAsync();
                result.Items = _mapper.Map<List<MessageVM>>(items);
                result.TotalPages = (result.TotalCount % result.PageSize > 0) ? (result.TotalCount / result.PageSize + 1) : (result.TotalCount / result.PageSize);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<MessageVM> CreateMessage(CreateMessageCommand command)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var chatRoomMembership = await _context.ChatRoomMemberships.Include(e => e.ChatRoom)
                    .SingleOrDefaultAsync(e => e.ChatRoomId == command.ChatRoomId && e.MemberId == authSystemEntity.Id);

                if (chatRoomMembership == null) throw new BusinessException($"Authenticated system entity is not memeber of chat room with id {command.ChatRoomId}");

                var message = new Message
                {
                    ChatRoomId = command.ChatRoomId,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Seen = false,
                    SenderId = authSystemEntity.Id,
                    Text = command.Text
                };

                _context.Messages.Add(message);

                await _context.SaveChangesAsync();

                var messageVM = _mapper.Map<MessageVM>(message);

                return messageVM;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<SystemEntityVM>> GetMembers(int chatRoomId)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var sysEntities = await _context.ChatRoomMemberships.Include(e => e.Member)
                    .Where(e => e.ChatRoomId == chatRoomId)
                    .Select(e => e.Member)
                    .ToListAsync();

                return _mapper.Map<List<SystemEntityVM>>(sysEntities);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<MessageVM> SetMessageSeen(int messageId)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var message = await _context.Messages.SingleOrDefaultAsync((m) => m.Id == messageId);
                if (message == null) throw new BusinessException($"Message with provided id not found");

                var previousUnseenMessages = await _context.Messages.Where(
                    (m) => m.ChatRoomId == message.ChatRoomId && m.SenderId != authSystemEntity.Id && !m.Seen
                ).ToListAsync();

                previousUnseenMessages.ForEach((m) => m.Seen = true);
                await _context.SaveChangesAsync();

                return _mapper.Map<MessageVM>(message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
    }
}
