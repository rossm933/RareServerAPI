namespace RareServerAPI.Models
{
    public class Tags
    {
        public int TagId { get; set; }
        public string? Label { get; set; }
        public Posts? Posts { get; set; }
    }
}
