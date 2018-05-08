using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

using KMPS.MD.Objects;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Data.OleDb;
using System.Globalization;
using System.Xml.Serialization;
using KMPlatform.Entity;

namespace MAF.NorthStarSendDimensionsImport
{
    class Program
    {
        private static int ImportEngineID = Convert.ToInt32(ConfigurationManager.AppSettings["EngineID"]);
        private string ImportLogFile = String.Format("{0}\\Import_log_{1}.log", ConfigurationManager.AppSettings["ImportLogFilePath"], DateTime.Now.ToString("yyyy_MM_dd"));
        private string ImportDirectory = ConfigurationManager.AppSettings["ImportLocation"];
        private string ArchiveDirectory = ConfigurationManager.AppSettings["ArchiveLocation"];
        private string ImportDirectoryServer = ConfigurationManager.AppSettings["ImportServerLocation"];
        private string ImportFile = ConfigurationManager.AppSettings["ImportFileName"] + DateTime.Now.ToString("MM_dd_yyyy") + ".txt";
        static int KMCommon_Application;

        Client client;
        KMPlatform.Object.ClientConnections clientconnections;

        static void Main(string[] args)
        {
            Console.Title = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            new Program().Start();
        }
        void Start()
        {
            bool importAll = false;
            bool.TryParse(ConfigurationManager.AppSettings["ImportAll"].ToString(), out importAll);

            client = new KMPlatform.BusinessLogic.Client().Select(false).Find(x => x.IsActive == true && x.IsAMS == true && x.ClientLiveDBConnectionString.Length > 0 && x.ClientID == int.Parse(ConfigurationManager.AppSettings["ClientID"]));

            clientconnections = new KMPlatform.Object.ClientConnections(client);

            if (importAll == true)
                ImportAll();
            else
                RunEngine();
        }
        public void ImportAll()
        {
            WriteStatus(string.Format("MAF.NorthStarSendDimensionImport  Import All Engine({0}) started", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)));

            

            KMCommon_Application = -1;
            int.TryParse(ConfigurationManager.AppSettings["KMCommon_Application"].ToString(), out KMCommon_Application);

