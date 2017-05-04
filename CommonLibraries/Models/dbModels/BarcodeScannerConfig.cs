using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Models.dbModels
{
    public class BarcodeScannerConfig
    {
        public string QueryString { get; set; }
        public string Value1 { get; set; }
        public string Key1 { get; set; }
        public string Value2 { get; set; }
        public string Key2 { get; set; }
        public List<string> PropertyNames { get; set; }
    }
}
