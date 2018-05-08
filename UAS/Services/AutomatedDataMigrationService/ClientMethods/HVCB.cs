using Core.ADMS.Events;
using Core_AMS.Utilities;
using FrameworkUAS.Entity;
using KM.Common.Import;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ADMS.ClientMethods.Common;
using Core.ADMS;
using KM.Common;
using FileFunctions = Core_AMS.Utilities.FileFunctions;

namespace ADMS.ClientMethods
{
    public class HVCB : ClientSpecialCommon
    {
        private const string ClickAgentGuidField = "click_agent_guid";
        private const string Processing = "Processing ";

        private const string AgentsConsortia = "AgentsConsortia";
        private const string AgentsHeardOfUs = "AgentsHeardOfUs";
        private const string AgentsStayOrders = "Agents StayOrders";
        private const string AgentsWholesalers = "Agents Wholesalers";

        private const string ConsortiaIdField = "ConsortiaID";
        private const string SourceDescriptionField = "SourceDescription";
        private const string OrderStayCountField = "OrderStayCount";
        private const string WholesalerIdField = "WholesalerID";

        private const string OrderStayCountKey = "OrderStayCount";
        private const string SourceDescriptionKey = "SourceDescription";

        public void HVCBRelationalFiles(KMPlatform.Entity.Client client, SourceFile cSpecialFile, ClientCustomProcedure ccp, FileMoved eventMessage)
        {
            Console.WriteLine("Starting HVCB Relational Data");
            var fileWorker = new FileWorker();
            var fileFunctions = new FileFunctions();
            var destinationPath = $"{BaseDirs.getClientArchiveDir()}\\{client.FtpFolder}\\ProcessRelational\\{DateTime.Now:MMddyyyy}";
            Console.WriteLine("Deleting Old");
            if (Directory.Exists(destinationPath))
            {
                Directory.Delete(destinationPath, true);
            }

            Console.WriteLine("Extracting Data");
            fileFunctions.ExtractZipFile(eventMessage.ImportFile, destinationPath);
            var checkFiles = new[]
            {
                "c2m_vw_Agents.txt",
                "c2m_Agents_Wholesalers.txt",
                "c2m_Agents_Consortia.txt",
                "c2m_Agent_Profile_Hds.txt",
                "c2m_Agents_HeardOfUsFromSource.txt",
                "c2m_Agents_Exams.txt",
                "c2m_Agents_Stays_Orders.txt"
            };
            var fileList = ClientMethodHelpers.GetFilesListWithExistenceCheck(destinationPath, checkFiles);
            if (!fileList.Any())
            {
                return;
            }

            try
            {
                Console.WriteLine("Starting Data Processing");
                FileConfiguration fcAgents = new FileConfiguration();
                fcAgents.FileColumnDelimiter = "tab";
                fcAgents.FileExtension = ".txt";
                fcAgents.FileFolder = destinationPath;
                fcAgents.IsQuoteEncapsulated = false;
                DataTable dtAgents = fileWorker.GetData(fileList.Single(x => x.Name.Equals("c2m_vw_Agents.txt", StringComparison.CurrentCultureIgnoreCase)), fcAgents);
                dtAgents.Columns.Add(new DataColumn("WholesalerID"));
                dtAgents.Columns.Add(new DataColumn("ConsortiaID"));
                dtAgents.Columns.Add(new DataColumn("AverageClientCount"));
                dtAgents.Columns.Add(new DataColumn("NotifyAboutAreaEvents"));
                dtAgents.Columns.Add(new DataColumn("SourceDescription"));
                dtAgents.Columns.Add(new DataColumn("ExamPassed"));
                dtAgents.Columns.Add(new DataColumn("ExamGrade"));
                dtAgents.Columns.Add(new DataColumn("OrderStayCount"));
                DataColumn[] accessKeys = new DataColumn[1];
                accessKeys[0] = dtAgents.Columns["click_agent_guid"];
                //dtAgents.PrimaryKey = accessKeys;
                DataTable dtAgentsWholesalers = fileWorker.GetData(fileList.Single(x => x.Name.Equals("c2m_Agents_Wholesalers.txt", StringComparison.CurrentCultureIgnoreCase)), fcAgents);
                DataTable dtAgentsConsortia = fileWorker.GetData(fileList.Single(x => x.Name.Equals("c2m_Agents_Consortia.txt", StringComparison.CurrentCultureIgnoreCase)), fcAgents);
                DataTable dtAgentsProfile = fileWorker.GetData(fileList.Single(x => x.Name.Equals("c2m_Agent_Profile_Hds.txt", StringComparison.CurrentCultureIgnoreCase)), fcAgents);
                DataTable dtAgentsHeardOfUs = fileWorker.GetData(fileList.Single(x => x.Name.Replace(" ", "").Equals("c2m_Agents_HeardOfUsFromSource.txt", StringComparison.CurrentCultureIgnoreCase)), fcAgents);
                DataTable dtAgentsExams = fileWorker.GetData(fileList.Single(x => x.Name.Equals("c2m_Agents_Exams.txt", StringComparison.CurrentCultureIgnoreCase)), fcAgents);
                DataTable dtAgentsStaysOrders = fileWorker.GetData(fileList.Single(x => x.Name.Equals("c2m_Agents_Stays_Orders.txt", StringComparison.CurrentCultureIgnoreCase)), fcAgents);
                //int currentRow = 0;
                string line = String.Empty;

                var whole = ProcessAgents(dtAgentsWholesalers, AgentsWholesalers, WholesalerIdField);
                var rowIndex = AddSpecificRows(dtAgentsWholesalers, whole, WholesalerIdField);

                var consort = ProcessAgents(dtAgentsConsortia, AgentsConsortia, ConsortiaIdField);
                rowIndex = AddSpecificRows(dtAgentsConsortia, consort, ConsortiaIdField);

                #region Agents Profile
                Console.WriteLine("");
                Console.WriteLine("Processing AgentsProfile");
                List<string> ProfileIds = (from myValue in dtAgentsProfile.DefaultView.ToTable(true, "click_agent_guid").AsEnumerable() select myValue.Field<string>("click_agent_guid")).ToList();
                Dictionary<string, List<FileProfile>> Profile = new Dictionary<string, List<FileProfile>>();
                int ProfileIndex = 1;
                    
                foreach (string s in ProfileIds)
                {
                    List<FileProfile> p = new List<FileProfile>();
                    string backup = new string('\b', line.Length);
                    Console.Write(backup);
                    line = string.Format("Getting distinct value " + ProfileIndex.ToString() + " of " + ProfileIds.Count.ToString());
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(line);                        
                    var value = (from myValue in dtAgentsProfile.AsEnumerable() where myValue.Field<string>("click_agent_guid").Equals(s) select new { AverageClientCount = myValue.Field<string>("AverageClientCount"), NotifyAboutAreaEvents = myValue.Field<string>("NotifyAboutAreaEvents") });
                    foreach (var v in value)
                    {
                        FileProfile fp = new FileProfile();
                        fp.FieldOne = v.AverageClientCount.ToString();
                        fp.FieldTwo = v.NotifyAboutAreaEvents.ToString();
                        p.Add(fp);
                    }
                    Profile.Add(s, p);
                    ProfileIndex++;
                }

                Dictionary<int, DataRow> newProfileRows = new Dictionary<int, DataRow>();
                Dictionary<int, DataRow> deleteProfileRows = new Dictionary<int, DataRow>();
                rowIndex = 0;
                foreach (DataRow dr in dtAgents.Rows)
                {
                    string backup = new string('\b', line.Length);
                    double percent = (int)Math.Round((double)(100 * rowIndex) / dtAgents.Rows.Count);
                    Console.Write(backup);
                    line = string.Format("Processing: " + rowIndex + " / " + dtAgents.Rows.Count + " " + DateTime.Now.ToString() + " " + percent.ToString() + "%");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(line);

                    if (Profile.ContainsKey(dr["click_agent_guid"].ToString()))
                    {
                        List<FileProfile> values = new List<FileProfile>();
                        values = Profile[dr["click_agent_guid"].ToString()];
                        foreach (FileProfile v in values)
                        {
                            AddSpecificColumnRow("AverageClientCount", v.FieldOne, "NotifyAboutAreaEvents", v.FieldTwo, newProfileRows.Count + 1, newProfileRows, dtAgents, dr);
                        }
                        if (!deleteProfileRows.ContainsValue(dr))
                            deleteProfileRows.Add(rowIndex, dr);

                    }
                    rowIndex++;
                }
                dtAgents.AcceptChanges();
                foreach (KeyValuePair<int, DataRow> kvp in deleteProfileRows.OrderByDescending(k => k.Key))
                {
                    dtAgents.Rows[kvp.Key].Delete();
                }
                if (deleteProfileRows.Count > 0)
                    dtAgents.AcceptChanges();

                foreach (KeyValuePair<int, DataRow> kvp in newProfileRows)
                {
                    dtAgents.Rows.Add(kvp.Value);
                }
                if (newProfileRows.Count > 0)
                    dtAgents.AcceptChanges();
                #endregion

                #region Agents Exams
                Console.WriteLine("");
                Console.WriteLine("Processing AgentsExams");
                List<string> ExamsIds = (from myValue in dtAgentsExams.DefaultView.ToTable(true, "click_agent_guid").AsEnumerable() select myValue.Field<string>("click_agent_guid")).ToList();
                Dictionary<string, List<FileProfile>> Exams = new Dictionary<string, List<FileProfile>>();
                int ExamsIndex = 1;

                foreach (string s in ExamsIds)
                {
                    List<FileProfile> p = new List<FileProfile>();
                    string backup = new string('\b', line.Length);
                    Console.Write(backup);
                    line = string.Format("Getting distinct value " + ExamsIndex.ToString() + " of " + ExamsIds.Count.ToString());
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(line);                        
                    var value = (from myValue in dtAgentsExams.AsEnumerable() where myValue.Field<string>("click_agent_guid").Equals(s) select new { ExamPassed = myValue.Field<string>("ExamPassed"), ExamGrade = myValue.Field<string>("ExamGrade") });
                    foreach (var v in value)
                    {
                        FileProfile fp = new FileProfile();
                        fp.FieldOne = v.ExamPassed.ToString();
                        fp.FieldTwo = v.ExamGrade.ToString();
                        p.Add(fp);
                    }
                    Exams.Add(s, p);
                    ExamsIndex++;
                }

                Dictionary<int, DataRow> newExamsRows = new Dictionary<int, DataRow>();
                Dictionary<int, DataRow> deleteExamsRows = new Dictionary<int, DataRow>();
                rowIndex = 0;
                foreach (DataRow dr in dtAgents.Rows)
                {
                    string backup = new string('\b', line.Length);
                    double percent = (int)Math.Round((double)(100 * rowIndex) / dtAgents.Rows.Count);
                    Console.Write(backup);
                    line = string.Format("Processing: " + rowIndex + " / " + dtAgents.Rows.Count + " " + DateTime.Now.ToString() + " " + percent.ToString() + "%");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(line);

                    if (Exams.ContainsKey(dr["click_agent_guid"].ToString()))
                    {
                        List<FileProfile> values = new List<FileProfile>();
                        values = Exams[dr["click_agent_guid"].ToString()];
                        foreach (FileProfile v in values)
                        {
                            AddSpecificColumnRow("ExamPassed", v.FieldOne, "ExamGrade", v.FieldTwo, newExamsRows.Count + 1, newExamsRows, dtAgents, dr);
                        }
                        if (!deleteExamsRows.ContainsValue(dr))
                            deleteExamsRows.Add(rowIndex, dr);

                    }
                    rowIndex++;
                }
                dtAgents.AcceptChanges();
                foreach (KeyValuePair<int, DataRow> kvp in deleteExamsRows.OrderByDescending(k => k.Key))
                {
                    dtAgents.Rows[kvp.Key].Delete();
                }
                if (deleteExamsRows.Count > 0)
                    dtAgents.AcceptChanges();

                foreach (KeyValuePair<int, DataRow> kvp in newExamsRows)
                {
                    dtAgents.Rows.Add(kvp.Value);
                }
                if (newExamsRows.Count > 0)
                    dtAgents.AcceptChanges();
                #endregion

                Console.WriteLine("");
                var stayOrders = ProcessAgents(dtAgentsStaysOrders, AgentsStayOrders, OrderStayCountField);
                rowIndex = AddSpecificRows(dtAgentsStaysOrders, stayOrders, OrderStayCountKey);

                Console.WriteLine("");
                var sourceDescription = ProcessAgents(dtAgentsHeardOfUs, AgentsHeardOfUs, SourceDescriptionField);
                rowIndex = AddSpecificRows(dtAgents, sourceDescription, SourceDescriptionKey);

                #region Create File And Upload
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("");
                Console.WriteLine("Creating File");

                if (File.Exists("C:\\ADMS\\Client Archive\\HVCB\\AutoGen_c2m_vw_Agents_Processed.csv"))
                    File.Delete("C:\\ADMS\\Client Archive\\HVCB\\AutoGen_c2m_vw_Agents_Processed.csv");

                //currentRow = 0;
                int rowProcessedCount = 0;
                int totalRowCount = dtAgents.Rows.Count;
                int batchProcessing = 2500;
                if (totalRowCount < batchProcessing)
                    batchProcessing = totalRowCount;

                DataTable workTable = dtAgents.Clone();
                while (rowProcessedCount < totalRowCount)
                {
                    workTable.Clear();
                    for (int i = rowProcessedCount; i < batchProcessing; i++)
                    {
                        workTable.ImportRow(dtAgents.Rows[i]);
                    }
                    fileFunctions.CreateCSVFromDataTable(workTable, "C:\\ADMS\\Client Archive\\HVCB\\AutoGen_c2m_vw_Agents_Processed.csv", false);
                    batchProcessing = batchProcessing + workTable.Rows.Count;
                    rowProcessedCount = rowProcessedCount + workTable.Rows.Count;

                    if (batchProcessing > totalRowCount)
                        batchProcessing = totalRowCount;

                    string backup = new string('\b', line.Length);
                    double percent = (int)Math.Round((double)(100 * rowProcessedCount) / dtAgents.Rows.Count);
                    Console.Write(backup);
                    line = string.Format("Processed: " + rowProcessedCount + " / " + dtAgents.Rows.Count + " " + DateTime.Now.ToString() + " " + percent.ToString() + "%");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(line);
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("");
                Console.WriteLine("Uploading to FTP " + DateTime.Now.ToString());
                FrameworkUAS.BusinessLogic.ClientFTP blcftp = new FrameworkUAS.BusinessLogic.ClientFTP();
                ClientFTP clientFTP = new ClientFTP();
                clientFTP = blcftp.SelectClient(client.ClientID).First();
                Core_AMS.Utilities.FtpFunctions ftp = new FtpFunctions(clientFTP.Server, clientFTP.UserName, clientFTP.Password);
                ftp.Upload(clientFTP.Folder + "\\AutoGen_c2m_vw_Agents_Processed.csv", "C:\\ADMS\\Client Archive\\HVCB\\AutoGen_c2m_vw_Agents_Processed.csv");
                Console.WriteLine("Finished Uploading to FTP " + DateTime.Now.ToString());
                dtAgents.Dispose();
                dtAgents = null;
                #endregion
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".HVCBRelationalFiles");
            }
        }

