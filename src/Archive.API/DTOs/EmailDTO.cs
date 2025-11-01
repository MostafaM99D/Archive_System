namespace Archive.API.DTOs
{
    public class EmailDTO
    {
        public int EmailID { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Year { get; set; }
        public string? Notes { get; set; }

        public int UserID { get; set; }
        public int EmailTypeID { get; set; }
        public int DepartmentID { get; set; }
    }
}
