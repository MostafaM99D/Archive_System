using Archive.DAL.Models;
using Archive.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.BLL.Services
{
    public class DocumentTypeService
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode = enMode.AddNew;

        public int DocumentTypeID { get; set; }
        public string TypeName { get; set; }

        public DocumentTypeService(DocumentType documentType, enMode mode = enMode.AddNew)
        {
            DocumentTypeID = documentType.DocumentTypeID;
            TypeName = documentType.TypeName!;
            _Mode = mode;
        }

        public DocumentType DocumentTypeDTO => new DocumentType
        {
            DocumentTypeID = this.DocumentTypeID,
            TypeName = this.TypeName
        };

        public static async Task<IEnumerable<DocumentType>> GetAllDocumentTypes()
        {
            return await DocumentTypeRepository.GetAllDocumentTypesAsync();
        }

        public static async Task<DocumentTypeService?> GetDocumentTypeByID(int documentTypeID)
        {
            var result = await DocumentTypeRepository.GetDocumentTypeByIDAsync(documentTypeID);
            return result != null ? new DocumentTypeService(result, enMode.Update) : null;
        }

        private async Task<bool> _AddNewDocumentType()
        {
            this.DocumentTypeID = await DocumentTypeRepository.AddNewDocumentTypeAsync(DocumentTypeDTO);
            return this.DocumentTypeID != -1;
        }

        private async Task<bool> _UpdateDocumentType()
        {
            return await DocumentTypeRepository.UpdateDocumentTypeAsync(DocumentTypeDTO);
        }

        public async Task<bool> Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewDocumentType())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return await _UpdateDocumentType();
                default:
                    return false;
            }
        }

        public static async Task<bool> DeleteDocumentType(int documentTypeID)
        {
            return await DocumentTypeRepository.DeleteDocumentTypeAsync(documentTypeID);
        }

        public static async Task<bool> IsDocumentTypeExist(int documentTypeID)
        {
            return await DocumentTypeRepository.IsDocumentTypeExistAsync(documentTypeID);
        }
    }
}
