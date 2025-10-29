using Archive.DAL.Global;
using Archive.DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.DAL.Repositories
{
    public class EmailRepository
    {
        private static readonly string _ConnectionString = Utility.GetConnectionString();

        public static async Task<List<Email>> GetAllEmailsAsync()
        {
            List<Email> emails = new();

            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = @"
                    SELECT e.*, 
                           u.FirstName, u.LastName, u.Username, 
                           et.TypeName AS EmailTypeName, 
                           d.DepartmentName
                    FROM Emails e
                    JOIN Users u ON e.UserID = u.UserID
                    JOIN EmailTypes et ON e.EmailTypeID = et.EmailTypeID
                    JOIN Departments d ON e.DepartmentID = d.DepartmentID";

                using (SqlCommand cmd = new(query, conn))
                {
                    try
                    {
                        await conn.OpenAsync();
                        using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                        while (await reader.ReadAsync())
                        {
                            Email email = new()
                            {
                                EmailID = Convert.ToInt32(reader["EmailID"]),
                                Content = reader["Content"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                Year = Convert.ToInt32(reader["Year"]),
                                Notes = reader["Notes"].ToString(),
                                UserID = Convert.ToInt32(reader["UserID"]),
                                EmailTypeID = Convert.ToInt32(reader["EmailTypeID"]),
                                DepartmentID = Convert.ToInt32(reader["DepartmentID"]),

                                User = new User
                                {
                                    UserID = Convert.ToInt32(reader["UserID"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Username = reader["Username"].ToString()
                                },
                                EmailType = new EmailType
                                {
                                    EmailTypeID = Convert.ToInt32(reader["EmailTypeID"]),
                                    TypeName = reader["EmailTypeName"].ToString()
                                },
                                Department = new Department
                                {
                                    DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                                    DepartmentName = reader["DepartmentName"].ToString()
                                }
                            };

                            emails.Add(email);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error in GetAllEmailsAsync: {ex.Message}");
                    }
                }
            }

            return emails;
        }

        public static async Task<Email?> GetEmailByIDAsync(int emailID)
        {
            Email? res = null;

            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = @"
                    SELECT e.*, 
                           u.FirstName, u.LastName, u.Username, 
                           et.TypeName AS EmailTypeName, 
                           d.DepartmentName
                    FROM Emails e
                    JOIN Users u ON e.UserID = u.UserID
                    JOIN EmailTypes et ON e.EmailTypeID = et.EmailTypeID
                    JOIN Departments d ON e.DepartmentID = d.DepartmentID
                    WHERE e.EmailID = @EmailID";

                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmailID", emailID);

                    try
                    {
                        await conn.OpenAsync();
                        using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                        if (await reader.ReadAsync())
                        {
                            res = new Email
                            {
                                EmailID = Convert.ToInt32(reader["EmailID"]),
                                Content = reader["Content"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                Year = Convert.ToInt32(reader["Year"]),
                                Notes = reader["Notes"].ToString(),
                                UserID = Convert.ToInt32(reader["UserID"]),
                                EmailTypeID = Convert.ToInt32(reader["EmailTypeID"]),
                                DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                                User = new User
                                {
                                    UserID = Convert.ToInt32(reader["UserID"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Username = reader["Username"].ToString()
                                },
                                EmailType = new EmailType
                                {
                                    EmailTypeID = Convert.ToInt32(reader["EmailTypeID"]),
                                    TypeName = reader["EmailTypeName"].ToString()
                                },
                                Department = new Department
                                {
                                    DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                                    DepartmentName = reader["DepartmentName"].ToString()
                                }
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error in GetEmailByIDAsync: {ex.Message}");
                    }
                }
            }

            return res;
        }

        public static async Task<int> AddNewEmailAsync(Email email)
        {
            int res = -1;

            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = @"
                    INSERT INTO Emails (Content, CreatedAt, Year, Notes, UserID, EmailTypeID, DepartmentID)
                    VALUES (@Content, @CreatedAt, @Year, @Notes, @UserID, @EmailTypeID, @DepartmentID);
                    SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Content", email.Content ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedAt", email.CreatedAt);
                    cmd.Parameters.AddWithValue("@Year", email.Year);
                    cmd.Parameters.AddWithValue("@Notes", (object?)email.Notes ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@UserID", email.UserID);
                    cmd.Parameters.AddWithValue("@EmailTypeID", email.EmailTypeID);
                    cmd.Parameters.AddWithValue("@DepartmentID", email.DepartmentID);

                    try
                    {
                        await conn.OpenAsync();
                        object? r = await cmd.ExecuteScalarAsync();
                        if (r != null)
                            res = Convert.ToInt32(r);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error in AddNewEmailAsync: {ex.Message}");
                    }
                }
            }

            return res;
        }

        public static async Task<bool> UpdateEmailAsync(Email email)
        {
            bool res = false;

            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = @"
                    UPDATE Emails 
                    SET Content=@Content, CreatedAt=@CreatedAt, Year=@Year, Notes=@Notes,
                        UserID=@UserID, EmailTypeID=@EmailTypeID, DepartmentID=@DepartmentID
                    WHERE EmailID=@EmailID";

                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Content", email.Content ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedAt", email.CreatedAt);
                    cmd.Parameters.AddWithValue("@Year", email.Year);
                    cmd.Parameters.AddWithValue("@Notes", (object?)email.Notes ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@UserID", email.UserID);
                    cmd.Parameters.AddWithValue("@EmailTypeID", email.EmailTypeID);
                    cmd.Parameters.AddWithValue("@DepartmentID", email.DepartmentID);
                    cmd.Parameters.AddWithValue("@EmailID", email.EmailID);

                    try
                    {
                        await conn.OpenAsync();
                        res = await cmd.ExecuteNonQueryAsync() > 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error in UpdateEmailAsync: {ex.Message}");
                    }
                }
            }

            return res;
        }

        public static async Task<bool> DeleteEmailAsync(int emailID)
        {
            bool res = false;

            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = "DELETE FROM Emails WHERE EmailID=@EmailID";

                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmailID", emailID);

                    try
                    {
                        await conn.OpenAsync();
                        res = await cmd.ExecuteNonQueryAsync() > 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error in DeleteEmailAsync: {ex.Message}");
                    }
                }
            }

            return res;
        }

        public static async Task<bool> IsEmailExistAsync(int emailID)
        {
            bool res = false;

            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = "SELECT 1 FROM Emails WHERE EmailID=@EmailID";

                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmailID", emailID);

                    try
                    {
                        await conn.OpenAsync();
                        object? r = await cmd.ExecuteScalarAsync();
                        res = r != null;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error in IsEmailExistAsync: {ex.Message}");
                    }
                }
            }

            return res;
        }
    }
}
