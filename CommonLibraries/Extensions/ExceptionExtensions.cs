using CommonLibraries.Models;
using CommonLibraries.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CommonLibraries.Extensions
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
            //Thread showExceptionWindowThread = new Thread(() => DisplayMessageBox(ex));
            //showExceptionWindowThread.SetApartmentState(ApartmentState.STA);
            //showExceptionWindowThread.Start();

            Application.Current.Dispatcher.Invoke(() => DisplayMessageBox(ex));

            return ex;
        }


        private static void DisplayMessageBox(Exception ex)
        {
            Views.MessageBox msgBox = new Views.MessageBox(ex);
            msgBox.ShowDialog();
        }
    }
}
