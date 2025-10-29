using Archive.DAL.Models;
using Archive.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.BLL.Services
{
    public class FileTypeService
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode _Mode = enMode.AddNew;
        public FileType FileTypeDTO => new(FileTypeID, TypeName);
        public int FileTypeID { get; set; }
        public string? TypeName { get; set; }

        public FileTypeService(FileType fileType, enMode mode = enMode.AddNew)
        {
            FileTypeID = fileType.FileTypeID;
            TypeName = fileType.TypeName;
            _Mode = mode;
        }

        public static async Task<List<FileType>> GetAllFileTypes() => await FileTypeRepository.GetAllFileTypesAsync();
        public static async Task<FileTypeService?> GetFileTypeByID(int id)
        {
            var res = await FileTypeRepository.GetFileTypeByIDAsync(id);
            return res == null ? null : new FileTypeService(res, enMode.Update);
        }

        private async Task<bool> _AddNewFileType()
        {
            FileTypeID = await FileTypeRepository.AddNewFileTypeAsync(FileTypeDTO);
            return FileTypeID != -1;
        }

        private async Task<bool> _UpdateFileType() => await FileTypeRepository.UpdateFileTypeAsync(FileTypeDTO);

        public async Task<bool> Save()
        {
            return _Mode switch
            {
                enMode.AddNew => await _AddNewFileType(),
                enMode.Update => await _UpdateFileType(),
                _ => false
            };
        }

        public static async Task<bool> DeleteFileType(int id) => await FileTypeRepository.DeleteFileTypeAsync(id);
        public static async Task<bool> IsFileTypeExist(int id) => await FileTypeRepository.IsFileTypeExistAsync(id);
    }
}
