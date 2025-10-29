namespace Archive.DAL.Models
{
    public class Operation
    {
        public Operation()
        {
            
        }
        public Operation(int operationID, string? operationName)
        {
            OperationID = operationID;
            OperationName = operationName;
        }

        public int OperationID {  get; set; }
        public string? OperationName { get; set; }
    }
}
