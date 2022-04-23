using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace webCaisse.Tools
{
    public class MyLogger
    {
        static readonly object _locker = new object();
        public MyLogger()
        {

        }
        public static void log(string _message, string _type)
        {
            switch (_type)
            {
                case MyLoggerCode.STANDARD:
                    LogStandard(_message);
                    break;
                default:
                    LogStandard(_message);
                    break;
            }
        }

        private static void LogStandard(string logMessage)
        {
            var logFilePath = ConfigInfrastructure.BO_FILE_ROOT + ConfigInfrastructure.LOG_STANDARD + @"\" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

            if (!File.Exists(logFilePath))
            {
                FileInfo fi = new FileInfo(logFilePath);
                FileStream fs = fi.Create();
                fs.Close();
            }

            logMessage = string.Format("Logged on [ {1} ]{0}Message : {2}{0}====================================================================={0}",
                        Environment.NewLine, DateTime.Now, logMessage);

            WriteToLog(logMessage, logFilePath);
        }

        private static void WriteToLog(string logMessage, string logFilePath)
        {
            lock (_locker)
            {
                File.AppendAllText(logFilePath, logMessage);
            }
        }
    }
}