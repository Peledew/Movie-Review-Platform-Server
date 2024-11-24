namespace MovieReviewerPlatform.Contracts.DTOs
{
    public class UserRatingDto
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public double Rating { get; set; }
    }
}
