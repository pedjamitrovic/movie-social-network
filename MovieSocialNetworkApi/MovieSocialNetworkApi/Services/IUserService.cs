using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface IUserService
    {
        public Task<User> Register();
        AuthenticatedUser Login(AuthenticateCommand command);
        public Task<PagedList<UserVM>> GetList(Paging paging, Sorting sorting, string q);
        public Task<UserVM> GetById(int id);
        public Task<bool> Report(ReportCommand command);
        public Task<bool> Ban(BanCommand command);
        public Task<bool> Delete(int id);
        public Task<bool> ChangeImage(ChangeImageCommand command);
        public Task<bool> ChangeAbout(ChangeAboutCommand command);
        public Task<bool> Follow(int id);
        public Task<bool> Unfollow(int id);
        public Task<User> GetFollowers(int id);
        public Task<User> GetFollowing(int id);
    }
}
