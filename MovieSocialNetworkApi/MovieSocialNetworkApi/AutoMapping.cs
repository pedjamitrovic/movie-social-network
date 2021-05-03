using AutoMapper;
using MovieSocialNetworkApi.Entities;
using MovieSocialNetworkApi.Models;

namespace MovieSocialNetworkApi
{
    public class AutoMapping: Profile
    {
        public AutoMapping()
        {
            CreateMap<User, AuthenticationInfo>()
                .ForMember(dest => dest.QualifiedName, opt => opt.MapFrom(src => src.Username))
                .ReverseMap();
            CreateMap<Group, AuthenticationInfo>()
                .ForMember(dest => dest.QualifiedName, opt => opt.MapFrom(src => src.Title))
                .ReverseMap();
            CreateMap<User, UserVM>()
                .ForMember(dest => dest.QualifiedName, opt => opt.MapFrom(src => src.Username))
                .ReverseMap();
            CreateMap<Group, GroupVM>()
                .ForMember(dest => dest.QualifiedName, opt => opt.MapFrom(src => src.Title))
                .ReverseMap();
            CreateMap<SystemEntityVM, SystemEntity>()
                .Include<UserVM, User>()
                .Include<GroupVM, Group>()
                .ReverseMap();
            CreateMap<PostVM, Post>().ReverseMap();
            CreateMap<CommentVM, Comment>().ReverseMap();
            CreateMap<ReactionVM, Reaction>().ReverseMap();
            CreateMap<MessageVM, Message>().ReverseMap();
        }
    }
}
