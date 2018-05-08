using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Text;
using KM.Common;
using KM.Common.Extensions;
using KMPlatform.Entity;
using KMPlatform.Object;
using KMPS.MD.Objects;
using Tamir.SharpSsh;
using DataFunctions = KMPS.MD.Objects.DataFunctions;
using Enums = KMPS.MD.Objects.Enums;

namespace MAF.FilterScheduleExport
{
    class Program
    {
        private const string KeyNotifyClient = "NotifyClient";
        private const string KeyScheduledDate = "ScheduledDate";
        private const string KeyScheduledTime = "ScheduledTime";
        private const string KeyMasterDBs = "MasterDBs";
        private const string KeyMasterDBsExclude = "MasterDBs_Exclude";
        private const string NameSubscriptionId = "SUBSCRIPTIONID";
        private const string NameNew = "NEW";
        private const string NameChanged = "Changed";
        private const string NameBaseUrl = "BASEURL";
        private const string NameClientId = "CLIENTID";
        private const string NameClientSecret = "CLIENTSECRET";
        private const string NamePartition = "PARTITION";
        private const string NameCsv = "CSV";
        private const string NameFileNameDateTime = "FileName_DateTime";
        private const string NameFileNameDate = "FileName_Date";
        private const char CommaChar = ',';

        private static readonly string[] ProfileFields =
        {
            "FIRSTNAME", "LASTNAME", "COMPANY", "TITLE", "ADDRESS", "ADDRESS2", "ADDRESS3", "CITY", "STATE", "ZIP",
            "COUNTRY", "PHONE", "FAX", "MOBILE"
        };

        private static readonly string[] NonUdfFields = 
        {
            "EMAIL", "FIRSTNAME", "LASTNAME", "COMPANY", "TITLE", "ADDRESS", "ADDRESS2", "ADDRESS3", "CITY", "STATE", "ZIP",
            "COUNTRY", "PHONE", "FAX", "MOBILE"
        };

        private string LogFile = string.Empty;
        private string FilesFolder = string.Empty;

        private DateTime dtCurrent = System.DateTime.Now;
        private string dt = System.DateTime.Now.ToString("MM/dd/yyyy");
        private string time = System.DateTime.Now.ToString("HH") + ":00:00";
        private bool bNotifyclient = true;


        static void Main(string[] args)
        {
            Console.Title = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            if (args.Count() > 0)
                new Program().CreateFilterScheduleSummaryLog();
            else
                new Program().RunEngine();
        }

