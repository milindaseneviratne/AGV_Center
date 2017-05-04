using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Models
{
    public class BarcodeScanner
    {
        public string Description { get; set; }
        public string PNPDeviceID { get; set; }
        public string COMPortName { get; internal set; }
        public Dictionary<string, string> Properties { get; internal set; }
        public SerialPort SerialPort { get; internal set; }
    }
}
