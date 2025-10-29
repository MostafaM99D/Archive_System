using Archive.DAL.Models;
using Archive.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.BLL.Services
{
    public class OperationService
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode _Mode = enMode.AddNew;
        public Operation OperationDTO => new(OperationID, OperationName);
        public int OperationID { get; set; }
        public string? OperationName { get; set; }

        public OperationService(Operation operation, enMode mode = enMode.AddNew)
        {
            OperationID = operation.OperationID;
            OperationName = operation.OperationName;
            _Mode = mode;
        }

        public static async Task<List<Operation>> GetAllOperations() => await OperationRepository.GetAllOperationsAsync();

        public static async Task<OperationService?> GetOperationByID(int id)
        {
            var res = await OperationRepository.GetOperationByIDAsync(id);
            return res == null ? null : new OperationService(res, enMode.Update);
        }

        private async Task<bool> _AddNewOperation()
        {
            OperationID = await OperationRepository.AddNewOperationAsync(OperationDTO);
            return OperationID != -1;
        }

        private async Task<bool> _UpdateOperation() => await OperationRepository.UpdateOperationAsync(OperationDTO);

        public async Task<bool> Save()
        {
            return _Mode switch
            {
                enMode.AddNew => await _AddNewOperation(),
                enMode.Update => await _UpdateOperation(),
                _ => false
            };
        }

        public static async Task<bool> DeleteOperation(int id) => await OperationRepository.DeleteOperationAsync(id);
        public static async Task<bool> IsOperationExist(int id) => await OperationRepository.IsOperationExistAsync(id);
    }
}