            try
            {
                DirectoryInfo di = new DirectoryInfo(ImportDirectory);
                
                foreach (FileInfo fi in di.GetFiles())
                {
                    Process myProcess = new Process();

                    myProcess.ColumnDelimiter = ColumnDelimiter.tab;

                    List<SendDimensionFile> lSendDimensionFile = ConvertDataTableToObject(LoadFile(fi, myProcess));

                    ImportData(lSendDimensionFile);

                    WriteStatus("Moving local import file to archive");
                    System.IO.File.Move(fi.FullName, ArchiveDirectory + "\\" + fi.Name);
                    WriteStatus("Deleteing import file from server");
                    System.IO.File.Delete(fi.FullName);

                    WriteStatus(string.Format("Personify Engine({0}) completed", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)));
                }
            }
            catch (Exception e)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(e, string.Format("Import engine({0}) encountered an exception when Handling Import.", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)), Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), string.Format("An exception Happened when handling Import Engine ID = {5} Exception Message: {1}{0} Exception Source: {2}{0} Stack Trace: {3}{0} Inner Exception: {4}{0}",
                    string.Format("{0}{1}{0}", Environment.NewLine, new string('-', 80)),
                    e.Message, e.Source, e.StackTrace, e.InnerException,
                    ImportEngineID));

                WriteStatus(e.Message);
            }
        }
        public void RunEngine()
        {
            WriteStatus(string.Format("MAF.NorthStarSendDimensionImport  Engine({0}) started", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)));

            KMCommon_Application = -1;
            int.TryParse(ConfigurationManager.AppSettings["KMCommon_Application"].ToString(), out KMCommon_Application);

            try
            {
                if (GetImportFile())
                {
                    FileInfo fi = new FileInfo(ImportDirectory + "\\" + ImportFile);

                    Process myProcess = new Process();

                    myProcess.ColumnDelimiter = ColumnDelimiter.tab;

                    List<SendDimensionFile> lSendDimensionFile = ConvertDataTableToObject(LoadFile(fi, myProcess));

                    ImportData(lSendDimensionFile);

                    WriteStatus("Moving local import file to archive");
                    System.IO.File.Move(ImportDirectory + "\\" + ImportFile, ArchiveDirectory + "\\" + ImportFile);
                    WriteStatus("Deleteing import file from server");
                    System.IO.File.Delete(ImportDirectoryServer + "\\" + ImportFile);

                    WriteStatus(string.Format("Personify Engine({0}) completed", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)));
                }
                else
                {
                    throw new Exception("Missing File: " + ImportFile);
                }

            }
            catch (Exception e)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(e, string.Format("Import engine({0}) encountered an exception when Handling Import.", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)), Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), string.Format("An exception Happened when handling Import Engine ID = {5} Exception Message: {1}{0} Exception Source: {2}{0} Stack Trace: {3}{0} Inner Exception: {4}{0}",
                    string.Format("{0}{1}{0}", Environment.NewLine, new string('-', 80)),
                    e.Message, e.Source, e.StackTrace, e.InnerException,
                    ImportEngineID));

                WriteStatus(e.Message);
            }
        }

        private bool GetImportFile()
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["ByPassDownload"]))
            {
                if (System.IO.File.Exists(ImportDirectory + "\\" + ImportFile))
                    return true;
                else
                    return false;
            }
            else
            {
                WriteStatus("DownloadingFile");
                System.IO.File.Copy(ImportDirectoryServer + "\\" + ImportFile, ImportDirectory + "\\" + ImportFile, true);
                return true;
            }
        }
        public static DataTable LoadFile(FileInfo file, Process myProcess)
        {
            Microsoft.VisualBasic.FileIO.TextFieldParser tfp = new Microsoft.VisualBasic.FileIO.TextFieldParser(file.FullName);
            tfp.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
            ColumnDelimiter delimiter = myProcess.ColumnDelimiter;
            if (delimiter == ColumnDelimiter.comma)
                tfp.SetDelimiters(",");
            else if (delimiter == ColumnDelimiter.semicolon)
                tfp.SetDelimiters(";");
            else if (delimiter == ColumnDelimiter.tab)
                tfp.SetDelimiters("\t");
            else if (delimiter == ColumnDelimiter.colon)
                tfp.SetDelimiters(":");
            else if (delimiter == ColumnDelimiter.tild)
                tfp.SetDelimiters("~");

            if (myProcess.IsQuoteEncapsulated == true)
                tfp.HasFieldsEnclosedInQuotes = true;
            else
                tfp.HasFieldsEnclosedInQuotes = false;

            //convert to dataset:
            DataSet ds = new DataSet();
            ds.Tables.Add("Data");

            String[] stringRow = tfp.ReadFields();
            foreach (String field in stringRow)
            {
                ds.Tables[0].Columns.Add(field, Type.GetType("System.String"));
            }

            //populate with data:
            while (!tfp.EndOfData)
            {
                stringRow = tfp.ReadFields();
                ds.Tables[0].Rows.Add(stringRow);
            }

            tfp.Close();

            return ds.Tables["Data"];
        }
        public static List<SendDimensionFile> ConvertDataTableToObject(DataTable dt)
        {
            List<SendDimensionFile> lSendDimensionFile = new List<SendDimensionFile>();

            foreach (DataRow currentRow in dt.Rows)
            {
                    SendDimensionFile s = new SendDimensionFile();

                    s.Email = currentRow["email"].ToString();
                    s.Code = currentRow["code"].ToString();
                    s.flag = bool.Parse(currentRow["flag"].ToString());

                    lSendDimensionFile.Add(s);
            }
            return lSendDimensionFile;
        }
        private void ImportData(List<SendDimensionFile> lSendDimensionFile)
        {
            #region Click Import
            WriteStatus("Total Records: " + lSendDimensionFile.Count.ToString());
            int counter = 0;
            int batch = 500;
            int batchcount = 0;
            int processedCount = 0;
            int total = lSendDimensionFile.Count;
            int.TryParse(ConfigurationManager.AppSettings["ImportBatchSize"].ToString(), out batch);
            StringBuilder xml = new StringBuilder();

            List<MasterGroup> lMasterGroup = MasterGroup.GetAll(clientconnections);

            MasterGroup SendDimension = lMasterGroup.Single(x => x.Name.ToUpper() == "NUMBER OF SENDS");

            if (SendDimension == null)
                throw new Exception("NUMBER OF SENDS Dimension do not exists.");

            List<MasterCodeSheet> lMasterCodeSheet = MasterCodeSheet.GetByMasterGroupID(clientconnections, SendDimension.MasterGroupID);

            var senddata = from c in lMasterCodeSheet
                           join s in lSendDimensionFile on c.MasterValue equals s.Code
                           select new {s.Email, c.MasterID, s.flag};

            foreach (var s in senddata)
            {
                xml.AppendLine("<SendDimension>");
                xml.AppendLine("<email>" + CleanXMLString(s.Email) + "</email>");
                xml.AppendLine("<masterid>" + s.MasterID + "</masterid>");

                //if flag is true in the file - add the record
                //if flag is false in the file - remove the record
                xml.AppendLine("<isdelete>" + (s.flag ? "0" : "1") + "</isdelete>");
                xml.AppendLine("</SendDimension>");

                counter++;
                processedCount++;

                if (processedCount == total || counter == batch)
                {
                    UpdatetoDB(xml.ToString(), SendDimension.MasterGroupID);
                    xml = new StringBuilder();
                    counter = 0;
                    batchcount++;
                    WriteStatus(string.Format("Batch count : {0} / {1} ", batchcount, DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt")));
                }

            }

            WriteStatus("Rebuilding SubscriberMasterValues ");
            RebuildSubscriberMasterValues(SendDimension.MasterGroupID);
            
            #endregion
        }

        public void UpdatetoDB(string xml, int MastergroupID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "job_UpdateSendDimension";
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@Xml", "<XML>" + xml + "</XML>");
                cmd.Parameters.AddWithValue("@MastergroupID", MastergroupID);

                DataFunctions.execute(cmd, DataFunctions.GetClientSqlConnection(clientconnections));
            }
            catch (Exception e)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(e, string.Format("Import engine({0}) encountered an exception when Handling Import.", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)), Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), string.Format("An exception Happened when handling Import Engine ID = {5} Exception Message: {1}{0} Exception Source: {2}{0} Stack Trace: {3}{0} Inner Exception: {4}{0}",
                    string.Format("{0}{1}{0}", Environment.NewLine, new string('-', 80)),
                    e.Message, e.Source, e.StackTrace, e.InnerException,
                    ImportEngineID));

                WriteStatus(e.Message);
            }
        }

        public void RebuildSubscriberMasterValues(int MastergroupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Usp_MergeSubscriberMasterValuesByMasterGroupId";
            cmd.CommandTimeout = 0;
            cmd.Parameters.AddWithValue("@MastergroupID", MastergroupID);

            DataFunctions.execute(cmd, DataFunctions.GetClientSqlConnection(clientconnections));

        }

        private void NotifyClient(string MailTo, string message)
        {
            SmtpClient smtpServer = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["KMCommon_SmtpServer"]);
            MailMessage mmessage = new MailMessage();
            mmessage.Priority = MailPriority.High;
            mmessage.IsBodyHtml = false;
            mmessage.To.Add(MailTo);
            mmessage.Bcc.Add(System.Configuration.ConfigurationManager.AppSettings["ClientNotificationBCC"].ToString());
            MailAddress msgSender = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["AdminEmail"].ToString());
            mmessage.From = msgSender;
            mmessage.Subject = "NorthStar Send Dimension Import Notification";
            mmessage.Body = message;

            smtpServer.Send(mmessage);
        }

        private void WriteStatus(string message)
        {
            message = DateTime.Now.ToString() + "   " + message;
            Console.WriteLine(message);
            string temp = ImportLogFile;
            using (StreamWriter file = new StreamWriter(new FileStream(ImportLogFile, System.IO.FileMode.Append)))
            {
                file.WriteLine(message);
                file.Close();
            }
        }

        string CleanXMLString(string dirty)
        {
            dirty = dirty.Replace("\0", string.Empty);
            if (string.IsNullOrEmpty(dirty))
                dirty = string.Empty;
            dirty = dirty.Trim();
            dirty = dirty.Replace("&", "&amp;");
            dirty = dirty.Replace("\"", "&quot;");
            dirty = dirty.Replace("<", "&lt;");
            dirty = dirty.Replace(">", "&gt;");
            dirty = dirty.Replace("'", "").Replace("’", "");
            byte[] bytes = Encoding.Default.GetBytes(dirty);
            return Encoding.UTF8.GetString(bytes); 
        }
    }

    [Serializable]
    public class SendDimensionFile
    {
        public SendDimensionFile() { }

        public string Email { get; set; }
        public string Code { get; set; }
        public bool flag { get; set; }
    }

    [XmlRoot("Process")]
    public class Process
    {
        public Process() { }

        [XmlElement("ProcessType")]
        public string ProcessType { get; set; }
        [XmlElement("FileFolder")]
        public string FileFolder { get; set; }
        [XmlElement("FileExtension")]
        public string FileExtension { get; set; }
        [XmlElement("IsQuoteEncapsulated")]
        public bool IsQuoteEncapsulated { get; set; }
        [XmlElement("ColumnDelimiter")]
        public ColumnDelimiter ColumnDelimiter { get; set; }
    }

    public enum FileExtension
    {
        xls,
        xlxs,
        csv,
        txt,
        xml
    }
    public enum ColumnDelimiter
    {
        comma,
        tab,
        semicolon,
        colon,
        tild
    }
}
