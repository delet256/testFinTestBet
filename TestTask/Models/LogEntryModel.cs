namespace TestTask.Models
{
    public class LogEntryModel
    {
        public int Id { get; set; }
        public string? Response { get; set; }
        public string Url { get; set; }
        public string? Query { get; set; }
        public string? Body { get; set; }
        public string Method { get; set; }
        public int StatusCode { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
