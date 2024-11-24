using AutoMapper;
using MovieReviewerPlatform.Contracts.DTOs;
using MovieReviewerPlatform.Domain.Entities;

namespace MovieReviewerPlatform.Contracts.Profiles
{
    public class UserCommentProfile : Profile
    {
        public UserCommentProfile() {
            CreateMap<UserComment, UserCommentDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username));

            CreateMap<UserCommentDto, UserComment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UserCommentDto, UserComment>();
        }
    }
}
