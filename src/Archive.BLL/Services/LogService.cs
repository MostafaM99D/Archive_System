using Archive.DAL.Models;
using Archive.DAL.Repositories;

namespace Archive.BLL.Services
{
    public class LogService
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode = enMode.AddNew;

        public int LogID { get; set; }
        public int OperationID { get; set; }
        public int? DocumentID { get; set; }
        public int UserID { get; set; }
        public int? EmailID { get; set; }
        public int StatusID { get; set; }
        public string? DeviceInfo { get; set; }
        public DateTime LogDate { get; set; }

        public LogService() { }

        public LogService(Log log, enMode mode = enMode.AddNew)
        {
            LogID = log.LogID;
            OperationID = log.OperationID;
            DocumentID = log.DocumentID;
            UserID = log.UserID;
            EmailID = log.EmailID;
            StatusID = log.StatusID;
            DeviceInfo = log.DeviceInfo;
            LogDate = log.LogDate;
            _Mode = mode;
        }

        private Log ToDTO() =>
            new(LogID, OperationID, DocumentID, UserID, EmailID, StatusID, DeviceInfo, LogDate);

        public static async Task<List<Log>> GetAll()
            => await LogRepository.GetAllAsync();

        public static async Task<LogService?> GetByID(int id)
        {
            var data = await LogRepository.GetByIdAsync(id);
            return data != null ? new LogService(data, enMode.Update) : null;
        }

        private async Task<bool> _AddNew()
        {
            LogID = await LogRepository.AddAsync(ToDTO());
            return LogID != -1;
        }

        private async Task<bool> _Update()
            => await LogRepository.UpdateAsync(ToDTO());

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
            => await LogRepository.DeleteAsync(id);
    }
}
