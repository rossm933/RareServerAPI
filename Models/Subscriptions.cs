namespace RareServerAPI.Models
{
    public class Subscriptions
    {
        public int? Id { get; set; }
        public int? AuthorId { get; set; }
        public int? FollowerId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
