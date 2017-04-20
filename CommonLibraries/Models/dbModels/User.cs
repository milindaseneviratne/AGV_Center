namespace CommonLibraries.Models
{
    using System.Collections.ObjectModel;
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.dbUserLogs = new ObservableCollection<UserActivityLog>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Group { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<UserActivityLog> dbUserLogs { get; set; }
    }
}
