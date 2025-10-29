using Archive.DAL.Global;
using Archive.DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.DAL.Repositories
{
    public class OperationRepository
    {
        private static readonly string _ConnectionString = Utility.GetConnectionString();

        public static async Task<List<Operation>> GetAllOperationsAsync()
        {
            List<Operation> operations = new();
            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = "SELECT * FROM Operations";
                using (SqlCommand cmd = new(query, conn))
                {
                    try
                    {
                        await conn.OpenAsync();
                        using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        while (await reader.ReadAsync())
                        {
                            operations.Add(new Operation
                            {
                                OperationID = Convert.ToInt32(reader["OperationID"]),
                                OperationName = reader["OperationName"].ToString()
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            return operations;
        }

        public static async Task<Operation?> GetOperationByIDAsync(int id)
        {
            Operation? res = null;
            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = "SELECT * FROM Operations WHERE OperationID=@OperationID";
                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OperationID", id);
                    try
                    {
                        await conn.OpenAsync();
                        using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        if (await reader.ReadAsync())
                        {
                            res = new Operation
                            {
                                OperationID = Convert.ToInt32(reader["OperationID"]),
                                OperationName = reader["OperationName"].ToString()
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            return res;
        }

        public static async Task<int> AddNewOperationAsync(Operation operation)
        {
            int res = -1;
            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = @"INSERT INTO Operations (OperationName)
                                 VALUES (@OperationName);
                                 SELECT SCOPE_IDENTITY();";
                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OperationName", operation.OperationName);
                    try
                    {
                        await conn.OpenAsync();
                        object? r = await cmd.ExecuteScalarAsync();
                        if (r != null)
                            res = Convert.ToInt32(r);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            return res;
        }

        public static async Task<bool> UpdateOperationAsync(Operation operation)
        {
            bool res = false;
            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = @"UPDATE Operations
                                 SET OperationName=@OperationName 
                                 WHERE OperationID=@OperationID";
                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OperationName", operation.OperationName);
                    cmd.Parameters.AddWithValue("@OperationID", operation.OperationID);
                    try
                    {
                        await conn.OpenAsync();
                        res = await cmd.ExecuteNonQueryAsync() > 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            return res;
        }

        public static async Task<bool> DeleteOperationAsync(int id)
        {
            bool res = false;
            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = @"DELETE FROM Operations WHERE OperationID=@OperationID";
                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OperationID", id);
                    try
                    {
                        await conn.OpenAsync();
                        res = await cmd.ExecuteNonQueryAsync() > 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            return res;
        }

        public static async Task<bool> IsOperationExistAsync(int id)
        {
            bool res = false;
            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = "SELECT 1 FROM Operations WHERE OperationID=@OperationID";
                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OperationID", id);
                    try
                    {
                        await conn.OpenAsync();
                        object? r = await cmd.ExecuteScalarAsync();
                        res = r != null;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            return res;
        }
    }
}
