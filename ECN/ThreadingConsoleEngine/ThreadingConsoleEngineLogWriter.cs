using System;
using System.IO;

namespace ECN_EngineFramework
{
    public static class ThreadingConsoleEngineLogWriter
    {
        [Flags]
        public enum LogTarget
        {
            Console = 1,
            File = 2,
            All = 3
        }

        public static string LogMessageTimeStampFormat = "[{0}] {1}";
        public static LogTarget DefaultLogTarget = LogTarget.All;

        static StreamWriter logStreamWriter;
        static object logStreamMutex = new Object();
        public static void WriteLogMessage(LogTarget target, string message)
        {
            string logMessage = String.Format(LogMessageTimeStampFormat, DateTime.Now, message);
            if ((target & LogTarget.Console) == LogTarget.Console)
            {
                Console.WriteLine(logMessage);
            }
            lock (logStreamMutex)
            {
                if ((target & LogTarget.File) == LogTarget.File)
                {
                    logStreamWriter.WriteLine(message);
                }
            }
        }
        public static void WriteLogMessage(LogTarget target, string format, params object[] args)
        {
            WriteLogMessage(target, String.Format(format, args));
        }

        public static void WriteLogMessage(string format, params object[] args)
        {
            WriteLogMessage(String.Format(format, args));
        }

        public static void WriteLogMessage(string message)
        {
            WriteLogMessage(LogTarget.All, message);
        }
    }
}
