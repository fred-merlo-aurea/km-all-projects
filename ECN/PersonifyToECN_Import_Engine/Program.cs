using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Data;
using System.Data.SqlClient;

namespace PersonifyToECN_Import_Engine
{
    class Engine
    {
        private static int ImportEngineID = Convert.ToInt32(ConfigurationManager.AppSettings["EngineID"]);
        private string ImportLogFile = String.Format("{0}\\PersonifyToECNImport_log_{1}.log", ConfigurationManager.AppSettings["ImportLogFilePath"], DateTime.Now.ToString("MM_dd_yyyy"));
        private string ImportDirectory = ConfigurationManager.AppSettings["ImportLocation"];
        private string ArchiveDirectory = ConfigurationManager.AppSettings["ArchiveLocation"];
        private string ImportDirectoryServer = ConfigurationManager.AppSettings["ImportServerLocation"];
        private string ImportFile = ConfigurationManager.AppSettings["ImportFileName"] + DateTime.Now.ToString("MMddyyyy") + ".txt";

        static void Main(string[] args)
        {
            Console.Title = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            new Engine().RunEngine();
        }

        public void RunEngine()
        {
            WriteStatus(string.Format("Personify Engine({0}) started", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)));
            try
            {
                if (GetImportFile())
                {
                    DataSet dsImport = CreateNewFileDataSet();
                    ImportData(dsImport);
                    System.IO.File.Move(ImportDirectory + "\\" + ImportFile, ArchiveDirectory + "\\" + ImportFile);
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
                System.IO.File.Copy(ImportDirectoryServer + "\\" + ImportFile, ImportDirectory + "\\" + ImportFile);
                return true;
            }
        }

        //private bool GetImportFileFTP()
        //{

        //    WriteStatus("DownloadingFile");

        //    string userName = ConfigurationManager.AppSettings.Get("username");
        //    string password = ConfigurationManager.AppSettings.Get("password");
        //    string server = ConfigurationManager.AppSettings.Get("ftpServer");
        //    FtpWebRequest reqFTP;
        //    FtpWebResponse response;
        //    Stream ftpStream;
        //    FileStream outputStream;

        //    outputStream = new FileStream(ImportDirectory + "\\" + ImportFile, FileMode.Create);

        //    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + server + "/QA/" + ImportFile));
        //    reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
        //    reqFTP.UseBinary = true;
        //    //reqFTP.EnableSsl = true;
        //    reqFTP.Credentials = new NetworkCredential(userName,
        //                                                password);
        //    response = (FtpWebResponse)reqFTP.GetResponse();
        //    ftpStream = response.GetResponseStream();
        //    long cl = response.ContentLength;
        //    int bufferSize = 2048;
        //    int readCount;
        //    byte[] buffer = new byte[bufferSize];

        //    readCount = ftpStream.Read(buffer, 0, bufferSize);
        //    while (readCount > 0)
        //    {
        //        outputStream.Write(buffer, 0, readCount);
        //        readCount = ftpStream.Read(buffer, 0, bufferSize);
        //    }

        //    ftpStream.Close();
        //    outputStream.Close();
        //    response.Close();
        //    return true;
        //}

        private DataSet CreateNewFileDataSet()
        {
            WriteStatus("Creating Dataset From File");

            string delimiter = "\t";

            //The DataSet to Return
            DataSet result = new DataSet();

            //Open the file in a stream reader.
            StreamReader s = new StreamReader(ImportDirectory + "\\" + ImportFile);

            //Split the first line into the columns 
            string AllData = s.ReadToEnd();
            string[] rows = AllData.Split("\r\n".ToCharArray());
            string[] columns = rows[2].Split(delimiter.ToCharArray());

            string TableName = "newResults";
            //Add the new DataTable to the RecordSet
            result.Tables.Add(TableName);

            //Cycle the colums, adding those that don't exist yet and sequencing the one that do.
            foreach (string col in columns)
            {
                if (!result.Tables[TableName].Columns.Contains(col))
                {
                    result.Tables[TableName].Columns.Add(col);
                    WriteStatus("column added : " + col.ToString());
                }
            }
            result.Tables[TableName].Columns.Add("Processed");

            //Now add each row to the DataSet        
            rows = rows.Distinct().ToArray();

            int recordCount = 0;
            foreach (string r in rows)
            {
                //Split the row at the delimiter.
                string[] items = r.Split(delimiter.ToCharArray());

                if (items[0].ToString().Trim() != string.Empty && items[0].ToString().Trim() != "MASTER_CUSTOMER_ID")   //Add the item
                {
                    recordCount++;
                    if (items.GetUpperBound(0) != 40)
                    {
                        WriteStatus("Invalid number of columns : " + items.GetUpperBound(0).ToString() + " row: " + recordCount);
                        foreach (string tempString in items)
                        {
                            WriteStatus("Value: " + tempString);
                        }
                    }
                    else
                    {
                        result.Tables[TableName].Rows.Add(items);
                    }
                }
            }

            //Return the imported data.  
            WriteStatus("Number of rows in new Import File : " + result.Tables["newResults"].Rows.Count.ToString());
            return result;
        }

        private void ImportData(DataSet dsImport)
        {
            WriteStatus("Importing Data");

            DataTable dtImport = dsImport.Tables["newResults"];
            foreach (DataRow row in dtImport.Rows)
            {
                row["Processed"] = 0;
            }

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ConnectionString))
            {
                connection.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.BulkCopyTimeout = 0;
                    bulkCopy.DestinationTableName = "Personify_Import";
                    bulkCopy.WriteToServer(dtImport);
                }
            }
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
    }
}
