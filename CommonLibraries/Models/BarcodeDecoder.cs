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

        public Barcode GetVCSCommand(byte[] rxMessageArray)
        {
            var scannedString = Encoding.ASCII.GetString(rxMessageArray, 0, rxMessageArray.Length);

            scannedString = scannedString.RemoveEOF();

            barcode = new Barcode();
            ScannedString = scannedString.EliminateExtraChars();

            barcode.Station = ScannedString.Split('@').ToList().FirstOrDefault().Split('+').ToList().LastOrDefault();
            barcode.Group = ScannedString.Split('@').ToList().FirstOrDefault().Split('+').ToList().FirstOrDefault();
            barcode.Status = ScannedString.Split('@').ToList().LastOrDefault();
            barcode.Destination = sqlCommunicator.GetDestination(barcode);
            barcode.Comand = sqlCommunicator.GetCommandType(barcode);

            return barcode;
        }

    }
}
