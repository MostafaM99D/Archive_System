using Archive.DAL.Models;
using Archive.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.BLL.Services
{
    public class EmailTypeService
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode = enMode.AddNew;

        public int EmailTypeID { get; set; }
        public string TypeName { get; set; }

        public EmailTypeService(EmailType emailType, enMode mode = enMode.AddNew)
        {
            EmailTypeID = emailType.EmailTypeID;
            TypeName = emailType.TypeName!;
            _Mode = mode;
        }

        public EmailType EmailTypeDTO => new EmailType
        {
            EmailTypeID = this.EmailTypeID,
            TypeName = this.TypeName
        };

        public static async Task<IEnumerable<EmailType>> GetAllEmailTypes()
        {
            return await EmailTypeRepository.GetAllEmailTypesAsync();
        }

        public static async Task<EmailTypeService?> GetEmailTypeByID(int emailTypeID)
        {
            var result = await EmailTypeRepository.GetEmailTypeByIDAsync(emailTypeID);
            return result != null ? new EmailTypeService(result, enMode.Update) : null;
        }

        private async Task<bool> _AddNewEmailType()
        {
            this.EmailTypeID = await EmailTypeRepository.AddNewEmailTypeAsync(EmailTypeDTO);
            return this.EmailTypeID != -1;
        }

        private async Task<bool> _UpdateEmailType()
        {
            return await EmailTypeRepository.UpdateEmailTypeAsync(EmailTypeDTO);
        }

        public async Task<bool> Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewEmailType())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return await _UpdateEmailType();
                default:
                    return false;
            }
        }

        public static async Task<bool> DeleteEmailType(int emailTypeID)
        {
            return await EmailTypeRepository.DeleteEmailTypeAsync(emailTypeID);
        }

        public static async Task<bool> IsEmailTypeExist(int emailTypeID)
        {
            return await EmailTypeRepository.IsEmailTypeExistAsync(emailTypeID);
        }
    }
}
