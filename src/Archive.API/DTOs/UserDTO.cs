namespace Archive.API.DTOs
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public long Permissions { get; set; }
        public bool IsActive { get; set; }
    }

   
}
