using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Models.dbModels
{
    public class agvAGV_Info
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Current_Location { get; set; }
        public agvStation_Info Current_Station { get; set; }
        public string Idle_Area { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Remark { get; set; }
    }
}
