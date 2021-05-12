using MovieSocialNetworkApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface IChatRoomService
    {
        Task<PagedList<ChatRoomVM>> GetMyChatRooms(Paging paging, Sorting sorting);
        Task<ChatRoomVM> Create(ICollection<int> memberIds);
        Task<PagedList<MessageVM>> GetMessageList(Paging paging, Sorting sorting, int chatRoomId);
        Task<MessageVM> CreateMessage(CreateMessageCommand command);
        Task<IEnumerable<SystemEntityVM>> GetMembers(int chatRoomId);
        Task<MessageVM> SetMessageSeen(int messageId);
    }
}
