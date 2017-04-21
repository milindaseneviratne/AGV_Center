using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Models.dbModels
{
    public class agvTask
    {
        public int Id { get; set; }
        public virtual ObservableCollection<agvModel_Info> Models { get; set; }
        public virtual ObservableCollection<agvTable_Info> Tables { get; set; }
        public virtual ObservableCollection<agvStation_Info> Current_Stations { get; set; }
        public virtual ObservableCollection<agvStation_Info> Dest_Stations { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public string Result { get; set; }
        public string ErrorMessage { get; set; }

    }
}
