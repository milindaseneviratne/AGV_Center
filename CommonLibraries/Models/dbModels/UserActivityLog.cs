namespace CommonLibraries.Models
{
    using System;
   
    public partial class UserActivityLog
    {
        public byte[] TimeStamp { get; set; }
        public long Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserGroup { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public virtual User dbUser { get; set; }
    }
}
