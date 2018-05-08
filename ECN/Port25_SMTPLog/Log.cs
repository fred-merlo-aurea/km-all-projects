//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.IO;

//namespace Port25_SMTPLog
//{
//    class Log
//    {
//        private string LogPath = Environment.CurrentDirectory + "\\Log\\";
//        private string LogFile = "Log-" + DateTime.Now.ToShortDateString() + ".txt";

//        public Log()
//        {
//            LogFile = LogFile.Replace("/", "-").ToString();

//            if (!Directory.Exists(LogPath))
//                Directory.CreateDirectory(LogPath);

//            if (!File.Exists(LogPath + LogFile))
//            {
//                StreamWriter SW;
//                SW = File.CreateText(LogPath + LogFile);
//                SW.WriteLine("********** CREATE LOG FILE - " + DateTime.Now.ToString() + " **********");
//                SW.Close();

//            }
//        }

//        public void LogMessage(string message)
//        {

//            Console.WriteLine(message);
//            StreamWriter SW;
//            SW = File.AppendText(LogPath + LogFile);
//            SW.WriteLine(message.ToString());
//            SW.Close();
//        }
//    }
//}
