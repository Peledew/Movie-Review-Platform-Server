using AutoMapper;
using MovieReviewerPlatform.Contracts.DTOs;
using MovieReviewerPlatform.Domain.Entities;

namespace MovieReviewerPlatform.Contracts.Profiles
{
    public class UserRatingProfile : Profile
    {
        public UserRatingProfile() {
            CreateMap<UserRating, UserRatingDto>().ReverseMap();
        }
    }
}
