namespace Archive.API.DTOs
{
    public class LogDTO
    {
        public int LogID { get; set; }
        public int OperationID { get; set; }
        public int? DocumentID { get; set; }
        public int UserID { get; set; }
        public int? EmailID { get; set; }
        public int StatusID { get; set; }
        public string? DeviceInfo { get; set; }
        public DateTime LogDate { get; set; }
    }
}
