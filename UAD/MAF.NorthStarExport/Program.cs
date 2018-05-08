using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using KM.Common;
using KMPlatform.Entity;
using KMPS.MD.Objects;

namespace MAF.NorthStarExport
{
    public class Program
    {
        private const int BatchRecordSize = 100;
        StreamWriter customerLog;
        StreamWriter mainLog;
        int KMCommon_Application;
        string AppName = "MAF.NorthStarExport";
        CustomerConfig CustomerList;
        Customer CurrentCustomer;

        static void Main(string[] arg)
        {
            List<string> args = arg.ToList();
            //args.Add("WebsiteSubscriberRequest");
            Program p = new Program();
            if (args.Count() == 1)
            {
                AppArg a = p.GetAppArg(args[0]);
                if (a == AppArg.WelcomeLetter)
                    p.CreateWelcomeLetter();

                if (a == AppArg.WebsiteSubscriberRequest)
                    p.CreateWebsiteSubscriberRequest();
            }

        }
        void CreateWelcomeLetter()
        {
            mainLog = new StreamWriter(ConfigurationManager.AppSettings["MainLog"].ToString() + "_" + DateTime.Now.ToString("MM-dd-yyyy") + ".log", true);
            MainLogWrite("Start Instance - Welcome Letter");
            try//use mainLog at this level
            {
                MainLogWrite("Start CreateCustomerList");
                CreateCustomerList();
                MainLogWrite("Done CreateCustomerList");
                foreach (Customer c in CustomerList.Customers)
                {
                    customerLog = new StreamWriter(c.LogPath.ToString() + c.CustomerName + "_" + DateTime.Now.ToString("MM-dd-yyyy") + ".log", true);
                    CurrentCustomer = c;

                    Client cl = new KMPlatform.BusinessLogic.Client().Select(false).Find(x => x.IsActive == true && x.IsAMS == true && x.ClientLiveDBConnectionString.Length > 0 && x.ClientID == c.ClientID);

                    KMPlatform.Object.ClientConnections clientconnections = new KMPlatform.Object.ClientConnections(cl);


                    string step = string.Empty;
                    MainLogWrite("Start Customer: " + c.CustomerName);
                    try
                    {
                        foreach (Brand b in c.Brands)
                        {
                            step = b.BrandCode;
                            if (b.IsEnabled == true)
                            {
                                step = b.BrandCode;
                                MainLogWrite("Start " + step + " for Customer: " + c.CustomerName);
                                CustomerLogWrite("Start " + step);
                                CustomerLogWrite("Get Brand Data " + step);
                                DataTable dtBrand = GetBrandData(b.BrandCode, clientconnections);
                                CustomerLogWrite("Create Brand File " + step);
                                FileInfo brandFile = CreateBrandFile(dtBrand, b);
                                //FileInfo compressedFile = CompressFile(brandFile);
                                CustomerLogWrite("Upload Brand File " + step);
                                bool uploaded = UploadBrandFile(brandFile, b);//brandFile
                                CustomerLogWrite("Upload Status: " + uploaded.ToString());
                                CustomerLogWrite("End " + step);
                                MainLogWrite("End " + step + " for Customer: " + c.CustomerName);
                            }
                        }
                        MainLogWrite("End File Imports for Customer: " + c.CustomerName);
                        CustomerLogWrite("End File Imports");
                        step = string.Empty;

                    }
                    catch (Exception ex)
                    {
                        LogMainExeception(ex, "CreateWelcomeLetter.Step: " + step);
                    }
                    CustomerLogWrite("End Customer Process");
                }
            }
            catch (Exception ex)
            {
                LogMainExeception(ex, "CreateWelcomeLetter");
            }

            MainLogWrite("Done Instance - Welcome Letter");
        }
        void CreateWebsiteSubscriberRequest()
        {
            mainLog = new StreamWriter(ConfigurationManager.AppSettings["MainLog"].ToString() + "_" + DateTime.Now.ToString("MM-dd-yyyy") + ".log", true);
            MainLogWrite("Start Instance - Website Subscriber Request");
            try//use mainLog at this level
            {
                MainLogWrite("Start CreateCustomerList");
                CreateCustomerList();
                MainLogWrite("Done CreateCustomerList");
                foreach (Customer c in CustomerList.Customers)
                {
                    customerLog = new StreamWriter(c.LogPath.ToString() + c.CustomerName + "_" + DateTime.Now.ToString("MM-dd-yyyy") + ".log", true);
                    CurrentCustomer = c;
                    MainLogWrite("Start CreateWebsiteSubscriberRequest Customer: " + c.CustomerName);

                    Client cl = new KMPlatform.BusinessLogic.Client().Select(false).Find(x => x.IsActive == true && x.IsAMS == true && x.ClientLiveDBConnectionString.Length > 0 && x.ClientID == c.ClientID) ;

                    KMPlatform.Object.ClientConnections clientconnections = new KMPlatform.Object.ClientConnections(cl);

                    string file = CurrentCustomer.WebsiteSubscriberRequest_FTPFileName + System.DateTime.Now.ToString("yyyyMMdd") + "." + CurrentCustomer.WebsiteSubscriberRequest_FileExtension;

                    string path = CurrentCustomer.ExportedFiles + CurrentCustomer.WebsiteSubscriberRequest_FTPFolder + @"\";
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    if (File.Exists(path + file))
                        File.Delete(path + file);
                    
                    try
                    {
                        FileInfo wsrFile = null;

                        CustomerLogWrite("Start CreateWebsiteSubscriberRequest");

                        List<Pubs> lpub = KMPS.MD.Objects.Pubs.GetAll(clientconnections);

                        foreach (Pubs p in lpub)
                        {
                            try
                            {

                                CustomerLogWrite("Get Data" + p.PubCode);
                                DataTable dtWSR = Get_WSR_Data(p.PubID, clientconnections);
                                CustomerLogWrite("Create File");
                                wsrFile = Create_WSR_File(dtWSR);

                            }

                            catch (Exception ex)
                            {
                                LogMainExeception(ex, "Error exporting Data for " + p.PubCode);
                            }
                        }
                        CustomerLogWrite("Upload File");
                        FileInfo compressedFile = CompressFile(wsrFile);
                        bool uploaded = Upload_WSR_File(compressedFile);//wsrFile
                        CustomerLogWrite("Upload Status: " + uploaded.ToString());
                        MainLogWrite("End CreateWebsiteSubscriberRequest for Customer: " + c.CustomerName);
                        CustomerLogWrite("End CreateWebsiteSubscriberRequest");
                    }
                    catch (Exception ex)
                    {
                        LogMainExeception(ex, "CreateWebsiteSubscriberRequest");
                    }
                    CustomerLogWrite("End Customer Process");
                }
            }
            catch (Exception ex)
            {
                LogMainExeception(ex, "CreateWebsiteSubscriberRequest");
            }

            MainLogWrite("Done Instance - Website Subscriber Request");
        }
        #region Configuration Methods
        void CreateCustomerList()
        {
            string path = ConfigurationManager.AppSettings["CustomerConfigFile"].ToString();
            XmlSerializer serializer = new XmlSerializer(typeof(CustomerConfig));
            StreamReader reader = new StreamReader(path);
            CustomerList = (CustomerConfig)serializer.Deserialize(reader);
            foreach (Customer c in CustomerList.Customers)
            {
                if (!c.ExportedFiles.EndsWith(@"\"))
                    c.ExportedFiles += @"\";
                if (!c.LogPath.EndsWith(@"\"))
                    c.LogPath += @"\";
            }
        }
        #endregion
        #region Welcome Letter
        DataTable GetBrandData(string brandCode, KMPlatform.Object.ClientConnections clientconnections)
        {
            DataTable dtBrand = Data.GetBrand(brandCode, clientconnections);

            return dtBrand;
        }
        #region Create File
        FileInfo CreateBrandFile(DataTable dtBrand, Brand b)
        {
            if (b.FileExtension.ToLower() != FileExtension.xml.ToString())
                return CreateNonXML(dtBrand, b);
            else
                return CreateXML(dtBrand, b);
            
        }
        FileInfo CreateNonXML(DataTable dataTableBrand, Brand brand)
        {
            var delimiter = GetColumnDelimiter(brand.ColumnDelimiter);

            if (brand.FileExtension.Equals(FileExtension.xls.ToString(), StringComparison.OrdinalIgnoreCase) 
                || brand.FileExtension.Equals(FileExtension.xlxs.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                delimiter = '\t';
                brand.IsQuoteEncapsulated = false;
            }

            var fileHeader = CreateFileHeader(dataTableBrand, brand.IsQuoteEncapsulated, delimiter);

            return CreateFile(brand, fileHeader);
        }

        private static StringBuilder CreateFileHeader(DataTable dataTable, bool quoteEnabled, char delimiter)
        {
            var builder = new StringBuilder();
            var columns = new StringBuilder();
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                columns.Append(
                    quoteEnabled
                        ? $"\"{dataColumn.ColumnName}\"{delimiter}"
                        : $"{dataColumn.ColumnName}{delimiter}");
            }

            builder.AppendLine(columns
                .ToString()
                .TrimEnd(delimiter));

            foreach (DataRow dataRow in dataTable.Rows)
            {
                var rowBuilder = new StringBuilder();
                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    var trimmedValue = dataRow[dataColumn.ColumnName]
                        .ToString()
                        .Trim();

                    rowBuilder.Append(
                        quoteEnabled
                            ? $"\"{trimmedValue}\"{delimiter}"
                            : $"{trimmedValue}{delimiter}");
                }

                builder.AppendLine(rowBuilder
                    .ToString()
                    .TrimEnd(delimiter));
            }

            return builder;
        }

        private FileInfo CreateFile(Brand brand, StringBuilder fileBuilder)
        {
            var path = $"{CurrentCustomer.ExportedFiles}{brand.FtpFolder}\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var file = $"Welcome_{brand.BrandCode}_{DateTime.Now:yyyyMMdd}.{brand.FileExtension}";
            var combinedPath = Path.Combine(path, file);

            if (File.Exists(combinedPath))
            {
                File.Delete(combinedPath);
            }

            File.WriteAllText(combinedPath, fileBuilder.ToString());

            return new FileInfo(combinedPath);
        }

        FileInfo CreateXML(DataTable dtBrand, Brand b)
        {
            StringBuilder sbFile = new StringBuilder();
            #region Create XML Data
            sbFile.AppendLine("<Brand>");

            foreach (DataRow dr in dtBrand.Rows)
            {
                sbFile.AppendLine("<BrandDetail>");
                foreach (DataColumn dc in dtBrand.Columns)
                {
                    sbFile.AppendLine("<" + dc.ColumnName + ">" + CleanXMLString(dr[dc.ColumnName].ToString().Trim()) + "</" + dc.ColumnName + ">");
                }
                sbFile.AppendLine("</BrandDetail>");
            }

            sbFile.AppendLine("</Brand>");
            #endregion
            #region Create File
            string path = CurrentCustomer.ExportedFiles + b.FtpFolder + @"\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string file = "Welcome_" + b.BrandCode + "_" + System.DateTime.Now.ToString("yyyyMMdd") + "." + b.FileExtension;
            if (File.Exists(path + file))
                File.Delete(path + file);
            File.WriteAllText(path + file, sbFile.ToString());
            FileInfo fi = new FileInfo(path + file);
            #endregion
            return fi;
        }
        #endregion
        
        bool UploadBrandFile(FileInfo brandFile, Brand b)
        {
            return UploadFile(brandFile, b.FtpFolder);
        }

        #endregion
        #region Website Subscriber Request
        DataTable Get_WSR_Data(int PubID, KMPlatform.Object.ClientConnections clientconnections)
        {
            DataTable dt = Data.GetWSR(PubID, clientconnections);

            return dt;
        }
        FileInfo Create_WSR_File(DataTable dt)
        {
            if (CurrentCustomer.WebsiteSubscriberRequest_FileExtension.ToLower() != FileExtension.xml.ToString())
                return CreateWSR_NonXML(dt);
            else
                return CreateWSR_XML(dt);
        }

        FileInfo CreateWSR_NonXML(DataTable dataTable)
        {
            var fileBuilder = new StringBuilder();

            var recordcount = 0;
            var fileName = $"{CurrentCustomer.WebsiteSubscriberRequest_FTPFileName}{DateTime.Now:yyyyMMdd}.{CurrentCustomer.WebsiteSubscriberRequest_FileExtension}";

            var path = CurrentCustomer.ExportedFiles + CurrentCustomer.WebsiteSubscriberRequest_FTPFolder + @"\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var delimiter = GetColumnDelimiter(CurrentCustomer.WebsiteSubscriberRequest_ColumnDelimiter);
          
            if (CurrentCustomer.WebsiteSubscriberRequest_FileExtension.Equals(FileExtension.xls.ToString(), StringComparison.OrdinalIgnoreCase) 
                || CurrentCustomer.WebsiteSubscriberRequest_FileExtension.Equals(FileExtension.xlxs.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                delimiter = '\t';
                CurrentCustomer.WebsiteSubscriberRequest_IsQuoteEncapsulated = false;
            }

            var fullFilePath = path + fileName;
            if (!File.Exists(fullFilePath))
            {
                var builderColumns = new StringBuilder();
                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    builderColumns.Append(
                        CurrentCustomer.WebsiteSubscriberRequest_IsQuoteEncapsulated
                            ? $"\"{dataColumn.ColumnName}\"{delimiter}"
                            : $"{dataColumn.ColumnName}{delimiter}");
                }

                var columns = builderColumns.ToString().TrimEnd(delimiter);
                fileBuilder.AppendLine(columns);
            }
           
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var builderRow = new StringBuilder();
                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    if (CurrentCustomer.WebsiteSubscriberRequest_IsQuoteEncapsulated)
                    {
                        builderRow.Append("\"" + dataRow[dataColumn.ColumnName].ToString().Trim() + "\"" + delimiter);
                    }
                    else
                    {
                        builderRow.Append(dataRow[dataColumn.ColumnName].ToString().Trim() + delimiter);
                    }
                }

                fileBuilder.AppendLine(builderRow
                    .ToString()
                    .TrimEnd(delimiter));

                recordcount++;

                if (recordcount != BatchRecordSize && dataTable.Rows.Count != recordcount)
                {
                    continue;
                }

                if (!File.Exists(fullFilePath))
                {
                    using (var streamWriter = File.CreateText(fullFilePath))
                    {
                        streamWriter.WriteLine(fileBuilder.ToString().TrimEnd());
                    }
                }
                else
                {
                    using (var streamWriter = File.AppendText(fullFilePath))
                    {
                        streamWriter.WriteLine(fileBuilder.ToString().TrimEnd());
                    }
                }

                fileBuilder.Clear();
            }
          
            return new FileInfo(path + fileName);
        }
        FileInfo CreateWSR_XML(DataTable dt)
        {
            StringBuilder sbFile = new StringBuilder();
            #region Create XML Data
            sbFile.AppendLine("<WSR>");

            foreach (DataRow dr in dt.Rows)
            {
                sbFile.AppendLine("WSRDetail>");

                    foreach (DataColumn dc in dt.Columns)
                    {
                        sbFile.AppendLine("<" + dc.ColumnName + ">" + CleanXMLString(dr[dc.ColumnName].ToString().Trim()) + "</" + dc.ColumnName + ">");
                    }

                sbFile.AppendLine("</WSRDetail>");
            }
            sbFile.AppendLine("</WSR>");
            #endregion
            #region Create File
            string path = CurrentCustomer.ExportedFiles + CurrentCustomer.WebsiteSubscriberRequest_FTPFolder + @"\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string file = CurrentCustomer.WebsiteSubscriberRequest_FTPFileName + System.DateTime.Now.ToString("yyyyMMdd") + "." + CurrentCustomer.WebsiteSubscriberRequest_FileExtension; 

            if (File.Exists(path + file))
                File.Delete(path + file);
            File.WriteAllText(path + file, sbFile.ToString());
            FileInfo fi = new FileInfo(path + file);
            #endregion
            return fi;
        }

        bool Upload_WSR_File(FileInfo wsrFile)
        {
            return UploadFile(wsrFile, CurrentCustomer.WebsiteSubscriberRequest_FTPFolder);
        }

        #endregion
        #region General Methods

        bool UploadFile(FileInfo file, string folder)
        {
            var uploaded = false;
            var ftp = new FtpFunctions(CurrentCustomer.FTP_Site, CurrentCustomer.FTP_User, CurrentCustomer.FTP_Password);
            var ftpFolder = string.Empty;

            if (!string.IsNullOrWhiteSpace(folder))
            {
                ftpFolder = folder + "/";

                var dirs = ftp.DirectoryListSimple(string.Empty).ToList();
                var exist = false;

                foreach (var dir in dirs)
                {
                    if (dir.Equals(folder, StringComparison.OrdinalIgnoreCase))
                    {
                        exist = true;
                    }
                }

                if (exist == false)
                {
                    ftp.CreateDirectory(ftpFolder);
                }
            }
            
            uploaded = ftp.Upload(ftpFolder + file.Name, file.FullName);
            return uploaded;
        }

        #endregion

        #region Helpers
        private FileInfo CompressFile(FileInfo baseFile)
        {
            string tempDirectory = baseFile.Directory + System.DateTime.Now.ToString("yyyyMMdd") + "\\";
            if (Directory.Exists(tempDirectory) == false)
                Directory.CreateDirectory(tempDirectory);
            File.Move(baseFile.FullName, tempDirectory + baseFile.Name);
            string archiveName = baseFile.Directory + "\\" + baseFile.Name.Replace(baseFile.Extension,"") + ".zip";
            ZipFile.CreateFromDirectory(tempDirectory, archiveName);
            FileInfo fiCompressed = new FileInfo(archiveName);
            Directory.Delete(tempDirectory, true);
            return fiCompressed;
        }
        public StringBuilder FormatException(Exception ex, string method)
        {
            StringBuilder sbLog = new StringBuilder();
            sbLog.AppendLine("**********************");
            sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
            sbLog.AppendLine("Method: " + method);
            sbLog.AppendLine("-- Message --");
            if (ex.Message != null)
                sbLog.AppendLine(ex.Message);
            sbLog.AppendLine("-- InnerException --");
            if (ex.InnerException != null)
                sbLog.AppendLine(ex.InnerException.ToString());
            sbLog.AppendLine("-- Stack Trace --");
            if (ex.StackTrace != null)
                sbLog.AppendLine(ex.StackTrace);
            sbLog.AppendLine("**********************");

            return sbLog;
        }

        private static string CleanXMLString(string dirty)
        {
            return XMLFunctions.EscapeXmlString(dirty);
        }

        private static void LogWrite(StreamWriter stream, string text)
        {
            Console.WriteLine(text);

            stream.AutoFlush = true;
            stream.WriteLine($"{DateTime.Now} {text}");
            stream.Flush();
        }

        private void CustomerLogWrite(string text)
        {
            LogWrite(customerLog, text);
        }

        private void MainLogWrite(string text)
        {
            LogWrite(mainLog, text);
        }

        public char GetColumnDelimiter(string delimiterString)
        {
            ColumnDelimiter delimeter;
            if (Enum.TryParse(delimiterString, out delimeter))
            {
                switch (delimeter)
                {
                    case ColumnDelimiter.colon:
                        return ':';
                    case ColumnDelimiter.comma:
                        return ',';
                    case ColumnDelimiter.semicolon:
                        return ';';
                    case ColumnDelimiter.tab:
                        return '\t';
                    case ColumnDelimiter.tild:
                        return '~';
                    default:
                        return ',';
                }
            }
            else
            {
                return ',';
            }
        }

        public AppArg GetAppArg(string arg)
        {
            try
            {
                return (AppArg)System.Enum.Parse(typeof(AppArg), arg, true);
            }
            catch { return AppArg.WelcomeLetter; }
        }
        public void LogCustomerExeception(Exception ex, string method)
        {
            KM.Common.Entity.ApplicationLog.LogCriticalError(ex, AppName + "." + method, KMCommon_Application, AppName + ": Unhandled Exception", -1, -1);
            CustomerLogWrite(StringFunctions.FormatException(ex));
        }
        public void LogMainExeception(Exception ex, string method)
        {
            KM.Common.Entity.ApplicationLog.LogCriticalError(ex, AppName + "." + method, KMCommon_Application, AppName + ": Unhandled Exception", -1, -1);
            MainLogWrite(StringFunctions.FormatException(ex));
        }
        #endregion
    }