        private IDictionary<string, string> ProcessAgents(DataTable dataTableAgents, string agentName, string idField)
        {
            Guard.NotNull(dataTableAgents, nameof(dataTableAgents));
            var agents = new Dictionary<string, string>();
            var agentsIndex = 1;
            var line = string.Empty;

            Console.WriteLine($"{Processing} {agentName}");
            var agentsGuids = dataTableAgents.DefaultView.ToTable(true, ClickAgentGuidField)
                .AsEnumerable()
                .Select(drValue => drValue.Field<string>(ClickAgentGuidField))
                .ToList();

            foreach (var guid in agentsGuids)
            {
                var backup = new string('\b', line.Length);
                Console.Write(backup);
                line = string.Format($"Getting distinct value {agentsIndex} of {agentsGuids.Count}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(line);

                var value = string.Join(Consts.CommaJoin,
                    dataTableAgents.AsEnumerable()
                        .Where(drValue => drValue.Field<string>(ClickAgentGuidField).Equals(guid))
                        .Select(drValue => drValue.Field<string>(idField)).ToList().Distinct());

                agents.Add(guid, value.TrimEnd(Consts.CommaSeparator));
                agentsIndex++;
            }
            return agents;
        }

        private int AddSpecificRows(DataTable dataTableAgents, IDictionary<string, string> source, string columnName)
        {
            Guard.NotNull(dataTableAgents, nameof(dataTableAgents));
            Guard.NotNull(source, nameof(source));

            var newRows = new Dictionary<int, DataRow>();
            var deleteRows = new Dictionary<int, DataRow>();
            var line = string.Empty;
            var rowIndex = 0;

            foreach (DataRow row in dataTableAgents.Rows)
            {
                var backup = new string('\b', line.Length);
                var percent = (int)Math.Round((double)(100 * rowIndex) / dataTableAgents.Rows.Count);
                Console.Write(backup);
                line = string.Format($"{Processing}: {rowIndex} / {dataTableAgents.Rows.Count} {DateTime.Now} {percent}%");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(line);

                if (source.ContainsKey(row[ClickAgentGuidField].ToString()))
                {
                    var guidList = source[row[ClickAgentGuidField].ToString()].Split(Consts.CommaSeparator).ToList();
                    foreach (var guid in guidList)
                    {
                        AddSpecificColumnRow(columnName, guid, newRows.Count + 1, newRows, dataTableAgents, row);
                    }

                    if (!deleteRows.ContainsValue(row))
                    {
                        deleteRows.Add(rowIndex, row);
                    }
                }
                rowIndex++;
            }
            dataTableAgents.AcceptChanges();
            UpdateDataTableAgents(dataTableAgents, newRows, deleteRows);

            return rowIndex;
        }

        private void UpdateDataTableAgents(DataTable dataTableAgents, IDictionary<int, DataRow> newRows, IDictionary<int, DataRow> deleteRows)
        {
            Guard.NotNull(dataTableAgents, nameof(dataTableAgents));
            Guard.NotNull(newRows, nameof(newRows));
            Guard.NotNull(deleteRows, nameof(deleteRows));

            foreach (var row in deleteRows.OrderByDescending(k => k.Key))
            {
                dataTableAgents.Rows[row.Key].Delete();
            }
            if (deleteRows.Any())
            {
                dataTableAgents.AcceptChanges();
            }

            foreach (var row in newRows)
            {
                dataTableAgents.Rows.Add(row.Value);
            }
            if (newRows.Any())
            {
                dataTableAgents.AcceptChanges();
            }
        }

        public void HVCBc2mAgentsAndSubscriptions(KMPlatform.Entity.Client client, SourceFile cSpecialFile, ClientCustomProcedure ccp, FileMoved eventMessage)
        {
            var fileWorker = new FileWorker();
            var fileFunctions = new FileFunctions();
            var destinationPath = $"{BaseDirs.getClientArchiveDir()}\\{client.FtpFolder}\\ProcessRelational\\{DateTime.Now:MMddyyyy}";
            fileFunctions.ExtractZipFile(eventMessage.ImportFile, destinationPath);

            var checkFiles = new[]
            {
                "c2m_vw_Agents.txt",
                "c2m_Agents_Subscriptions.txt",
            };
            var fileList = ClientMethodHelpers.GetFilesListWithExistenceCheck(destinationPath, checkFiles);

            if (!fileList.Any())
            {
                return;
            }

            try
            {

                #region vwAgents
                //1) Read in c2m_vw_Agents.txt
                FileConfiguration fcAgents = new FileConfiguration();
                fcAgents.FileColumnDelimiter = "tab";
                fcAgents.FileExtension = ".txt";
                fcAgents.FileFolder = destinationPath;
                fcAgents.IsQuoteEncapsulated = false;
                DataTable dtAgents = fileWorker.GetData(fileList.Single(x => x.Name.Equals("c2m_vw_Agents.txt", StringComparison.CurrentCultureIgnoreCase)), fcAgents);
                DataColumn[] accessKeys = new DataColumn[1];
                accessKeys[0] = dtAgents.Columns["click_agent_guid"];
                dtAgents.PrimaryKey = accessKeys;
                Console.WriteLine("Getting Access " + DateTime.Now.ToString());

                //dtAgents.Dispose();
                //dtAgents = null;
                #endregion

                #region AgentSubscriptions
                //1) Read in Access.csv
                FileConfiguration fcSubscriptions = new FileConfiguration();
                fcSubscriptions.FileColumnDelimiter = "tab";
                fcSubscriptions.FileExtension = ".txt";
                fcSubscriptions.FileFolder = destinationPath;
                fcSubscriptions.IsQuoteEncapsulated = false;
                DataTable dtSubscriptions = fileWorker.GetData(fileList.Single(x => x.Name.Equals("c2m_Agents_Subscriptions.txt", StringComparison.CurrentCultureIgnoreCase)), fcSubscriptions);
                dtSubscriptions.Columns.Add(new DataColumn("Source"));
                dtSubscriptions.Columns.Add(new DataColumn("Sub_Source"));
                dtSubscriptions.Columns.Add(new DataColumn("CTIRecipientID"));
                dtSubscriptions.Columns.Add(new DataColumn("Fingerprint"));
                dtSubscriptions.Columns.Add(new DataColumn("click_agent_id"));
                dtSubscriptions.Columns.Add(new DataColumn("click_recipient_type"));
                dtSubscriptions.Columns.Add(new DataColumn("AgentCategory"));
                dtSubscriptions.Columns.Add(new DataColumn("FirstName"));
                dtSubscriptions.Columns.Add(new DataColumn("LastName"));
                dtSubscriptions.Columns.Add(new DataColumn("Title"));
                dtSubscriptions.Columns.Add(new DataColumn("CompanyName"));
                dtSubscriptions.Columns.Add(new DataColumn("IATANumber"));
                dtSubscriptions.Columns.Add(new DataColumn("Address1"));
                dtSubscriptions.Columns.Add(new DataColumn("Address2"));
                dtSubscriptions.Columns.Add(new DataColumn("City"));
                dtSubscriptions.Columns.Add(new DataColumn("State"));
                dtSubscriptions.Columns.Add(new DataColumn("PostalCode"));
                dtSubscriptions.Columns.Add(new DataColumn("Country"));
                dtSubscriptions.Columns.Add(new DataColumn("PhoneNumber"));
                dtSubscriptions.Columns.Add(new DataColumn("FaxNumber"));
                dtSubscriptions.Columns.Add(new DataColumn("EmailAddress"));
                dtSubscriptions.Columns.Add(new DataColumn("Password"));                    
                dtSubscriptions.Columns.Add(new DataColumn("click_update_date"));
                dtSubscriptions.Columns.Add(new DataColumn("IsUnverified"));
                dtSubscriptions.Columns.Add(new DataColumn("IsInvalidAddress"));
                dtSubscriptions.Columns.Add(new DataColumn("AlternateConsortia"));
                dtSubscriptions.Columns.Add(new DataColumn("AcceptReferrals"));
                dtSubscriptions.Columns.Add(new DataColumn("IsHDSCertified"));
                dtSubscriptions.Columns.Add(new DataColumn("IsLoeaCertified"));
                dtSubscriptions.Columns.Add(new DataColumn("TemporaryPassword"));
                dtSubscriptions.Columns.Add(new DataColumn("SourceSubjectName"));
                dtSubscriptions.Columns.Add(new DataColumn("MSAIdentifier"));
                dtSubscriptions.Columns.Add(new DataColumn("LastLoginDate"));
                dtSubscriptions.Columns.Add(new DataColumn("LastLoginDays"));
                dtSubscriptions.Columns.Add(new DataColumn("PassedExamCount"));
                dtSubscriptions.Columns.Add(new DataColumn("isKMS"));
                dtSubscriptions.Columns.Add(new DataColumn("isMMS"));
                dtSubscriptions.Columns.Add(new DataColumn("isOMS"));
                dtSubscriptions.Columns.Add(new DataColumn("isHIMS"));
                dtSubscriptions.Columns.Add(new DataColumn("isNationalAgent"));
                DataColumn[] subKeys = new DataColumn[1];
                subKeys[0] = dtSubscriptions.Columns["click_agent_guid"];
                //dtSubscriptions.PrimaryKey = subKeys;
                //FrameworkUAD.Object.ImportVessel iv2 = null;
                int currentRow = 0;
                string line = String.Empty;

                foreach (DataRow dr in dtSubscriptions.Rows)
                {
                    string backup = new string('\b', line.Length);
                    double percent = (int)Math.Round((double)(100 * currentRow) / dtSubscriptions.Rows.Count);
                    Console.Write(backup);
                    line = string.Format("Processing: " + currentRow + " / " + dtSubscriptions.Rows.Count + " " + DateTime.Now.ToString() + " " + percent.ToString() + "%");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(line);

                    string matchValue = dr["click_agent_guid"].ToString();

                    DataRow match = dtAgents.Rows.Find(matchValue);
                    if (match != null)
                    {
                        dr["Source"] = match["Source"];
                        dr["Sub_Source"] = match["Sub_Source"];
                        dr["CTIRecipientID"] = match["CTIRecipientID"];
                        dr["Fingerprint"] = match["Fingerprint"];
                        dr["click_agent_id"] = match["click_agent_id"];
                        dr["click_recipient_type"] = match["click_recipient_type"];
                        dr["AgentCategory"] = match["AgentCategory"];
                        dr["FirstName"] = match["FirstName"];
                        dr["LastName"] = match["LastName"];
                        dr["Title"] = match["Title"];
                        dr["CompanyName"] = match["CompanyName"];
                        dr["IATANumber"] = match["IATANumber"];
                        dr["Address1"] = match["Address1"];
                        dr["Address2"] = match["Address2"];
                        dr["City"] = match["City"];
                        dr["State"] = match["State"];
                        dr["PostalCode"] = match["PostalCode"];
                        dr["Country"] = match["Country"];
                        dr["PhoneNumber"] = match["PhoneNumber"];
                        dr["FaxNumber"] = match["FaxNumber"];
                        dr["EmailAddress"] = match["EmailAddress"];
                        dr["Password"] = match["Password"];
                        dr["click_insert_date"] = match["click_insert_date"];
                        dr["click_update_date"] = match["click_update_date"];
                        dr["IsUnverified"] = match["IsUnverified"];
                        dr["IsInvalidAddress"] = match["IsInvalidAddress"];
                        dr["AlternateConsortia"] = match["AlternateConsortia"];
                        dr["AcceptReferrals"] = match["AcceptReferrals"];
                        dr["IsHDSCertified"] = match["IsHDSCertified"];
                        dr["IsLoeaCertified"] = match["IsLoeaCertified"];
                        dr["TemporaryPassword"] = match["TemporaryPassword"];
                        dr["SourceSubjectName"] = match["SourceSubjectName"];
                        dr["MSAIdentifier"] = match["MSAIdentifier"];
                        dr["LastLoginDate"] = match["LastLoginDate"];
                        dr["LastLoginDays"] = match["LastLoginDays"];
                        dr["PassedExamCount"] = match["PassedExamCount"];
                        dr["isKMS"] = match["isKMS"];
                        dr["isMMS"] = match["isMMS"];
                        dr["isOMS"] = match["isOMS"];
                        dr["isHIMS"] = match["isHIMS"];
                        dr["isNationalAgent"] = match["isNationalAgent"];

                        currentRow++;
                    }
                }
                    
                #endregion    

                fileFunctions.CreateCSVFromDataTable(dtSubscriptions, destinationPath + "\\HVCB_c2mAgentSubscriptions.csv", false);
                Console.WriteLine("Uploading to FTP " + DateTime.Now.ToString());
                FrameworkUAS.BusinessLogic.ClientFTP blcftp = new FrameworkUAS.BusinessLogic.ClientFTP();
                ClientFTP clientFTP = new ClientFTP();
                clientFTP = blcftp.SelectClient(client.ClientID).First();
                Core_AMS.Utilities.FtpFunctions ftp = new FtpFunctions(clientFTP.Server, clientFTP.UserName, clientFTP.Password);
                ftp.Upload(clientFTP.Folder + "\\AutoGen_HVCB_c2mAgentSubscriptions.csv", destinationPath + "\\HVCB_c2mAgentSubscriptions.csv");
                Console.WriteLine("Finished Uploading to FTP " + DateTime.Now.ToString());
                dtSubscriptions.Dispose();
                dtSubscriptions = null;
                dtAgents.Dispose();
                dtAgents = null;
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".HVCBc2mAgentsAndSubscriptions");
            }
        }

        public void AddSpecificColumnRow(string column, string value, int pos, Dictionary<int, DataRow> rows, DataTable data, DataRow original)
        {
            DataRow newRow = data.NewRow();
            newRow.ItemArray = original.ItemArray;
            newRow.SetField(column, value);
            rows.Add(pos, newRow);
        }
        public void AddSpecificColumnRow(string column1, string value1, string column2, string value2, int pos, Dictionary<int, DataRow> rows, DataTable data, DataRow original)
        {
            DataRow newRow = data.NewRow();
            newRow.ItemArray = original.ItemArray;
            newRow.SetField(column1, value1);
            newRow.SetField(column2, value2);
            rows.Add(pos, newRow);
        }

        public class FileProfile
        {
            public string FieldOne;
            public string FieldTwo;
        }
    }
}
