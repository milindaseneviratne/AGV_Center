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

            if (ScannedString.Contains("TableMove"))
            {
                barcode.Comand = ScannedString;
                barcode.Station = ScannedString.Split('@').ToList().LastOrDefault();
                barcode.Type = BarcodesTypes.TableMove;

                barcode.Destination = sqlCommunicator.GetDestination(barcode);
            }

            return barcode;
        }

    }
}
