using AutoMapper;
using MovieReviewerPlatform.Contracts.DTOs;
using MovieReviewerPlatform.Domain.Entities;

namespace MovieReviewerPlatform.Contracts.Profiles
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile() { 
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
