using Archive.DAL.Models;
using Archive.DAL.Repositories;

namespace Archive.BLL.Services
{
    public class EmailAttachmentService
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode = enMode.AddNew;

        public int EmailAttachmentID { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
        public int FileTypeID { get; set; }
        public int EmailID { get; set; }

        public EmailAttachmentService() { }

        public EmailAttachmentService(EmailAttachment emailAttachment, enMode mode = enMode.AddNew)
        {
            EmailAttachmentID = emailAttachment.EmailAttachmentID;
            FileName = emailAttachment.FileName;
            FilePath = emailAttachment.FilePath;
            UploadedAt = emailAttachment.UploadedAt;
            FileTypeID = emailAttachment.FileTypeID;
            EmailID = emailAttachment.EmailID;

            _Mode = mode;
        }

        private EmailAttachment EmailAttachmentDTO =>
            new EmailAttachment(EmailAttachmentID, FileName, FilePath, UploadedAt, FileTypeID, EmailID);

        public static async Task<List<EmailAttachment>> GetAll()
        {
            return await EmailAttachmentRepository.GetAllAsync();
        }

        public static async Task<EmailAttachmentService?> GetByID(int id)
        {
            var data = await EmailAttachmentRepository.GetByIdAsync(id);
            return data != null ? new EmailAttachmentService(data, enMode.Update) : null;
        }

        private async Task<bool> _AddNew()
        {
            EmailAttachmentID = await EmailAttachmentRepository.AddAsync(EmailAttachmentDTO);
            return EmailAttachmentID != -1;
        }

        private async Task<bool> _Update()
        {
            return await EmailAttachmentRepository.UpdateAsync(EmailAttachmentDTO);
        }

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
        {
            return await EmailAttachmentRepository.DeleteAsync(id);
        }
    }
}
