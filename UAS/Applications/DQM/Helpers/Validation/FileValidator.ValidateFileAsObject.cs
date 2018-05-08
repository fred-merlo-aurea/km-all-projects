using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Core.ADMS;
using Core_AMS.Utilities;
using FrameworkServices;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.Entity;
using FrameworkUAS.Object;
using KM.Common.Functions;
using KM.Common.Import;
using UAD_WS.Interface;
using UAS_WS.Interface;
using Client = KMPlatform.Entity.Client;
using Enums = FrameworkUAD_Lookup.Enums;
using BusinessEnums = KMPlatform.BusinessLogic.Enums;
using StringFunctions = KM.Common.StringFunctions;

namespace DQM.Helpers.Validation
{
    public partial class FileValidator
    {
        private const string VarcharDataType = "varchar";

        public Dictionary<string, string> ValidateFileAsObject(
            FileInfo myCheckFile,
            Client myClient,
            SourceFile mySourceFile)
        {
            SetText(currentOperation, "Starting File Validation.");
            SetText(currentProgress, "0");
            SetText(overallProgress, "0");

            InitializeValidator(myCheckFile, myClient, mySourceFile);

            var extension = checkFile.Extension;
            var path = BaseDirs.getAppsDir() + "\\DQM\\";
            var rawFileName = Path.GetFileNameWithoutExtension(checkFile.Name);
            if (IsExcel(extension))
            {
                extension = ".csv";
            }

            var tranFileName = $"Transformed_{rawFileName}{extension}";
            var tranFile = path + tranFileName;
            if (File.Exists(tranFile))
            {
                File.Delete(tranFile);
            }

            var origFileName = $"Original_{rawFileName}{extension}";
            var origFile = path + origFileName;
            if (File.Exists(origFile))
            {
                File.Delete(origFile);
            }

            var downloadFiles = new Dictionary<string, string>();
            codeWorker = ServiceClient.UAD_Lookup_CodeClient();
            var servWorker = ServiceClient.UAS_ServiceClient();
            var servFeatureWorker = ServiceClient.UAS_ServiceFeatureClient();

            try
            {
                GetServiceForFile(servWorker, servFeatureWorker);
                var fileWorker = new FileWorker();
                var rejectFile = ValidateFileTextQualifier(fileWorker);
                if (!rejectFile)
                {
                    //rename any nonFile columns
                    SetText(currentOperation, "Creating Details File.");
                    SetText(overallProgress, "5");

                    List<string> finalUnexpected;
                    List<FieldMapping> finalNotFound;
                    FileConfiguration fileConfig;
                    LoadVariablesAndContainers(out finalUnexpected, out finalNotFound, out fileConfig);
                    ProcessFile(myClient, fileWorker, fileConfig, finalNotFound, finalUnexpected);

                    validationResult.OriginalDuplicateRecordCount = dataIV.OriginalDuplicateRecordCount;
                    validationResult.TransformedDuplicateRecordCount = dataIV.TransformedDuplicateRecordCount;
                }
                else
                {
                    SetText(currentOperation, "Rejecting file.");
                    SetText(currentProgress, "100");
                    SetText(overallProgress, "90");
                }
            }
            catch (Exception ex)
            {
                ErrorMessages.Add(StringFunctions.FormatException(ex));
            }

            CreateDownloadFiles(path, downloadFiles);
            GC.Collect();
            return downloadFiles;
        }

