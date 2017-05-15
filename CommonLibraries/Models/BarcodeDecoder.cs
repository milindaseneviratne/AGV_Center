using CommonLibraries.Database;
using CommonLibraries.Enumerations;
using CommonLibraries.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Models
{
    public class BarcodeDecoder
    {
        private SQLCommunicator sqlCommunicator = new SQLCommunicator();
        private Barcode barcode;
        public string ScannedString { get; set; }
        public string Command { get; set; }
        public agvStationTestFlow TestFlow { get; set; }
        public Barcode GetVCSCommand(string scannedString)
        {
            barcode = new Barcode();
            ScannedString = scannedString.EliminateExtraChars();

            //if (ScannedString.Contains("OK") || ScannedString.Contains("Initial"))
            //{
                barcode.Station = ScannedString.Split('@').ToList().FirstOrDefault().Split('+').ToList().LastOrDefault();
                barcode.Group = ScannedString.Split('@').ToList().FirstOrDefault().Split('+').ToList().FirstOrDefault();
                barcode.Status = ScannedString.Split('@').ToList().LastOrDefault();
                barcode.Destination = sqlCommunicator.GetDestination(barcode);
                barcode.Comand = sqlCommunicator.GetCommandType(barcode);
            //}

            return barcode;
        }

    }
}
