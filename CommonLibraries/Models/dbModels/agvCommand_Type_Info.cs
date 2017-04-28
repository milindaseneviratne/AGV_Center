using System.ComponentModel.DataAnnotations;

namespace CommonLibraries.Models
{
    public class agvCommand_Type_Info
    {
        public int Id { get; set; }
        [StringLength(maximumLength:250,MinimumLength =1)]
        public string Name { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Byte { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Type { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string Argument { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 1)]
        public string DataSource { get; set; }
        [StringLength(maximumLength: 250, MinimumLength = 0)]
        public string Remark { get; set; }
    }
}