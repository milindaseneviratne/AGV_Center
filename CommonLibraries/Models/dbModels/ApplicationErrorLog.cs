namespace CommonLibraries.Models
{
    public class ApplicationErrorLog
    {
        public int Id { get; set; }
        public string HelpLink { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string HashCode { get; set; }
        public string DumpString { get; set; }
    }
}
