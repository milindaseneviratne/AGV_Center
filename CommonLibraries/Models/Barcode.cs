using CommonLibraries.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Models
{
    public class Barcode
    {
        public string Comand { get; set; }
        public string Station { get; set; }
        public string Destination { get; set; }
        public BarcodesTypes Type { get; set; }

    }
}
