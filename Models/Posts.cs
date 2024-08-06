namespace RareServerAPI.Models
{
    public class Posts
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string? Title { get; set; }
        public DateTime PublishedOn { get; set; }
        public string? ImageUrl { get; set; }

        public string? Content { get; set; }

        public bool Approved { get; set; }

    }
}
