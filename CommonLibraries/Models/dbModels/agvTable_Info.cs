using System.Collections.ObjectModel;

namespace CommonLibraries.Models.dbModels
{
    public class agvTable_Info
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual agvZone_Info Zone { get; set; }
        public virtual agvStation_Info Current_Station { get; set; }
        public virtual agvStation_Info Dest_Station { get; set; }
        public string Idle_Area { get; set; }
        public string Current_Location { get; set; }
        public string Dest_Location { get; set; }
        public virtual agvTask Task { get; set; }
    }
}