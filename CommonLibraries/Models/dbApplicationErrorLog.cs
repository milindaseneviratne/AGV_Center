
namespace CommonLibraries.Models
{
    public class dbApplicationErrorLog
    {
        public int Id { get; set; }
        public string HelpLink { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string HashCode { get; set; }
        public string Dump { get; set; }
    }
}
