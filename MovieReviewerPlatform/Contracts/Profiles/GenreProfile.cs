using AutoMapper;
using MovieReviewerPlatform.Contracts.DTOs;
using MovieReviewerPlatform.Domain.Entities;

namespace MovieReviewerPlatform.Contracts.Profiles
{
    public class GenreProfile : Profile
    {
        public GenreProfile() {
            CreateMap<Genre, GenreDto>().ReverseMap();
        }
    }
}
