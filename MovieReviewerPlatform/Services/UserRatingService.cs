using AutoMapper;
using MovieReviewerPlatform.Contracts.DTOs;
using MovieReviewerPlatform.Contracts.Interfaces;
using MovieReviewerPlatform.Domain.Entities;

namespace MovieReviewerPlatform.Services
{
    public class UserRatingService : IUserRatingService
    {
        private readonly IUserRatingRepository _userRatingRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IMovieService _movieService;
        public UserRatingService(IMapper mapper, IUserRatingRepository userRatingRepository, IUserService userService, IMovieService movieService)
        {
            _mapper = mapper;
            _userRatingRepository = userRatingRepository;
            _userService = userService;
            _movieService = movieService;
        }
        public async Task<string> AddAsync(UserRatingDto newRating)
        {
            if (newRating == null)
                return "Invalid data for rating.";
            
            var rating = _mapper.Map<UserRating>(newRating);

            rating.MovieId = newRating.MovieId;
            var currentId = _userService.GetCurrentUserId();
             rating.User = await _userService.GetByIdAsync(currentId);
            await _userRatingRepository.AddAsync(rating);

            return "UserRating Added!";
        }

        public async Task<List<UserRatingDto>> GetAllAsync()
        {
            return _mapper.Map<List<UserRatingDto>>(await _userRatingRepository.GetAllAsync());
        }

        public async Task<UserRatingDto?> GetByIdAsync(int userId, int movieId)
        {
            var rating = await _userRatingRepository.GetByIdAsync(userId, movieId);
            if (rating == null)
            {
                throw new KeyNotFoundException("Rating was not found.");
            }

            return _mapper.Map<UserRatingDto>(rating);
        }

        public async Task<List<UserRatingDto>> GetByMovieIdAsync(int id)
        {
            var ratings = _mapper.Map<List<UserRatingDto>>(await _userRatingRepository.GetByMovieIdAsync(id));

            if (ratings == null)
            {
                throw new KeyNotFoundException("Rating was not found.");
            }

            return ratings;
        }

        public async Task<bool> TryRatingAsync(int movieId)
        {
            var ratings = _mapper.Map<List<UserRating>>(await _userRatingRepository.GetByMovieIdAsync(movieId));

            if (ratings == null)
            {
                throw new KeyNotFoundException("Ratings were not found.");
            }
            var currentUserId = _userService.GetCurrentUserId();

            foreach (var rating in ratings) { 
                if(rating.UserId == currentUserId)
                    return false;
            }
            return true;
        }


    }
}
