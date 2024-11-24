using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieReviewerPlatform.Domain.Entities
{
    [Table("Movies")]
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        [ForeignKey("GenreId")]
        public Genre? Genre { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public string Directors { get; set; }

        [Required]
        public string Cast {  get; set; }
        public string? ImageUrl { get; set; }
        // Ratings for this movie
        public ICollection<UserRating>? UserRatings { get; set; }

        // Collection of UserComments for this Movie
        public ICollection<UserComment> UserComments { get; set; } = new List<UserComment>();


    }
}
