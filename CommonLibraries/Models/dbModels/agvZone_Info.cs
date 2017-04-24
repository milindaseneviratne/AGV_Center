using System.ComponentModel.DataAnnotations;

namespace CommonLibraries.Models
{
    public class agvZone_Info
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Name { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 0)]
        public string Remark { get; set; }
    }
}