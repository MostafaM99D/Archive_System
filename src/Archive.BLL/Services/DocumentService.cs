using Archive.DAL.Models;
using Archive.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.BLL.Services
{
    public class DocumentService
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode _Mode = enMode.AddNew;

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

        public Document DocumentDTO =>
            new(DocumentID, Content, CreatedAt, Year, Notes, InternalNumber, UserID, DocumentTypeID, DepartmentID)
            {
                User = User,
                DocumentType = DocumentType,
                Department = Department
            };

        public DocumentService() { }

        public DocumentService(Document document, enMode mode = enMode.AddNew)
        {
            DocumentID = document.DocumentID;
            Content = document.Content;
            CreatedAt = document.CreatedAt;
            Year = document.Year;
            Notes = document.Notes;
            InternalNumber = document.InternalNumber;
            UserID = document.UserID;
            DocumentTypeID = document.DocumentTypeID;
            DepartmentID = document.DepartmentID;
            User = document.User;
            DocumentType = document.DocumentType;
            Department = document.Department;

            _Mode = mode;
        }

        public static async Task<List<Document>> GetAllDocuments()
        {
            return await DocumentRepository.GetAllDocumentsAsync();
        }

        public static async Task<DocumentService?> GetDocumentByID(int documentID)
        {
            var res = await DocumentRepository.GetDocumentByIDAsync(documentID);
            if (res == null)
                return null;
            return new DocumentService(res, enMode.Update);
        }

        private async Task<bool> _AddNewDocument()
        {
            this.DocumentID = await DocumentRepository.AddNewDocumentAsync(DocumentDTO);
            return this.DocumentID != -1;
        }

        private async Task<bool> _UpdateDocument()
        {
            return await DocumentRepository.UpdateDocumentAsync(DocumentDTO);
        }

        public async Task<bool> Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewDocument())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return await _UpdateDocument();

                default:
                    return false;
            }
        }

        public static async Task<bool> DeleteDocument(int documentID)
        {
            return await DocumentRepository.DeleteDocumentAsync(documentID);
        }

        public static async Task<bool> IsDocumentExist(int documentID)
        {
            return await DocumentRepository.IsDocumentExistAsync(documentID);
        }
    }
}
