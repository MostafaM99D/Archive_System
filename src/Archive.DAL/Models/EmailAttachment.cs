namespace Archive.DAL.Models
{
    public class EmailAttachment
    {
        public EmailAttachment() { }

        public EmailAttachment(int emailAttachmentID, string? fileName, string? filePath, DateTime uploadedAt, int fileTypeID, int emailID)
        {
            EmailAttachmentID = emailAttachmentID;
            FileName = fileName;
            FilePath = filePath;
            UploadedAt = uploadedAt;
            FileTypeID = fileTypeID;
            EmailID = emailID;
        }

        public int EmailAttachmentID { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
        public int FileTypeID { get; set; }
        public int EmailID { get; set; }

     
        public FileType? FileType { get; set; }
        public Email? Email { get; set; }
    }
}
