namespace Archive.API.DTOs
{
    public class DocumentAttachmentDTO
    {
        public int DocumentAttachmentID { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public DateTime UploadedAt { get; set; }

        public int FileTypeID { get; set; }
        public int DocumentID { get; set; }
    }
}
