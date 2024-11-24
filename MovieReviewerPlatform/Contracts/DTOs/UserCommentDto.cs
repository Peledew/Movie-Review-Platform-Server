namespace MovieReviewerPlatform.Contracts.DTOs
{
    public class UserCommentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string? UserName { get; set; }
        public string? Comment { get; set; }
    }
}