        public void RunEngine()
        {
            LoadConfigurations();
            EnsureRootDirectoriesCreated();

            WriteStatus($"Date : {dt}");
            WriteStatus($"Time : {time}");

            var masterDBs = ConfigurationManager.AppSettings[KeyMasterDBs]
                .Split(CommaChar)
                .ToList();
            var masterDBsExclude = ConfigurationManager.AppSettings[KeyMasterDBsExclude]
                .Split(CommaChar)
                .ToList();
            var activeClients = new KMPlatform.BusinessLogic.Client()
                .Select()
                .FindAll(x => x.IsActive && x.IsAMS && x.ClientLiveDBConnectionString.Length > 0);

            foreach (var client in activeClients)
            {
                try
                {
                    var clientConnections = new ClientConnections(client);
                    var database = DataFunctions.GetDBName(clientConnections);

                    try
                    {
                        if (!ValidateClientDb(masterDBs, masterDBsExclude, database))
                        {
                            continue;
                        }

                        var filterSchedules = FilterSchedule.GetScheduleByDateTime(clientConnections, dt, time);
                        ProcessExport(filterSchedules, clientConnections, database);
                    }
                    catch (Exception e)
                    {
                        WriteStatus($"ERROR : {e.Message}");

                        var stringBuilder = new StringBuilder();
                        stringBuilder.AppendLine("An exception Happened when handling FilterScheduleExport.</br>");
                        stringBuilder.AppendLine($"<b>Database:</b>{database}</br>");
                        stringBuilder.AppendLine($"<b>Exception Message:</b>{e.Message}</br>");
                        stringBuilder.AppendLine($"<b>Exception Source:</b>{e.Source}</br>");
                        stringBuilder.AppendLine($"<b>Stack Trace:</b>{e.StackTrace}</br>");
                        stringBuilder.AppendLine($"<b>Inner Exception:</b>{e.InnerException}</br>");

                        KM.Common.Entity.ApplicationLog.LogCriticalError(
                            e,
                            string.Format("FilterScheduleExport engine({0}) encountered an exception.", Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName)),
                            Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]),
                            stringBuilder.ToString());
                    }
                }
                catch (Exception e)
                {
                    WriteStatus($"ERROR : {e.Message}");

                    var stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine("An exception Happened when handling FilterScheduleExport.</br>");
                    stringBuilder.AppendLine($"<b>Exception Message:</b>{e.Message}</br>");
                    stringBuilder.AppendLine($"<b>Exception Source:</b>{e.Source}</br>");
                    stringBuilder.AppendLine($"<b>Stack Trace:</b>{e.StackTrace}</br>");
                    stringBuilder.AppendLine($"<b>Inner Exception:</b>{e.InnerException}</br>");

                    KM.Common.Entity.ApplicationLog.LogCriticalError(
                        e,
                        string.Format("FilterScheduleExport engine({0}) encountered an exception.", Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName)),
                        Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]),
                        stringBuilder.ToString());
                }
            }
        }

        public void CreateFilterScheduleSummaryLog()
        {
            string database = string.Empty;

            LogFile = String.Format("{0}\\{1}_{2}_summary.log", Environment.CurrentDirectory + "\\log", dt.Replace("/", "_"), time.Replace(":", "_"));

            if (!Directory.Exists(Environment.CurrentDirectory + "\\log"))
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\log");

            if (!Directory.Exists(Environment.CurrentDirectory + "\\Files"))
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\Files");

            FilesFolder = Environment.CurrentDirectory + "\\Files\\";

            WriteStatus("Date : " + dt);
            WriteStatus("Time : " + time);

            List<string> MasterDBs = ConfigurationManager.AppSettings["MasterDBs"].ToString().Split(',').ToList();
            List<string> MasterDBs_Exclude = ConfigurationManager.AppSettings["MasterDBs_Exclude"].ToString().Split(',').ToList();

            List<Client> lc = new KMPlatform.BusinessLogic.Client().Select(false).FindAll(x => x.IsActive == true && x.IsAMS == true && x.ClientLiveDBConnectionString.Length > 0).ToList();

            foreach (KMPlatform.Entity.Client c in lc)
            {
                try
                {
                    KMPlatform.Object.ClientConnections clientconnections = new KMPlatform.Object.ClientConnections(c);

                    database = DataFunctions.GetDBName(clientconnections);

                    if (MasterDBs.Count > 0 && ConfigurationManager.AppSettings["MasterDBs"].ToString() != "")
                    {
                        if (!MasterDBs.Contains(database, StringComparer.OrdinalIgnoreCase))
                        {
                            WriteStatus("Database : " + database + " Excluded");
                            continue;
                        }
                    }

                    if (MasterDBs_Exclude.Count > 0 && ConfigurationManager.AppSettings["MasterDBs_Exclude"].ToString() != "")
                    {
                        if (MasterDBs_Exclude.Contains(database, StringComparer.OrdinalIgnoreCase))
                        {
                            WriteStatus("Database : " + database + " Excluded");
                            continue;
                        }
                    }

                    WriteStatus("Database : " + database);

                    try
                    {
                        List<Config> config = Config.get(clientconnections);
                        if (config.Count > 0)
                        {
                            config = config.FindAll(x => x.Name == Enums.Config.FilterScheduleSummaryNotificationEmail.ToString());
                            if (config.Count > 0)
                            {
                                string email = config.FirstOrDefault().Value;

                                DataTable dtfsl = FilterScheduleLog.GetByDate(clientconnections, dt);

                                if (dtfsl.Rows.Count > 0)
                                {
                                    String summary = ToSummaryTXT(dtfsl);
                                    WriteStatus("Txt created successfully");
                                    EmailFilterScheduleSummary(email, "The following scheduled exports were successfully exported for " + Convert.ToDateTime(dt).AddDays(-1).ToShortDateString(), summary, "ScheduledExportSummary_" + Convert.ToDateTime(dt).AddDays(-1).ToShortDateString() + ".txt");
                                    WriteStatus("Email sent successfully");
                                }
                            }
                            else
                            {
                                WriteStatus("    Email not found");
                            }
                        }
                        else
                        {
                            WriteStatus("    Email not found");
                        }
                    }
                    catch (Exception e)
                    {
                        WriteStatus("ERROR : " + e.Message);

                        StringBuilder sbEx = new StringBuilder();
                        sbEx.AppendLine("An exception Happened when handling FilterScheduleSummary.</br>");
                        sbEx.AppendLine("<b>Database:</b>" + database + "</br>");
                        sbEx.AppendLine("<b>Exception Message:</b>" + e.Message + "</br>");
                        sbEx.AppendLine("<b>Exception Source:</b>" + e.Source + "</br>");
                        sbEx.AppendLine("<b>Stack Trace:</b>" + e.StackTrace + "</br>");
                        sbEx.AppendLine("<b>Inner Exception:</b>" + e.InnerException + "</br>");

                        KM.Common.Entity.ApplicationLog.LogCriticalError(e, string.Format("FilterScheduleSummary engine({0}) encountered an exception.", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)), Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), sbEx.ToString());

                    }
                    WriteStatus("---------------------------------------------------------------------------------------");
                }
                catch (Exception e)
                {
                    WriteStatus("ERROR : " + e.Message);

                    StringBuilder sbEx = new StringBuilder();
                    sbEx.AppendLine("An exception Happened when chandling FilterScheduleSummary.</br>");
                    sbEx.AppendLine("<b>Database:</b>" + database + "</br>");
                    sbEx.AppendLine("<b>Exception Message:</b>" + e.Message + "</br>");
                    sbEx.AppendLine("<b>Exception Source:</b>" + e.Source + "</br>");
                    sbEx.AppendLine("<b>Stack Trace:</b>" + e.StackTrace + "</br>");
                    sbEx.AppendLine("<b>Inner Exception:</b>" + e.InnerException + "</br>");

                    KM.Common.Entity.ApplicationLog.LogCriticalError(e, string.Format("FilterScheduleSummary engine({0}) encountered an exception.", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)), Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), sbEx.ToString());
                }
            }
        }

        public void WriteResultstoFile(int FilterScheduleID, string clientdbname, List<KM.Integration.Marketo.Result> results)
        {
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(List<KM.Integration.Marketo.Result>));

            var path = String.Format("{0}\\{1}_{2}_{3}.xml", Environment.CurrentDirectory + "\\Files\\" + clientdbname, FilterScheduleID, dt.Replace("/", "_"), time.Replace(":", "_"));

            //Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SerializationOverview.xml";
            System.IO.FileStream file = System.IO.File.Create(path);

            writer.Serialize(file, results);
            file.Close();
        }

        private void LoadConfigurations()
        {
            if (!bool.TryParse(ConfigurationManager.AppSettings[KeyNotifyClient], out bNotifyclient))
            {
                throw new InvalidOperationException($"Unable get bNotifyclient from '{ConfigurationManager.AppSettings[KeyNotifyClient]}'");
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[KeyScheduledDate]))
                {
                    dt = ConfigurationManager.AppSettings[KeyScheduledDate];
                }

                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[KeyScheduledTime]))
                {
                    time = ConfigurationManager.AppSettings[KeyScheduledTime];
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Unable to load all configurations: {0}", ex);
            }
        }

        private void EnsureRootDirectoriesCreated()
        {
            var logFolder = Path.Combine(Environment.CurrentDirectory, "log");

            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

            LogFile = Path.Combine(logFolder, $"{dt.Replace("/", "_")}_{time.Replace(":", "_")}.log");

            FilesFolder = Path.Combine(Environment.CurrentDirectory, "Files");

            if (!Directory.Exists(FilesFolder))
            {
                Directory.CreateDirectory(FilesFolder);
            }
        }

        private bool ValidateClientDb(IList<string> masterDBs, IList<string> masterDBsExclude, string database)
        {
            Guard.NotNull(masterDBs, nameof(masterDBs));
            Guard.NotNull(masterDBsExclude, nameof(masterDBsExclude));
            if (masterDBs.Any() && ConfigurationManager.AppSettings[KeyMasterDBs] != string.Empty)
            {
                if (!masterDBs.Contains(database, StringComparer.OrdinalIgnoreCase))
                {
                    WriteStatus($"    Database : {database} Excluded");
                    return false;
                }
            }

            if (masterDBsExclude.Any() && ConfigurationManager.AppSettings[KeyMasterDBsExclude] != string.Empty)
            {
                if (masterDBsExclude.Contains(database, StringComparer.OrdinalIgnoreCase))
                {
                    WriteStatus($"    Database : {database} Excluded");
                    return false;
                }
            }

            WriteStatus($"    Database : {database}");

            FilesFolder = Path.Combine(Environment.CurrentDirectory, "Files", database);
            if (!Directory.Exists(FilesFolder))
            {
                Directory.CreateDirectory(FilesFolder);
            }

            return true;
        }

        private void ProcessExport(IEnumerable<FilterSchedule> filterSchedules, ClientConnections clientConnections, string database)
        {
            foreach (var schedule in filterSchedules)
            {
                WriteStatus(string.Empty);
                WriteStatus($"       FilterScheduleID : {schedule.FilterScheduleID}");
                WriteStatus($"       FilterID : {schedule.FilterID}");
                WriteStatus($"       FilterName : {schedule.FilterName}");
                WriteStatus($"       Starttime : {DateTime.Now:MM/dd/yyyy hh:mm:ss}");

                try
                {
                    var filterExportFields = FilterExportField.getByFilterScheduleID(clientConnections, schedule.FilterScheduleID);

                    if (filterExportFields.Count > 0)
                    {
                        var tupData = FilterSchedule.Export(clientConnections, schedule.FilterScheduleID);

                        WriteStatus($"       Count : {tupData.Item1.Rows.Count}");

                        if (tupData.Item4)
                        {
                            WriteStatus("       Message : Data has been masked. Cannot export data.");

                            if (bNotifyclient && !string.IsNullOrWhiteSpace(schedule.EmailNotification))
                            {
                                NotifyClient(schedule.EmailNotification, "Data has been masked. Cannot export data.", schedule.FilterName);
                            }
                        }
                        else
                        {
                            var dtFilterScheduleData = tupData.Item1;
                            WriteStatus($"       FilterName : {schedule.FilterName}");

                            switch (schedule.ExportTypeID)
                            {
                                case Enums.ExportType.FTP:
                                    ExportToFtp(schedule, tupData.Item2, dtFilterScheduleData, clientConnections);
                                    break;
                                case Enums.ExportType.ECN:
                                    ExportToEcn(schedule, dtFilterScheduleData, clientConnections);
                                    break;
                                case Enums.ExportType.Marketo:
                                    ExportToMarketo(schedule, dtFilterScheduleData, clientConnections, filterExportFields, database);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        WriteStatus("       ERROR : Export Fields not defined");
                    }
                }
                catch (Exception e)
                {
                    LogScheduleExportException(e, schedule, database);
                }
                finally
                {
                    WriteStatus($"       finishTime : {DateTime.Now:MM/dd/yyyy hh:mm:ss}");
                }

                WriteStatus("---------------------------------------------------------------------------------------");
            }
        }

        private void LogScheduleExportException(Exception exception, FilterSchedule schedule, string database)
        {
            Guard.NotNull(exception, nameof(exception));
            Guard.NotNull(schedule, nameof(schedule));

            WriteStatus($"       ERROR : {exception.Message}");

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("An exception Happened when handling FilterScheduleExport.</br>");
            stringBuilder.AppendLine($"<b>Database:</b>{database}</br>");
            stringBuilder.AppendLine($"<b>FilterScheduleID:</b>{schedule.FilterScheduleID}</br>");
            stringBuilder.AppendLine($"<b>FilterName:</b>{schedule.FilterName}</br>");
            stringBuilder.AppendLine($"<b>Exception Message:</b>{exception.Message}</br>");
            stringBuilder.AppendLine($"<b>Exception Source:</b>{exception.Source}</br>");
            stringBuilder.AppendLine($"<b>Stack Trace:</b>{exception.StackTrace}</br>");
            stringBuilder.AppendLine($"<b>Inner Exception:</b>{exception.InnerException}</br>");

            KM.Common.Entity.ApplicationLog.LogCriticalError(
                exception,
                string.Format("FilterScheduleExport engine({0}) encountered an exception.", Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName)),
                Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]),
                stringBuilder.ToString());

            if (bNotifyclient && !string.IsNullOrWhiteSpace(schedule.EmailNotification))
            {
                NotifyClient(schedule.EmailNotification, "There is an issue in export. Please contact customer support.", schedule.FilterName);
            }
        }

        private void ExportToFtp(FilterSchedule schedule, string header, DataTable filterScheduleData, ClientConnections clientConnections)
        {
            Guard.NotNull(schedule, nameof(schedule));
            string fileName;

            if (schedule.FileNameFormat.EqualsIgnoreCase(NameFileNameDateTime))
            {
                fileName = string.Format(
                    "{0}_{1}.{2}",
                    schedule.FileName.ToLower().Replace("." + schedule.ExportFormat.ToLower(), string.Empty),
                    dtCurrent.ToString("yyyyMMdd_HHmmss"),
                    schedule.ExportFormat.ToLower());
            }
            else if (schedule.FileNameFormat.EqualsIgnoreCase(NameFileNameDate))
            {
                fileName = string.Format(
                    "{0}_{1}.{2}",
                    schedule.FileName.ToLower().Replace("." + schedule.ExportFormat.ToLower(), string.Empty),
                    dtCurrent.ToString("yyyyMMdd"),
                    schedule.ExportFormat.ToLower());
            }
            else
            {
                fileName = string.Format(
                    "{0}.{1}", schedule.FileName.ToLower().Replace("." + schedule.ExportFormat.ToLower(), string.Empty),
                    schedule.ExportFormat.ToLower());
            }

            var reportFullLocalPath = Path.Combine(FilesFolder, fileName);

            File.WriteAllText(
                reportFullLocalPath,
                schedule.ExportFormat.EqualsIgnoreCase(NameCsv)
                    ? ToCSV(filterScheduleData, header, schedule.ShowHeader)
                    : ToTXT(filterScheduleData, header, schedule.ShowHeader));

            FTPReport(fileName, FilesFolder, schedule.Server, schedule.UserName, schedule.Password, schedule.Folder);
            SaveFilterScheduleLog(clientConnections, schedule, filterScheduleData.Rows.Count, fileName);

            if (bNotifyclient && !string.IsNullOrWhiteSpace(schedule.EmailNotification))
            {
                NotifyClient(
                    schedule.EmailNotification,
                    $"file: {fileName} has been uploaded successfully to FTP folder :{schedule.Server}",
                    schedule.FilterName);
            }
        }

        private void ExportToEcn(FilterSchedule schedule, DataTable filterScheduleData, ClientConnections clientConnections)
        {
            Guard.NotNull(schedule, nameof(schedule));
            Guard.NotNull(filterScheduleData, nameof(filterScheduleData));

            var exportFields = PrepareExportFieldsForEcn(schedule, filterScheduleData, clientConnections).ToList();
            filterScheduleData.DefaultView.Sort = "Email Asc";
            filterScheduleData = filterScheduleData.DefaultView.ToTable();

            var startDateTime = DateTime.Now;
            var updatedRecords = Utilities.ExportToECN(
                schedule.GroupID.GetValueOrDefault(),
                string.Empty,
                schedule.CustomerID.GetValueOrDefault(),
                schedule.FolderID.GetValueOrDefault(),
                string.Empty,
                string.Empty,
                exportFields, filterScheduleData,
                schedule.UpdatedBy > 0 ? schedule.UpdatedBy : schedule.CreatedBy,
                Enums.GroupExportSource.UADScheduledExport);
            var exportCount = 0;
            var dtResult = Utilities.getImportedResult(updatedRecords, startDateTime);

            if (dtResult.Rows.Count > 0)
            {
                foreach (DataRow row in dtResult.Rows)
                {
                    var action = row["Action"].ToString();
                    if (action.EqualsAnyIgnoreCase(NameNew, NameChanged))
                    {
                        int totalCount;
                        int.TryParse(row["Totals"].ToString(), out totalCount);
                        exportCount += totalCount;
                    }
                }
            }

            SaveFilterScheduleLog(clientConnections, schedule, exportCount, string.Empty);

            if (bNotifyclient && !string.IsNullOrWhiteSpace(schedule.EmailNotification))
            {
                var sb = new StringBuilder();

                if (dtResult.Rows.Count > 0)
                {
                    var dataView = dtResult.DefaultView;
                    dataView.Sort = "sortorder asc";
                    dtResult = dataView.ToTable();

                    sb.Append($"Data has been uploaded successfully to ECN :{Groups.GetGroupByID(schedule.GroupID.GetValueOrDefault()).GroupName}");
                    sb.Append("</br></br><html><body><table border='1' cellspacing='0' cellpadding='1'>");

                    foreach (DataRow row in dtResult.Rows)
                    {
                        sb.Append($"<tr><td>{row["Action"]}</td>");
                        sb.Append($"<td>{row["Totals"]}</td></tr>");
                    }

                    sb.Append("</table></body></html>");
                }
                else
                {
                    sb.Append("No new records available for scheduled export.");
                    sb.Append("</br></br><html><body><table border='1' cellspacing='0' cellpadding='1'>");
                    sb.Append("<tr><td>Total Records in the File</td>");
                    sb.Append("<td>0</td></tr>");
                    sb.Append("</table></body></html>");
                }

                NotifyClient(schedule.EmailNotification, sb.ToString(), schedule.FilterName);
            }
        }

        private IEnumerable<ExportFields> PrepareExportFieldsForEcn(FilterSchedule schedule, DataTable filterScheduleData, ClientConnections clientConnections)
        {
            Guard.NotNull(schedule, nameof(schedule));
            Guard.NotNull(filterScheduleData, nameof(filterScheduleData));
            
            var groupEntity = Groups.GetGroupByID(schedule.GroupID.GetValueOrDefault());

            if (groupEntity.GroupID != 0)
            {
                //if group is MasterSupression then only profile field exported
                if (groupEntity.MasterSupression)
                {
                    var columnsToDelete = new List<DataColumn>();
                    foreach (DataColumn column in filterScheduleData.Columns)
                    {
                        if (!ProfileFields.Contains(column.ColumnName.ToUpper()))
                        {
                            columnsToDelete.Add(column);
                        }
                    }

                    foreach (var col in columnsToDelete)
                    {
                        filterScheduleData.Columns.Remove(col);
                    }
                }
            }
            else
            {
                throw new KeyNotFoundException($"Group not Found - {schedule.GroupID.GetValueOrDefault()}"); 
            }

            var exportFields = new List<ExportFields>();
            var filterExportFields = FilterExportField.getByFilterScheduleID(clientConnections, schedule.FilterScheduleID)
                .FindAll(x => !x.IsCustomValue && !x.IsDescription);

            foreach (DataColumn column in filterScheduleData.Columns)
            {
                if (column.ColumnName.EqualsIgnoreCase(NameSubscriptionId))
                {
                    if (filterExportFields.Exists(x => x.ExportColumn.EqualsIgnoreCase(NameSubscriptionId)))
                    {
                        exportFields.Add(new ExportFields(column.ColumnName.ToUpper(), string.Empty, true, 0));
                    }
                }
                else
                {
                    exportFields.Add(NonUdfFields.Contains(column.ColumnName.ToUpper())
                        ? new ExportFields(column.ColumnName.ToUpper(), string.Empty, false, 0)
                        : new ExportFields(column.ColumnName.ToUpper(), string.Empty, true, 0));
                }
            }

            return exportFields;
        }

        private void ExportToMarketo(
            FilterSchedule schedule,
            DataTable filterScheduleData,
            ClientConnections clientConnections,
            IList<FilterExportField> filterExportFields,
            string database)
        {
            Guard.NotNull(schedule, nameof(schedule));
            Guard.NotNull(filterScheduleData, nameof(filterScheduleData));
            Guard.NotNull(filterExportFields, nameof(filterExportFields));

            var leads = GetLeadsDictionary(schedule, filterScheduleData, clientConnections, filterExportFields)
                .ToList();

            Console.WriteLine($"Total Records : {leads.Count}");

            if (leads.Count > 0)
            {
                WriteStatus($"       Exporting : {leads.Count} records to marketo");
                var integrations = FilterScheduleIntegration.getByFilterScheduleID(clientConnections, schedule.FilterScheduleID);
                var baseUrl = integrations.Find(x => x.IntegrationParamName.EqualsIgnoreCase(NameBaseUrl)).IntegrationParamValue;
                var clientId = integrations.Find(x => x.IntegrationParamName.EqualsIgnoreCase(NameClientId)).IntegrationParamValue;
                var clientSecret = integrations.Find(x => x.IntegrationParamName.EqualsIgnoreCase(NameClientSecret)).IntegrationParamValue;
                var partition = integrations.Find(x => x.IntegrationParamName.EqualsIgnoreCase(NamePartition)).IntegrationParamValue;
                var restApiProcess = new KM.Integration.Marketo.Process.MarketoRestAPIProcess(baseUrl, clientId, clientSecret);

                //TODO we have to populate listId 
                var results = restApiProcess.CreateUpdateLeads(leads, "email", partition, schedule.GroupID);
                WriteResultstoFile(schedule.FilterScheduleID, database, results);

                SaveFilterScheduleLog(clientConnections, schedule, leads.Count, "");

                var resultTable = new DataTable();
                var stringBuilder = new StringBuilder();

                resultTable.Columns.Add("Type");
                resultTable.Columns.Add("Action");
                resultTable.Columns.Add("Totals");

                var query = results.GroupBy(n => new {n.type, n.status})
                    .Select(n => new { Type = n.Key.type, Status = n.Key.status, Count = n.Count() })
                    .OrderBy(n => n.Type)
                    .ThenBy(n => n.Status);

                foreach (var item in query)
                {
                    var row = resultTable.NewRow();
                    row["Type"] = item.Type;
                    row["Action"] = item.Status;
                    row["Totals"] = item.Count;
                    resultTable.Rows.Add(row);
                }

                stringBuilder.Append("Data has been uploaded successfully.");
                stringBuilder.Append("</br></br><html><body><table border='1' cellspacing='0' cellpadding='1'>");

                foreach (DataRow row in resultTable.Rows)
                {
                    stringBuilder.Append($"<tr><td>{row["Action"]}</td>");
                    stringBuilder.Append($"<td>{row["Totals"]}</td></tr>");
                }

                stringBuilder.Append("</table></body></html>");

                if (bNotifyclient)
                {
                    NotifyClient(schedule.EmailNotification, stringBuilder.ToString(), schedule.FilterName);
                }
            }
            else
            {
                WriteStatus("       Lead count = 0.  No Records to Export");
            }
        }

        private static IEnumerable<Dictionary<string, string>> GetLeadsDictionary(
            FilterSchedule schedule,
            DataTable filterScheduleData,
            ClientConnections clientConnections,
            IList<FilterExportField> filterExportFields)
        {
            Guard.NotNull(schedule, nameof(schedule));
            Guard.NotNull(filterScheduleData, nameof(filterScheduleData));
            Guard.NotNull(filterExportFields, nameof(filterExportFields));

            var filterExportDisplayNames = FilterExportField.getDisplayName(clientConnections, schedule.FilterScheduleID);
            var leads = new List<Dictionary<string, string>>();

            foreach (DataRow dataRow in filterScheduleData.Rows)
            {
                var leadsDictionary = new Dictionary<string, string>();

                foreach (var filterExportField in filterExportFields)
                {
                    string exportColumn;
                    switch (filterExportField.ExportColumn.ToUpper())
                    {
                        case "ADDRESS1":
                            exportColumn = "Address";
                            break;
                        case "REGIONCODE":
                            exportColumn = "State";
                            break;
                        case "ZIPCODE":
                            exportColumn = "Zip";
                            break;
                        case "PUBTRANSACTIONDATE":
                            exportColumn = "TransactionDate";
                            break;
                        case "QUALIFICATIONDATE":
                            exportColumn = "QDate";
                            break;
                        case "FNAME":
                            exportColumn = "FirstName";
                            break;
                        case "LNAME":
                            exportColumn = "LastName";
                            break;
                        case "ISLATLONVALID":
                            exportColumn = "GeoLocated";
                            break;
                        default:
                            var field = filterExportDisplayNames.Find(x => x.ExportColumn == filterExportField.ExportColumn);
                            exportColumn = field != null
                                ? field.DisplayName
                                : filterExportField.ExportColumn;
                            break;
                    }

                    leadsDictionary.Add(
                        filterExportField.MappingField,
                        filterExportField.IsCustomValue ? filterExportField.CustomValue : dataRow[exportColumn].ToString());
                }

                leads.Add(leadsDictionary);
            }

            return leads;
        }

        private void SaveFilterScheduleLog ( KMPlatform.Object.ClientConnections clientconnections, FilterSchedule fs, int count, string fileName)
        {
            FilterScheduleLog fsl = new FilterScheduleLog();
            fsl.FilterScheduleID = fs.FilterScheduleID;
            fsl.StartDate = Convert.ToDateTime(dt);
            fsl.StartTime = time;
            fsl.FileName = fileName;
            fsl.DownloadCount = count;
            FilterScheduleLog.Save(clientconnections, fsl);
        }


        private string ToCSV(DataTable table, string headerText, bool showHeader)
        {
            WriteStatus("Creating CSV");

            var result = new StringBuilder();

            if (showHeader)
            {
                result.Append("*****************************************************************************");
                result.Append("\r\n");
                result.Append(headerText);
                result.Append("*****************************************************************************");
                result.Append("\r\n");
            }

            //table.Columns.Remove("subscriptionid");

            for (int i = 0; i < table.Columns.Count; i++)
            {
                result.Append(table.Columns[i].ColumnName);
                result.Append(i == table.Columns.Count - 1 ? "\r\n" : ",");
            }

            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    result.Append("\"" + row[i].ToString() + "\"");
                    result.Append(i == table.Columns.Count - 1 ? "\r\n" : ",");
                }
            }

            return result.ToString();
        }

        private string ToTXT(DataTable table, string headerText, bool showHeader)
        {
            WriteStatus("Creating TXT");

            var result = new StringBuilder();

            if (showHeader)
            {
                result.Append("*****************************************************************************");
                result.Append("\r\n");
                result.Append(headerText);
                result.Append("*****************************************************************************");
                result.Append("\r\n");
            }

            //table.Columns.Remove("subscriptionid");

            for (int i = 0; i < table.Columns.Count; i++)
            {
                result.Append(table.Columns[i].ColumnName);
                result.Append(i == table.Columns.Count - 1 ? "\r\n" : "\t");
            }

            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    //if (table.Columns[i].ColumnName.ToLower() != "subscriptionid")
                    //{
                    result.Append(row[i].ToString());
                    result.Append(i == table.Columns.Count - 1 ? "\r\n" : "\t");
                    //}
                    //else
                    //{
                    //    result.Append(i == table.Columns.Count - 1 ? "\r\n" : "");
                    //}
                }
            }

            return result.ToString();
        }

        private List<string> getListofDatabase(string serverIP)
        {
            string masterdbconnectionstring = ConfigurationManager.ConnectionStrings["MasterDB"].ConnectionString.ToString().Replace("%%serverIP%%", serverIP).Replace("%%database%%", "master");

            List<string> retList = new List<string>();
            string sqlQuery = "select name from sys.databases where name like '%masterDB%' and name not like '%refresh%'";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.Text;
            SqlConnection conn = new SqlConnection(masterdbconnectionstring);
            cmd.Connection = conn;
            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (rdr.Read())
            {
                retList.Add(rdr["name"].ToString());
            }

            cmd.Connection.Close();
            return retList;

        }

        private void FTPReport(string reportName, string reportLocation, string FTPServer, string FTPUserName, string FTPPassword, string FTPFolder)
        {
            WriteStatus("FTPing " + reportName);

            string OriginalFTPServer = FTPServer;

            int totaltries = 5;
            int tries = 1;
            while (tries <= totaltries)
            {
                try
                {
                    if (OriginalFTPServer.Contains("sftp://"))
                    {
                        FTPServer = FTPServer.Replace("sftp://", "");
                        int port = 22;
                        Sftp oSftp = new Sftp(FTPServer.Trim(), FTPUserName.Trim(), FTPPassword.Trim());

                        if (System.Configuration.ConfigurationManager.AppSettings["SFTP_PrivateKeyPath"] != "")
                        {
                            oSftp.AddIdentityFile(System.Configuration.ConfigurationManager.AppSettings["SFTP_PrivateKeyPath"], System.Configuration.ConfigurationManager.AppSettings["SFTP_PrivateKeyPassPhrase"]);
                        }

                        WriteStatus("Beginning SFTP export");
                        oSftp.Connect(port);
                        oSftp.Put(reportLocation + reportName, (FTPFolder == string.Empty ? "" : FTPFolder.Trim() + "/") + reportName);
                        oSftp.Close();
                        WriteStatus("Finished SFTP export");
                        break;
                    }
                    else if (OriginalFTPServer.Contains("ftps://"))
                    {
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateUserCert);
                        FTPServer = FTPServer.Replace("ftps://", "ftp://");
                        UriBuilder uriBuilder = new UriBuilder(FTPServer.Trim() + "//" + (FTPFolder == string.Empty ? "" : FTPFolder.Trim() + "/") + reportName);
                        uriBuilder.Port = 990;
                        NetworkCredential nc = new NetworkCredential(FTPUserName.Trim(), FTPPassword.Trim());
                        WriteStatus("Beginning FTP export");
                        using (AlexPilotti.FTPS.Client.FTPSClient ftps = new AlexPilotti.FTPS.Client.FTPSClient())
                        {
                            ftps.Connect(uriBuilder.Uri.Host, 990, nc, AlexPilotti.FTPS.Client.ESSLSupportMode.Implicit, new RemoteCertificateValidationCallback(ValidateUserCert), null, 0, 0, 0, null);
                            ftps.PutFile(reportLocation + reportName, (FTPFolder == string.Empty ? "" : FTPFolder.Trim() + "/") + reportName);
                            ftps.Close();
                            ftps.Dispose();
                        }
                        WriteStatus("Finished FTP export");
                        break;
                    }
                    else if (OriginalFTPServer.Contains("ftp://"))
                    {
                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FTPServer.Trim() + "/" + (FTPFolder == string.Empty ? "" : FTPFolder.Trim() + "/") + reportName);
                        request.Method = WebRequestMethods.Ftp.UploadFile;
                        request.UseBinary = true;
                        request.UsePassive = true;
                        request.KeepAlive = true;
                        request.Timeout = 300000;

                        request.Credentials = new NetworkCredential(FTPUserName.Trim(), FTPPassword.Trim());

                        StreamReader sourceStream = new StreamReader(reportLocation + reportName);
                        byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                        sourceStream.Close();
                        request.ContentLength = fileContents.Length;

                        Stream requestStream = request.GetRequestStream();

                        requestStream.Write(fileContents, 0, fileContents.Length);
                        requestStream.Close();

                        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                        WriteStatus(string.Format("Remote Response: {0}", response.StatusDescription));

                        response.Close();
                        break;
                    }
                }
                catch (Exception ex)
                {
                    if (tries == totaltries)
                    {
                        throw ex;
                    }
                }
                tries++;
            }
        }

        private static bool ValidateUserCert(object sender, System.Security.Cryptography.X509Certificates.X509Certificate cert, System.Security.Cryptography.X509Certificates.X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors ==
           SslPolicyErrors.RemoteCertificateChainErrors)
            {
                return false;
            }
            else if (sslPolicyErrors ==
             SslPolicyErrors.RemoteCertificateNameMismatch)
            {
                System.Security.Policy.Zone z =
                   System.Security.Policy.Zone.CreateFromUrl
                   (((HttpWebRequest)sender).RequestUri.ToString());
                if (z.SecurityZone ==
                   System.Security.SecurityZone.Intranet ||
                   z.SecurityZone ==
                   System.Security.SecurityZone.MyComputer)
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        private void NotifyClient(string MailTo, string message, string filtername)
        {
            SmtpClient smtpServer = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["KMCommon_SmtpServer"]);
            MailMessage mmessage = new MailMessage();
            mmessage.Priority = MailPriority.High;
            mmessage.IsBodyHtml = true;
            mmessage.To.Add(MailTo);
            mmessage.Bcc.Add(System.Configuration.ConfigurationManager.AppSettings["ClientNotificationBCC"].ToString());
            MailAddress msgSender = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["AdminEmail"].ToString());
            mmessage.From = msgSender;
            mmessage.Subject = (filtername == string.Empty || filtername == null ? "" : filtername + " - ") + "MAF Data Export Notification";
            mmessage.Body = message;

            smtpServer.Send(mmessage);
        }

        private void WriteStatus(string message)
        {
            message = DateTime.Now.ToString() + "   " + message;
            Console.WriteLine(message);
            string temp = LogFile;
            using (StreamWriter file = new StreamWriter(new FileStream(LogFile, System.IO.FileMode.Append)))
            {
                file.WriteLine(message);
                file.Close();
            }
        }

        //FilterScheduleSummary 
        private string ToSummaryTXT(DataTable dt)
        {
            var result = new StringBuilder();

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                result.Append(dt.Columns[i].ColumnName);
                result.Append(i == dt.Columns.Count - 1 ? "\r\n" : "\t");
            }

            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    result.Append(row[i].ToString());
                    result.Append(i == dt.Columns.Count - 1 ? "\r\n" : "\t");
                }
            }

            return result.ToString();
        }

        private void EmailFilterScheduleSummary(string MailTo, string message, string summary, string filename)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(summary);
            writer.Flush();
            stream.Position = 0;

            SmtpClient smtpServer = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["KMCommon_SmtpServer"]);
            MailMessage mmessage = new MailMessage();
            mmessage.Priority = MailPriority.High;
            mmessage.IsBodyHtml = true;
            mmessage.To.Add(MailTo);
            mmessage.Bcc.Add(System.Configuration.ConfigurationManager.AppSettings["ClientNotificationBCC"].ToString());
            MailAddress msgSender = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["AdminEmail"].ToString());
            mmessage.From = msgSender;
            mmessage.Subject = "UAD Data Export Summary";
            mmessage.Body = message;
            if (!string.IsNullOrEmpty(summary))
            {
                mmessage.Attachments.Add(new Attachment(stream, filename, "text/plain"));
            }

            try
            {
                smtpServer.Send(mmessage);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            writer.Dispose();
            stream.Dispose();
        }
    }
}
