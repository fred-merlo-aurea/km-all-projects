using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Core.ADMS.Events;
using Core_AMS.Utilities;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.Entity;
using KM.Common.Functions;
using KM.Common.Import;
using KMPlatform.Entity;
using CommonEnums = KM.Common.Enums;

namespace ADMS.ClientMethods
{
    public class TradePress : ClientSpecialCommon
    {
        private const int DemographicsFieldMappingType = 3;
        private const int StandardFieldMappingType = 2;
        private const string IngoneFieldName = "Ignore";
        private const string EmailAddressFieldName = "EmailAddress";
        private const string PubCodeFieldName = "PubCode";

        public void Test(SourceFile cSpecialFile, FileMoved eventMessage)
        {
            //ConsoleMessage("Processing " + eventMessage.Client.FtpFolder + " Method: ", true);
        }

        public void File_RailTrends2005_2013(KMPlatform.Entity.Client client, SourceFile cSpecialFile, ClientCustomProcedure ccp, FileMoved eventMessage)
        {
            FileWorker fw = new FileWorker();

            DataTable dataIV = new DataTable();
            dataIV = fw.GetData(eventMessage.ImportFile, null);

            if (File.Exists("C:\\ADMS\\Client Archive\\TradePress\\RailTrends2005-2013.xlsx"))
                File.Delete("C:\\ADMS\\Client Archive\\TradePress\\RailTrends2005-2013.xlsx");

            DataColumn newColumn = new DataColumn { ColumnName = "Xact" };

            dataIV.Columns.Add(newColumn);

            foreach (DataRow dr_row in dataIV.Rows)
            {
                dr_row["Xact"] = dr_row["AttendeeStatus"];
            }

            Core_AMS.Utilities.FileFunctions ff = new FileFunctions();
            ff.CreateCSVFromDataTable(dataIV, "C:\\ADMS\\Client Archive\\TradePress\\RailTrends2005-2013_Processed.csv");

            KMPlatform.BusinessLogic.Client clientData = new KMPlatform.BusinessLogic.Client();
            int clientID = clientData.Select("TradePress").ClientID;
            FrameworkUAS.BusinessLogic.ClientFTP clientFTPData = new FrameworkUAS.BusinessLogic.ClientFTP();
            FrameworkUAS.Entity.ClientFTP clientFTP = clientFTPData.SelectClient(clientID).FirstOrDefault();
            bool uploaded = false;
            if (clientFTP != null)
            {
                Core_AMS.Utilities.FtpFunctions ftpData = new Core_AMS.Utilities.FtpFunctions(clientFTP.Server, clientFTP.UserName, clientFTP.Password);
                uploaded = ftpData.Upload(clientFTP.Folder + "\\RailTrends2005-2013_Processed.csv", "C:\\ADMS\\Client Archive\\TradePress\\RailTrends2005-2013_Processed.csv");
            }

            //ConsoleMessage("Uploaded = " + uploaded.ToString());
        }
        public void Process_PRWEB_ALL(KMPlatform.Entity.Client client, SourceFile cSpecialFile, ClientCustomProcedure ccp, FileMoved eventMessage)
        {
            //lets see if we can get the entire file into a dataset without blowing up - 666,625 records
            FileConfiguration fc = new FileConfiguration();
            fc.FileExtension = ".txt";
            fc.FileColumnDelimiter = CommonEnums.ColumnDelimiter.tab.ToString();
            fc.IsQuoteEncapsulated = false;

            DataTable dt = FileImporter.LoadFile(eventMessage.ImportFile, fc);
            int rows = dt.Rows.Count;
            foreach (DataRow dr in dt.Rows)
                dr["EmailAddress"] = dr["EmailAddress"].ToString().Replace("'", "");

            //group by EmailAddress to get distinct list
            var distinctEmails = from row in dt.AsEnumerable()
                                 group row by row.Field<string>("EmailAddress") into grp
                                 select new
                                 {
                                     EmailAddress = grp.Key.ToString().Replace("'", ""),
                                     MemberCount = grp.Count()
                                 };
            int distEmailCount = distinctEmails.Count();
            List<string> topicValues = new List<string>();
            //List<string> locationValues = new List<string>();
            //will build lists of EmailAddress:Value - can parse out later based on :
            int loopCounter = 1;
            foreach (var de in distinctEmails)
            {
                ConsoleMessage(loopCounter.ToString() + " of " + distEmailCount.ToString(), eventMessage.AdmsLog.ProcessCode);
                var dtDetail = dt.Select("EmailAddress = '" + de.EmailAddress.ToString() + "'").ToList();
                foreach (var d in dtDetail)
                {
                    if (!string.IsNullOrEmpty(d["TOPIC"].ToString()) && !topicValues.Contains(de.EmailAddress + ":" + d["TOPIC"].ToString()))
                        topicValues.Add(de.EmailAddress + ":" + d["TOPIC"].ToString());
                    //if (!locationValues.Contains(de.EmailAddress + ":" + d["LOCATION"].ToString()))
                    //    locationValues.Add(de.EmailAddress + ":" + d["LOCATION"].ToString());
                }
                loopCounter++;
            }


            int topValuesCount = topicValues.Count;
            //int locValuesCount = locationValues.Count;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("EmailAddress,TOPIC");
            foreach (string s in topicValues)
            {
                //need to make sure r[1] is a number and only 3 character long
                //r[0] should be checked for valid email address and not longer than 100 characters
                string[] r = s.Split(':');
                sb.AppendLine(r[0].ToString() + "," + r[1].ToString());
            }


            //sb.AppendLine("<XML>");
            //foreach(string s in topicValues)
            //{
            //    sb.AppendLine("<Record>");

            //    string[] r = s.Split(':');
            //    sb.AppendLine("<EmailAddress>" + r[0].ToString() + "</EmailAddress>");
            //    sb.AppendLine("<TOPIC>" + r[1].ToString() + "</TOPIC>");
            //    sb.AppendLine("<SourceFileId>" + eventMessage.SourceFile.SourceFileID.ToString() + "</SourceFileId>");
            //    sb.AppendLine("<ProcessCode>" + eventMessage.ProcessCode + "</ProcessCode>");

            //    sb.AppendLine("</Record>");
            //}
            //sb.AppendLine("</XML>");

            string csv = sb.ToString();
            //string xml = sb.ToString();

        }

