namespace Archive.API.DTOs
{
    public class DocumentDTO
    {
        public int DocumentID { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int Year { get; set; }
        public string? Notes { get; set; }
        public long InternalNumber { get; set; }

        public int UserID { get; set; }
        public int DocumentTypeID { get; set; }
        public int DepartmentID { get; set; }
    }
}
