using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Core.ADMS
{
    [XmlRoot("exception"), XmlType("exception")]
    public class SerializableException
    {
        [XmlElement("message")]
        public string Message { get; set; }

        [XmlElement("innerException")]
        public SerializableException InnerException { get; set; }
    }

    public class Logging
    {
        public string pathToLogLocation = BaseDirs.getLogDir() + "\\";
        public string logFileName = DateTime.Now.ToString("yyyy-dd-MM") + ".log";
        public string xmlFileName = DateTime.Now.ToString("yyyy-dd-MM") + ".xml";
        StreamWriter logWriter;
        public string appSettingLogFileName
        {
            get
            {
                string name = string.Empty;

                if (System.Configuration.ConfigurationManager.AppSettings["LogFileName"] != null)
                    name = System.Configuration.ConfigurationManager.AppSettings["LogFileName"].ToString() + "_";
                else
                    name = "ErrorLog_";
                return name;
            }
        }
        public void LogIssue(string exception, int sourceFileID = 0)
        {
            if (sourceFileID > 0)
                logWriter = new StreamWriter(new FileStream(pathToLogLocation + sourceFileID.ToString() + "_" + appSettingLogFileName + logFileName, FileMode.Append));
            else
                logWriter = new StreamWriter(new FileStream(pathToLogLocation + appSettingLogFileName + logFileName, FileMode.Append));
            try
            {
                logWriter.AutoFlush = true;
                if (sourceFileID > 0)
                    logWriter.WriteLine("SourceFileID: " + sourceFileID.ToString());
                logWriter.WriteLine(DateTime.Now.ToString() + ": " + exception);
                logWriter.Flush();
                logWriter.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                logWriter.Close();
                logWriter.Dispose();
            }
        }
        public void LogIssue(Exception exception, int sourceFileID = 0)
        {
            if (sourceFileID > 0)
                logWriter = new StreamWriter(new FileStream(pathToLogLocation + sourceFileID.ToString() + "_" + appSettingLogFileName + logFileName, FileMode.Append));
            else
                logWriter = new StreamWriter(new FileStream(pathToLogLocation + appSettingLogFileName + logFileName, FileMode.Append));
            try
            {
                logWriter.AutoFlush = true;
                logWriter.WriteLine(DateTime.Now.ToString());
                if (sourceFileID > 0)
                    logWriter.WriteLine("SourceFileID: " + sourceFileID.ToString());
                logWriter.WriteLine(Core_AMS.Utilities.StringFunctions.FormatException(exception));

                logWriter.Flush();
                logWriter.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                logWriter.Close();
                logWriter.Dispose();
            }
        }
        public void LogIssue(string exception, string fileName, int sourceFileID = 0)
        {
            logWriter = new StreamWriter(new FileStream(pathToLogLocation + fileName + "_" + logFileName, FileMode.Append));
            try
            {
                logWriter.AutoFlush = true;
                if (sourceFileID > 0)
                    logWriter.WriteLine("SourceFileID: " + sourceFileID.ToString());
                logWriter.WriteLine(DateTime.Now.ToString() + ": " + exception);
                logWriter.Flush();
                logWriter.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                logWriter.Close();
                logWriter.Dispose();
            }
        }
        public void LogIssue(Exception exception, string fileName, int sourceFileID = 0)
        {
            logWriter = new StreamWriter(new FileStream(pathToLogLocation + fileName + "_" + logFileName, FileMode.Append));
            try
            {
                logWriter.AutoFlush = true;
                logWriter.WriteLine(DateTime.Now.ToString());
                if (sourceFileID > 0)
                    logWriter.WriteLine("SourceFileID: " + sourceFileID.ToString());
                logWriter.WriteLine(Core_AMS.Utilities.StringFunctions.FormatException(exception));

                logWriter.Flush();
                logWriter.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                logWriter.Close();
                logWriter.Dispose();
            }
        }
        public static void LogXMLIssue(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException("exception");
            StringWriter sw = new StringWriter();
            using (XmlWriter xw = XmlWriter.Create(sw))
            {
                WriteException(xw, "exception", exception);
            }
        }

        public static void WriteException(XmlWriter writer, string name, Exception exception)
        {
            if (exception == null) return;
            writer.WriteStartElement(name);
            writer.WriteElementString("message", exception.Message);
            writer.WriteElementString("source", exception.Source);
            WriteException(writer, "innerException", exception.InnerException);
            writer.WriteEndElement();
        }
    }
}
