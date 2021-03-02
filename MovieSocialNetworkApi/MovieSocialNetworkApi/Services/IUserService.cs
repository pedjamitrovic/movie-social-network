using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface IUserService
    {
        public Task<AuthenticatedUser> Register(RegisterCommand command);
        public Task<AuthenticatedUser> Login(LoginCommand command);
        public Task<PagedList<AbstractUserVM>> GetList(Paging paging, Sorting sorting, string q);
        public Task<AbstractUserVM> GetById(int id);
        public Task Report(ReportCommand command);
        public Task Ban(BanCommand command);
        public Task ChangeImage(ChangeImageCommand command);
        public Task ChangeDescription(ChangeAboutCommand command);
        public Task Follow(int id);
        public Task Unfollow(int id);
        public Task<IEnumerable<AbstractUserVM>> GetFollowers(int id);
        public Task<IEnumerable<AbstractUserVM>> GetFollowing(int id);
    }
}
