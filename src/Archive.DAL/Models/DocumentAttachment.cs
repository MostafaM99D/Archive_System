using Archive.DAL.Models;

public class DocumentAttachment
{
    public DocumentAttachment() { }

    public DocumentAttachment(int documentAttachmentID, string? fileName, string? filePath, DateTime uploadedAt, int fileTypeID, int documentID)
    {
        DocumentAttachmentID = documentAttachmentID;
        FileName = fileName;
        FilePath = filePath;
        UploadedAt = uploadedAt;
        FileTypeID = fileTypeID;
        DocumentID = documentID;
    }

    public int DocumentAttachmentID { get; set; }
    public string? FileName { get; set; }
    public string? FilePath { get; set; }
    public DateTime UploadedAt { get; set; }
    public int FileTypeID { get; set; }
    public int DocumentID { get; set; }

    public FileType? FileType { get; set; }
    public Document? Document { get; set; }
}