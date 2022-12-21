using MongoDB.Bson;

namespace Models
{
    public class DirectoryDocument
    {
        public ObjectId Id { get; set; }
        public string DirectoryPath { get; set; }
        public ProcessStatus Status { get; set; }
        public string ErrorMessage { get; set; }
    }

    public enum ProcessStatus
    {
        Found,
        Success,
        Failed
    }
}