        public void ProcessWebFiles(KMPlatform.Entity.Client client, SourceFile cSpecialFile, ClientCustomProcedure ccp, FileMoved eventMessage)
        {
            #region parse file to DataTable
            FileConfiguration fc = new FileConfiguration();
            fc.FileExtension = ".txt";
            fc.FileColumnDelimiter = CommonEnums.ColumnDelimiter.tab.ToString();
            fc.IsQuoteEncapsulated = false;

            var dt = FileImporter.LoadFile(eventMessage.ImportFile, fc);
            foreach (DataRow dr in dt.Rows)
                dr["EmailAddress"] = dr["EmailAddress"].ToString().Replace("'", "");
            #endregion

            #region get a list of Distinct Emails
            List<EmailProfile> distinctEmails = new List<EmailProfile>();
            var groupedEmails = dt.AsEnumerable().GroupBy(x => string.Format("{0}:{1}", x.Field<string>("EmailAddress"), x.Field<string>("PUBCODE")));
            //string and dataRow
            foreach (var t in groupedEmails)
            {
                string[] key = t.Key.Split(':');
                string email = key[0].ToString();
                string pubCode = key[1].ToString();
                EmailProfile ep = new EmailProfile(email, pubCode);

                foreach (var z in t)
                {
                    ep.RowList.Add(z);
                }

                distinctEmails.Add(ep);
            }
            #endregion

            FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            FrameworkUAD_Lookup.Entity.Code standardCode = cWorker.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Field_Mapping, FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString());
            FrameworkUAD_Lookup.Entity.Code demoCode = cWorker.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Field_Mapping, FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString());

