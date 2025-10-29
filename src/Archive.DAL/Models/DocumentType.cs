namespace Archive.DAL.Models
{
    public class DocumentType
    {
        public DocumentType()
        {
            
        }
        public DocumentType(int documentTypeID, string? typeName)
        {
            DocumentTypeID = documentTypeID;
            TypeName = typeName;
        }

        public int DocumentTypeID { get; set; }
        public string? TypeName { get; set; }

    }
}
