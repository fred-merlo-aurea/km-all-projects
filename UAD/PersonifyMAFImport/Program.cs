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

namespace PersonifyMAFImport
{
    class Engine
    {
        private static int ImportEngineID = Convert.ToInt32(ConfigurationManager.AppSettings["EngineID"]);
        private string ImportLogFile = String.Format("{0}\\PersonifyToECNImport_log_{1}.log", ConfigurationManager.AppSettings["ImportLogFilePath"], DateTime.Now.ToString("yyyy_MM_dd"));
        private string ImportDirectory = ConfigurationManager.AppSettings["ImportLocation"];
        private string ArchiveDirectory = ConfigurationManager.AppSettings["ArchiveLocation"];
        private string ImportDirectoryServer = ConfigurationManager.AppSettings["ImportServerLocation"];
        private string ImportFile = ConfigurationManager.AppSettings["ImportFileName"] + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        private string ProductCodes = ConfigurationManager.AppSettings["ProductCodes"];
        private string AccessKey = ConfigurationManager.AppSettings["ECNAccessKey"];//Nate Smith
        private bool RunSubscriberUpdate = Convert.ToBoolean(ConfigurationManager.AppSettings["UpdateECN"].ToString());

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
                    DeleteOldRecords();
                    DataSet dsImport = CreateNewFileDataSet();
                    ImportData(dsImport);
                    WriteStatus("Moving local import file to archive");
                    System.IO.File.Move(ImportDirectory + "\\" + ImportFile, ArchiveDirectory + "\\" + ImportFile);
                    WriteStatus("Deleteing import file from server");
                    System.IO.File.Delete(ImportDirectoryServer + "\\" + ImportFile);
                    CheckForMissingPubs();   
                    if(RunSubscriberUpdate)
                        UpdateSubscriberStatus();
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

        private void UpdateSubscriberStatus()
        {
            List<AuditReport> lAuditReport = new List<AuditReport>();

            WriteStatus("Updating Email Marketing subscriber status");

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ConnectionString);
            SqlCommand cmd = null;
            DataTable dtImport = null;
            
            string sqlSelect = "select distinct FIRST_NAME FirstName, LAST_NAME LastName, PRIMARY_PHONE Voice, PRIMARY_FAX Fax, PRIMARY_EMAIL_ADDRESS EmailAddress, " +
                                "PRIMARY_JOB_TITLE Title, PRIMARY_JOB_TITLE Occupation, COMPANY_NAME Company, ADDRESS_1 Address, ADDRESS_2 Address2, CITY City, STATE State, " +
                                "POSTAL_CODE Zip, COUNTRY_DESCR Country, OPTED_IN_FLAG " +
                                "from Personify_Import " +
                                "where PRODUCT_CODE = @Code and ISNULL(ltrim(rtrim(PRIMARY_EMAIL_ADDRESS)),'') <> ''";

            string sqlSelectCORPCONN = "select distinct FIRST_NAME FirstName, LAST_NAME LastName, PRIMARY_PHONE Voice, PRIMARY_FAX Fax, PRIMARY_EMAIL_ADDRESS EmailAddress, " +
                                "PRIMARY_JOB_TITLE Title, PRIMARY_JOB_TITLE Occupation, COMPANY_NAME Company, ADDRESS_1 Address, ADDRESS_2 Address2, CITY City, STATE State, " +
                                "POSTAL_CODE Zip, COUNTRY_DESCR Country, case when Ismember = 1 and OPTED_IN_FLAG = 'Y' and membershiptype like '%c%' then 'Y' else 'N' end as OPTED_IN_FLAG " +
                                "from Personify_Import " +
                                "where PRODUCT_CODE = @Code and ISNULL(ltrim(rtrim(PRIMARY_EMAIL_ADDRESS)),'') <> ''"; //PRODUCT_CODE = @Code and -- not required as per corey - 2/4/2014 //Adding PRODUCT_CODE BACK --4/3/2012

