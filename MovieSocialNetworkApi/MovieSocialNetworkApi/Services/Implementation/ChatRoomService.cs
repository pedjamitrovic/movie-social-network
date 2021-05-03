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
    public class ChatRoomService : IChatRoomService
    {
        private readonly MovieSocialNetworkDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IAuthService _auth;

        public ChatRoomService(
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

        public async Task<PagedList<ChatRoomVM>> GetMyChatRooms(Paging paging, Sorting sorting)
        {
            try
            {
                var authSystemEntity = await _auth.GetAuthenticatedSystemEntity();
                if (authSystemEntity == null) throw new BusinessException($"Authenticated system entity not found");

                var chatRooms = _context.ChatRooms.Include(e => e.Memberships).Include(e => e.Messages).AsQueryable();

                if (string.IsNullOrWhiteSpace(sorting.SortBy))
                {
                    sorting.SortBy = "createdOn";
                }

                if (sorting.SortBy == "createdOn")
                {
                    if (sorting.SortOrder == SortOrder.Desc)
                    {
                        chatRooms = chatRooms.OrderByDescending((e) => e.Messages.LastOrDefault() != null ? e.Messages.LastOrDefault().CreatedOn: DateTimeOffset.UtcNow);
                    }
                    else
                    {
                        chatRooms = chatRooms.OrderBy((e) => e.Messages.LastOrDefault() != null ? e.Messages.LastOrDefault().CreatedOn : DateTimeOffset.UtcNow);
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
                result.Items.ForEach(async (e) => e.NewestMessage = _mapper.Map<MessageVM>(await _context.Messages.FirstOrDefaultAsync((q) => q.Id == e.Id)));

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
                    .Where(e => e.ChatRoomId == chatRoomId && e.MemberId == authSystemEntity.Id)
                    .SingleOrDefaultAsync();

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
                    .Where(e => e.ChatRoomId == command.ChatRoomId && e.MemberId == authSystemEntity.Id)
                    .SingleOrDefaultAsync();

                if (chatRoomMembership == null) throw new BusinessException($"Authenticated system entity is not memeber of chat room with id {command.ChatRoomId}");

                var message = new Message
                {
                    ChatRoomId = command.ChatRoomId,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Delivered = false,
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
    }
}
