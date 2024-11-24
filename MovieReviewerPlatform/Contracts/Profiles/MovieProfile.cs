using AutoMapper;
using MovieReviewerPlatform.Contracts.DTOs;
using MovieReviewerPlatform.Domain.Entities;

namespace MovieReviewerPlatform.Contracts.Profiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile() {
            CreateMap<MovieDto, Movie>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Movie, MovieDto>().ReverseMap();

        }
    }
}
