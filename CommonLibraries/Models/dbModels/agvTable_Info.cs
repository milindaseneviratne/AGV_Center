using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace CommonLibraries.Models.dbModels
{
    public class agvTable_Info
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Zone { get; set; }
        [Required]
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Current_Station { get; set; }
        [Required]
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Dest_Station { get; set; }
        [Required]
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Idle_Area { get; set; }
        [Required]
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Current_Location { get; set; }
        [Required]
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Dest_Location { get; set; }
        [Required]
        public int TaskId { get; set; }
        public virtual agvTask Task { get; set; }
    }
}