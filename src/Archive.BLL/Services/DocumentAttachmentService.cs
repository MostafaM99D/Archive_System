using Archive.DAL.Models;
using Archive.DAL.Repositories;

namespace Archive.BLL.Services
{
    public class DocumentAttachmentService
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode = enMode.AddNew;

        public int DocumentAttachmentID { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
        public int FileTypeID { get; set; }
        public int DocumentID { get; set; }

        public DocumentAttachmentService() { }

        public DocumentAttachmentService(DocumentAttachment documentAttachment, enMode mode = enMode.AddNew)
        {
            DocumentAttachmentID = documentAttachment.DocumentAttachmentID;
            FileName = documentAttachment.FileName;
            FilePath = documentAttachment.FilePath;
            UploadedAt = documentAttachment.UploadedAt;
            FileTypeID = documentAttachment.FileTypeID;
            DocumentID = documentAttachment.DocumentID;
            _Mode = mode;
        }

        private DocumentAttachment ToDTO() =>
            new(DocumentAttachmentID, FileName, FilePath, UploadedAt, FileTypeID, DocumentID);

        public static async Task<List<DocumentAttachment>> GetAll()
            => await DocumentAttachmentRepository.GetAllAsync();

        public static async Task<DocumentAttachmentService?> GetByID(int id)
        {
            var data = await DocumentAttachmentRepository.GetByIdAsync(id);
            return data != null ? new DocumentAttachmentService(data, enMode.Update) : null;
        }

        private async Task<bool> _AddNew()
        {
            DocumentAttachmentID = await DocumentAttachmentRepository.AddAsync(ToDTO());
            return DocumentAttachmentID != -1;
        }

        private async Task<bool> _Update()
            => await DocumentAttachmentRepository.UpdateAsync(ToDTO());

        public async Task<bool> Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (await _AddNew())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return await _Update();
                default:
                    return false;
            }
        }

        public static async Task<bool> Delete(int id)
            => await DocumentAttachmentRepository.DeleteAsync(id);
    }
}
