using Archive.DAL.Global;
using Archive.DAL.Models;
using Microsoft.Data.SqlClient;

namespace Archive.DAL.Repositories
{
    public class EmailAttachmentRepository
    {
        private static readonly string _ConnectionString = Utility.GetConnectionString();

        public static async Task<List<EmailAttachment>> GetAllAsync()
        {
            List<EmailAttachment> list = new();
            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = "SELECT * FROM EmailAttachments";
                using SqlCommand cmd = new(query, conn);
                await conn.OpenAsync();
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new EmailAttachment
                    {
                        EmailAttachmentID = (int)reader["EmailAttatchmentID"],
                        FileName = reader["FileName"].ToString(),
                        FilePath = reader["FilePath"].ToString(),
                        UploadedAt = Convert.ToDateTime(reader["UploadedAt"]),
                        FileTypeID = Convert.ToInt32(reader["FileTypeID"]),
                        EmailID = Convert.ToInt32(reader["EmailID"])
                    });
                }
            }
            return list;
        }

        public static async Task<EmailAttachment?> GetByIdAsync(int id)
        {
            EmailAttachment? e = null;
            using SqlConnection conn = new(_ConnectionString);
            string query = "SELECT * FROM EmailAttachments WHERE EmailAttachmentID=@id";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                e = new EmailAttachment
                {
                    EmailAttachmentID = Convert.ToInt32(reader["EmailAttachmentID"]),
                    FileName = reader["FileName"].ToString(),
                    FilePath = reader["FilePath"].ToString(),
                    UploadedAt = Convert.ToDateTime(reader["UploadedAt"]),
                    FileTypeID = Convert.ToInt32(reader["FileTypeID"]),
                    EmailID = Convert.ToInt32(reader["EmailID"])
                };
            }
            return e;
        }

        public static async Task<int> AddAsync(EmailAttachment attachment)
        {
            using SqlConnection conn = new(_ConnectionString);
            string query = @"INSERT INTO EmailAttachments (FileName, FilePath, UploadedAt, FileTypeID, EmailID)
                             VALUES (@FileName, @FilePath, @UploadedAt, @FileTypeID, @EmailID);
                             SELECT SCOPE_IDENTITY();";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@FileName", attachment.FileName);
            cmd.Parameters.AddWithValue("@FilePath", attachment.FilePath);
            cmd.Parameters.AddWithValue("@UploadedAt", attachment.UploadedAt);
            cmd.Parameters.AddWithValue("@FileTypeID", attachment.FileTypeID);
            cmd.Parameters.AddWithValue("@EmailID", attachment.EmailID);

            await conn.OpenAsync();
            object? result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : -1;
        }

        public static async Task<bool> UpdateAsync(EmailAttachment attachment)
        {
            using SqlConnection conn = new(_ConnectionString);
            string query = @"UPDATE EmailAttachments SET 
                                FileName=@FileName,
                                FilePath=@FilePath,
                                UploadedAt=@UploadedAt,
                                FileTypeID=@FileTypeID,
                                EmailID=@EmailID
                             WHERE EmailAttachmentID=@EmailAttachmentID";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@FileName", attachment.FileName);
            cmd.Parameters.AddWithValue("@FilePath", attachment.FilePath);
            cmd.Parameters.AddWithValue("@UploadedAt", attachment.UploadedAt);
            cmd.Parameters.AddWithValue("@FileTypeID", attachment.FileTypeID);
            cmd.Parameters.AddWithValue("@EmailID", attachment.EmailID);
            cmd.Parameters.AddWithValue("@EmailAttachmentID", attachment.EmailAttachmentID);

            await conn.OpenAsync();
            int result = await cmd.ExecuteNonQueryAsync();
            return result > 0;
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using SqlConnection conn = new(_ConnectionString);
            string query = "DELETE FROM EmailAttachments WHERE EmailAttachmentID=@id";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            await conn.OpenAsync();
            int result = await cmd.ExecuteNonQueryAsync();
            return result > 0;
        }
    }
}
