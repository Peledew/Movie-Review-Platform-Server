using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieReviewerPlatform.Domain.Entities
{
    [Table("UserComments")]
    public class UserComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }

        [StringLength(500)]
        public string? Comment {  get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("MovieId")]
        [JsonIgnore]
        public Movie Movie { get; set; }
    }
}
