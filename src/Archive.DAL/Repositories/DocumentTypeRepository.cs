using Archive.DAL.Global;
using Archive.DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.DAL.Repositories
{
    public class DocumentTypeRepository
    {
        private static readonly string _ConnectionString = Utility.GetConnectionString();

        public static async Task<IEnumerable<DocumentType>> GetAllDocumentTypesAsync()
        {
            var types = new List<DocumentType>();

            using var conn = new SqlConnection(_ConnectionString);
            const string query = "SELECT * FROM DocumentTypes";

            using var cmd = new SqlCommand(query, conn);
            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                types.Add(new DocumentType
                {
                    DocumentTypeID = reader.GetInt32(reader.GetOrdinal("DocumentTypeID")),
                    TypeName = reader["TypeName"].ToString()!
                });
            }

            return types;
        }

        public static async Task<DocumentType?> GetDocumentTypeByIDAsync(int documentTypeID)
        {
            using var conn = new SqlConnection(_ConnectionString);
            const string query = "SELECT * FROM DocumentTypes WHERE DocumentTypeID=@DocumentTypeID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@DocumentTypeID", documentTypeID);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new DocumentType
                {
                    DocumentTypeID = reader.GetInt32(reader.GetOrdinal("DocumentTypeID")),
                    TypeName = reader["TypeName"].ToString()!
                };
            }

            return null;
        }

        public static async Task<int> AddNewDocumentTypeAsync(DocumentType documentType)
        {
            using var conn = new SqlConnection(_ConnectionString);
            const string query = @"INSERT INTO DocumentTypes (TypeName)
                                   VALUES (@TypeName);
                                   SELECT SCOPE_IDENTITY();";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@TypeName", documentType.TypeName);

            await conn.OpenAsync();
            object? result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : -1;
        }

        public static async Task<bool> UpdateDocumentTypeAsync(DocumentType documentType)
        {
            using var conn = new SqlConnection(_ConnectionString);
            const string query = @"UPDATE DocumentTypes 
                                   SET TypeName=@TypeName 
                                   WHERE DocumentTypeID=@DocumentTypeID;";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@TypeName", documentType.TypeName);
            cmd.Parameters.AddWithValue("@DocumentTypeID", documentType.DocumentTypeID);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public static async Task<bool> DeleteDocumentTypeAsync(int documentTypeID)
        {
            using var conn = new SqlConnection(_ConnectionString);
            const string query = @"DELETE FROM DocumentTypes WHERE DocumentTypeID=@DocumentTypeID;";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@DocumentTypeID", documentTypeID);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public static async Task<bool> IsDocumentTypeExistAsync(int documentTypeID)
        {
            using var conn = new SqlConnection(_ConnectionString);
            const string query = "SELECT 1 FROM DocumentTypes WHERE DocumentTypeID=@DocumentTypeID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@DocumentTypeID", documentTypeID);

            await conn.OpenAsync();
            object? result = await cmd.ExecuteScalarAsync();
            return result != null;
        }
    }
}