        private void CreateDownloadFiles(string path, IDictionary<string, string> downloadFiles)
        {
            try
            {
                SetText(currentOperation, "Creating File Validation Result Files.");
                SetText(currentProgress, "0");
                SetText(overallProgress, "95");

                var vrWorker = new FrameworkUAD.BusinessLogic.ValidationResult();

                //Used to send one email per request
                var reportBody = GetReportBodyForSingleEmail(
                    IsKnownCustomerFileName,
                    isValidFileType,
                    isFileSchemaValid,
                    validationResult);
                var badDataAttachment = string.Empty;
                if (validationResult.RecordImportErrorCount > 0)
                {
                    badDataAttachment = vrWorker.GetBadData(validationResult, sourceFile);
                }

                SetText(currentProgress, "25");

                var fileFunctions = new FileFunctions();
                var detailFile = $"{path}{sourceFile.FileName}_Details.html";
                var errorFile = $"{path}{sourceFile.FileName}_ErrorMessages.txt";
                var badDataFile = $"{path}{sourceFile.FileName}_BadData_{Path.GetFileNameWithoutExtension(checkFile.Name)}.csv";

                if (File.Exists(detailFile))
                {
                    File.Delete(detailFile);
                }

                if (File.Exists(errorFile))
                {
                    File.Delete(errorFile);
                }

                if (File.Exists(badDataFile))
                {
                    File.Delete(badDataFile);
                }

                fileFunctions.CreateFile(detailFile, reportBody.ToString());
                downloadFiles.Add("Details", detailFile);
                SetText(currentProgress, "50");

                if (ErrorMessages.Count > 0)
                {
                    var sbErrorMessages = new StringBuilder();
                    ErrorMessages.ForEach(x => sbErrorMessages.AppendLine(x));
                    fileFunctions.CreateFile(errorFile, sbErrorMessages.ToString());
                    downloadFiles.Add("Errors", errorFile);
                }

                SetText(currentProgress, "75");

                if (!string.IsNullOrEmpty(badDataAttachment)) //Whitespace is a valid processing reason here
                {
                    fileFunctions.CreateFile(badDataFile, badDataAttachment);
                    downloadFiles.Add("BadData", badDataFile);
                }

                SetText(currentProgress, "100");
                SetText(overallProgress, "100");

                //reset variables
                dataIV = null;
                validationResult = null;
                listIES = null;
                sourceFile = null;
            }
            catch (Exception ex)
            {
                ErrorMessages.Add(StringFunctions.FormatException(ex));
            }
        }

        private void ProcessFile(
            Client myClient,
            FileWorker fileWorker,
            FileConfiguration fileConfig,
            List<FieldMapping> finalNotFound,
            List<string> finalUnexpected)
        {
            var dbWorker = ServiceClient.UAS_DBWorkerClient();
            var stWorker = ServiceClient.UAD_SubscriberTransformedClient();
            var fvIeWorker = ServiceClient.UAD_FileValidator_ImportErrorClient();
            var codeSheetWorker = ServiceClient.UAD_CodeSheetClient();
            var operationsWorker = ServiceClient.UAD_OperationsClient();
            var ieSummaryWorker = ServiceClient.UAD_ImportErrorSummaryClient();

            if (fileWorker.AcceptableFileType(checkFile) == false)
            {
                isValidFileType = false;
                ErrorMessages.Add("File is not an acceptable type for processing.");
            }
            else
            {
                SetText(currentOperation, "Start file processing");
                SetText(overallProgress, "6");

                var fileTotalRowCount = PrepareDataProcessing(myClient, fileWorker, fileConfig, dbWorker);
                var isPubCodeMissing = EnsurePubCodeIsMapped();
                if (fileTotalRowCount > 0)
                {
                    SetText(currentOperation, "Disable Indexes");
                    SetText(overallProgress, "13");
                    stWorker.Proxy.DisableIndexes(accessKey, myClient.ClientConnections);
                    if (isPubCodeMissing)
                    {
                        ReportPubCodeMissing();
                    }
                    else
                    {
                        LoadFileData(myClient, fileConfig, fvIeWorker);
                        ProcessFileData(myClient, finalNotFound, finalUnexpected);
                        ValidateCodeSheet(myClient, codeSheetWorker);
                        ValidateQSource(myClient, operationsWorker);
                        ProcessDemographicsImportErrors(myClient, ieSummaryWorker);
                    }
                }
                else
                {
                    ReportFileHasNoRows(myClient);
                }
            }
        }

        private void ReportFileHasNoRows(Client myClient)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Attention: {myClient.FtpFolder}");
            sb.AppendLine("We were unable to read this file.");
            sb.AppendLine($"File: {checkFile.Name}");
            sb.AppendLine("Make sure that the file name does not include invalid characters or punctuation and that it is not too long.");
            ErrorMessages.Add(sb.ToString());

            validationResult.OriginalRowCount = 0;
            validationResult.TransformedRowCount = 0;
            validationResult.TotalRowCount = 0;

            validationResult.HasError = true;
            validationResult.RecordImportErrorCount++;

            var importError = new ImportError(-1, processCode, sourceFile.SourceFileID, sb.ToString());
            validationResult.RecordImportErrors.Add(importError);

