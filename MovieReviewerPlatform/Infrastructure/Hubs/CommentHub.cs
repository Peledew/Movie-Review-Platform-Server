using Microsoft.AspNetCore.SignalR;

namespace MovieReviewerPlatform.Infrastructure.Hubs
{
    public class CommentHub : Hub
    {
        // When a user joins a movie page, add them to the group associated with that movie
        public async Task JoinMovieGroup(string movieId)
        {
            // Add the user to the group
            await Groups.AddToGroupAsync(Context.ConnectionId, movieId);
        }

        // When a user leaves a movie page, remove them from the group
        public async Task LeaveMovieGroup(string movieId)
        {
            // Remove the user from the group
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, movieId);
        }

        // Send a comment to all users in the same group (movie)
        public async Task SendCommentToMovieGroup(string movieId, string commentText)
        {
            // Send the comment to all users in the group
            await Clients.Group(movieId).SendAsync("ReceiveComment", movieId, commentText);
        }





    }
}
