namespace Archive.DAL.Models
{
    public class Email
    {
        public Email()
        {
        }

        public Email(int emailID, string? content, DateTime createdAt, int year, string? notes, int userID, int emailTypeID, int departmentID)
        {
            EmailID = emailID;
            Content = content;
            CreatedAt = createdAt;
            Year = year;
            Notes = notes;
            UserID = userID;
            EmailTypeID = emailTypeID;
            DepartmentID = departmentID;
        }

        public int EmailID { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Year { get; set; }
        public string? Notes { get; set; }
        public int UserID { get; set; }
        public int EmailTypeID { get; set; }
        public int DepartmentID { get; set; }

        // navy
        public User? User { get; set; }
        public EmailType? EmailType { get; set; }
        public Department? Department { get; set; }
    }

}
