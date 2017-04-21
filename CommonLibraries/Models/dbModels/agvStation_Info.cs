using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Models.dbModels
{
    public class agvStation_Info
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ObservableCollection<agvZone_Info> Zones { get; set; }
        public virtual ObservableCollection<agvModel_Info> Models { get; set; }
        public string Location { get; set; }
        public string Face { get; set; }
        public string Status { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string Remark { get; set; }
    }
}
