using CommonLibraries.Extensions;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;


namespace CommonLibraries.Models
{
    public class SerialCommunicator
    {
        public static SerialPort SerialPortObj = new SerialPort();
        public Device BarcodeScanner { get; set; }
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

        public List<Device> FindMatchingSerialPorts()
        {
            List<Device> barcodeScanners = new List<Device>();

            try
            {
                //SELECT * FROM Win32_PnPEntity - can get the scanner but cant Find the XYZ Printers.
                //SELECT * FROM WIN32_SerialPort - can find serialPort devices but not devices that are connected using the USB CDC/ACM driver for windows.
                //Barcode scanner DeviceID: USBCDCACM\VID_0C2E&PID_092A\1&2B53A856&0&16364B062D_00
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity"))
                {
                    var availableSerialPortsList = SerialPort.GetPortNames().ToList();

                    var connectedDeviceList = searcher.Get().Cast<ManagementBaseObject>().ToList();

                    //var connectedSerialDeviceList = (from serialPort in availableSerialPortsList
                    //                                 join connectedDevice in connectedDeviceList on serialPort equals connectedDevice["Name"].ToString()
                    //                                 select serialPort + " - " + connectedDevice["Caption"]).ToList();

                    var connectedSerialDeviceList = connectedDeviceList.Where(x => x["DeviceID"].ToString().Contains("USBCDCACM"));

                    foreach (var driverName in connectedSerialDeviceList)
                    {
                        bool IsCorrectDevice = driverName["Name"].ToString().Contains("VID_0C2E&PID_092A" ?? "Unknown Driver Name");
                        BarcodeScanner.COMPortName = driverName["Name"].ToString();
                    }

                    if (barcodeScanners == null)
                    {
                        throw new Exception("Correct driver name:  " + BarcodeScanner.DriverName + "\n Please Check the driver name!!");
                    }

                }
            }
            catch (Exception e)
            {
                e.WriteLog().Display();
            }
            return barcodeScanners;
        }

        public bool AttemptConnectionToPort()
        {
            FindMatchingSerialPorts();
            OpenSerialPort(BarcodeScanner.COMPortName);


            CloseSerialPort(BarcodeScanner.COMPortName);

            return true;
        }
    }
}
