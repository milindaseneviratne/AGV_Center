using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common_Lib.Lib
{
    public class Error
    {
        #region "frmMsgBox"
        #region "Error Const"
        /// <summary>
        /// Purpose: Constent Value for Error Info         
        /// </summary>
        public const string Error_TitleMsg = "ErrorHandle";
        public const string Exception_TitleMsg = "ErrorHandle";

        #endregion

        #region "Error Argument"
        /// <summary>
        /// Purpose: Constent Value for Error Info         
        /// </summary>
        public static string errorMsg = string.Empty;
        public static string exceptionMsg = string.Empty;

        #endregion

        #region "ExceptionHandle"
        /// <summary>
        /// ExceptionHandle Type 1
        /// </summary>
        /// <param name="TitleMsg"></param>
        /// <param name="txtMsg"></param>
        //public static void ExceptionHandle(string TitleMsg, string txtMsg)
        //{
        //    new frmMsgBox(TitleMsg, txtMsg).ShowDialog();
        //}

        /// <summary>
        /// ExceptionHandle Type 2
        /// </summary>
        /// <param name="txtEng"></param>
        /// <param name="txtJap"></param>
        /// <param name="lblUnitSerial"></param>
        /// <param name="lblMacAddress"></param>
        /// <param name="lblStation"></param>
        /// <param name="lblDateTime"></param>
        /// <param name="lblRemindMsg"></param>
        //public static void ExceptionHandle(string txtEng, string txtJap)
        //{
        //    new frmErrorMsgBox(txtEng, txtJap).ShowDialog();
        //}       
        #endregion

        #region "ErrorHandle"
        /// <summary>
        /// ErrorHandle Type 1
        /// </summary>
        /// <param name="TitleMsg"></param>
        /// <param name="txtMsg"></param>
        public static void ErrorHandle(string TitleMsg, string txtMsg)
        {
            //new frmMsgBox(TitleMsg, txtMsg).ShowDialog();
        }

        /// <summary>
        /// ErrorHandle Type 2
        /// </summary>
        /// <param name="txtEng"></param>
        /// <param name="txtJap"></param>
        /// <param name="lblUnitSerial"></param>
        /// <param name="lblMacAddress"></param>
        /// <param name="lblStation"></param>
        /// <param name="lblDateTime"></param>
        /// <param name="lblRemindMsg"></param>
        //public static void ErrorHandle( string txtEng, string txtJap)
        //{
        //    new frmErrorMsgBox(txtEng, txtJap).ShowDialog();
        //}        
        #endregion

        #endregion
    }
}
