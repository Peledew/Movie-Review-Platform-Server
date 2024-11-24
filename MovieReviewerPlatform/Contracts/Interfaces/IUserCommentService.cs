using MovieReviewerPlatform.Contracts.DTOs;

namespace MovieReviewerPlatform.Contracts.Interfaces
{
    public interface IUserCommentService
    {
        Task<List<UserCommentDto>> GetAllAsync();
        Task<UserCommentDto?> GetByIdAsync(int id);
        Task<string> AddAsync(UserCommentDto newComment);
        Task<UserCommentDto> UpdateAsync(int id, UserCommentDto newComment);
        Task DeleteAsync(int id);
    }
}
