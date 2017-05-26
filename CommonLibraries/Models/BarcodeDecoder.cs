using CommonLibraries.Database;
using CommonLibraries.Enumerations;
using CommonLibraries.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace CommonLibraries.Models
{
    public class BarcodeDecoder
    {
        private SQLCommunicator sqlCommunicator = new SQLCommunicator();
        private Barcode barcode;
        private ConcurrentQueue<string> _agvRxQueue;
        private ConcurrentQueue<Barcode> _vcsTxQueue;

        public BarcodeDecoder(ConcurrentQueue<string> agvRxQueue, ConcurrentQueue<Barcode> vcsTxQueue)
        {
            _agvRxQueue = agvRxQueue;
            _vcsTxQueue = vcsTxQueue;
        }

        public string ScannedString { get; set; }

        private Barcode GetVCSCommand(string rxString)
        {
            rxString = rxString.RemoveEOF();

            barcode = new Barcode();
            ScannedString = rxString.EliminateExtraChars();

            barcode.Station = ScannedString.Split('@').ToList().FirstOrDefault().Split('+').ToList().LastOrDefault();
            barcode.Group = ScannedString.Split('@').ToList().FirstOrDefault().Split('+').ToList().FirstOrDefault();
            barcode.Status = ScannedString.Split('@').ToList().LastOrDefault();
            barcode.Destination = sqlCommunicator.GetDestination(barcode);
            barcode.Comand = sqlCommunicator.GetCommandType(barcode);

            return barcode;
        }

        public void ProcessBarcodes()
        {
            while (true)
            {
                string result;
                bool success = _agvRxQueue.TryDequeue(out result);
                if (!success) continue;
                _vcsTxQueue.Enqueue(GetVCSCommand(result));
            }
            
        }
    }
}
