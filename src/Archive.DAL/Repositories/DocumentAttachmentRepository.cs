using Archive.DAL.Global;
using Archive.DAL.Models;
using Microsoft.Data.SqlClient;

namespace Archive.DAL.Repositories
{
    public class DocumentAttachmentRepository
    {
        private static readonly string _ConnectionString = Utility.GetConnectionString();

        public static async Task<List<DocumentAttachment>> GetAllAsync()
        {
            List<DocumentAttachment> list = new();
            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = "SELECT * FROM DocumentAttachments";
                using SqlCommand cmd = new(query, conn);
                await conn.OpenAsync();
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new DocumentAttachment
                    {
                        DocumentAttachmentID = Convert.ToInt32(reader["DocumentAttachmentID"]),
                        FileName = reader["FileName"].ToString(),
                        FilePath = reader["FilePath"].ToString(),
                        UploadedAt = Convert.ToDateTime(reader["UploadedAt"]),
                        FileTypeID = Convert.ToInt32(reader["FileTypeID"]),
                        DocumentID = Convert.ToInt32(reader["DocumentID"])
                    });
                }
            }
            return list;
        }

        public static async Task<DocumentAttachment?> GetByIdAsync(int id)
        {
            DocumentAttachment? e = null;
            using SqlConnection conn = new(_ConnectionString);
            string query = "SELECT * FROM DocumentAttachments WHERE DocumentAttachmentID=@id";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                e = new DocumentAttachment
                {
                    DocumentAttachmentID = Convert.ToInt32(reader["DocumentAttachmentID"]),
                    FileName = reader["FileName"].ToString(),
                    FilePath = reader["FilePath"].ToString(),
                    UploadedAt = Convert.ToDateTime(reader["UploadedAt"]),
                    FileTypeID = Convert.ToInt32(reader["FileTypeID"]),
                    DocumentID = Convert.ToInt32(reader["DocumentID"])
                };
            }
            return e;
        }

        public static async Task<int> AddAsync(DocumentAttachment attachment)
        {
            using SqlConnection conn = new(_ConnectionString);
            string query = @"INSERT INTO DocumentAttachments (FileName, FilePath, UploadedAt, FileTypeID, DocumentID)
                             VALUES (@FileName, @FilePath, @UploadedAt, @FileTypeID, @DocumentID);
                             SELECT SCOPE_IDENTITY();";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@FileName", attachment.FileName);
            cmd.Parameters.AddWithValue("@FilePath", attachment.FilePath);
            cmd.Parameters.AddWithValue("@UploadedAt", attachment.UploadedAt);
            cmd.Parameters.AddWithValue("@FileTypeID", attachment.FileTypeID);
            cmd.Parameters.AddWithValue("@DocumentID", attachment.DocumentID);

            await conn.OpenAsync();
            object? result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : -1;
        }

        public static async Task<bool> UpdateAsync(DocumentAttachment attachment)
        {
            using SqlConnection conn = new(_ConnectionString);
            string query = @"UPDATE DocumentAttachments SET 
                                FileName=@FileName,
                                FilePath=@FilePath,
                                UploadedAt=@UploadedAt,
                                FileTypeID=@FileTypeID,
                                DocumentID=@DocumentID
                             WHERE DocumentAttachmentID=@DocumentAttachmentID";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@FileName", attachment.FileName);
            cmd.Parameters.AddWithValue("@FilePath", attachment.FilePath);
            cmd.Parameters.AddWithValue("@UploadedAt", attachment.UploadedAt);
            cmd.Parameters.AddWithValue("@FileTypeID", attachment.FileTypeID);
            cmd.Parameters.AddWithValue("@DocumentID", attachment.DocumentID);
            cmd.Parameters.AddWithValue("@DocumentAttachmentID", attachment.DocumentAttachmentID);

            await conn.OpenAsync();
            int result = await cmd.ExecuteNonQueryAsync();
            return result > 0;
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using SqlConnection conn = new(_ConnectionString);
            string query = "DELETE FROM DocumentAttachments WHERE DocumentAttachmentID=@id";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            await conn.OpenAsync();
            int result = await cmd.ExecuteNonQueryAsync();
            return result > 0;
        }
    }
}