            string sqlSelectWKLYINSIDER = "select distinct FIRST_NAME FirstName, LAST_NAME LastName, PRIMARY_PHONE Voice, PRIMARY_FAX Fax, PRIMARY_EMAIL_ADDRESS EmailAddress, " +
                                "PRIMARY_JOB_TITLE Title, PRIMARY_JOB_TITLE Occupation, COMPANY_NAME Company, ADDRESS_1 Address, ADDRESS_2 Address2, CITY City, STATE State, " +
                                "POSTAL_CODE Zip, COUNTRY_DESCR Country, case when Ismember = 1 and OPTED_IN_FLAG = 'Y'  then 'Y' else 'N' end as OPTED_IN_FLAG " +
                                "from Personify_Import " +
                                "where PRODUCT_CODE = @Code and ISNULL(ltrim(rtrim(PRIMARY_EMAIL_ADDRESS)),'') <> ''"; // PRODUCT_CODE = @Code and -- not required as per corey - 2/4/2014 //Adding PRODUCT_CODE BACK --4/3/2012

            
            string[] codes = ProductCodes.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder importXML = null;
            int totalCount = 0;
            for (int i = 0; i <= codes.GetUpperBound(0); i++)
            {
                int ActiveRecordsinFile = 0;
                int InActiveRecordsinFile = 0;

                string[] code = codes[i].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

                AuditReport a = new AuditReport();

                a.pubcode = code[0];
                a.GroupID = int.Parse(code[1]);
                a.ECNActivebeforeImport= GetCountsFromECN(a.GroupID);

                if (code[0] == "CORPCONN")
                {
                    cmd = new SqlCommand(sqlSelectCORPCONN, conn);
                    cmd.Parameters.AddWithValue("@Code", code[0]);
                }
                else if (code[0] == "WKLYINSIDER")
                {
                    cmd = new SqlCommand(sqlSelectWKLYINSIDER, conn);
                    cmd.Parameters.AddWithValue("@Code", code[0]);
                }
                else
                {
                    cmd = new SqlCommand(sqlSelect, conn);
                    cmd.Parameters.AddWithValue("@Code", code[0]);
                }

                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                
                dtImport = GetDataTable(cmd, conn);
                if (dtImport != null && dtImport.Rows.Count > 0)
                {
                    totalCount = totalCount + dtImport.Rows.Count;
                    a.TotalRecordsinFile = dtImport.Rows.Count;

                    //build import xml
                    importXML = new StringBuilder();
                    importXML.Append("<xml>");
                    int currRow = 0;
                    foreach (DataRow row in dtImport.Rows)
                    {
                        currRow ++;
                        importXML.Append("<Emails>");
                        importXML.Append("<EmailAddress><![CDATA[" + row["EmailAddress"].ToString() + "]]></EmailAddress>");
                        importXML.Append("<Title><![CDATA[" + row["Title"].ToString() + "]]></Title>");
                        importXML.Append("<FirstName><![CDATA[" + row["FirstName"].ToString() + "]]></FirstName>");
                        importXML.Append("<LastName><![CDATA[" + row["LastName"].ToString() + "]]></LastName>");
                        importXML.Append("<FullName></FullName>");
                        importXML.Append("<Company><![CDATA[" + row["Company"].ToString() + "]]></Company>");
                        importXML.Append("<Occupation><![CDATA[" + row["Occupation"].ToString() + "]]></Occupation>");
                        importXML.Append("<Address><![CDATA[" + row["Address"].ToString() + "]]></Address>");
                        importXML.Append("<Address2><![CDATA[" + row["Address2"].ToString() + "]]></Address2>");
                        importXML.Append("<City><![CDATA[" + row["City"].ToString() + "]]></City>");
                        importXML.Append("<State><![CDATA[" + row["State"].ToString() + "]]></State>");
                        importXML.Append("<Zip><![CDATA[" + row["Zip"].ToString() + "]]></Zip>");
                        importXML.Append("<Country><![CDATA[" + row["Country"].ToString() + "]]></Country>");
                        importXML.Append("<Voice><![CDATA[" + row["Voice"].ToString() + "]]></Voice>");
                        importXML.Append("<Mobile></Mobile>");
                        importXML.Append("<Fax><![CDATA[" + row["Fax"].ToString() + "]]></Fax>");
                        importXML.Append("<Website></Website>");
                        importXML.Append("<Age></Age>");
                        importXML.Append("<Income></Income>");
                        importXML.Append("<Gender></Gender>");
                        importXML.Append("<User1></User1>");
                        importXML.Append("<User2></User2>");
                        importXML.Append("<User3></User3>");
                        importXML.Append("<User4></User4>");
                        importXML.Append("<User5></User5>");
                        importXML.Append("<User6></User6>");
                        importXML.Append("<Birthdate></Birthdate>");
                        importXML.Append("<UserEvent1></UserEvent1>");
                        importXML.Append("<UserEvent1Date></UserEvent1Date>");
                        importXML.Append("<UserEvent2></UserEvent2>");
                        importXML.Append("<UserEvent2Date></UserEvent2Date>");
                        importXML.Append("<Notes></Notes>");
                        importXML.Append("<Password></Password>");
                        if (row["OPTED_IN_FLAG"].ToString() == "N")
                        {
                            importXML.Append("<SubscribeTypeCode>U</SubscribeTypeCode>");
                            InActiveRecordsinFile++;
                        }
                        else
                        {
                            ActiveRecordsinFile++;
                        }
                        importXML.Append("</Emails>");
                        if (currRow % 1000 == 0)
                        {
                            //update subscribers
                            importXML.Append("</xml>");
                            ecn5.webservices.com.ListManager ws = new ecn5.webservices.com.ListManager();
                            ws.Timeout = 600000;
                            //string result = ws.AddSubscribers(AccessKey, Convert.ToInt32(code[1]), "S", "html", importXML.ToString());
                            importXML = new StringBuilder();
                            importXML.Append("<xml>");
                        }
                    }
                    importXML.Append("</xml>");
                }
                if (importXML.ToString().Length > 11)
                {
                    //update subscribers
                    ecn5.webservices.com.ListManager ws = new ecn5.webservices.com.ListManager();
                    ws.Timeout = 600000;
                    //string result2 = ws.AddSubscribers(AccessKey, Convert.ToInt32(code[1]), "S", "html", importXML.ToString());
                }

                a.ActiveRecordsinFile = ActiveRecordsinFile;
                a.InActiveRecordsinFile = InActiveRecordsinFile;
                a.ECNActiveafterImport = GetCountsFromECN(a.GroupID);

                lAuditReport.Add(a);
            }
            GenerateAuditReport(lAuditReport);
            WriteStatus("Subscriber count imported: " + totalCount.ToString());
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
                if (System.IO.File.Exists(ImportDirectoryServer + "\\" + ImportFile))
                {
                    System.IO.File.Copy(ImportDirectoryServer + "\\" + ImportFile, ImportDirectory + "\\" + ImportFile);
                    return true;
                }
                else
                    return false;
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

