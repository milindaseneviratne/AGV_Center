using AGV_Control_Center.Models;
using AGV_Control_Center.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGV_Control_Center.Extensions
{
    public static class ExceptionExtensions
    {
        private static SQLCommunicator sqlComm = new SQLCommunicator();
        public static Exception WriteLog(this Exception ex)
        {
            ErrorLogger.WriteErrorLog(ex);
            return ex;
        }

        public static Exception SaveToDataBase(this Exception ex)
        {
            sqlComm.InsertExceptionInformation(ex);
            return ex;
        }

        public static Exception Display(this Exception ex)
        {
            MessageBox msgBox = new MessageBox(ex);
            msgBox.ShowDialog();
            return ex;
        }

    }
}
