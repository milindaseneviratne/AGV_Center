using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Models
{
    public class Device
    {
        public string Name { get; set; }
        public string DriverName { get; set; }
        public string COMPortName { get; internal set; }
    }
}
