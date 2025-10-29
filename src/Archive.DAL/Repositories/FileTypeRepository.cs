using Archive.DAL.Global;
using Archive.DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.DAL.Repositories
{
    public class FileTypeRepository
    {
        private static readonly string _ConnectionString = Utility.GetConnectionString();

        public static async Task<List<FileType>> GetAllFileTypesAsync()
        {
            List<FileType> fileTypes = new();
            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = "SELECT * FROM FileTypes";
                using (SqlCommand cmd = new(query, conn))
                {
                    try
                    {
                        await conn.OpenAsync();
                        using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        while (await reader.ReadAsync())
                        {
                            fileTypes.Add(new FileType
                            {
                                FileTypeID = Convert.ToInt32(reader["FileTypeID"]),
                                TypeName = reader["TypeName"].ToString()
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            return fileTypes;
        }

        public static async Task<FileType?> GetFileTypeByIDAsync(int fileTypeID)
        {
            FileType? res = null;
            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = "SELECT * FROM FileTypes WHERE FileTypeID=@FileTypeID";
                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FileTypeID", fileTypeID);
                    try
                    {
                        await conn.OpenAsync();
                        using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        if (await reader.ReadAsync())
                        {
                            res = new FileType
                            {
                                FileTypeID = Convert.ToInt32(reader["FileTypeID"]),
                                TypeName = reader["TypeName"].ToString()
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

        public static async Task<int> AddNewFileTypeAsync(FileType fileType)
        {
            int res = -1;
            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = @"INSERT INTO FileTypes (TypeName)
                                 VALUES (@TypeName);
                                 SELECT SCOPE_IDENTITY();";
                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TypeName", fileType.TypeName);
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

        public static async Task<bool> UpdateFileTypeAsync(FileType fileType)
        {
            bool res = false;
            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = @"UPDATE FileTypes 
                                 SET TypeName=@TypeName 
                                 WHERE FileTypeID=@FileTypeID";
                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TypeName", fileType.TypeName);
                    cmd.Parameters.AddWithValue("@FileTypeID", fileType.FileTypeID);
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

        public static async Task<bool> DeleteFileTypeAsync(int fileTypeID)
        {
            bool res = false;
            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = @"DELETE FROM FileTypes WHERE FileTypeID=@FileTypeID";
                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FileTypeID", fileTypeID);
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

        public static async Task<bool> IsFileTypeExistAsync(int fileTypeID)
        {
            bool res = false;
            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = "SELECT 1 FROM FileTypes WHERE FileTypeID=@FileTypeID";
                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FileTypeID", fileTypeID);
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
