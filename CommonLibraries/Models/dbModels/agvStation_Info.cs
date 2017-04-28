using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Models.dbModels
{
    public class agvStation_Info
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        //[StringLength(maximumLength: 250, MinimumLength = 1)]
        public int ZoneId { get; set; }
        public agvZone_Info Zone { get; set; }

        [Required]
        public int ModelId { get; set; }
        public virtual agvModel_Info Model { get; set; }

        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Location { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Face { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Status { get; set; }
        public DateTime? UpdateTime { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 0)]
        public string Remark { get; set; }

    }
}
