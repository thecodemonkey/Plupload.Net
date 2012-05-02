using System;     
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plupload.Net.Model;
using System.IO;  
using System.Xml; 

namespace Plupload.Net.Utils
{
    public class LogWriter
    {
        private static object lockThis = new object();

        public static void Info(string message) 
        {
            Write(LogLevel.Info, message);
        }

        public static void Error(Exception exp)
        {
            string message = String.Format("{0} : {1}", exp.Message, exp.StackTrace);

            Write(LogLevel.Error, message);
        }

        public static void Warning(string message)
        {
            Write(LogLevel.Warning, message);
        }

        public static void Debug(string message)
        {
            if (Configuration.Debug.HasValue && Configuration.Debug.Value)
                Write(LogLevel.Debug, message);
        }

        private static void Write(LogLevel loglevel, string message)
        {                                                               
            PluploadConfiguration config = PluploadContext.GetConfiguration();  
            string logPath = config.GetPhysicalLogPath();

            lock (lockThis)
            {                           
                if (!Directory.Exists(logPath)) Directory.CreateDirectory(logPath);

                string logFile = Path.Combine(logPath, "plupload.log");
                if (!File.Exists(logFile))
                {
                    using (FileStream fs = File.Create(logFile)) { }
                }

                using (StreamWriter sw = File.AppendText(logFile))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine(String.Format("{0} | {1} | {2} #end#", DateTime.Now, loglevel, message));
                }
            }
        }

        private static PluploadConfiguration Configuration 
        {
            get 
            {
                return PluploadContext.GetConfiguration();
            }
        }

        private static PluploadContext PluploadContext
        {
            get 
            {
                return PluploadContext.Instance;
            }        
        }
    }


    enum LogLevel 
    { 
        Info,
        Debug,
        Error,
        Warning
    }
}
