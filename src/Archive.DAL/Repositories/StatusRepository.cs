using Archive.DAL.Global;
using Archive.DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.DAL.Repositories
{
    public class StatusRepository
    {
        private static readonly string _ConnectionString = Utility.GetConnectionString();

        public static async Task<List<Status>> GetAllStatusesAsync()
        {
            List<Status> statuses = new();
            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = "SELECT * FROM Status";
                using (SqlCommand cmd = new(query, conn))
                {
                    try
                    {
                        await conn.OpenAsync();
                        using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        while (await reader.ReadAsync())
                        {
                            statuses.Add(new Status
                            {
                                StatusID = Convert.ToInt32(reader["StatusID"]),
                                StatusName = reader["StatusName"].ToString()
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            return statuses;
        }

        public static async Task<Status?> GetStatusByIDAsync(int statusID)
        {
            Status? res = null;
            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = "SELECT * FROM Status WHERE StatusID=@StatusID";
                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StatusID", statusID);
                    try
                    {
                        await conn.OpenAsync();
                        using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        if (await reader.ReadAsync())
                        {
                            res = new Status
                            {
                                StatusID = Convert.ToInt32(reader["StatusID"]),
                                StatusName = reader["StatusName"].ToString()
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

        public static async Task<int> AddNewStatusAsync(Status status)
        {
            int res = -1;
            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = @"INSERT INTO Status (StatusName)
                                 VALUES (@StatusName);
                                 SELECT SCOPE_IDENTITY();";
                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StatusName", status.StatusName);
                    try
                    {
                        await conn.OpenAsync();
                        object? r = await cmd.ExecuteScalarAsync();
                        if (r != null) res = Convert.ToInt32(r);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            return res;
        }

        public static async Task<bool> UpdateStatusAsync(Status status)
        {
            bool res = false;
            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = @"UPDATE Status 
                                 SET StatusName=@StatusName 
                                 WHERE StatusID=@StatusID";
                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StatusName", status.StatusName);
                    cmd.Parameters.AddWithValue("@StatusID", status.StatusID);
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

        public static async Task<bool> DeleteStatusAsync(int statusID)
        {
            bool res = false;
            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = @"DELETE FROM Status WHERE StatusID=@StatusID";
                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StatusID", statusID);
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

        public static async Task<bool> IsStatusExistAsync(int statusID)
        {
            bool res = false;
            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = "SELECT 1 FROM Status WHERE StatusID=@StatusID";
                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StatusID", statusID);
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
