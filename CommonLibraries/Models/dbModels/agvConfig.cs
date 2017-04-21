
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Models.dbModels
{
    public class agvConfig
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Remark { get; set; }
    }
}
