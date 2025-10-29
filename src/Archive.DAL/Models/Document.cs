namespace Archive.DAL.Models
{
    public class Document
    {
        public Document() { }

        public Document(int documentID, string? content, DateTime? createdAt, int year, string? notes,
                        long internalNumber, int userID, int documentTypeID, int departmentID)
        {
            DocumentID = documentID;
            Content = content;
            CreatedAt = createdAt;
            Year = year;
            Notes = notes;
            InternalNumber = internalNumber;
            UserID = userID;
            DocumentTypeID = documentTypeID;
            DepartmentID = departmentID;
        }

        public int DocumentID { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int Year { get; set; }
        public string? Notes { get; set; }
        public long InternalNumber { get; set; }
        public int UserID { get; set; }
        public int DocumentTypeID { get; set; }
        public int DepartmentID { get; set; }

        
        public User? User { get; set; }
        public DocumentType? DocumentType { get; set; }
        public Department? Department { get; set; }

    }
}
