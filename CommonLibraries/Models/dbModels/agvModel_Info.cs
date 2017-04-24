using System.ComponentModel.DataAnnotations;

namespace CommonLibraries.Models.dbModels
{
    public class agvModel_Info
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Name { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Code { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 0)]
        public string Remark { get; set; }
    }
}