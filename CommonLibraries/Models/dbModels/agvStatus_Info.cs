using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace CommonLibraries.Models
{
    public class agvStatus_Info
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Status { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Arg1 { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Arg2 { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Arg3 { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 0)]
        public string Remark { get; set; }
    }
}