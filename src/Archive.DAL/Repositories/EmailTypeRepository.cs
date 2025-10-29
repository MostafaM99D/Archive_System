using Archive.DAL.Global;
using Archive.DAL.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.DAL.Repositories
{
    public class EmailTypeRepository
    {
        private static readonly string _ConnectionString = Utility.GetConnectionString();

        public static async Task<IEnumerable<EmailType>> GetAllEmailTypesAsync()
        {
            var emailTypes = new List<EmailType>();

            using var conn = new SqlConnection(_ConnectionString);
            const string query = "SELECT * FROM EmailTypes";

            using var cmd = new SqlCommand(query, conn);
            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                emailTypes.Add(new EmailType
                {
                    EmailTypeID = reader.GetInt32(reader.GetOrdinal("EmailTypeID")),
                    TypeName = reader["TypeName"].ToString()!
                });
            }

            return emailTypes;
        }

        public static async Task<EmailType?> GetEmailTypeByIDAsync(int emailTypeID)
        {
            using var conn = new SqlConnection(_ConnectionString);
            const string query = "SELECT * FROM EmailTypes WHERE EmailTypeID=@EmailTypeID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@EmailTypeID", emailTypeID);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new EmailType
                {
                    EmailTypeID = reader.GetInt32(reader.GetOrdinal("EmailTypeID")),
                    TypeName = reader["TypeName"].ToString()!
                };
            }

            return null;
        }

        public static async Task<int> AddNewEmailTypeAsync(EmailType emailType)
        {
            using var conn = new SqlConnection(_ConnectionString);
            const string query = @"INSERT INTO EmailTypes (TypeName)
                                   VALUES (@TypeName);
                                   SELECT SCOPE_IDENTITY();";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@TypeName", emailType.TypeName);

            await conn.OpenAsync();
            object? result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : -1;
        }

        public static async Task<bool> UpdateEmailTypeAsync(EmailType emailType)
        {
            using var conn = new SqlConnection(_ConnectionString);
            const string query = @"UPDATE EmailTypes 
                                   SET TypeName=@TypeName 
                                   WHERE EmailTypeID=@EmailTypeID;";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@TypeName", emailType.TypeName);
            cmd.Parameters.AddWithValue("@EmailTypeID", emailType.EmailTypeID);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public static async Task<bool> DeleteEmailTypeAsync(int emailTypeID)
        {
            using var conn = new SqlConnection(_ConnectionString);
            const string query = @"DELETE FROM EmailTypes WHERE EmailTypeID=@EmailTypeID;";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@EmailTypeID", emailTypeID);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public static async Task<bool> IsEmailTypeExistAsync(int emailTypeID)
        {
            using var conn = new SqlConnection(_ConnectionString);
            const string query = "SELECT 1 FROM EmailTypes WHERE EmailTypeID=@EmailTypeID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@EmailTypeID", emailTypeID);

            await conn.OpenAsync();
            object? result = await cmd.ExecuteScalarAsync();
            return result != null;
        }
    }
}
