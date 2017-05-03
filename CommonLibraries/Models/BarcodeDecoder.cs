using CommonLibraries.Database;
using CommonLibraries.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Models
{
    public static class BarcodeDecoder
    {
        private static SQLCommunicator sqlCommunicator = new SQLCommunicator();
        private static Barcode barcode = new Barcode();
        public static string ScannedString { get; set; }
        public static string Command { get; set; }
        public static agvStationTestFlow TestFlow { get; set; }
        public static Barcode GetBarcode(string scannedString)
        {
            ScannedString = scannedString;

            if (ScannedString.Contains("OK"))
            {
                barcode.Comand = ScannedString.Split('@').ToList().LastOrDefault();
                barcode.Station = ScannedString.Split('@').ToList().FirstOrDefault().Split('+').ToList().LastOrDefault();
                barcode.Group = ScannedString.Split('@').ToList().FirstOrDefault().Split('+').ToList().FirstOrDefault().TrimStart('*');

                barcode.Destination = sqlCommunicator.GetDestination(barcode);
            }

            return barcode;
        }

    }
}