            isFileSchemaValid = false;
        }

        private void ReportPubCodeMissing()
        {
            validationResult.OriginalRowCount = 0;
            validationResult.TransformedRowCount = 0;
            validationResult.TotalRowCount = 0;

            validationResult.HasError = true;
            validationResult.RecordImportErrorCount++;

            var ie = new ImportError(
                -1,
                processCode,
                sourceFile.SourceFileID,
                "File requires a PUBCODE column");
            validationResult.RecordImportErrors.Add(ie);

            isFileSchemaValid = false;
        }

        private void ProcessDemographicsImportErrors(Client myClient, ServiceClient<IImportErrorSummary> ieSummaryWorker)
        {
            try
            {
                //get any error info from CSV - will be written to table ImportError
                //add errors to allDataIV and remove bad rows from allDataIV
                var ieSummaryResp = ieSummaryWorker
                    .Proxy
                    .Select(accessKey, sourceFile.SourceFileID, processCode, myClient.ClientConnections);
                if (ieSummaryResp.Result != null &&
                    ieSummaryResp.Status == Enums.ServiceResponseStatusTypes.Success)
                {
                    listIES = ieSummaryResp.Result;
                    var errorTotal = listIES.Sum(x => x.ErrorCount);
                    if (listIES.Count > 0)
                    {
                        validationResult.HasError = true;
                        validationResult.DimensionImportErrorCount += errorTotal;
                    }

                    foreach (var ies in listIES)
                    {
                        validationResult.DimensionImportErrorSummaries.Add(ies);
                    }
                }
            }
            catch (Exception ex)
            {
                var message = StringFunctions.FormatException(ex);
                var sbDetail = new StringBuilder();
                sbDetail.AppendLine("DQM.Helpers.Validation.FileValidator.ProcessFileAsObject - Select ImportErrors");
                sbDetail.AppendLine(Environment.NewLine);
                ErrorMessages.Add(message);
            }
        }

        private void ValidateQSource(Client myClient, ServiceClient<IOperations> operationsWorker)
        {
            if (BusinessEnums.GetUADFeature(serviceFeature.SFName) == BusinessEnums.UADFeatures.Data_Compare)
            {
                return;
            }

            try
            {
                operationsWorker.Proxy.QSourceValidation(accessKey,
                    sourceFile.SourceFileID,
                    processCode,
                    myClient.ClientConnections);
            }
            catch (Exception ex)
            {
                var message = StringFunctions.FormatException(ex);
                var sbDetail = new StringBuilder();
                sbDetail.AppendLine("DQM.Helpers.Validation.FileValidator.ProcessFileAsObject - QSource Validation");
                sbDetail.AppendLine(Environment.NewLine);
                sbDetail.AppendLine(message);
                ErrorMessages.Add(message);
            }
        }

