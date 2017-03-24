using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common_Lib.Lib;

namespace Common_Lib.Config
{
    public class TestResult
    {
        #region "Static Arguments"
        /// <summary>
        /// Static Arguments
        /// </summary>
        public static string Msg = string.Empty;
        public static DialogResult ButtonResult = DialogResult.None;

        #endregion

        #region "Const Arguments"
        /// <summary>
        /// Purpose: Constent Value for Test Result Return 
        /// Usage: {Captical Constent Value} + {Test Result}, Ex, {SUCCESS}Server IP is 192.168.0.103
        /// {SUCCESS}/{ERROR}/{EXCEPTION}: For Method Test Result
        /// {PASS}/{FAIL}/{ERROR}/{EXCEPTION}: For App Test Result 
        /// </summary>
        public const string SUCCESS = "{SUCCESS}";
        public const string ERROR = "{ERROR}";
        public const string EXCEPTION = "{EXCEPTION}";
        public const string PASS = "{PASS}";
        public const string FAIL = "{FAIL}";

        public const string NewLine = "\r\n";
        
        #endregion

        public enum eDBStatus
        {
            Database_Open_Error,
            Database_Open_Normal,
            Database_Open_Exception
        }

        public enum DialogResult
        {
            None,
            OK,
            Cancel,
            Abort,
            Retry,
            Ignore,
            Yes,
            No
        }

        
    }
}
