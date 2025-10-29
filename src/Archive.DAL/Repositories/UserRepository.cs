using Archive.DAL.Global;
using Archive.DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive.DAL.Repositories
{
    public class UserRepository
    {
        private static readonly string _ConnectionString = Utility.GetConnectionString();
        public static async Task<List<User>> GetAllUsersAsync()
        {
            List<User> users = new List<User>();
            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                string? query = $"SELECT * FROM Users";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                users.Add(new User
                                {
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Username = reader["Username"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    Permissions = Convert.ToInt64(reader["Permissions"]),
                                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                                    UserID = Convert.ToInt32(reader["UserID"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        throw new Exception($"{ex.Message}");
                    }
                }
            }
            return users;
        }
        public static async Task<User> GetUserByIDAsync(int userId)
        {
            User? res = null;
            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                string query = $@"SELECT * FROM Users WHERE UserID=@UserID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    try
                    {
                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                res = new User
                                {
                                    UserID = Convert.ToInt32(reader["UserID"]),
                                    Username = reader["Username"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    FirstName = reader["FirstName"].ToString(),
                                    Permissions = Convert.ToInt64(reader["Permissions"]),
                                    LastName = reader["LastName"].ToString(),
                                    IsActive = Convert.ToBoolean(reader["IsActive"])
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        throw new Exception($"{ex.Message}");
                    }
                }
            }
            if (res == null) return null!;
            return res;
        }
        public static async Task<int> AddNewUserAsync(User user)
        {
            int res = 0;
            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                string query = @"INSERT INTO [dbo].[Users]
                             ([FirstName]
                              ,[LastName]
                              ,[Username]
                              ,[Password]
                              ,[Permissions]
                              ,[IsActive])
                               VALUES
         (@FirstName,@LastName,@Username,@Password,@Permissions,@IsActive);
                                Select SCOPE_IDENTITY();";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Permissions", user.Permissions);
                    cmd.Parameters.AddWithValue("@IsActive", user.IsActive);
                    try
                    {
                        await conn.OpenAsync();
                        object? r = await cmd.ExecuteScalarAsync();
                        if (r != null)
                            res = Convert.ToInt32(r);
                        else
                            res = -1;
                    }
                    catch (Exception ex)
                    {

                        throw new Exception($"{ex.Message}");
                    }
                }
            }
            return res;

        }
        public static async Task<bool> UpdateUserAsync(User user)
        {
            bool res = false;
            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                string query = @"UPDATE [dbo].[Users]
                        SET [FirstName] =@FirstName
                        ,[LastName] = @LastName
                        ,[Username] = @Username
                        ,[Password] = @Password
                        ,[Permissions] = @Permissions
                        ,[IsActive] = @IsActive
                        WHERE UserID=@UserID;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Permissions", user.Permissions);
                    cmd.Parameters.AddWithValue("@IsActive", user.IsActive);
                    cmd.Parameters.AddWithValue("@UserID", user.UserID);
                    try
                    {
                        await conn.OpenAsync();
                        int? r = await cmd.ExecuteNonQueryAsync();
                        if (r > 0)
                            res = true;
                        else
                            res = false;
                    }
                    catch (Exception ex)
                    {

                        throw new Exception($"{ex.Message}");
                    }
                }
            }
            return res;
        }
        public static async Task<bool> DeleteUserAsync(int userId)
        {
            bool res = false;
            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                string query = @"DELETE FROM [dbo].[Users] WHERE UserID=@UserID;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    try
                    {
                        await conn.OpenAsync();
                        int? r = await cmd.ExecuteNonQueryAsync();
                        if (r > 0)
                            res = true;
                        else
                            res = false;
                    }
                    catch (Exception ex)
                    {

                        throw new Exception($"{ex.Message}");
                    }
                }
            }
            return res;
        }
        public static async Task<bool> IsUserExist(int userId)
        {
            bool res = false;
            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                string query = @"SELECT Found = 1 FROM [dbo].[Users] WHERE UserID=@UserID;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    try
                    {
                        await conn.OpenAsync();
                        object? r = await cmd.ExecuteScalarAsync();
                        if (r != null)
                            res = true;
                        else
                            res = false;
                    }
                    catch (Exception ex)
                    {

                        throw new Exception($"{ex.Message}");
                    }
                }
            }
            return res;
        }
    }
}