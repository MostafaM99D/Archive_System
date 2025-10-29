using Archive.DAL.Global;
using Archive.DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.DAL.Repositories
{
    public class DocumentRepository
    {
        private static readonly string _ConnectionString = Utility.GetConnectionString();

        public static async Task<List<Document>> GetAllDocumentsAsync()
        {
            List<Document> documents = new();

            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = @"
                    SELECT d.*, 
                           u.FirstName, u.LastName, u.Username, 
                           dt.TypeName AS DocumentTypeName, 
                           dep.DepartmentName
                    FROM Documents d
                    JOIN Users u ON d.UserID = u.UserID
                    JOIN DocumentTypes dt ON d.DocumentTypeID = dt.DocumentTypeID
                    JOIN Departments dep ON d.DepartmentID = dep.DepartmentID";

                using (SqlCommand cmd = new(query, conn))
                {
                    try
                    {
                        await conn.OpenAsync();
                        using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                        while (await reader.ReadAsync())
                        {
                            documents.Add(new Document
                            {
                                DocumentID = Convert.ToInt32(reader["DocumentID"]),
                                Content = reader["Content"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                Year = Convert.ToInt32(reader["Year"]),
                                Notes = reader["Notes"].ToString(),
                                InternalNumber = Convert.ToInt64(reader["InternalNumber"]),
                                UserID = Convert.ToInt32(reader["UserID"]),
                                DocumentTypeID = Convert.ToInt32(reader["DocumentTypeID"]),
                                DepartmentID = Convert.ToInt32(reader["DepartmentID"]),

                                // Navigation
                                User = new User
                                {
                                    UserID = Convert.ToInt32(reader["UserID"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Username = reader["Username"].ToString()
                                },
                                DocumentType = new DocumentType
                                {
                                    DocumentTypeID = Convert.ToInt32(reader["DocumentTypeID"]),
                                    TypeName = reader["DocumentTypeName"].ToString()
                                },
                                Department = new Department
                                {
                                    DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                                    DepartmentName = reader["DepartmentName"].ToString()
                                }
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error in GetAllDocumentsAsync: {ex.Message}");
                    }
                }
            }

            return documents;
        }

        public static async Task<Document?> GetDocumentByIDAsync(int documentID)
        {
            Document? res = null;

            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = @"
                    SELECT d.*, 
                           u.FirstName, u.LastName, u.Username, 
                           dt.TypeName AS DocumentTypeName, 
                           dep.DepartmentName
                    FROM Documents d
                    JOIN Users u ON d.UserID = u.UserID
                    JOIN DocumentTypes dt ON d.DocumentTypeID = dt.DocumentTypeID
                    JOIN Departments dep ON d.DepartmentID = dep.DepartmentID
                    WHERE d.DocumentID = @DocumentID";

                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DocumentID", documentID);

                    try
                    {
                        await conn.OpenAsync();
                        using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                        if (await reader.ReadAsync())
                        {
                            res = new Document
                            {
                                DocumentID = Convert.ToInt32(reader["DocumentID"]),
                                Content = reader["Content"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                Year = Convert.ToInt32(reader["Year"]),
                                Notes = reader["Notes"].ToString(),
                                InternalNumber = Convert.ToInt64(reader["InternalNumber"]),
                                UserID = Convert.ToInt32(reader["UserID"]),
                                DocumentTypeID = Convert.ToInt32(reader["DocumentTypeID"]),
                                DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                                User = new User
                                {
                                    UserID = Convert.ToInt32(reader["UserID"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Username = reader["Username"].ToString()
                                },
                                DocumentType = new DocumentType
                                {
                                    DocumentTypeID = Convert.ToInt32(reader["DocumentTypeID"]),
                                    TypeName = reader["DocumentTypeName"].ToString()
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
                        throw new Exception($"Error in GetDocumentByIDAsync: {ex.Message}");
                    }
                }
            }

            return res;
        }

        public static async Task<int> AddNewDocumentAsync(Document doc)
        {
            int res = -1;

            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = @"
                    INSERT INTO Documents 
                    (Content, CreatedAt, Year, Notes, InternalNumber, UserID, DocumentTypeID, DepartmentID)
                    VALUES 
                    (@Content, @CreatedAt, @Year, @Notes, @InternalNumber, @UserID, @DocumentTypeID, @DepartmentID);
                    SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Content", doc.Content ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedAt", doc.CreatedAt);
                    cmd.Parameters.AddWithValue("@Year", doc.Year);
                    cmd.Parameters.AddWithValue("@Notes", (object?)doc.Notes ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@InternalNumber", doc.InternalNumber);
                    cmd.Parameters.AddWithValue("@UserID", doc.UserID);
                    cmd.Parameters.AddWithValue("@DocumentTypeID", doc.DocumentTypeID);
                    cmd.Parameters.AddWithValue("@DepartmentID", doc.DepartmentID);

                    try
                    {
                        await conn.OpenAsync();
                        object? r = await cmd.ExecuteScalarAsync();
                        if (r != null)
                            res = Convert.ToInt32(r);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error in AddNewDocumentAsync: {ex.Message}");
                    }
                }
            }

            return res;
        }

        public static async Task<bool> UpdateDocumentAsync(Document doc)
        {
            bool res = false;

            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = @"
                    UPDATE Documents
                    SET Content=@Content, CreatedAt=@CreatedAt, Year=@Year, Notes=@Notes, 
                        InternalNumber=@InternalNumber, UserID=@UserID, 
                        DocumentTypeID=@DocumentTypeID, DepartmentID=@DepartmentID
                    WHERE DocumentID=@DocumentID";

                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Content", doc.Content ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedAt", doc.CreatedAt);
                    cmd.Parameters.AddWithValue("@Year", doc.Year);
                    cmd.Parameters.AddWithValue("@Notes", (object?)doc.Notes ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@InternalNumber", doc.InternalNumber);
                    cmd.Parameters.AddWithValue("@UserID", doc.UserID);
                    cmd.Parameters.AddWithValue("@DocumentTypeID", doc.DocumentTypeID);
                    cmd.Parameters.AddWithValue("@DepartmentID", doc.DepartmentID);
                    cmd.Parameters.AddWithValue("@DocumentID", doc.DocumentID);

                    try
                    {
                        await conn.OpenAsync();
                        res = await cmd.ExecuteNonQueryAsync() > 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error in UpdateDocumentAsync: {ex.Message}");
                    }
                }
            }

            return res;
        }

        public static async Task<bool> DeleteDocumentAsync(int documentID)
        {
            bool res = false;

            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = "DELETE FROM Documents WHERE DocumentID=@DocumentID";

                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DocumentID", documentID);

                    try
                    {
                        await conn.OpenAsync();
                        res = await cmd.ExecuteNonQueryAsync() > 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error in DeleteDocumentAsync: {ex.Message}");
                    }
                }
            }

            return res;
        }

        public static async Task<bool> IsDocumentExistAsync(int documentID)
        {
            bool res = false;

            using (SqlConnection conn = new(_ConnectionString))
            {
                string query = "SELECT 1 FROM Documents WHERE DocumentID=@DocumentID";

                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DocumentID", documentID);

                    try
                    {
                        await conn.OpenAsync();
                        object? r = await cmd.ExecuteScalarAsync();
                        res = r != null;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error in IsDocumentExistAsync: {ex.Message}");
                    }
                }
            }

            return res;
        }
    }
}