        private void CheckForMissingPubs()
        {
            WriteStatus("Checking for missing pub codes");

            string sqlSelect = "Select distinct imp.PRODUCT_CODE as PubCode From Personify_Import imp with (nolock) left outer join Pubs p with (nolock) on imp.PRODUCT_CODE = p.PubCode Where p.PubCode is null";

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sqlSelect, conn);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;
            DataTable dtPubs = GetDataTable(cmd, conn);
            CreateReport(dtPubs);
        }

        private void DeleteOldRecords()
        {
            WriteStatus("Deleting old records");

            string sqlSelect = "delete Personify_Import";

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sqlSelect, conn);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void UpdateOrderDate()
        {
            WriteStatus("Updating Order_Date");

            string sqlSelect = "update Personify_Import set ORDER_DATE = CONVERT(date, ORDER_DATE) where ORDER_DATE is not null and LEN(rtrim(ltrim(ORDER_DATE))) > 0";

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sqlSelect, conn);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            conn.Open();
            int result = 0;
            result = cmd.ExecuteNonQuery();
            conn.Close();
            WriteStatus("Number of records updated: " + result.ToString());
        }

        private int GetCountsFromECN(int groupID)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["communicator"].ConnectionString);
            SqlCommand cmd = new SqlCommand(string.Format("select count(emailgroupID) from emailgroups where groupID ={0} and subscribetypecode='s'", groupID), conn);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            conn.Open();
            int result = 0;
            result = int.Parse(cmd.ExecuteScalar().ToString());
            conn.Close();
            return result;
        }

        private DataTable GetDataTable(SqlCommand Cmd, SqlConnection Conn)
        {
            //Cmd.CommandTimeout = 0;
            Cmd.Connection = Conn;
            Conn.Open();
            SqlDataReader dr = Cmd.ExecuteReader(CommandBehavior.CloseConnection);
            DataTable dt = new DataTable();
            dt.Load(dr);
            Conn.Close();
            return dt;
        }

        private void GenerateAuditReport(List<AuditReport> lAuditReport)
        {
            WriteStatus("GenerateAuditReport");

            StringBuilder message = new StringBuilder();
            //create the report
            message.Append("<table><tr><td>GroupID</td><td>PubCode</td><td>Total Records in File</td><td>Active Records in File </td><td>In Active Records in File</td><td>ECN Active (before Import)</td><td>ECN Active (after Import)</td></tr>");

            foreach (AuditReport a in lAuditReport)
            {
                    message.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>", a.GroupID, a.pubcode, a.TotalRecordsinFile, a.ActiveRecordsinFile, a.InActiveRecordsinFile, a.ECNActivebeforeImport, a.ECNActiveafterImport));
            }

            message.Append("</table>");
            SendAuditReportMail(message.ToString());
            WriteStatus("Audit Report Sent");
            
        }

        private void CreateReport(DataTable dt)
        {
            WriteStatus("Creating missing pub codes report");

            string subject = string.Empty;
            StringBuilder message = new StringBuilder();
            //create the report
            if (dt.Rows.Count > 0)
            {
                subject = "Missing Pub Code values for Personify import";

                message.Append("<table><tr><td>PubCode</td></tr>");
                foreach (DataRow row in dt.Rows)
                {
                    message.Append("<tr><td>");
                    message.Append(row["PubCode"].ToString());
                    message.Append("</td></tr>");
                    WriteStatus("Missing PubCode: " + row["PubCode"].ToString());
                }
                message.Append("</table>");
                SendMail(subject, message.ToString());
                WriteStatus("Report Sent");
            }
        }

        private void SendAuditReportMail(string Message)
        {
            WriteStatus("Sending report");

            string subject = "Personify import - Audit Report";

            string emailfrom = ConfigurationManager.AppSettings["EmailFrom"].ToString();
            string email = ConfigurationManager.AppSettings["AuditReportEmailTo"].ToString();

            MailMessage msg = new MailMessage(emailfrom, email);
            msg.Subject = subject;
            msg.Body = Message;
            msg.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"].ToString());
            smtp.Send(msg);
        }

        private void SendMail(string Subject, string Message)
        {
            WriteStatus("Sending report");

            string subject = string.Empty;
            string message = string.Empty;
            string email = string.Empty;

            email = ConfigurationManager.AppSettings["MissingPubCodeEmailTo"].ToString();

            MailMessage msg = new MailMessage("domain_admin@teckman.com", email);
            msg.Subject = Subject;
            msg.Body = Message;
            msg.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"].ToString());
            smtp.Send(msg);
        }

        private DataSet CreateNewFileDataSet()
        {
            WriteStatus("Creating Dataset From File");

            string delimiter = "\t";

            //The DataSet to Return
            DataSet result = new DataSet();

            //Open the file in a stream reader.
            //StreamReader s = new StreamReader(ImportDirectory + "\\" + ImportFile);
            string AllData = string.Empty;
            using (StreamReader s = new StreamReader(ImportDirectory + "\\" + ImportFile))
            {
                AllData = s.ReadToEnd();
            }

            //Split the first line into the columns 
            //string AllData = s.ReadToEnd();
            string[] rows = AllData.Split("\r\n".ToCharArray());
            string[] columns = rows[0].Split(delimiter.ToCharArray());

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

            int recordCountImported = 0;
            int recordCountSkipped = 0;
            int recordCountTotal = 0;
            foreach (string r in rows)
            {
                //Split the row at the delimiter.
                string[] items = r.Split(delimiter.ToCharArray());

                if (items[0].ToString().Trim() != string.Empty && items[0].ToString().Trim() != "MASTER_CUSTOMER_ID")   //Add the item
                {
                    recordCountTotal++;
                    if (items.GetUpperBound(0) != (columns.GetUpperBound(0)))
                    {
                        recordCountSkipped++;
                        WriteStatus("Invalid number of columns : " + items.GetUpperBound(0).ToString() + " row: " + recordCountTotal);
                        foreach (string tempString in items)
                        {
                            WriteStatus("Value: " + tempString);
                        }
                    }
                    else
                    {
                        recordCountImported++;
                        result.Tables[TableName].Rows.Add(items);
                    }
                }
            }

            //Return the imported data.  
            WriteStatus("Number of rows in new import file : " + recordCountTotal.ToString());
            WriteStatus("Number of rows imported : " + recordCountImported.ToString());
            WriteStatus("Number of rows skipped : " + recordCountSkipped.ToString());
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

    public class AuditReport
    {
        public AuditReport()
        {
            GroupID = 0;
            GroupName = string.Empty;
            pubcode = string.Empty;
            TotalRecordsinFile = 0;
            ActiveRecordsinFile = 0;
            InActiveRecordsinFile = 0;
            ECNActiveafterImport = 0;
            ECNActivebeforeImport = 0;
        }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string pubcode { get; set; }
        public int TotalRecordsinFile { get; set; }
        public int ActiveRecordsinFile { get; set; }
        public int InActiveRecordsinFile { get; set; }
        public int ECNActivebeforeImport { get; set; }
        public int ECNActiveafterImport { get; set; }
    }
}

