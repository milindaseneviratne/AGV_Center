using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Models.dbModels
{
    public class agvAGV_Info
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Name { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Ip { get; set; }
        [Required]
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Current_Location { get; set; }
        [Required]
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Current_Station { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Idle_Area { get; set; }
        public DateTime UpdateTime { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 0)]
        public string Remark { get; set; }
    }
}
