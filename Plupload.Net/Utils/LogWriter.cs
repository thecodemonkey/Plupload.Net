using System;     
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plupload.Net.Model;
using System.IO;  
using System.Xml; 

namespace Plupload.Net.Utils
{
    /// <summary>
    /// creates and writes logs.
    /// The location of the LogFiles will be defined by PluploadConfiguration.LogDirectory property.
    /// Default location is AppRoot/PluploadLogs/plupload.log
    /// </summary>
    public class LogWriter
    {
        private static object lockThis = new object();

        /// <summary>
        /// writes a simple info message to the logfile
        /// </summary>
        /// <param name="message">text of the info entry</param>
        public static void Info(string message) 
        {
            Write(LogLevel.Info, message);
        }

        /// <summary>
        /// writes an exception
        /// </summary>
        /// <param name="exp">an exception</param>
        public static void Error(Exception exp)
        {
            string message = String.Format("{0} : {1}", exp.Message, exp.StackTrace);

            Write(LogLevel.Error, message);
        }

        /// <summary>
        /// writes an warning
        /// </summary>
        /// <param name="message">text of the warning entry</param>
        public static void Warning(string message)
        {
            Write(LogLevel.Warning, message);
        }

        /// <summary>
        /// writes an warning
        /// </summary>
        /// <param name="message">text of the debug entry</param>
        public static void Debug(string message)
        {
            if (Configuration.Debug.HasValue && Configuration.Debug.Value)
                Write(LogLevel.Debug, message);
        }

        /// <summary>
        /// writes a message using specific loglevel
        /// </summary>
        /// <param name="loglevel">a specific loglevel</param>
        /// <param name="message">text of the debug entry</param>
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

        /// <summary>
        /// gets the merged plupload configuration. 
        /// There are many levels(default => application => instance), 
        /// where you can configure the plupload.net. 
        /// this instance of configuration is merged. You should always working
        /// with the merged configuration.
        /// </summary>
        private static PluploadConfiguration Configuration 
        {
            get 
            {
                return PluploadContext.GetConfiguration();
            }
        }

        /// <summary>
        /// gets the plupload context. PluploadContex is the central(singleton) context, wich
        /// contains the common funtionality. For example the configuration of plupload.net.
        /// </summary>
        private static PluploadContext PluploadContext
        {
            get 
            {
                return PluploadContext.Instance;
            }        
        }
    }


    /// <summary>
    /// the loglevel of a log entry
    /// </summary>
    enum LogLevel 
    { 
        Info,
        Debug,
        Error,
        Warning
    }
}
