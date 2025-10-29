using Archive.DAL.Models;
using Archive.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.BLL.Services
{
    public class EmailService
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode _Mode = enMode.AddNew;

        public Email EmailDTO => new Email(
            EmailID,
            Content,
            CreatedAt,
            Year,
            Notes,
            UserID,
            EmailTypeID,
            DepartmentID
        );

        public int EmailID { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Year { get; set; }
        public string? Notes { get; set; }
        public int UserID { get; set; }
        public int EmailTypeID { get; set; }
        public int DepartmentID { get; set; }

        
        public User? User { get; set; }
        public EmailType? EmailType { get; set; }
        public Department? Department { get; set; }

        public EmailService(Email email, enMode mode = enMode.AddNew)
        {
            EmailID = email.EmailID;
            Content = email.Content;
            CreatedAt = email.CreatedAt;
            Year = email.Year;
            Notes = email.Notes;
            UserID = email.UserID;
            EmailTypeID = email.EmailTypeID;
            DepartmentID = email.DepartmentID;

            User = email.User;
            EmailType = email.EmailType;
            Department = email.Department;

            _Mode = mode;
        }

        public static async Task<List<Email>> GetAllEmails()
        {
            return await EmailRepository.GetAllEmailsAsync();
        }

        public static async Task<EmailService?> GetEmailByID(int id)
        {
            var res = await EmailRepository.GetEmailByIDAsync(id);
            return res == null ? null : new EmailService(res, enMode.Update);
        }

        private async Task<bool> _AddNewEmail()
        {
            this.EmailID = await EmailRepository.AddNewEmailAsync(EmailDTO);
            return this.EmailID != -1;
        }

        private async Task<bool> _UpdateEmail()
        {
            return await EmailRepository.UpdateEmailAsync(EmailDTO);
        }

        public async Task<bool> Save()
        {
            return _Mode switch
            {
                enMode.AddNew => await _AddNewEmail(),
                enMode.Update => await _UpdateEmail(),
                _ => false
            };
        }

        public static async Task<bool> DeleteEmail(int id)
        {
            return await EmailRepository.DeleteEmailAsync(id);
        }

        public static async Task<bool> IsEmailExist(int id)
        {
            return await EmailRepository.IsEmailExistAsync(id);
        }
    }
}