        private void ValidateCodeSheet(Client myClient, ServiceClient<ICodeSheet> codeSheetWorker)
        {
            if (KMPlatform.BusinessLogic.Enums.GetUADFeature(serviceFeature.SFName) == KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
            {
                return;
            }

            try
            {
                codeSheetWorker.Proxy.CodeSheetValidation(accessKey,
                    sourceFile.SourceFileID,
                    processCode,
                    myClient.ClientConnections);
            }
            catch (Exception ex)
            {
                var message = StringFunctions.FormatException(ex);
                var sbDetail = new StringBuilder();
                sbDetail.AppendLine("DQM.Helpers.Validation.FileValidator.ProcessFileAsObject - Codesheet Validation");
                sbDetail.AppendLine(Environment.NewLine);
                sbDetail.AppendLine(message);
                ErrorMessages.Add(message);
            }
        }

        private void ProcessFileData(Client myClient, List<FieldMapping> finalNotFound, List<string> finalUnexpected)
        {
            if (dataIV.TotalRowCount > 0)
            {
                ValidateTransformations();
                InitializeHeaders();
                ValidateColumns(myClient, finalNotFound, finalUnexpected);
                ApplyAdHocDimensions();
                if (!ValidateQDateFormat())
                {
                    var reason = $"Your file is configured to have a QDate format of {sourceFile.QDateFormat} but your QDate data does not match that format.  Please either correctly format your QDate file data or adjust the setup configuration.";
                    ErrorMessages.Add(reason);
                }
                else
                {
                    ValidateData();

                    if (dataIV.HasError)
                    {
                        validationResult.HasError = true;
                    }

                    validationResult.OriginalRowCount = dataIV.OriginalRowCount;
                    validationResult.TransformedRowCount = dataIV.TransformedRowCount;
                    validationResult.RecordImportErrorCount = dataIV.ImportErrorCount;
                    validationResult.ImportedRowCount = dataIV.ImportedRowCount;

                    if (dataIV.ImportErrorCount > 0 && dataIV.ImportErrors != null)
                    {
                        try
                        {
                            validationResult.RecordImportErrors =
                                new HashSet<ImportError>(dataIV.ImportErrors.ToList());
                        }
                        catch (Exception ex)
                        {
                            var message = StringFunctions.FormatException(ex);
                            ErrorMessages.Add(message);
                        }
                    }
                }
            }
            else
            {
                ValidationError("No data from file, stopping processing.");
                ErrorMessages.Add("No data from file, stopping processing.");
            }
        }

        private bool ValidateQDateFormat()
        {
            var validQDateFormat = true;
            var qDateColumnHeader = string.Empty;
            var thisFm = sourceFile
                .FieldMappings
                .SingleOrDefault(x => x.MAFField.Equals(Core_AMS.Utilities.Enums.MAFFieldStandardFields.QUALIFICATIONDATE.ToString(),StringComparison.CurrentCultureIgnoreCase));
            if (thisFm != null && dataIV.HeadersTransformed.ContainsKey(thisFm.MAFField))
            {
                qDateColumnHeader = thisFm.MAFField;
            }

            if (string.IsNullOrWhiteSpace(qDateColumnHeader))
            {
                return true;
            }

            var qDatePattern2 = sourceFile.QDateFormat.Replace('D', 'd').Replace('Y', 'y');
            foreach (var key in dataIV.DataTransformed.Keys)
            {
                var myRow = dataIV.DataTransformed[key];
                if (!string.IsNullOrWhiteSpace(myRow[qDateColumnHeader]))
                {
                    //Get QDateValue and then remove all characters not numeric for comparison.
                    var qDateValue = myRow[qDateColumnHeader];
                    qDateValue = new string(qDateValue.Where(char.IsDigit).ToArray());
                    if (string.IsNullOrWhiteSpace(qDateValue))
                    {
                        myRow[qDateColumnHeader] = DateTime.Now.ToShortDateString();
                    }
                    else
                    {
                        try
                        {
                            var defaultDateTime = new DateTime();
                            DateTime qDateConvertValue;
                            DateTime.TryParseExact(qDateValue,
                                qDatePattern2,
                                null,
                                DateTimeStyles.None,
                                out qDateConvertValue);
                            if (qDateConvertValue == defaultDateTime)
                            {
                                DateTime.TryParse(myRow[qDateColumnHeader], out qDateConvertValue);
                                if (qDateConvertValue == defaultDateTime)
                                {
                                    DateFormat format;
                                    Enum.TryParse(sourceFile.QDateFormat, out format);
                                    qDateConvertValue = DateTimeFunctions.ParseDate(format, qDateValue);

                                    if (qDateConvertValue >= defaultDateTime)
                                    {
                                        validQDateFormat = false;
                                    }
                                    else
                                    {
                                        validQDateFormat = true;
                                        myRow[qDateColumnHeader] = qDateConvertValue.ToShortDateString();
                                        SetText(currentOperation, "Auto fixing your QDate data.");
                                    }
                                }
                                else
                                {
                                    validQDateFormat = true;
                                    myRow[qDateColumnHeader] = qDateConvertValue.ToShortDateString();
                                    SetText(currentOperation, "Auto fixing your QDate data.");
                                }
                            }
                            else
                            {
                                validQDateFormat = true;
                                myRow[qDateColumnHeader] = qDateConvertValue.ToShortDateString();
                                SetText(currentOperation, "Auto fixing your QDate data.");
                            }
                        }
                        catch (Exception)
                        {
                            validQDateFormat = false;
                            myRow[qDateColumnHeader] = DateTime.Now.ToShortDateString();
                        }
                    }
                }
                else
                {
                    myRow[qDateColumnHeader] = DateTime.Now.ToShortDateString();
                }
            }

            return validQDateFormat;
        }

        private void ValidateColumns(Client myClient, List<FieldMapping> finalNotFound, List<string> finalUnexpected)
        {
            var fieldMappingTypeCodes = codeWorker.Proxy.Select(accessKey, Enums.CodeType.Field_Mapping).Result;
            CreateAdHocDimensionIfMissing(myClient, fieldMappingTypeCodes);
            RemoveIgnoredColumns(fieldMappingTypeCodes);
            LogColumnsNotFound(finalNotFound);
            LogUnexpectedColumns(finalUnexpected);
        }

        private void LogUnexpectedColumns(List<string> finalUnexpected)
        {
            foreach (var col in dataIV.HeadersTransformed.Keys)
            {
                if (col.ToString().Equals("OriginalImportRow", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                if (!sourceFile.FieldMappings.Any(x => x.IncomingField.Equals(col.ToString(), StringComparison.CurrentCultureIgnoreCase)))
                {
                    finalUnexpected.Add(col.ToString());
                }
            }

            if (finalUnexpected.Count > 0)
            {
                validationResult.HasError = true;
                validationResult.RecordImportErrorCount++;
                validationResult.UnexpectedColumns = new HashSet<string>(finalUnexpected);
                var sb = new StringBuilder();
                sb.AppendLine($"One or more unmapped columns were found in your import file: {checkFile.Name}<br/>");
                sb.AppendLine("The columns have been excluded.<br/>");
                sb.AppendLine("If these columns contain data that is required for processing or you intent to have in your UAD,");
                sb.Append(" please add these columns to your file mapping and resubmit for processing.<br/>");
                sb.AppendLine($"Unexpected Columns - {string.Join(", ", finalUnexpected)}<br/>");
                var ie = new ImportError(
                    -1,
                    processCode,
                    sourceFile.SourceFileID,
                    sb.ToString());
                validationResult.RecordImportErrors.Add(ie);
                ErrorMessages.Add(sb.ToString());

                foreach (var u in finalUnexpected)
                {
                    dataIV.HeadersTransformed.Remove(u);
                    foreach (var dt in dataIV.DataTransformed.Keys)
                    {
                        var myRow = dataIV.DataTransformed[dt];
                        myRow.Remove(u);
                    }
                }
            }
        }

        private void LogColumnsNotFound(List<FieldMapping> finalNotFound)
        {
            foreach (var map in sourceFile.FieldMappings.Where(x => x.MAFField.ToUpper() != "IGNORE"))
            {
                if (!dataIV.HeadersTransformed.ContainsKey(map.IncomingField))
                {
                    finalNotFound.Add(map);
                }
            }

            if (finalNotFound.Count > 0)
            {
                validationResult.HasError = true;
                validationResult.RecordImportErrorCount++;
                foreach (var nf in finalNotFound)
                {
                    validationResult.NotFoundColumns.Add(nf.IncomingField);
                }

                var notFoundError = $"VALIDATOR: Not Found Columns -  {string.Join(", ", validationResult.NotFoundColumns)}<br/>";
                var ie = new ImportError(
                    -1,
                    processCode,
                    sourceFile.SourceFileID,
                    notFoundError);
                validationResult.RecordImportErrors.Add(ie);
                ErrorMessages.Add(notFoundError);

                foreach (var nf in finalNotFound)
                {
                    sourceFile.FieldMappings.Remove(nf);
                }
            }
        }

        private void RemoveIgnoredColumns(List<Code> fieldMappingTypeCodes)
        {
            var ignoreId = fieldMappingTypeCodes.Single(x => x.CodeName.Equals(Enums.FieldMappingTypes.Ignored.ToString())).CodeId;
            var deleteIgnoredFieldMappings =
                (from i in sourceFile.FieldMappings
                    where i.FieldMappingTypeID == ignoreId
                    select i)
                .ToList();
            foreach (var del in deleteIgnoredFieldMappings)
            {
                sourceFile.FieldMappings.Remove(del);
                if (dataIV.HeadersOriginal.ContainsKey(del.IncomingField))
                {
                    dataIV.HeadersOriginal.Remove(del.IncomingField);
                    foreach (var key in dataIV.DataOriginal.Keys)
                    {
                        var myRow = dataIV.DataOriginal[key];
                        myRow.Remove(del.IncomingField);
                    }
                }

                if (dataIV.HeadersTransformed.ContainsKey(del.IncomingField))
                {
                    dataIV.HeadersTransformed.Remove(del.IncomingField);
                    foreach (var key in dataIV.DataTransformed.Keys)
                    {
                        var myRow = dataIV.DataTransformed[key];
                        myRow.Remove(del.IncomingField);
                    }
                }
            }
        }

        private void CreateAdHocDimensionIfMissing(Client myClient, List<Code> fieldMappingTypeCodes)
        {
            var fieldMappingTypeId = fieldMappingTypeCodes.Single(x => x.CodeName.Equals(Enums.FieldMappingTypes.Demographic.ToString())).CodeId;
            var ahdgResp = ServiceClient.UAS_AdHocDimensionGroupClient().Proxy.Select(accessKey, myClient.ClientID, true);
            var ahdGroups =
                new List<AdHocDimensionGroup>();
            if (ahdgResp.Result != null)
            {
                ahdGroups = ahdgResp.Result.Where(x => x.IsActive).ToList();
            }

            if (ahdGroups.Count > 0)
            {
                var distCreatedDims = new List<string>();
                foreach (var adg in ahdGroups)
                {
                    if (!distCreatedDims.Contains(adg.CreatedDimension))
                    {
                        distCreatedDims.Add(adg.CreatedDimension);
                    }
                }

                foreach (var s in distCreatedDims)
                {
                    if (!sourceFile.FieldMappings.Any(x =>x.MAFField.Equals(s, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        var dimFm = new FieldMapping
                        {
                            FieldMappingTypeID = fieldMappingTypeId,
                            IsNonFileColumn = true,
                            SourceFileID = sourceFile.SourceFileID,
                            IncomingField = s,
                            MAFField = s,
                            PubNumber = 0,
                            DataType = VarcharDataType,
                            PreviewData = string.Empty,
                            HasMultiMapping = false,
                            CreatedByUserID = 1,
                            ColumnOrder = sourceFile.FieldMappings.Count + 1,
                            DateCreated = DateTime.Now
                        };

                        sourceFile.FieldMappings.Add(dimFm);
                        if (!sourceFile.FieldMappings.Contains(dimFm))
                        {
                            sourceFile.FieldMappings.Add(dimFm);
                        }

                        if (!dataIV.HeadersTransformed.ContainsKey(s))
                        {
                            dataIV.HeadersTransformed.Add(s,
                                (dataIV.HeadersTransformed.Count + 1).ToString());
                        }
                    }
                }
            }
        }

        private void InitializeHeaders()
        {
            foreach (string column in dataIV.HeadersOriginal.Keys)
            {
                if (column.Equals(PubCodeFieldName, StringComparison.CurrentCultureIgnoreCase))
                {
                    var incomingCol = sourceFile.FieldMappings.Single(x =>x.MAFField.Equals(column, StringComparison.CurrentCultureIgnoreCase)).MAFField.ToUpper();
                    if (!string.IsNullOrWhiteSpace(incomingCol) && !dataIV.HeadersTransformed.ContainsKey(incomingCol))
                    {
                        dataIV.HeadersTransformed.Add(incomingCol,
                            (dataIV.HeadersTransformed.Count + 1).ToString());
                    }
                    else
                    {
                        if (!dataIV.HeadersTransformed.ContainsKey(column))
                        {
                            dataIV.HeadersTransformed.Add(column,
                                (dataIV.HeadersTransformed.Count + 1).ToString());
                        }
                    }
                }
                else if (sourceFile.FieldMappings.Any(x =>x.IncomingField.Equals(column,StringComparison.CurrentCultureIgnoreCase)))
                {
                    var incomingCol = sourceFile.FieldMappings.Single(x =>x.IncomingField.Equals(column,StringComparison.CurrentCultureIgnoreCase)).IncomingField.ToUpper();
                    if (!string.IsNullOrWhiteSpace(incomingCol) &&
                        !dataIV.HeadersTransformed.ContainsKey(incomingCol))
                    {
                        dataIV.HeadersTransformed.Add(incomingCol, (dataIV.HeadersTransformed.Count + 1).ToString());
                    }
                    else
                    {
                        if (!dataIV.HeadersTransformed.ContainsKey(column))
                        {
                            dataIV.HeadersTransformed.Add(column, (dataIV.HeadersTransformed.Count + 1).ToString());
                        }
                    }
                }
            }
        }

        private void ValidateTransformations()
        {
            try
            {
                SetText(currentOperation, "Transform Import File Data");
                SetText(overallProgress, "15");
                TransformImportFileData();
                SetText(overallProgress, "25");
            }
            catch (Exception ex)
            {
                var message = StringFunctions.FormatException(ex);
                var sbDetail = new StringBuilder();
                sbDetail.AppendLine(
                    "DQM.Helpers.Validation.FileValidator.ValidateFileAsObject - Transform Data");
                sbDetail.AppendLine(Environment.NewLine);

                sbDetail.AppendLine(message);
                ErrorMessages.Add(sbDetail.ToString());
                ValidationError(sbDetail.ToString());
            }
        }

        private void LoadFileData(Client myClient, FileConfiguration fileConfig, ServiceClient<IFileValidator_ImportError> fvIeWorker)
        {
            SetText(currentOperation, "Load data from the file");
            SetText(overallProgress, "14");
            var ifWorker = new FrameworkUAD.BusinessLogic.ImportFile();
            dataIV = ifWorker.GetImportFile(checkFile, fileConfig);
            SetText(currentOperation, "Data loaded");
            SetText(overallProgress, "15");
            dataIV.ClientId = myClient.ClientID;
            dataIV.SourceFileId = sourceFile.SourceFileID;
            dataIV.ProcessCode = processCode;
            if (dataIV.ImportErrorCount > 0)
            {
                fvIeWorker.Proxy.SaveBulkSqlInsert(accessKey, dataIV.ImportErrors.ToList(), myClient.ClientConnections);
                validationResult.RecordImportErrorCount = dataIV.ImportErrorCount;
                validationResult.RecordImportErrors = dataIV.ImportErrors;
            }
        }

        private bool EnsurePubCodeIsMapped()
        {
            var isPubCodeMissing = false;
            dbFileType = codeWorker.Proxy.SelectCodeId(accessKey, sourceFile.DatabaseFileTypeId).Result;

            var mappedColumns = (from c in sourceFile.FieldMappings
                select c.MAFField.ToUpper()).ToList();

            SetText(currentOperation, "Check that file has a PubCode mapped");
            SetText(overallProgress, "12");
            if (!mappedColumns.Contains("PUBCODE"))
            {
                var msg = "File requires a PUBCODE column";
                isPubCodeMissing = true;
                ErrorMessages.Add(msg);
                ValidationError(msg);
            }

            return isPubCodeMissing;
        }

        private int PrepareDataProcessing(
            Client myClient,
            FileWorker fileWorker,
            FileConfiguration fileConfig,
            ServiceClient<IDBWorker> dbWorker)
        {
            SetText(currentOperation, "Get row count");
            SetText(overallProgress, "7");
            var fileTotalRowCount = fileWorker.GetRowCount(checkFile);
            SetText(currentOperation, "Get headers");
            SetText(overallProgress, "8");
            validationResult.HeadersOriginal = fileWorker.GetFileHeaders(checkFile, fileConfig);
            validationResult.BadDataOriginalHeaders = validationResult.HeadersOriginal.DeepClone();
            validationResult.ImportFile = checkFile;
            validationResult.TotalRowCount = fileTotalRowCount;
            validationResult.HasError = false;
            validationResult.RecordImportErrors = new HashSet<ImportError>();

            SetText(currentOperation, "Get client Pubcodes");
            SetText(overallProgress, "10");
            clientPubCodes = dbWorker.Proxy.GetPubIDAndCodesByClient(accessKey, myClient).Result;
            return fileTotalRowCount;
        }

        private void LoadVariablesAndContainers(
            out List<string> finalUnexpected,
            out List<FieldMapping> finalNotFound,
            out FileConfiguration fileConfig)
        {
            foreach (var fieldMapping in sourceFile.FieldMappings)
            {
                if (fieldMapping.IsNonFileColumn)
                {
                    fieldMapping.IncomingField = fieldMapping.MAFField;
                }

                fieldMapping.IncomingField = fieldMapping.IncomingField.ToUpper();
                fieldMapping.MAFField = fieldMapping.MAFField.ToUpper();
            }

            var codes = codeWorker.Proxy
                .Select(accessKey, Enums.CodeType.Field_Mapping)
                .Result;
            if (codes != null)
            {
                standarTypeID = codes.Single(x => x.CodeName.Equals(Enums.FieldMappingTypes.Standard.ToString())).CodeId;
                demoTypeID = codes.Single(x => x.CodeName.Equals(Enums.FieldMappingTypes.Demographic.ToString())).CodeId;
                ignoredTypeID = codes.Single(x =>x.CodeName.Equals(Enums.FieldMappingTypes.Ignored.ToString())).CodeId;
                demoRespOtherTypeID = codes.Single(x =>x.CodeName.Equals(Enums.FieldMappingTypes.Demographic_Other.ToString().Replace('_', ' '))).CodeId;
            }

            isValidFileType = true;
            isFileSchemaValid = true;
            IsKnownCustomerFileName = true;
            finalUnexpected = new List<string>();
            finalNotFound = new List<FieldMapping>();

            fileConfig = new FileConfiguration
            {
                FileColumnDelimiter = sourceFile.Delimiter,
                IsQuoteEncapsulated = sourceFile.IsTextQualifier
            };
        }

        private bool ValidateFileTextQualifier(FileWorker fileWorker)
        {
            SetText(currentOperation, "Check that configured TextQualifier matches actual file.");
            SetText(currentProgress, "0");
            SetText(overallProgress, "3");

            var rejectFile = false;
            var first = fileWorker.GetFirstCharacter(checkFile);
            SetText(currentProgress, "90");

            if (!IsExcel(checkFile.Extension))
            {
                var isFirstQuote = first == '"';

                if (isFirstQuote != sourceFile.IsTextQualifier)
                {
                    rejectFile = true;
                    var reason = string.Empty;
                    if (isFirstQuote == false && sourceFile.IsTextQualifier)
                    {
                        reason = "Your file is configured to be quote encapsulated but the actual file is not quote encapsulated.  Please either quote encapsulate your file or adjust the setup configuration.";
                    }
                    else if (isFirstQuote && sourceFile.IsTextQualifier == false)
                    {
                        reason = "Your file is quote encapsulated but the configuration setup is set to not quote encapsulated.  Please either remove quote encapsulation from your file or adjust the setup configuration.";
                    }

                    ErrorMessages.Add(reason);
                    SetText(currentOperation, reason);
                }
            }

            SetText(currentProgress, "100");
            SetText(overallProgress, "5");
            return rejectFile;
        }

        private bool IsExcel(string extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException(nameof(extension));
            }

            return extension.Equals(".xlsx", StringComparison.CurrentCultureIgnoreCase) ||
                   extension.Equals(".xls", StringComparison.CurrentCultureIgnoreCase);
        }

        private void GetServiceForFile(ServiceClient<IService> servWorker, ServiceClient<IServiceFeature> servFeatureWorker)
        {
            SetText(currentOperation, "Check which Service the file is for.");
            SetText(currentProgress, "0");
            SetText(overallProgress, "2");
            service = servWorker.Proxy.Select(accessKey, sourceFile.ServiceID).Result;
            serviceFeature = servFeatureWorker.Proxy.SelectServiceFeature(accessKey, sourceFile.ServiceFeatureID).Result;
            SetText(currentProgress, "100");
        }

        private void InitializeValidator(FileInfo myCheckFile, Client myClient, SourceFile mySourceFile)
        {
            accessKey = AppData.myAppData.AuthorizedUser.AuthAccessKey;
            if (mySourceFile.FieldMappings.Count == 0)
            {
                var sfWorker = ServiceClient.UAS_SourceFileClient();
                var sfResp = sfWorker.Proxy.SelectForSourceFile(accessKey, mySourceFile.SourceFileID, true);
                if (sfResp.Result != null)
                {
                    mySourceFile = sfResp.Result;
                }
            }

            sourceFile = mySourceFile.DeepClone();
            client = myClient;
            checkFile = myCheckFile;
            dataIV = new ImportFile();
            processCode = $"FileValidator_{StringFunctions.GenerateProcessCode()}";
            validationResult = new ValidationResult(checkFile, sourceFile.SourceFileID, processCode);
            listIES = new List<ImportErrorSummary>();
            ErrorMessages = new List<string>();
            isValidFileType = false;
            isFileSchemaValid = false;
            IsKnownCustomerFileName = false;
        }
    }
}