    #region xml file CustomerConfig classes
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRoot("Customers")]
    public class CustomerConfig
    {
        [XmlElement("Customer")]
        public List<Customer> Customers { get; set; }
    }
    public class Customer
    {
        public Customer() { }

        [XmlElement("CustomerID")]
        public int CustomerID { get; set; }
        [XmlElement("ClientID")]
        public int ClientID { get; set; }
        [XmlElement("CustomerName")]
        public string CustomerName { get; set; }
        [XmlElement("FTP_Site")]
        public string FTP_Site { get; set; }
        [XmlElement("FTP_User")]
        public string FTP_User { get; set; }
        [XmlElement("FTP_Password")]
        public string FTP_Password { get; set; }
        [XmlElement("FtpFolder")]
        public string FtpFolder { get; set; }
        [XmlElement("ExportedFiles")]
        public string ExportedFiles { get; set; }
        [XmlElement("LogPath")]
        public string LogPath { get; set; }

        [XmlElement("WebsiteSubscriberRequest_FTPFileName")]
        public string WebsiteSubscriberRequest_FTPFileName { get; set; }
        [XmlElement("WebsiteSubscriberRequest_FTPFolder")]
        public string WebsiteSubscriberRequest_FTPFolder { get; set; }
        [XmlElement("WebsiteSubscriberRequest_FileExtension")]
        public string WebsiteSubscriberRequest_FileExtension { get; set; }
        [XmlElement("WebsiteSubscriberRequest_IsQuoteEncapsulated")]
        public bool WebsiteSubscriberRequest_IsQuoteEncapsulated { get; set; }
        [XmlElement("WebsiteSubscriberRequest_ColumnDelimiter")]
        public string WebsiteSubscriberRequest_ColumnDelimiter { get; set; }


        [XmlArray("Brands"), XmlArrayItem("Brand")]
        public List<Brand> Brands { get; set; }
    }
    [XmlRoot("Brand")]
    public class Brand
    {
        public Brand() { }

        [XmlElement("BrandCode")]
        public string BrandCode { get; set; }
        [XmlElement("IsEnabled")]
        public bool IsEnabled { get; set; }
        [XmlElement("FtpFolder")]
        public string FtpFolder { get; set; }
        [XmlElement("FileExtension")]
        public string FileExtension { get; set; }
        [XmlElement("IsQuoteEncapsulated")]
        public bool IsQuoteEncapsulated { get; set; }
        [XmlElement("ColumnDelimiter")]
        public string ColumnDelimiter { get; set; }
    }
    #endregion

    #region Enums
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
    public enum AppArg
    {
        WelcomeLetter,
        WebsiteSubscriberRequest
    }
    #endregion
}
