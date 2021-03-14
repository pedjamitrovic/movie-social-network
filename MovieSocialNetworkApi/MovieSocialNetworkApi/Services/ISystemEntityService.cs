using MovieSocialNetworkApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public interface ISystemEntityService
    {
        public Task Report(int id, ReportCommand command);
        public Task Ban(int id, BanCommand command);
        public Task ChangeImage(int id, string type, string imagePath);
        public Task ChangeDescription(int id, ChangeDescriptionCommand command);
        public Task Follow(int id);
        public Task Unfollow(int id);
        public Task<PagedList<SystemEntityVM>> GetFollowers(int id, Paging paging, Sorting sorting);
        public Task<PagedList<SystemEntityVM>> GetFollowing(int id, Paging paging, Sorting sorting);
        public Task<PagedList<PostVM>> GetPosts(int id, Paging paging, Sorting sorting);
        public Task<PagedList<CommentVM>> GetComments(int id, Paging paging, Sorting sorting);
    }
}
