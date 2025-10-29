namespace Archive.DAL.Models
{
    public class Status
    {
        public Status()
        {
            
        }
        public Status(int statusID, string? statusName)
        {
            StatusID = statusID;
            StatusName = statusName;
        }

        public int StatusID { get; set; }
        public string? StatusName { get; set; }
    }
}
