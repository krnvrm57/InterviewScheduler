using System;
using System.Configuration;
using System.IO;

namespace InterviewScheduler.Common
{
    public class Logger
    {
        public static void LogEntry(ErrorType errorType, string className, string methodName, string exceptionString, string message)
        {
            string errorLogFilePath = GetLogPath(errorType);
            bool allowlogs = true;
            if (errorType == ErrorType.Message && (ConfigurationManager.AppSettings["RecordMessageLogs"].ToString() != "True"))
            {
                allowlogs = false;
            }
            else if (errorType == ErrorType.Warning && (ConfigurationManager.AppSettings["RecordWarningLogs"].ToString() != "True"))
            {
                allowlogs = false;
            }
            if (allowlogs)
            {
                if (errorLogFilePath != null)
                {
                    Random _rNumber = new Random();
                    string refnumber = _rNumber.Next().ToString();
                    try
                    {
                        using (FileStream LogFileStream = File.Open(errorLogFilePath, FileMode.Append, FileAccess.Write, FileShare.Read))
                        {
                            using (StreamWriter LogWriter = new StreamWriter(LogFileStream))
                            {
                                ProcessIncomingMessages(LogWriter, className, methodName, errorType, exceptionString, "Reference Number is = " + refnumber + " \n \t " + message);
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Writting to the Log File 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="ex"></param>
        private static void ProcessIncomingMessages(TextWriter writer, string className, string methodName, ErrorType errorType, string exceptionString, string message)
        {
            writer.Write("TIME        :: ");
            writer.Write(DateTime.Now.ToString());
            writer.WriteLine();
            if (ErrorType.Message != errorType)
            {
                writer.Write("Class Name       :: ");
                writer.Write(className.ToString());
                writer.WriteLine();

                writer.Write("Method Name        :: ");
                writer.Write(methodName.ToString());
                writer.WriteLine();

            }
            writer.Write("INFORMATION TYPE :: ");
            writer.Write(errorType.ToString());
            writer.WriteLine();
            if (ErrorType.Message != errorType)
            {
                writer.Write("EXCEPTION   :: ");
                if (string.Equals(errorType.ToString(), ErrorType.Error.ToString()))
                    writer.Write(exceptionString.ToString());
                else
                    writer.Write("NULL ");
                writer.WriteLine();
            }
            writer.Write("MESSAGE     :: ");
            writer.Write(message.ToString());
            writer.WriteLine();
            writer.Write(Format());
            writer.WriteLine();
        }
        /// <summary>
        /// To check whether directory can be created
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        static bool hasWriteAccessToFolder(string folderPath)
        {
            try
            {
                // Attempt to get a list of security permissions from the folder. 
                // This will raise an exception if the path is read only or do not have access to view the permissions. 
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        /// <summary>
        /// To get log file Location from COnnstant File
        /// </summary>
        /// <returns></returns>
        private static string GetLogPath(ErrorType errorType)
        {
            string formattedDate = GetFormattedDate();
            string TraceFileName = string.Empty;

            TraceFileName = formattedDate + "_" + ConfigurationManager.AppSettings["LogFileName"].ToString();

            string TraceFolderPath = ConfigurationManager.AppSettings["LogPath"].ToString();

            if (!TraceFolderPath.EndsWith("\\"))
            {
                TraceFolderPath = TraceFolderPath + "\\";
            }
            string TraceFilePath = TraceFolderPath + TraceFileName;
            try
            {
                if (!Directory.Exists(TraceFolderPath))
                {
                    if (hasWriteAccessToFolder(ConfigurationManager.AppSettings["LogPath"].ToString()))
                        Directory.CreateDirectory(TraceFolderPath);
                }
            }
            catch (Exception)
            {
            }
            return TraceFilePath;
        }

        /// <summary>
        /// To convert the datetime.now to dd-mm-yyyy
        /// </summary>
        /// <returns></returns>
        private static string GetFormattedDate()
        {
            string date = DateTime.Now.ToShortDateString();
            string formattedDate = string.Empty;
            char oldChar = '/';
            char newChar = '-';
            formattedDate = date.Replace(oldChar, newChar);
            return formattedDate;
        }

        /// <summary>
        /// To formate the log Message
        /// </summary>
        /// <returns></returns>
        public static string Format()
        {
            string pattern = string.Empty;
            for (int count = 0; count < 2; count++)
            {
                pattern = pattern + "---------------------------------------------------------------------------";
            }
            return pattern;
        }

        public enum ErrorType
        {
            Warning,
            Error,
            Message
        }
    }
}
