using CommonLibraries.Models.dbModels;
using System;
using System.Collections.ObjectModel;

namespace CommonLibraries.Models
{
    public class agvStationTestFlow
    {
        public int Id { get; set; }
        public virtual ObservableCollection<agvZone_Info> Zone { get; set; }
        public virtual ObservableCollection<agvStation_Info> Current_Stations { get; set; }
        public virtual ObservableCollection<agvStation_Info> Dest_Stations { get; set; }
        public virtual ObservableCollection<agvStatus_Info> Statuses { get; set; }
        public virtual ObservableCollection<agvCommand_Type_Info> Command_Types { get; set; }
        public string Idle_Area { get; set; }
        public string Arg1 { get; set; }
        public string Arg2 { get; set; }
        public string Arg3 { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Remark { get; set; }
    }
}
