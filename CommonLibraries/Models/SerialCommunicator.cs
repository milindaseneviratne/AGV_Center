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

namespace CommonLibraries.Models
{
    public class SerialCommunicator
    {
        public static SerialPort SerialPortObj = new SerialPort();
        private BarcodeScanner BarcodeScanner { get; set; }

        //Added By Milinda, Universal function to Open the Serial port.
        public static bool OpenSerialPort(string PortName)
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
        public static bool CloseSerialPort(string PortName)
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

        public BarcodeScanner FindBarcodeScanner()
        {
            try
            {
                //SELECT * FROM Win32_PnPEntity - can get the scanner but cant Find the XYZ Printers.
                //SELECT * FROM WIN32_SerialPort - can find serialPort devices but not devices that are connected using the USB CDC/ACM driver for windows.
                //Barcode scanner DeviceID: USBCDCACM\VID_0C2E&PID_092A\1&2B53A856&0&16364B062D_00
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity"))
                {
                    var availableSerialPortsList = SerialPort.GetPortNames().ToList();

                    var connectedDeviceList = searcher.Get().Cast<ManagementBaseObject>().ToList();

                    var connectedSerialDeviceList = connectedDeviceList.Where(x => x["DeviceID"].ToString().Contains(@"USBCDCACM\VID_0C2E&PID_092A\1&2B53A856&0&16364B062D_00")).ToList();

                    Dictionary<string, string> scannerProperties = new Dictionary<string, string>();

                    List<string> propertyNames = new List<string>();
                    propertyNames.AddRange(new string[] 
                    {
                        "Caption",
                        "ClassGuid",
                        "CompatibleID",
                        "ConfigManagerErrorCode",
                        "ConfigManagerUserConfig",
                        "CreationClassName",
                        "Description",
                        "DeviceID",
                        "ErrorCleared",
                        "ErrorDescription",
                        "HardwareID",
                        "InstallDate",
                        "LastErrorCode",
                        "Name",
                        "PowerManagementCapabilities",
                        "Service",
                        "Status",
                        "StatusInfo",
                        "SystemCreationClassName",
                        "SystemName",
                    });

                    foreach (var propertyName in propertyNames)
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
                    Match m = r.Match(scannerProperties[propertyNames[13]]);

                    if (m.Success)
                    {
                        BarcodeScanner = new BarcodeScanner();
                        BarcodeScanner.COMPortName = m.Groups[1].ToString();
                        BarcodeScanner.Description = scannerProperties["Description"];
                        BarcodeScanner.DriverName = scannerProperties["Name"];
                        BarcodeScanner.Properties = scannerProperties;
                    }
                    else
                    {
                        throw new Exception("Could Not find DeviceID" + @"[USBCDCACM\VID_0C2E&PID_092A\1&2B53A856&0&16364B062D_00]" + "\n Please Check the settings!");
                    }
                }
            }
            catch (Exception e)
            {
                e.WriteLog().Display();
            }

            return BarcodeScanner;
        }

        public async Task<string> GetScannedBarcode(CancellationToken token)
        {
            OpenSerialPort(BarcodeScanner.COMPortName);

            List<string> rxVals = new List<string>();
            bool FinishedReading = false;
            string rxString = string.Empty;

            while (!FinishedReading)
            {
                rxVals.AddRange(await readStringsfromSerialPortAsync(token));
                rxString = string.Join("", rxVals.ToArray());

                int bytesInBuffer = SerialPortObj.BytesToRead;
                FinishedReading = bytesInBuffer == 0;
            }
            return rxString;
        }

        public async Task<List<string>> readStringsfromSerialPortAsync(CancellationToken ct)
        {
            List<string> RecCmds = new List<string>();
            byte[] buffer = new byte[1000];
            string rxData;
            try
            {
                RecCmds.Clear();
                Array.Clear(buffer, 0, buffer.Length);
                Task<int> bytes = SerialPortObj.BaseStream.ReadAsync(buffer, 0, buffer.Length, ct);
                int bytesCount = await bytes;
                rxData = Encoding.ASCII.GetString(buffer).TrimEnd('\0').TrimEnd('\r');
                RecCmds.Add(rxData);
            }
            catch (OperationCanceledException e)
            {
                e.WriteLog();
            }
            catch (Exception e)
            {
                e.WriteLog().Display();
            }
            return RecCmds;
        }

        public bool AttemptConnectionToScanner()
        {
            FindBarcodeScanner();
            if (!string.IsNullOrWhiteSpace(BarcodeScanner.COMPortName))
            {
                OpenSerialPort(BarcodeScanner.COMPortName);

                CloseSerialPort(BarcodeScanner.COMPortName);

                return true;
            }
            return false;
        }

        public BarcodeScanner GetBarcodeScanner()
        {
            return BarcodeScanner;
        }
    }
}
