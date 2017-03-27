using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Common_Lib.Lib
{
    public class Common
    {        

        /// <summary>
        /// SN Const Arguments
        /// </summary>
        public const int SNLength = 13;
        public const int ModelLengh = 4;
        public const int SKULength = 3;
        public const string caOther = "Other";

        /// <summary>
        /// SNLengthCheck Function
        /// </summary>
        /// <param name="SerialNumber"></param>
        /// <param name="SNLength"></param>
        /// <returns></returns>
        public static string SNLengthCheck(string SerialNumber, int iSNLength)
        {         
            string TestResult;

            if (SerialNumber.Length == iSNLength)
            {
                TestResult = "Test Pass";
            }
            else
            {
                TestResult = "Test Failed";
            }

            return TestResult;
        }

        /// <summary>
        /// Model Function
        /// </summary>
        /// <param name="SerialNumber"></param>
        /// <returns></returns>
        public static string DecodeSN_Model(string SN, int ModelLength)
        {
            string ModelName;

            ModelName = SN.Substring(0, ModelLength);

            return ModelName;
        }

        /// <summary>
        /// SKU Function
        /// </summary>
        /// <param name="SerialNumber"></param>
        /// <param name="SKULength"></param>
        /// <returns></returns>
        public static string DecodeSN_SKU(string SN, int SKULength)
        {
            string SKUName;

            SKUName = SN.Substring(0, SNLength);

            return SKUName;
        }

        /// <summary>
        /// Reset_ComboBox
        /// Reset ComboBox object, Set DataSource and Set SelectedItem
        /// </summary>
        /// <param name="cbboxObject"></param>
        /// <param name="Datasource"></param>
        /// <param name="SelectedItem"></param>
        //public static void Reset_ComboBox(ComboBox cbboxObject, string[] Datasource, string SelectedItem)
        //{
        //    //this.cbboxStation.Items.Clear();
        //    //this.cbboxStation.DataSource = ODBC.Config.List_Stations;
        //    //this.cbboxStation.SelectedItem = INIFile.Initial.StationName;
        //    try
        //    {
        //        cbboxObject.Items.Clear();
        //        cbboxObject.DataSource = Datasource;

        //        if (SelectedItem != string.Empty)
        //        {
        //            cbboxObject.SelectedItem = SelectedItem;
        //        }
        //        else
        //        {
        //            cbboxObject.SelectedIndex = 0;
        //        }

                
        //    }
        //    catch (Exception eMsg)
        //    {

        //        Error.ExceptionHandle(Error.Exception_TitleMsg, eMsg.ToString());
        //    }

        //}

        #region "Timer Method"
        private static System.Timers.Timer myTimer;

        public static string CurrentTime = string.Empty;

        private void Run_Timmer(int Interval)
        {        
            // Create a timer with a ten second interval.
            myTimer = new System.Timers.Timer(Interval);

            // Hook up the Elapsed event for the timer.
            myTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            // Set the Interval to 1 seconds (1000 milliseconds).
            myTimer.Interval = Interval;
            myTimer.Enabled = true;            
        }

        // Specify what you want to happen when the Elapsed event is raised. 
        private  void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
            //String.Format("{0:d/M/yyyy HH:mm:ss}", dt); // "9/3/2008 16:05:07" - english (en-US)

            Common.CurrentTime = String.Format("{0:yyyy/M/d HH:mm:ss}", e.SignalTime);
            //this.tsslCurrentTime.Text = Common.CurrentTime;
        }

        #endregion

    }

    public class DLS
    {
        /// <summary>
        /// DLS Arguments
        /// </summary>
        public static string SN = string.Empty;
        public static string Model = string.Empty;
        public static string SKU = string.Empty;
        public static string PCName = string.Empty;
        public static string OPID = string.Empty;
        public static string Lines = string.Empty;
        public static string Stations = string.Empty;
        public static string Language = string.Empty;
        public static string DefectCode = string.Empty;
        public static string Description = string.Empty;
        public static string DLSTime = string.Empty;
                                       
    }

    public class DCS
    {
        
    }

    public class DRS

    {
        public static string DRSTime = string.Empty;    
    }
}
