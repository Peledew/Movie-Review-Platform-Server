using Microsoft.EntityFrameworkCore;
using MovieReviewerPlatform.Contracts.Interfaces;
using MovieReviewerPlatform.Domain.Entities;
using MovieReviewerPlatform.Infrastructure.Context;

namespace MovieReviewerPlatform.Infrastructure.Repositories
{
    public class UserCommentRepository : IUserCommentRepository
    {
        private readonly AppDbContext _context;

        public UserCommentRepository(AppDbContext context) 
        {
            _context = context;
        }
        public async Task AddAsync(UserComment comment)
        {
            await _context.UserComments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserComment comment)
        {
            _context.UserComments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserComment>> GetAllAsync()
        {
            return await _context.UserComments
                .Include(u => u.User)
                .ToListAsync();
        }

        public async Task<UserComment?> GetByIdAsync(int id)
        {
            return await _context.UserComments.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
