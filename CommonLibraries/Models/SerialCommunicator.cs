using CommonLibraries.Extensions;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using CommonLibraries.Models.dbModels;

namespace CommonLibraries.Models
{
    public class SerialCommunicator
    {
        //public static SerialPort SerialPortObj = new SerialPort();
        private SQLCommunicator sqlComm = new SQLCommunicator();

        //Added By Milinda, Universal function to Open the Serial port.
        public static bool OpenSerialPort(SerialPort SerialPortObj, string PortName)
        {
            try
            {
                if (!SerialPortObj.IsOpen)
                {
                    SerialPortObj.PortName = PortName;
                    SerialPortObj.Open();
                }
            }
            catch (Exception e)
            {
                e.WriteLog().Display();
            }

            return SerialPortObj.IsOpen;
        }

        //Added by Milinda, Universal Function to Close the serial Port.
        public static bool CloseSerialPort(SerialPort SerialPortObj, string PortName)
        {
            try
            {
                if (SerialPortObj.IsOpen)
                {
                    Task.Delay(200);
                    SerialPortObj.Close();
                }
            }
            catch (Exception e)
            {
                e.WriteLog().Display();
            }
            return SerialPortObj.IsOpen;
        }

        public BarcodeScanner FindBarcodeScanner(BarcodeScannerConfig scannerConfig)
        {
            BarcodeScanner barcodeScanner = null;

            
            try
            {
                //SELECT * FROM Win32_PnPEntity - can get the scanner but cant Find the XYZ Printers.
                //SELECT * FROM WIN32_SerialPort - can find serialPort devices but not devices that are connected using the USB CDC/ACM driver for windows.
                using (var searcher = new ManagementObjectSearcher(scannerConfig.QueryString))
                {
                    var availableSerialPortsList = SerialPort.GetPortNames().ToList();

                    var connectedDeviceList = searcher.Get().Cast<ManagementBaseObject>().ToList();

                    var connectedSerialDeviceList = connectedDeviceList.Where(x => x[scannerConfig.Key1].ToString().Contains(scannerConfig.Value1)).ToList();

                    Dictionary<string, string> scannerProperties = new Dictionary<string, string>();

                    foreach (var propertyName in scannerConfig.PropertyNames)
                    {
                        foreach (var detectedDevice in connectedSerialDeviceList)
                        {
                            if (detectedDevice[propertyName] != null)
                            {
                                scannerProperties.Add(propertyName, detectedDevice[propertyName].ToString());
                            }
                        }                        
                    }

                    string re1 = ".*?"; // Non-greedy match on filler
                    string re2 = "((?:[a-z][a-z]*[0-9]+[a-z0-9]*))";    // Alphanum 1

                    Regex r = new Regex(re1 + re2, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    Match m = null;

                    foreach (var propValPair in scannerProperties)
                    {
                        m = r.Match(propValPair.Value);
                        if (m.Success) break;
                    }

                    if (m != null && m.Success)
                    {
                        barcodeScanner = new BarcodeScanner();
                        barcodeScanner.COMPortName = m.Groups[1].ToString();
                        barcodeScanner.Description = scannerProperties[scannerConfig.Key2];
                        barcodeScanner.PNPDeviceID = scannerProperties[scannerConfig.Key1];
                        barcodeScanner.Properties = scannerProperties;
                    }
                    else
                    {
                        var cantFindBarcodeScanner =  new Exception("Could Not find " + scannerConfig.Value2 + " " + scannerConfig.Key1 + " with Value: " + scannerConfig.Value1 + "\n Please Check the configurations!");
                        cantFindBarcodeScanner.WriteLog().SaveToDataBase().Display();
                    }
                }
            }
            catch (Exception e)
            {
                e.WriteLog().SaveToDataBase().Display();
            }

            return barcodeScanner;
        }

        public async Task<string> GetScannedBarcode(BarcodeScanner barcodeScanner,CancellationToken token)
        {
            OpenSerialPort(barcodeScanner.SerialPort, barcodeScanner.COMPortName);

            List<string> rxVals = new List<string>();
            bool FinishedReading = false;
            string rxString = string.Empty;

            while (!FinishedReading)
            {
                rxVals.AddRange(await readStringsfromSerialPortAsync(barcodeScanner, token));
                rxString = string.Join("", rxVals.ToArray());

                int bytesInBuffer = barcodeScanner.SerialPort.BytesToRead;
                FinishedReading = bytesInBuffer == 0;
            }

            CloseSerialPort(barcodeScanner.SerialPort, barcodeScanner.COMPortName);

            return rxString;
        }

        private async Task<List<string>> readStringsfromSerialPortAsync(BarcodeScanner barcodeScanner, CancellationToken ct)
        {
            List<string> RecCmds = new List<string>();
            byte[] buffer = new byte[1000];
            string rxData;
            try
            {
                RecCmds.Clear();
                Array.Clear(buffer, 0, buffer.Length);
                Task<int> bytes = barcodeScanner.SerialPort.BaseStream.ReadAsync(buffer, 0, buffer.Length, ct);
                int bytesCount = await bytes;
                rxData = Encoding.ASCII.GetString(buffer).EliminateExtraChars();
                RecCmds.Add(rxData);
            }
            catch (IOException e)
            {
                e.WriteLog().SaveToDataBase();
            }
            catch (OperationCanceledException e)
            {
                e.WriteLog().SaveToDataBase();
            }
            catch (Exception e)
            {
                e.WriteLog().Display();
            }
            return RecCmds;
        }

        public List<BarcodeScanner> AttemptConnectionToScanners()
        {
            //GetScannerConfigsFrom SQL Server.
            List<BarcodeScannerConfig> scannerConfigs = sqlComm.GetBarcodeScannerConfigs();
            List<BarcodeScanner> barcodeScanners = new List<BarcodeScanner>();

            foreach (var scannerConfig in scannerConfigs)
            {
                SerialPort SerialPortObj = new SerialPort();
                BarcodeScanner barcodeScanner = FindBarcodeScanner(scannerConfig);
                if (barcodeScanner != null && !string.IsNullOrWhiteSpace(barcodeScanner.COMPortName))
                {
                    OpenSerialPort(SerialPortObj, barcodeScanner.COMPortName);

                    CloseSerialPort(SerialPortObj, barcodeScanner.COMPortName);

                    barcodeScanner.SerialPort = SerialPortObj;
                    barcodeScanners.Add(barcodeScanner);
                }
            }
            return barcodeScanners;
        }

    }
}
