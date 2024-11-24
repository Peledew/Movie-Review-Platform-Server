namespace MovieReviewerPlatform.Contracts.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public GenreDto? Genre { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Directors { get; set; }

        public string Cast { get; set; }
        public string? ImageUrl { get; set; }
        public List<UserCommentDto> UserComments { get; set; } = new List<UserCommentDto>();
    }
}
