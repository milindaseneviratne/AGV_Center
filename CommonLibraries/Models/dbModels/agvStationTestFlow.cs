using CommonLibraries.Models.dbModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonLibraries.Models
{
    public class agvStationTestFlow
    {
        public int Id { get; set; }

        [Required]
        public int ZoneId { get; set; }
        public virtual agvZone_Info Zone { get; set; }

        [Required]
        public int Current_StationId { get; set; }
        public virtual agvStation_Info Current_Station { get; set; }

        [Required]
        public int Dest_StationId { get; set; }
        public virtual agvStation_Info Dest_Station { get; set; }

        [Required]
        public int StatusId { get; set; }
        public virtual agvStatus_Info Status { get; set; }

        [Required]
        public int Command_TypeId { get; set; }
        public virtual agvCommand_Type_Info Command_Type { get; set; }

        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Idle_Area { get; set; }

        //[StringLength(maximumLength: 250, MinimumLength = 1)]
        //public string Arg1 { get; set; }
        //[StringLength(maximumLength: 250, MinimumLength = 1)]
        //public string Arg2 { get; set; }
        //[StringLength(maximumLength: 250, MinimumLength = 1)]
        //public string Arg3 { get; set; }
        public DateTime UpdateTime { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 0)]
        public string Remark { get; set; }
    }
}
