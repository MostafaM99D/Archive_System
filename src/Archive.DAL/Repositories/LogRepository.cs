using Archive.DAL.Global;
using Archive.DAL.Models;
using Microsoft.Data.SqlClient;

namespace Archive.DAL.Repositories
{
    public class LogRepository
    {
        private static readonly string _ConnectionString = Utility.GetConnectionString();

        public static async Task<List<Log>> GetAllAsync()
        {
            List<Log> list = new();
            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = "SELECT * FROM Logs";
                using SqlCommand cmd = new(query, conn);
                await conn.OpenAsync();
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new Log
                    {
                        LogID = Convert.ToInt32(reader["LogID"]),
                        OperationID = Convert.ToInt32(reader["OperationID"]),
                        DocumentID = reader["DocumentID"] != DBNull.Value ? Convert.ToInt32(reader["DocumentID"]) : null,
                        UserID = Convert.ToInt32(reader["UserID"]),
                        EmailID = reader["EmailID"] != DBNull.Value ? Convert.ToInt32(reader["EmailID"]) : null,
                        StatusID = Convert.ToInt32(reader["StatusID"]),
                        DeviceInfo = reader["DeviceInfo"].ToString(),
                        LogDate = Convert.ToDateTime(reader["LogDate"])
                    });
                }
            }
            return list;
        }

        public static async Task<Log?> GetByIdAsync(int id)
        {
            Log? log = null;
            using SqlConnection conn = new(_ConnectionString);
            string query = "SELECT * FROM Logs WHERE LogID=@id";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                log = new Log
                {
                    LogID = Convert.ToInt32(reader["LogID"]),
                    OperationID = Convert.ToInt32(reader["OperationID"]),
                    DocumentID = reader["DocumentID"] != DBNull.Value ? Convert.ToInt32(reader["DocumentID"]) : null,
                    UserID = Convert.ToInt32(reader["UserID"]),
                    EmailID = reader["EmailID"] != DBNull.Value ? Convert.ToInt32(reader["EmailID"]) : null,
                    StatusID = Convert.ToInt32(reader["StatusID"]),
                    DeviceInfo = reader["DeviceInfo"].ToString(),
                    LogDate = Convert.ToDateTime(reader["LogDate"])
                };
            }
            return log;
        }

        public static async Task<int> AddAsync(Log log)
        {
            using SqlConnection conn = new(_ConnectionString);
            string query = @"INSERT INTO Logs (OperationID, DocumentID, UserID, EmailID, StatusID, DeviceInfo, LogDate)
                             VALUES (@OperationID, @DocumentID, @UserID, @EmailID, @StatusID, @DeviceInfo, @LogDate);
                             SELECT SCOPE_IDENTITY();";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@OperationID", log.OperationID);
            cmd.Parameters.AddWithValue("@DocumentID", (object?)log.DocumentID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UserID", log.UserID);
            cmd.Parameters.AddWithValue("@EmailID", (object?)log.EmailID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@StatusID", log.StatusID);
            cmd.Parameters.AddWithValue("@DeviceInfo", log.DeviceInfo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@LogDate", log.LogDate);

            await conn.OpenAsync();
            object? result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : -1;
        }
        public static async Task<bool> UpdateAsync(Log log)
        {
            using SqlConnection conn = new(_ConnectionString);
            string query = @"UPDATE Logs SET 
                        OperationID=@OperationID,
                        DocumentID=@DocumentID,
                        UserID=@UserID,
                        EmailID=@EmailID,
                        StatusID=@StatusID,
                        DeviceInfo=@DeviceInfo,
                        LogDate=@LogDate
                     WHERE LogID=@LogID";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@OperationID", log.OperationID);
            cmd.Parameters.AddWithValue("@DocumentID", (object?)log.DocumentID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UserID", log.UserID);
            cmd.Parameters.AddWithValue("@EmailID", (object?)log.EmailID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@StatusID", log.StatusID);
            cmd.Parameters.AddWithValue("@DeviceInfo", log.DeviceInfo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@LogDate", log.LogDate);
            cmd.Parameters.AddWithValue("@LogID", log.LogID);

            await conn.OpenAsync();
            int result = await cmd.ExecuteNonQueryAsync();
            return result > 0;
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using SqlConnection conn = new(_ConnectionString);
            string query = "DELETE FROM Logs WHERE LogID=@id";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            await conn.OpenAsync();
            int result = await cmd.ExecuteNonQueryAsync();
            return result > 0;
        }
    }
}
