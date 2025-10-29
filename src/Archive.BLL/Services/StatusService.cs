using Archive.DAL.Models;
using Archive.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.BLL.Services
{
    public class StatusService
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode _Mode = enMode.AddNew;
        public Status StatusDTO => new(StatusID, StatusName);
        public int StatusID { get; set; }
        public string? StatusName { get; set; }

        public StatusService(Status status, enMode mode = enMode.AddNew)
        {
            StatusID = status.StatusID;
            StatusName = status.StatusName;
            _Mode = mode;
        }

        public static async Task<List<Status>> GetAllStatuses() => await StatusRepository.GetAllStatusesAsync();
        public static async Task<StatusService?> GetStatusByID(int id)
        {
            var res = await StatusRepository.GetStatusByIDAsync(id);
            return res == null ? null : new StatusService(res, enMode.Update);
        }

        private async Task<bool> _AddNewStatus()
        {
            StatusID = await StatusRepository.AddNewStatusAsync(StatusDTO);
            return StatusID != -1;
        }

        private async Task<bool> _UpdateStatus() => await StatusRepository.UpdateStatusAsync(StatusDTO);

        public async Task<bool> Save()
        {
            return _Mode switch
            {
                enMode.AddNew => await _AddNewStatus(),
                enMode.Update => await _UpdateStatus(),
                _ => false
            };
        }

        public static async Task<bool> DeleteStatus(int id) => await StatusRepository.DeleteStatusAsync(id);
        public static async Task<bool> IsStatusExist(int id) => await StatusRepository.IsStatusExistAsync(id);
    }
}
