using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using MovieReviewerPlatform.Contracts.DTOs;
using MovieReviewerPlatform.Contracts.Interfaces;
using MovieReviewerPlatform.Domain.Entities;
using MovieReviewerPlatform.Infrastructure.Hubs;

namespace MovieReviewerPlatform.Services
{
    public class UserCommentService : IUserCommentService
    {
        private readonly IUserCommentRepository _userCommentRepository;
        private readonly IMapper _mapper;
        private readonly IMovieService _movieService;
        private readonly IUserService _userService;
        private readonly IHubContext<CommentHub> _hubContext;

        public UserCommentService(IUserCommentRepository userCommentRepository, IMapper mapper, IMovieService movieService, IUserService userService, IHubContext<CommentHub> hubContext)
        {
            _userCommentRepository = userCommentRepository;
            _movieService = movieService;
            _mapper = mapper;
            _userService = userService;
            _hubContext = hubContext;
        }

        public async Task<string> AddAsync(UserCommentDto newComment)
        {
            if (newComment == null)
                return "Invalid data for comment.";

            var comment = _mapper.Map<UserComment>(newComment);

            comment.MovieId = newComment.MovieId;
            var currentId = _userService.GetCurrentUserId();
            comment.User = await _userService.GetByIdAsync(currentId);
            comment.UserId = currentId;
            await _userCommentRepository.AddAsync(comment);

            var commentDto = _mapper.Map<UserCommentDto>(comment);
            // Send the comment to all connected clients using SignalR
            await _hubContext.Clients.All.SendAsync("ReceiveComment", commentDto);

            return "UserComment Added!";
        }

        public async Task DeleteAsync(int id)
        {
            var userComment = await GetByIdAsync(id);
            if (userComment == null)
            {
                throw new Exception("UserComment not found");
            }

            await _userCommentRepository.DeleteAsync(_mapper.Map<UserComment>(userComment));
        }

        public async Task<List<UserCommentDto>> GetAllAsync()
        {
            return _mapper.Map<List<UserCommentDto>>(await _userCommentRepository.GetAllAsync());
        }

        public async Task<UserCommentDto?> GetByIdAsync(int id)
        {
            var comment = await _userCommentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                throw new KeyNotFoundException("Comment was not found.");
            }

            return _mapper.Map<UserCommentDto>(comment);
        }

        public async Task<UserCommentDto> UpdateAsync(int id, UserCommentDto newComment)
        {
            var oldComment = await _userCommentRepository.GetByIdAsync(id);
            if (oldComment == null)
            {
                throw new Exception("Comment not found.");
            }
            _mapper.Map(newComment, oldComment);

            await _userCommentRepository.SaveChangesAsync();
            return _mapper.Map<UserCommentDto>(oldComment);
        }
    }
}