            #region Get Valid and Invalid Eamil profiles based on PubCode
            List<EmailProfile> validEmailProfiles = new List<EmailProfile>();
            List<EmailProfile> invalidEmailProfiles = new List<EmailProfile>();

            Dictionary<string, FrameworkUAD.Entity.Product> clientPubCodes = FrameworkUAD.DataAccess.Product.Select(client.ClientConnections).ToDictionary(o => o.PubCode.ToUpper());
            //Dictionary<int, string> clientPubCodes = FrameworkUAS.Object.DBWorker.GetPubIDAndCodesByClient(eventMessage.Client);
            //bool sentError = false;
            foreach (EmailProfile ep in distinctEmails)
            {
                try
                {
                    //put profiles into Valid/Not valid lists based on pubcode
                    if (clientPubCodes.ContainsKey(ep.PubCode))
                        validEmailProfiles.Add(ep);
                    else
                        invalidEmailProfiles.Add(ep);
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".ProcessWebFiles");
                }
            }

            #endregion

            FillListByFieldMapping(client, eventMessage, validEmailProfiles, demoCode, standardCode);
            FillListByFieldMapping(client, eventMessage, invalidEmailProfiles, demoCode, standardCode);

            //passing xml may be too large, will need to batch
            //or create my list of SO and ST and SI and pass to bulk operations
            List<FrameworkUAD.Entity.SubscriberOriginal> soList = new List<FrameworkUAD.Entity.SubscriberOriginal>();
            List<FrameworkUAD.Entity.SubscriberTransformed> stList = new List<FrameworkUAD.Entity.SubscriberTransformed>();
            List<FrameworkUAD.Entity.SubscriberInvalid> siList = new List<FrameworkUAD.Entity.SubscriberInvalid>();

            #region Prepare Valid
            int counter = 1;
            foreach (EmailProfile ep in validEmailProfiles)
            {
                try
                {
                    List<DateTime> qDates = new List<DateTime>();
                    foreach (WebFileColumnValue wcv in ep.StandardList.Where(x => x.MafField.Equals("QDATE", StringComparison.CurrentCultureIgnoreCase)))
                    {
                        DateTime qd = DateTime.Now;
                        DateTime.TryParse(wcv.Value, out qd);
                        if (!qDates.Contains(qd))
                            qDates.Add(qd);
                    }
                    var minDate = DateTimeFunctions.GetMinDate();
                    if (qDates.Count > 0)
                        minDate = qDates.Min();

                    //SubscriberOriginal and SubscriberTransformed = ep.EmailAddress, pubCode, minDate
                    FrameworkUAD.Entity.SubscriberOriginal so = new FrameworkUAD.Entity.SubscriberOriginal(eventMessage.SourceFile.SourceFileID, counter, eventMessage.AdmsLog.ProcessCode);
                    so.Email = ep.EmailAddress;
                    so.PubCode = ep.PubCode;
                    so.QDate = minDate;

                    int pubCodeID = 0;
                    if (clientPubCodes.ContainsKey(ep.PubCode))
                        pubCodeID = clientPubCodes.SingleOrDefault(x => x.Key.Equals(ep.PubCode, StringComparison.CurrentCultureIgnoreCase)).Value.PubID;

                    //so.DemographicOriginalList
                    foreach (WebFileColumnValue wcv in ep.DemographicList)
                    {
                        FrameworkUAD.Entity.SubscriberDemographicOriginal sdo = new FrameworkUAD.Entity.SubscriberDemographicOriginal();
                        sdo.CreatedByUserID = so.CreatedByUserID;
                        sdo.DateCreated = DateTime.Now;
                        sdo.MAFField = wcv.MafField;
                        sdo.NotExists = false;
                        sdo.PubID = pubCodeID;
                        sdo.SORecordIdentifier = so.SORecordIdentifier;
                        sdo.Value = wcv.Value.ToString();

                        so.DemographicOriginalList.Add(sdo);
                    }

                    FrameworkUAD.Entity.SubscriberTransformed st = new FrameworkUAD.Entity.SubscriberTransformed(eventMessage.SourceFile.SourceFileID, so.SORecordIdentifier, so.ProcessCode);
                    st.ImportRowNumber = so.ImportRowNumber;
                    st.OriginalImportRow = so.ImportRowNumber;
                    st.Email = ep.EmailAddress;
                    st.PubCode = ep.PubCode;
                    st.QDate = minDate;
                    foreach (WebFileColumnValue wcv in ep.DemographicList)
                    {
                        FrameworkUAD.Entity.SubscriberDemographicTransformed sdt = new FrameworkUAD.Entity.SubscriberDemographicTransformed();
                        sdt.CreatedByUserID = so.CreatedByUserID;
                        sdt.DateCreated = DateTime.Now;
                        sdt.MAFField = wcv.MafField;
                        sdt.NotExists = false;
                        sdt.PubID = pubCodeID;
                        sdt.SORecordIdentifier = so.SORecordIdentifier;
                        sdt.STRecordIdentifier = st.STRecordIdentifier;
                        sdt.Value = wcv.Value.ToString();

                        st.DemographicTransformedList.Add(sdt);
                    }

                    soList.Add(so);
                    stList.Add(st);

                    counter++;
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".ProcessWebFiles");
                }
            }
            //sentError = false;
            #endregion

            #region Prepare InValid
            counter = 1;
            foreach (EmailProfile ep in invalidEmailProfiles)
            {
                try
                {
                    List<DateTime> qDates = new List<DateTime>();
                    foreach (WebFileColumnValue wcv in ep.StandardList.Where(x => x.MafField.Equals("QDATE", StringComparison.CurrentCultureIgnoreCase)))
                    {
                        DateTime qd = DateTime.Now;
                        DateTime.TryParse(wcv.Value, out qd);
                        if (!qDates.Contains(qd))
                            qDates.Add(qd);
                    }
                    var minDate = DateTimeFunctions.GetMinDate();
                    if (qDates.Count > 0)
                        minDate = qDates.Min();
                    //SubscriberOriginal and SubscriberTransformed = ep.EmailAddress, pubCode, minDate
                    FrameworkUAD.Entity.SubscriberOriginal so = new FrameworkUAD.Entity.SubscriberOriginal(eventMessage.SourceFile.SourceFileID, counter, eventMessage.AdmsLog.ProcessCode);
                    so.Email = ep.EmailAddress;
                    so.PubCode = ep.PubCode;
                    so.QDate = minDate;

                    int pubCodeID = 0;
                    if (clientPubCodes.ContainsKey(ep.PubCode))
                        pubCodeID = clientPubCodes.SingleOrDefault(x => x.Key.Equals(ep.PubCode, StringComparison.CurrentCultureIgnoreCase)).Value.PubID;
                    //so.DemographicOriginalList
                    foreach (WebFileColumnValue wcv in ep.DemographicList)
                    {
                        FrameworkUAD.Entity.SubscriberDemographicOriginal sdo = new FrameworkUAD.Entity.SubscriberDemographicOriginal();
                        sdo.CreatedByUserID = so.CreatedByUserID;
                        sdo.DateCreated = DateTime.Now;
                        sdo.MAFField = wcv.MafField;
                        sdo.NotExists = false;
                        sdo.PubID = pubCodeID;
                        sdo.SORecordIdentifier = so.SORecordIdentifier;
                        sdo.Value = wcv.Value.ToString();

                        so.DemographicOriginalList.Add(sdo);
                    }

                    FrameworkUAD.Entity.SubscriberInvalid si = new FrameworkUAD.Entity.SubscriberInvalid(eventMessage.SourceFile.SourceFileID, so.SORecordIdentifier, so.ProcessCode);
                    si.ImportRowNumber = so.ImportRowNumber;
                    si.Email = ep.EmailAddress;
                    si.PubCode = ep.PubCode;
                    si.QDate = minDate;
                    foreach (WebFileColumnValue wcv in ep.DemographicList)
                    {
                        FrameworkUAD.Entity.SubscriberDemographicInvalid sdi = new FrameworkUAD.Entity.SubscriberDemographicInvalid();
                        sdi.CreatedByUserID = so.CreatedByUserID;
                        sdi.DateCreated = DateTime.Now;
                        sdi.MAFField = wcv.MafField;
                        sdi.NotExists = false;
                        sdi.PubID = pubCodeID;
                        sdi.SORecordIdentifier = so.SORecordIdentifier;
                        sdi.SIRecordIdentifier = si.SIRecordIdentifier;
                        sdi.Value = wcv.Value.ToString();

                        si.DemographicInvalidList.Add(sdi);
                    }

                    soList.Add(so);
                    siList.Add(si);

                    counter++;
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".ProcessWebFiles");
                }
            }
            //sentError = false;
            #endregion

            #region Save Lists to DB - que for DQM
            try
            {
                ConsoleMessage("Orginal Count: " + soList.Count.ToString(), eventMessage.AdmsLog.ProcessCode);
                ConsoleMessage("Transformed Count: " + stList.Count.ToString(), eventMessage.AdmsLog.ProcessCode);
                ConsoleMessage("Invalid Count: " + siList.Count.ToString(), eventMessage.AdmsLog.ProcessCode);
                ConsoleMessage("Check Count: " + soList.Count.ToString() + " : " + (siList.Count + stList.Count).ToString(), eventMessage.AdmsLog.ProcessCode);

                FrameworkUAD.BusinessLogic.SubscriberOriginal soWorker = new FrameworkUAD.BusinessLogic.SubscriberOriginal();
                FrameworkUAD.BusinessLogic.SubscriberTransformed stWorker = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
                FrameworkUAD.BusinessLogic.SubscriberInvalid siWorker = new FrameworkUAD.BusinessLogic.SubscriberInvalid();
                ConsoleMessage("Start SO Bulk Insert: " + DateTime.Now.ToString(), eventMessage.AdmsLog.ProcessCode);
                soWorker.SaveBulkSqlInsert(soList, eventMessage.Client.ClientConnections);
                ConsoleMessage("Start ST Bulk Insert: " + DateTime.Now.ToString(), eventMessage.AdmsLog.ProcessCode);
                //stWorker.DisableTableIndexes(client.ClientConnections, eventMessage.SourceFile.SourceFileID, eventMessage.AdmsLog.ProcessCode);
                stWorker.SaveBulkSqlInsert(stList, eventMessage.Client.ClientConnections, false);
                ConsoleMessage("Start SI Bulk Insert: " + DateTime.Now.ToString(), eventMessage.AdmsLog.ProcessCode);
                siWorker.SaveBulkSqlInsert(siList, eventMessage.Client.ClientConnections);
                ConsoleMessage("Start CodesheetValidation: " + DateTime.Now.ToString(), eventMessage.AdmsLog.ProcessCode);
                CodesheetValidation(eventMessage);
                ConsoleMessage("Start QSourceValidation: " + DateTime.Now.ToString(), eventMessage.AdmsLog.ProcessCode);
                QSourceValidation(eventMessage);
                ConsoleMessage("Get ImportErrors: " + DateTime.Now.ToString(), eventMessage.AdmsLog.ProcessCode);
                FrameworkUAD.Object.ValidationResult vr = SelectImportErrors(eventMessage);
                ConsoleMessage("GoToDQM: " + DateTime.Now.ToString(), eventMessage.AdmsLog.ProcessCode);
                GoToDQM(eventMessage, vr);
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".ProcessWebFiles");
            }
            #endregion

        }

        private void FillListByFieldMapping(
            Client client,
            FileMoved eventMessage,
            IEnumerable<EmailProfile> emailProfiles,
            Code demoCode,
            Code standardCode)
        {
            foreach (var emailProfile in emailProfiles)
            {
                try
                {
                    var demoList = new List<WebFileColumnValue>();
                    var standardList = new List<WebFileColumnValue>();
                    var detailRow = 0;
                    foreach (var dr in emailProfile.RowList)
                    {
                        ApplyFieldMappingForRow(client, eventMessage, demoCode, standardCode, dr, detailRow, demoList, standardList);

                        detailRow++;
                    }

                    emailProfile.StandardList = standardList;
                    emailProfile.DemographicList = demoList;
                }
                catch (Exception ex)
                {
                    LogError(ex, client, $"{GetType().Name}.ProcessWebFiles");
                }
            }
        }

        private void ApplyFieldMappingForRow(
            Client client,
            FileMoved eventMessage,
            Code demoCode,
            Code standardCode,
            DataRow dr,
            int detailRow,
            IList<WebFileColumnValue> demoList,
            IList<WebFileColumnValue> standardList)
        {
            foreach (var fieldMapping in eventMessage.SourceFile.FieldMappings)
            {
                try
                {
                    if (fieldMapping.MAFField.Equals(IngoneFieldName, StringComparison.CurrentCultureIgnoreCase) ||
                        fieldMapping.IncomingField.Equals(EmailAddressFieldName, StringComparison.CurrentCultureIgnoreCase) ||
                        fieldMapping.IncomingField.Equals(PubCodeFieldName, StringComparison.CurrentCultureIgnoreCase) ||
                        !dr.Table.Columns.Contains(fieldMapping.IncomingField))
                    {
                        continue;
                    }

                    var webFileColumnValue = new WebFileColumnValue
                    {
                        IncomingField = fieldMapping.IncomingField,
                        MafField = fieldMapping.MAFField,
                        Value = dr[fieldMapping.IncomingField].ToString(),
                        DetailRow = detailRow
                    };
                    if (fieldMapping.FieldMappingTypeID == demoCode.CodeId || fieldMapping.FieldMappingTypeID == DemographicsFieldMappingType)
                    {
                        if (!demoList.Contains(webFileColumnValue))
                        {
                            demoList.Add(webFileColumnValue);
                        }
                    }
                    else if ((fieldMapping.FieldMappingTypeID == standardCode.CodeId ||
                              fieldMapping.FieldMappingTypeID == StandardFieldMappingType) &&
                             !standardList.Contains(webFileColumnValue))
                    {
                        standardList.Add(webFileColumnValue);
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex, client, $"{GetType().Name}.ProcessWebFiles");
                }
            }
        }

        private void CodesheetValidation(FileMoved eventMessage)
        {
            #region Codesheet validation
            ConsoleMessage("Start: Codesheet validation " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
            try
            {
                //dataIV = CodesheetValidations(dataIV, fieldMappingDetails, eventMessage.Client.FtpFolder, pubcodeColumnId, eventMessage, clientPubCodes);
                //call Sunil's job_CodesheetValidation sourceFileID, ProcessCode
                FrameworkUAD.BusinessLogic.CodeSheet csData = new FrameworkUAD.BusinessLogic.CodeSheet();
                csData.CodeSheetValidation(eventMessage.SourceFile.SourceFileID, eventMessage.AdmsLog.ProcessCode, eventMessage.Client.ClientConnections);
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".CodesheetValidation");
            }
            ConsoleMessage("Done: Codesheet validation " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
            #endregion
        }
        private void QSourceValidation(FileMoved eventMessage)
        {
            #region QSource validation
            ConsoleMessage("Start: QSource validation " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
            try
            {
                FrameworkUAD.BusinessLogic.Operations opsData = new FrameworkUAD.BusinessLogic.Operations();
                opsData.QSourceValidation(eventMessage.Client.ClientConnections, eventMessage.SourceFile.SourceFileID, eventMessage.AdmsLog.ProcessCode);
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".QSourceValidation");
            }
            ConsoleMessage("Done: QSource validation " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
            #endregion
        }
        private FrameworkUAD.Object.ValidationResult SelectImportErrors(FileMoved eventMessage)
        {
            FrameworkUAD.Object.ValidationResult validationResult = new FrameworkUAD.Object.ValidationResult(eventMessage.ImportFile, eventMessage.SourceFile.SourceFileID, eventMessage.AdmsLog.ProcessCode);
            #region Select ImportErrors
            try
            {
                List<FrameworkUAD.Object.ImportErrorSummary> listIES = new List<FrameworkUAD.Object.ImportErrorSummary>();
                //get any error info from CSV - will be written to table ImportError
                //add errors to allDataIV and remove bad rows from allDataIV
                FrameworkUAD.BusinessLogic.ImportErrorSummary ieData = new FrameworkUAD.BusinessLogic.ImportErrorSummary();
                listIES = ieData.Select(eventMessage.SourceFile.SourceFileID, eventMessage.AdmsLog.ProcessCode, eventMessage.Client.ClientConnections);
                int errorTotal = listIES.Sum(x => x.ErrorCount);
                if (listIES.Count > 0)
                {
                    validationResult.HasError = true;
                    validationResult.DimensionImportErrorCount += errorTotal;
                }
                foreach (FrameworkUAD.Object.ImportErrorSummary ies in listIES)
                    validationResult.DimensionImportErrorSummaries.Add(ies);
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".SelectImportErrors");
            }
            #endregion

            return validationResult;
        }
        private void GoToDQM(FileMoved eventMessage, FrameworkUAD.Object.ValidationResult validationResult)
        {
            #region Done validating - move on to DQM
            FileValidated fileProcessedDetails = new FileValidated(eventMessage.ImportFile, eventMessage.Client, eventMessage.IsKnownCustomerFileName, true, true, eventMessage.SourceFile, eventMessage.AdmsLog, validationResult, eventMessage.ThreadId);
            //should now update the FileStatus.FileStatusTypeID to "Validated"
            admsWrk.Update(eventMessage.AdmsLog.ProcessCode,
                            FrameworkUAD_Lookup.Enums.FileStatusType.Processing,
                            FrameworkUAD_Lookup.Enums.ADMS_StepType.Validator_End,
                            FrameworkUAD_Lookup.Enums.ProcessingStatusType.Validated,
                            FrameworkUAD_Lookup.Enums.ExecutionPointType.Post_Validation_Process, 1,
                            "Done: data validation " + DateTime.Now.TimeOfDay.ToString(), true,
                            eventMessage.AdmsLog.SourceFileId);

            ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " Done data validation for file: " + eventMessage.ImportFile.Name + " client: " + eventMessage.Client.FtpFolder);
            ADMS.Services.DataCleanser.AddressClean ac = new Services.DataCleanser.AddressClean();
            ac.HandleFileValidated(fileProcessedDetails);
            #endregion
        }
    }

    class EmailProfile
    {
        public EmailProfile()
        {
            EmailAddress = string.Empty;
            PubCode = string.Empty;
            RowList = new List<DataRow>();
            StandardList = new List<WebFileColumnValue>();
            DemographicList = new List<WebFileColumnValue>();
        }
        public EmailProfile(string emailAddress, string pubCode)
        {
            EmailAddress = emailAddress;
            PubCode = pubCode;
            RowList = new List<DataRow>();
            StandardList = new List<WebFileColumnValue>();
            DemographicList = new List<WebFileColumnValue>();
        }

        public string EmailAddress { get; set; }
        public string PubCode { get; set; }
        public List<DataRow> RowList { get; set; }
        public List<WebFileColumnValue> StandardList { get; set; }
        public List<WebFileColumnValue> DemographicList { get; set; }

    }
    class WebFileColumnValue
    {
        public string IncomingField { get; set; }
        public string MafField { get; set; }
        public string Value { get; set; }
        public int DetailRow { get; set; }
    }
}
