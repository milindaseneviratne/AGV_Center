using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGV_Control_Center.Models
{
    public static class ErrorLogger
    {
        private static DateTime Now;
        private static string logFileName;
        private static string logFolderPath;
        private static string logFullPath;
        private static bool appendText;

        public static void WriteErrorLog(Exception e)
        {
            appendText = true;
            logFileName = DateTime.Today.ToString("ddMMyyyy");
            logFileName = "Error Log " + logFileName + ".log";
            logFolderPath = Directory.GetCurrentDirectory() + "\\" + "Application Error Log";
            logFullPath = logFolderPath + "\\" + logFileName;

            FileInfo file = new FileInfo(logFullPath);
            file.Directory.Create();

            using (StreamWriter writer = new StreamWriter(logFullPath, appendText))
            {
                Now = DateTime.Now;
                writer.Write(Environment.NewLine + Environment.NewLine + "<---------------" + Now.ToString() + "--------------->" + Environment.NewLine);
                writer.Write(e.ToString());
            }
        }
    }

}
