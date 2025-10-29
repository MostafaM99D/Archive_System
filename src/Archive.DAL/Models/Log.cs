namespace Archive.DAL.Models
{
    public class Log
    {
        public Log() { }

        public Log(int logID, int operationID, int? documentID, int userID, int? emailID, int statusID, string? deviceInfo, DateTime logDate)
        {
            LogID = logID;
            OperationID = operationID;
            DocumentID = documentID;
            UserID = userID;
            EmailID = emailID;
            StatusID = statusID;
            DeviceInfo = deviceInfo;
            LogDate = logDate;
        }

        public int LogID { get; set; }
        public int OperationID { get; set; }
        public int? DocumentID { get; set; }
        public int UserID { get; set; }
        public int? EmailID { get; set; }
        public int StatusID { get; set; }
        public string? DeviceInfo { get; set; }
        public DateTime LogDate { get; set; }


        public Operation? Operation { get; set; }
        public Document? Document { get; set; }
        public User? User { get; set; }
        public Email? Email { get; set; }
        public Status? Status { get; set; }
    }
}
