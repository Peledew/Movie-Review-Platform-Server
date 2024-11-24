namespace MovieReviewerPlatform.Contracts.DTOs
{
    public class GenreDto
    {
        public int Id {  get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; } = "";
        public GenreDto() { }
        public GenreDto(string name, string description)
        {
            Name = name;
            Description = description;
        }

        
    }
}
