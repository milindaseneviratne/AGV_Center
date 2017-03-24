using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common_Lib.Lib;
using Common_Lib.Config;
using System.Runtime.InteropServices;   //For INI File Class

namespace Common_Lib.Lib
{
    public class Files
    {
        #region "Const Arguments"
        /// <summary>
        /// Const Arguments
        /// </summary>
        public const string DRRS_Log_File = "\\DRRS_Log.txt";
        
        #endregion

        #region "Static Arguments"
        /// <summary>
        /// Static Arguments
        /// </summary>
        public static string LogFile = string.Empty;
        public static string IniFile = string.Empty;
        public static string CsvFile = string.Empty;
        public static string ExeFile = string.Empty;

        #endregion

        #region "Methods"
        //StreamWriter
        public static void SaveLogFile(string FileName, string TitleMsg, string Text)
        {
            StreamWriter sw = new StreamWriter(FileName, true);
            sw.WriteLine("< " + TitleMsg + " >");
            sw.WriteLine(DateTime.Now.ToString()); 
            sw.WriteLine(Text + TestResult.NewLine);            
            sw.Flush(); 
            sw.Close();
        }

        #endregion
        
    }

    public class INIFile
    {
        #region "Arguments"
        /// <summary>
        /// Const Arguments
        /// </summary>
        public const string caINIFileName = "DRRS_Config.ini";
        public const string caConfigFilePath = "\\Config";

        public const string caSection_AppInfo = "AppInfo";
        public const string caSection_Initial = "Initial";
        public const string caSection_NKG_DRRS = "NKG_DRRS";
        public const string caSection_DRRS_Config = "DRRS_Config";
        public const string caSection_DRRS_Database = "DRRS_Database";
        
        /// <summary>
        /// Static Arguments
        /// </summary>        
        public static string ConfigFileName = Application.StartupPath + "\\" + caINIFileName;
        public static string SectionName = string.Empty;
        public static string KeyName = string.Empty;
        public static string KeyValue = string.Empty;

        /// <summary>
        /// AppInfo section Arguments
        /// </summary>
        public class AppInfo
        {                        
            public static string ProgramName = string.Empty;
            public static string Version = string.Empty;        
        }

        /// <summary>
        /// Initial Section Arguments
        /// </summary>
        public class Initial
        {            
            public static string Customer = string.Empty;
            public static string LineNo = string.Empty;
            public static string StationName = string.Empty;
            public static string Language = string.Empty;
            public static string OPID = string.Empty;
        }

        /// <summary>
        /// NKG_DRRS section Arguments
        /// </summary>
        public class NKG_DRRS
        {         
            public static string DSN = string.Empty;
            public static string ServerName = string.Empty;
            public static string DatabaseName = string.Empty;
        }

        /// <summary>
        /// DRRS_Config section Arguments
        /// </summary>
        public class DRRS_Config
        {
            public static string DSN = string.Empty;    
        }

        /// <summary>
        /// DRRS_Database section Arguments
        /// </summary>
        public class DRRS_Database
        {
            public static string DSN = string.Empty;    
        }

        #endregion

        #region "DLLImport"
        
        [DllImport("kernel32.dll")]
        private static extern int WritePrivateProfileString(string ApplicationName, string KeyName, string StrValue, string FileName);

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(string ApplicationName, string KeyName, string DefaultValue, StringBuilder ReturnString, int Size
                                                            , string FileName);

        #endregion

        #region "Methods"
        public static void WriteValue(string SectionName, string KeyName, string KeyValue, string FileName)
        {

            WritePrivateProfileString(SectionName, KeyName, KeyValue, FileName);

        }

        public static string ReadValue(string SectionName, string KeyName, string FileName)
        {
            StringBuilder szStr = new StringBuilder(255);

            GetPrivateProfileString(SectionName, KeyName, "", szStr, 255, FileName);

            return szStr.ToString().Trim();

        }

        ///User Sample Code
        //string FileName = Application.StartupPath + "\\" + "DRRS_Config.ini";
        //INIFile.WriteValue("Initial", "Customer", "KM", FileName);
        //string KeyValue = INIFile.ReadValue("Initial", "Customer", FileName);

        /// <summary>
        /// Load_Config_KeyValue
        /// </summary>
        public static void Load_DRRS_Config_Arguments()
        {
            INIFile.Load_AppInfo();
            INIFile.Load_Initial();
            INIFile.Load_NKG_DRRS();
            INIFile.Load_DRRS_Config();
            INIFile.Load_DRRS_Database();
        }

        public static void Load_AppInfo()
        {
            INIFile.AppInfo.ProgramName = ReadValue(INIFile.caSection_AppInfo, "ProgramName", ConfigFileName);
            INIFile.AppInfo.Version = ReadValue(INIFile.caSection_AppInfo, "Version", ConfigFileName);
        }

        public static void Load_Initial()
        {
            INIFile.Initial.Customer = ReadValue(INIFile.caSection_Initial, "Customer", ConfigFileName);
            INIFile.Initial.LineNo = ReadValue(INIFile.caSection_Initial, "LineNo", ConfigFileName);
            INIFile.Initial.StationName = ReadValue(INIFile.caSection_Initial, "StationName", ConfigFileName);
            INIFile.Initial.Language = ReadValue(INIFile.caSection_Initial, "Language", ConfigFileName);
            INIFile.Initial.OPID = ReadValue(INIFile.caSection_Initial, "OPID", ConfigFileName);
        }

        public static void Load_NKG_DRRS()
        {
            INIFile.NKG_DRRS.DSN = ReadValue(INIFile.caSection_NKG_DRRS, "DSN", ConfigFileName);
            INIFile.NKG_DRRS.ServerName = ReadValue(INIFile.caSection_NKG_DRRS, "ServerName", ConfigFileName);
            INIFile.NKG_DRRS.DatabaseName = ReadValue(INIFile.caSection_NKG_DRRS, "DatabaseName", ConfigFileName);
        }

        public static void Load_DRRS_Config()
        {
            INIFile.DRRS_Config.DSN = ReadValue(INIFile.caSection_DRRS_Config, "DSN", ConfigFileName);
        }

        public static void Load_DRRS_Database()
        {
            INIFile.DRRS_Database.DSN = ReadValue(INIFile.caSection_DRRS_Database, "DSN", ConfigFileName);
        }

        public static void Set_Initial_OPID()
        {
            WriteValue(INIFile.caSection_Initial, "OPID", INIFile.Initial.OPID, ConfigFileName);
            INIFile.Initial.OPID = ReadValue(INIFile.caSection_Initial, "OPID", ConfigFileName);
        }

        #endregion
    }
}
