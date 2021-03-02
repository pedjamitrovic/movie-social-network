using AutoMapper;
using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Models;

namespace MovieSocialNetworkApi
{
    public class AutoMapping: Profile
    {
        public AutoMapping()
        {
            CreateMap<AuthenticatedUser, User>().ReverseMap();
            CreateMap<AbstractUserVM, User>().ReverseMap();
        }
    }
}
