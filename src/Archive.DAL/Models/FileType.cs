namespace Archive.DAL.Models
{
    public class FileType
    {
        public FileType()
        {
            
        }
        public FileType(int fileTypeID, string? typeName)
        {
            FileTypeID = fileTypeID;
            TypeName = typeName;
        }

        public int FileTypeID { get; set; }
        public string? TypeName { get; set; }

    }
}
