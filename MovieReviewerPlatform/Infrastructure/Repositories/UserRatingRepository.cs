using Microsoft.EntityFrameworkCore;
using MovieReviewerPlatform.Contracts.Interfaces;
using MovieReviewerPlatform.Domain.Entities;
using MovieReviewerPlatform.Infrastructure.Context;

namespace MovieReviewerPlatform.Infrastructure.Repositories
{
    public class UserRatingRepository : IUserRatingRepository
    {
        private readonly AppDbContext _context;

        public UserRatingRepository(AppDbContext context) 
        {
            _context = context; 
        }
        public async Task AddAsync(UserRating rating)
        {
            await _context.UserRatings.AddAsync(rating);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserRating rating)
        {
            _context.UserRatings.Remove(rating);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserRating>> GetAllAsync()
        {
            return await _context.UserRatings.ToListAsync();
        }

        public async Task<UserRating?> GetByIdAsync(int userId, int movieId)
        {
            return await _context.UserRatings.FirstOrDefaultAsync(x => x.UserId == userId && x.MovieId == movieId);
        }

        public async Task<List<UserRating>> GetByMovieIdAsync(int id)
        {
            return await _context.UserRatings
                .Where(x => x.MovieId == id)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
