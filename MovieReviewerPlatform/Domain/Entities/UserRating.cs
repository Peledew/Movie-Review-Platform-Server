using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieReviewerPlatform.Domain.Entities
{
    [Table("UserRatings")]
    [PrimaryKey(nameof(UserId), nameof(MovieId))]
    public class UserRating
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public double Rating { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }
       

    }
}
