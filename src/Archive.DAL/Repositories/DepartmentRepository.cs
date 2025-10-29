using Archive.DAL.Global;
using Archive.DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.DAL.Repositories
{
    public class DepartmentRepository
    {
        private static readonly string _ConnectionString = Utility.GetConnectionString();

        public static async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            var departments = new List<Department>();

            using var conn = new SqlConnection(_ConnectionString);
            const string query = "SELECT * FROM Departments";

            using var cmd = new SqlCommand(query, conn);
            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                departments.Add(new Department
                {
                    DepartmentID = reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                    DepartmentName = reader["DepartmentName"].ToString()!
                });
            }

            return departments;
        }

        public static async Task<Department?> GetDepartmentByIDAsync(int departmentID)
        {
            using var conn = new SqlConnection(_ConnectionString);
            const string query = "SELECT * FROM Departments WHERE DepartmentID=@DepartmentID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@DepartmentID", departmentID);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Department
                {
                    DepartmentID = reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                    DepartmentName = reader["DepartmentName"].ToString()!
                };
            }

            return null;
        }

        public static async Task<int> AddNewDepartmentAsync(Department department)
        {
            using var conn = new SqlConnection(_ConnectionString);
            const string query = @"INSERT INTO Departments (DepartmentName)
                                   VALUES (@DepartmentName);
                                   SELECT SCOPE_IDENTITY();";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);

            await conn.OpenAsync();
            object? result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : -1;
        }

        public static async Task<bool> UpdateDepartmentAsync(Department department)
        {
            using var conn = new SqlConnection(_ConnectionString);
            const string query = @"UPDATE Departments 
                                   SET DepartmentName=@DepartmentName 
                                   WHERE DepartmentID=@DepartmentID;";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
            cmd.Parameters.AddWithValue("@DepartmentID", department.DepartmentID);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public static async Task<bool> DeleteDepartmentAsync(int departmentID)
        {
            using var conn = new SqlConnection(_ConnectionString);
            const string query = @"DELETE FROM Departments WHERE DepartmentID=@DepartmentID;";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@DepartmentID", departmentID);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public static async Task<bool> IsDepartmentExistAsync(int departmentID)
        {
            using var conn = new SqlConnection(_ConnectionString);
            const string query = "SELECT 1 FROM Departments WHERE DepartmentID=@DepartmentID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@DepartmentID", departmentID);

            await conn.OpenAsync();
            object? result = await cmd.ExecuteScalarAsync();
            return result != null;
        }
    }
}
