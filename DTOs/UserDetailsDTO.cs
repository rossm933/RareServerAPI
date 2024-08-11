namespace RareServerAPI.DTOs
{
    public class UserDetailsDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public string? Username { get; set; }
        public string? UserImage { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
