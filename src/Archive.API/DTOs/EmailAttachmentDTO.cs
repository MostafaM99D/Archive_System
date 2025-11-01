namespace Archive.API.DTOs
{
    public class EmailAttachmentDTO
    {
        public int EmailAttachmentID { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public DateTime UploadedAt { get; set; }

        public int FileTypeID { get; set; }
        public int EmailID { get; set; }
    }
}
