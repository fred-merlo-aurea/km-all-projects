using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Core.ADMS;
using Core_AMS.Utilities;
using FrameworkUAD.BusinessLogic.Helpers;
using FrameworkUAD.BusinessLogic.Transformations;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using FrameworkUAS.Entity;
using KM.Common;
using KM.Common.Functions;
using KM.Common.Import;
using KM.Common.Utilities.Email;
using FileFunctions = Core_AMS.Utilities.FileFunctions;

namespace AMS_Operations
{
    public class FileValidator : FileValidatorBase
    {
        #region class variables        
        Dictionary<int, string> clientPubCodes;
        KMPlatform.Entity.Service service;        
        KMPlatform.Entity.Client client;
        FileInfo checkFile;
       
        FrameworkUAD.Object.ValidationResult validationResult;
        List<FrameworkUAD.Object.ImportErrorSummary> listIES;
        FrameworkUAD_Lookup.Entity.Code dbFileType;
        //Guid accessKey;

        bool isValidFileType;
        bool isFileSchemaValid;
        private bool IsKnownCustomerFileName;

        FrameworkUAD_Lookup.BusinessLogic.Code codeWorker;

        #endregion

        public Dictionary<string, string> ValidateFileAsObject(FileInfo myCheckFile, KMPlatform.Entity.Client myClient, SourceFile mySourceFile)
        {
            //accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;

            if(mySourceFile.FieldMappings.Count == 0)
            {
                FrameworkUAS.BusinessLogic.SourceFile sfWorker = new FrameworkUAS.BusinessLogic.SourceFile();
                FrameworkUAS.Entity.SourceFile sfResp = new FrameworkUAS.Entity.SourceFile();
                sfResp = sfWorker.SelectSourceFileID(mySourceFile.SourceFileID, true);
                if (sfResp != null)
                    mySourceFile = sfResp;
            }

            sourceFile = mySourceFile;
            client = myClient;
            checkFile = myCheckFile;
            dataIV = new FrameworkUAD.Object.ImportFile();
            processCode = "FileValidator_" + Core_AMS.Utilities.StringFunctions.GenerateProcessCode();
            validationResult = new FrameworkUAD.Object.ValidationResult(checkFile, sourceFile.SourceFileID, processCode);
            listIES = new List<FrameworkUAD.Object.ImportErrorSummary>();
            ErrorMessages = new List<string>();
            isValidFileType = false;
            isFileSchemaValid = false;
            IsKnownCustomerFileName = false;

            string extension = checkFile.Extension;
            string path = Core.ADMS.BaseDirs.getAppsDir() + "\\DQM\\";
            string rawFileName = System.IO.Path.GetFileNameWithoutExtension(checkFile.Name);
            if (extension.Equals(".xlsx", StringComparison.CurrentCultureIgnoreCase) || extension.Equals(".xls", StringComparison.CurrentCultureIgnoreCase))
                extension = ".csv";

            string tranFileName = "Transformed_" + rawFileName + extension;

            string tranFile = path + tranFileName;
            if (File.Exists(tranFile))
                File.Delete(tranFile);

            string origFileName = "Original_" + rawFileName + extension;
            string origFile = path + origFileName;
            if (File.Exists(origFile))
                File.Delete(origFile);

            Dictionary<string, string> downloadFiles = new Dictionary<string, string>();
            codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            FrameworkUAS.Object.DBWorker dbWorker = new FrameworkUAS.Object.DBWorker();
            FrameworkUAD.BusinessLogic.SubscriberTransformed stWorker = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
            FrameworkUAD.BusinessLogic.FileValidator_ImportError fvIeWorker = new FrameworkUAD.BusinessLogic.FileValidator_ImportError();
            KMPlatform.BusinessLogic.Service servWorker = new KMPlatform.BusinessLogic.Service();
            KMPlatform.BusinessLogic.ServiceFeature servFeatureWorker = new KMPlatform.BusinessLogic.ServiceFeature();
            FrameworkUAD.BusinessLogic.CodeSheet codeSheetWorker = new FrameworkUAD.BusinessLogic.CodeSheet();
            FrameworkUAD.BusinessLogic.Operations operationsWorker = new FrameworkUAD.BusinessLogic.Operations();
            FrameworkUAD.BusinessLogic.ImportErrorSummary ieSummaryWorker = new FrameworkUAD.BusinessLogic.ImportErrorSummary();
            Console.WriteLine("Workers created");
            try
            {
                #region check which Service the file is for
                service = servWorker.Select(sourceFile.ServiceID);
                //get which feature the file is for
                serviceFeature = servFeatureWorker.SelectServiceFeature(sourceFile.ServiceFeatureID);
                #endregion

                FileWorker fileWorker = new FileWorker();
                bool rejectFile = false;
                #region check file TextQualifier matches actual file
                //this will be a simple check - if SourceFile.IsTextQualifier is TRUE then the very first character in the file should be a quotation if not then Reject the file
                //first lets get the very first character in the file
                char first = fileWorker.GetFirstCharacter(checkFile);
                //if not an excel file check for quote encapsulation
                if (!checkFile.Extension.Equals(".xlsx", StringComparison.CurrentCultureIgnoreCase) && !checkFile.Extension.Equals(".xls", StringComparison.CurrentCultureIgnoreCase))
                {
                    bool isFirstQuote = false;
                    if (first == '"')
                        isFirstQuote = true;

                    if (isFirstQuote != sourceFile.IsTextQualifier)
                    {
                        rejectFile = true;
                        //reject file because setup and file do not match
                        string reason = string.Empty;
                        if (isFirstQuote == false && sourceFile.IsTextQualifier == true)
                        {
                            reason = "Your file is configured to be quote encapsulated but the actual file is not quote encapsulated.  Please either quote encapsulate your file or adjust the setup configuration.";
                        }
                        else if (isFirstQuote == true && sourceFile.IsTextQualifier == false)
                        {
                            reason = "Your file is quote encapsulated but the configuration setup is set to not quote encapsulated.  Please either remove quote encapsulation from your file or adjust the setup configuration.";
                        }
                        ErrorMessages.Add(reason);
                    }
                }
                #endregion

                if (rejectFile == false)
                {
                    //rename any nonFile columns
                    foreach (var x in sourceFile.FieldMappings)
                    {
                        if (x.IsNonFileColumn == true)
                            x.IncomingField = x.MAFField;

                        x.IncomingField = x.IncomingField.ToUpper();
                        x.MAFField = x.MAFField.ToUpper();
                    }

                    #region Method variables and containers
                    List<FrameworkUAD_Lookup.Entity.Code> codes = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Field_Mapping);
                    if (codes != null)
                    {
                        standarTypeID = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString())).CodeId;
                        demoTypeID = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString())).CodeId;
                        ignoredTypeID = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString())).CodeId;
                        demoRespOtherTypeID = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Other.ToString().Replace('_', ' '))).CodeId;
                        kmTransformTypeId = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.kmTransform.ToString())).CodeId;
                    }

                    bool IsPubCodeMissing = false;
                    isValidFileType = true;
                    isFileSchemaValid = true;
                    IsKnownCustomerFileName = true;
                    int fileTotalRowCount = 0;

                    //Lists for Unexpected and NotFound data
                    List<string> finalUnexpected = new List<string>();
                    List<FrameworkUAS.Entity.FieldMapping> finalNotFound = new List<FrameworkUAS.Entity.FieldMapping>();

                    FileConfiguration fileConfig = new FileConfiguration()
                    {
                        FileColumnDelimiter = sourceFile.Delimiter,
                        IsQuoteEncapsulated = sourceFile.IsTextQualifier,
                    };
                    #endregion

                    #region Process File
                    bool dateParsingFailure = false;
                    if (fileWorker.AcceptableFileType(checkFile) == false)
                    {
                        isValidFileType = false;
                        ErrorMessages.Add("File is not an acceptable type for processing.");
                    }
                    else
                    {

                        #region data processing preparation
                        fileTotalRowCount = fileWorker.GetRowCount(checkFile);
                        validationResult.HeadersOriginal = fileWorker.GetFileHeaders(checkFile, fileConfig);
                        validationResult.BadDataOriginalHeaders = validationResult.HeadersOriginal.DeepClone();
                        validationResult.ImportFile = checkFile;
                        validationResult.TotalRowCount = fileTotalRowCount;
                        validationResult.HasError = false;
                        validationResult.RecordImportErrors = new HashSet<ImportError>();

                        clientPubCodes = FrameworkUAS.Object.DBWorker.GetPubIDAndCodesByClient(myClient);
                        #endregion

                        #region Check: Ensure PUBCODE column is mapped
                        dbFileType = codeWorker.SelectCodeId(sourceFile.DatabaseFileTypeId);

                        var mappedColumns = (from c in sourceFile.FieldMappings
                                             select c.MAFField.ToUpper()).ToList();

                        if (!mappedColumns.Contains("PUBCODE"))
                        {
                            string msg = "File requires a PUBCODE column";
                            IsPubCodeMissing = true;
                            ErrorMessages.Add(msg);
                            ValidationError(msg);
                        }
                        #endregion

                        if (fileTotalRowCount > 0)
                        {
                            //stWorker.DisableIndexes(myClient.ClientConnections);
                            if (IsPubCodeMissing == false)
                            {
                                #region Get Data and handle SpecialFile
                                //loads the data from the file
                                FrameworkUAD.BusinessLogic.ImportFile ifWorker = new FrameworkUAD.BusinessLogic.ImportFile();
                                dataIV = ifWorker.GetImportFile(checkFile, fileConfig);
                                #endregion

                                #region set ImportFile properties
                                dataIV.ClientId = myClient.ClientID;
                                dataIV.SourceFileId = sourceFile.SourceFileID;
                                dataIV.ProcessCode = processCode;
                                #endregion
                                #region Insert Import Errors to SubscriberInvalid
                                if (dataIV.ImportErrorCount > 0)
                                {
                                    fvIeWorker.SaveBulkSqlInsert(dataIV.ImportErrors.ToList(), myClient.ClientConnections);
                                    validationResult.RecordImportErrorCount = dataIV.ImportErrorCount;
                                    validationResult.RecordImportErrors = dataIV.ImportErrors;
                                }
                                #endregion
                                #region Process data
                                if (dataIV.TotalRowCount > 0)
                                {
                                    #region Transformations
                                    try
                                    {
                                        var _transformationWorker = new TransformationWorker(dataIV, ValidationError, ErrorMessages, client, sourceFile, codeWorker, LogException, serviceFeature, clientPubCodes);
                                        _transformationWorker.TransformImportFileData();
                                    }
                                    catch (Exception ex)
                                    {
                                        string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                                        StringBuilder sbDetail = new StringBuilder();
                                        sbDetail.AppendLine("DQM.Helpers.Validation.FileValidator.ValidateFileAsObject - Transform Data");
                                        sbDetail.AppendLine(System.Environment.NewLine);

                                        sbDetail.AppendLine(message);
                                        ErrorMessages.Add(sbDetail.ToString());
                                        ValidationError(sbDetail.ToString());
                                    }
                                    #endregion

                                    //not doing column renaming
                                    #region Column Validation
                                    List<FrameworkUAD_Lookup.Entity.Code> fieldMappingTypeCodes = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Field_Mapping);

                                    #region need to add any CreatedDimension values from AdHocDimensionGroup to the FieldMapping list if they do not exist
                                    int fieldMappingTypeID = fieldMappingTypeCodes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString())).CodeId;

                                    FrameworkUAS.BusinessLogic.AdHocDimensionGroup ahdgWorker = new FrameworkUAS.BusinessLogic.AdHocDimensionGroup();
                                    List<FrameworkUAS.Entity.AdHocDimensionGroup> ahdgResp = new List<AdHocDimensionGroup>();
                                    ahdgResp = ahdgWorker.Select(myClient.ClientID, true);
                                    List<FrameworkUAS.Entity.AdHocDimensionGroup> ahdGroups = new List<AdHocDimensionGroup>();
                                    if(ahdgResp != null)
                                        ahdGroups = ahdgResp.Where(x => x.IsActive == true).ToList();

                                    if (ahdGroups != null && ahdGroups.Count > 0)
                                    {
                                        List<string> distCreatedDims = new List<string>();
                                        foreach (FrameworkUAS.Entity.AdHocDimensionGroup adg in ahdGroups)
                                        {
                                            if (!distCreatedDims.Contains(adg.CreatedDimension))
                                                distCreatedDims.Add(adg.CreatedDimension);
                                        }

                                        foreach (string s in distCreatedDims)
                                        {
                                            if (sourceFile.FieldMappings.Count(x => x.MAFField.Equals(s, StringComparison.CurrentCultureIgnoreCase)) == 0)
                                            {
                                                FieldMapping dimFM = new FieldMapping();
                                                dimFM.FieldMappingTypeID = fieldMappingTypeID;
                                                dimFM.IsNonFileColumn = true;
                                                dimFM.SourceFileID = sourceFile.SourceFileID;
                                                dimFM.IncomingField = s;
                                                dimFM.MAFField = s;
                                                dimFM.PubNumber = 0;
                                                dimFM.DataType = "varchar";
                                                dimFM.PreviewData = string.Empty;
                                                dimFM.HasMultiMapping = false;
                                                dimFM.CreatedByUserID = 1;
                                                dimFM.ColumnOrder = sourceFile.FieldMappings.Count + 1;
                                                dimFM.DateCreated = DateTime.Now;

                                                sourceFile.FieldMappings.Add(dimFM);
                                                if (!sourceFile.FieldMappings.Contains(dimFM))
                                                    sourceFile.FieldMappings.Add(dimFM);
                                                if (!dataIV.HeadersTransformed.ContainsKey(s))
                                                    dataIV.HeadersTransformed.Add(s, (dataIV.HeadersTransformed.Count + 1).ToString());
                                            }
                                        }

                                    }
                                    ahdGroups = null;
                                    #endregion

                                    #region lets drop any "ignored" columns
                                    int ignoreId = fieldMappingTypeCodes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString())).CodeId;
                                    List<FrameworkUAS.Entity.FieldMapping> deleteIgnoredFieldMappings = (from i in sourceFile.FieldMappings
                                                                                                         where i.FieldMappingTypeID == ignoreId
                                                                                                         select i).ToList();
                                    foreach (FrameworkUAS.Entity.FieldMapping del in deleteIgnoredFieldMappings)
                                    {
                                        sourceFile.FieldMappings.Remove(del);
                                        if (dataIV.HeadersOriginal.ContainsKey(del.IncomingField))
                                        {
                                            dataIV.HeadersOriginal.Remove(del.IncomingField);
                                            foreach (var key in dataIV.DataOriginal.Keys)
                                            {
                                                StringDictionary myRow = dataIV.DataOriginal[key];
                                                myRow.Remove(del.IncomingField);
                                            }
                                        }
                                        if (dataIV.HeadersTransformed.ContainsKey(del.IncomingField))
                                        {
                                            dataIV.HeadersTransformed.Remove(del.IncomingField);
                                            foreach (var key in dataIV.DataTransformed.Keys)
                                            {
                                                StringDictionary myRow = dataIV.DataTransformed[key];
                                                myRow.Remove(del.IncomingField);
                                            }
                                        }
                                    }
                                    #endregion

                                    #region Not found check
                                    foreach (FrameworkUAS.Entity.FieldMapping map in sourceFile.FieldMappings)
                                    {
                                        if (!dataIV.HeadersTransformed.ContainsKey(map.IncomingField))
                                            finalNotFound.Add(map);
                                    }
                                    //If finalNotFound.Count > 0 log that a mapped column was missing from the file but continue processing
                                    if (finalNotFound.Count > 0)
                                    {
                                        validationResult.HasError = true;
                                        validationResult.RecordImportErrorCount++;
                                        foreach (FrameworkUAS.Entity.FieldMapping nf in finalNotFound)
                                            validationResult.NotFoundColumns.Add(nf.IncomingField);
                                        string notFoundError = "VALIDATOR: Not Found Columns - " + String.Join(", ", validationResult.NotFoundColumns) + "<br/>";
                                        ImportError ie = new ImportError(-1, processCode, sourceFile.SourceFileID, notFoundError);
                                        validationResult.RecordImportErrors.Add(ie);
                                        ErrorMessages.Add(notFoundError);

                                        //remove the column from FieldMapping
                                        foreach (FrameworkUAS.Entity.FieldMapping nf in finalNotFound)
                                            sourceFile.FieldMappings.Remove(nf);
                                    }
                                    #endregion
                                    #region Check: Undefined Columns - Unexpected
                                    foreach (var col in dataIV.HeadersTransformed.Keys)
                                    {
                                        if (!col.ToString().Equals("OriginalImportRow", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            if (sourceFile.FieldMappings.Count(x => x.IncomingField.Equals(col.ToString(), StringComparison.CurrentCultureIgnoreCase)) == 0)
                                                finalUnexpected.Add(col.ToString());
                                        }
                                    }

                                    if (finalUnexpected.Count > 0)
                                    {
                                        validationResult.HasError = true;
                                        validationResult.RecordImportErrorCount++;
                                        //validationResult.UnexpectedColumns.AddRange(finalUnexpected);
                                        finalUnexpected.ToList().ForEach(x => { validationResult.UnexpectedColumns.Add(x); });
                                        StringBuilder sb = new StringBuilder();
                                        sb.AppendLine("One or more unmapped columns were found in your import file: " + checkFile.Name + "<br/>");
                                        sb.AppendLine("The columns have been excluded.<br/>");
                                        sb.AppendLine("If these columns contain data that is required for processing or you intent to have in your UAD,");
                                        sb.Append(" please add these columns to your file mapping and resubmit for processing.<br/>");
                                        sb.AppendLine("Unexpected Columns - " + String.Join(", ", finalUnexpected) + "<br/>");
                                        ImportError ie = new ImportError(-1, processCode, sourceFile.SourceFileID, sb.ToString());
                                        validationResult.RecordImportErrors.Add(ie);
                                        ErrorMessages.Add(sb.ToString());

                                        //remove the column from the DataTransformed Headers and Data
                                        foreach (string u in finalUnexpected)
                                        {
                                            dataIV.HeadersTransformed.Remove(u);
                                            foreach (var dt in dataIV.DataTransformed.Keys)
                                            {
                                                StringDictionary myRow = dataIV.DataTransformed[dt];
                                                myRow.Remove(u);
                                            }
                                        }
                                    }

                                    #endregion
                                    #endregion

                                    #region Apply any AdHocDimensions
                                    ApplyAdHocDimensions();
                                    #endregion

                                    #region QDate Format
                                    string qDateColumnHeader = string.Empty;
                                    FieldMapping thisFM = sourceFile.FieldMappings.SingleOrDefault(x => x.MAFField.Equals(Core_AMS.Utilities.Enums.MAFFieldStandardFields.QUALIFICATIONDATE.ToString(), StringComparison.CurrentCultureIgnoreCase));
                                    if (thisFM != null && dataIV.HeadersTransformed.ContainsKey(thisFM.MAFField))
                                        qDateColumnHeader = thisFM.MAFField;

                                    thisFM = null;
                                    if (!string.IsNullOrEmpty(qDateColumnHeader))
                                    {
                                        string QDatePattern2 = sourceFile.QDateFormat.Replace('D', 'd').Replace('Y', 'y');
                                        foreach (var key in dataIV.DataTransformed.Keys)
                                        {
                                            StringDictionary myRow = dataIV.DataTransformed[key];
                                            if (!string.IsNullOrEmpty(myRow[qDateColumnHeader]))
                                            {
                                                //Get QDateValue and then remove all characters not numeric for comparison.
                                                string QDateValue = myRow[qDateColumnHeader].ToString();
                                                QDateValue = new string(QDateValue.Where(c => char.IsDigit(c)).ToArray());
                                                if (string.IsNullOrEmpty(QDateValue))
                                                    myRow[qDateColumnHeader] = DateTime.Now.ToShortDateString();
                                                else
                                                {
                                                    try
                                                    {
                                                        ValidatorMethods.TryParseDateColumnHeader(
                                                            myRow, sourceFile.QDateFormat, qDateColumnHeader, QDatePattern2, QDateValue);
                                                    }
                                                    catch (Exception)
                                                    {
                                                        dateParsingFailure = true;
                                                        myRow[qDateColumnHeader] = DateTime.Now.ToShortDateString();
                                                    }
                                                }
                                            }
                                            else
                                                myRow[qDateColumnHeader] = DateTime.Now.ToShortDateString();
                                        }
                                    }
                                    #endregion

                                    //if this is a circ file and we had Qdate parsing failure need to reject file and stop processing
                                    if (dateParsingFailure == true)// && FrameworkUAD_Lookup.Enums.GetService(service.ServiceName) == KMPlatform.Enums.Services.FULFILLMENT)
                                    {
                                        string reason = string.Empty;
                                        reason = "Your file is configured to have a QDate format of " + sourceFile.QDateFormat + " but your QDate data does not match that format.  Please either correctly format your QDate file data or adjust the setup configuration.";
                                        ErrorMessages.Add(reason);
                                    }
                                    else
                                    {
                                        dateParsingFailure = false;//just to be safe reset back to false
                                        ValidateData();

                                        if (dataIV.HasError == true)
                                            validationResult.HasError = true;

                                        validationResult.OriginalRowCount = dataIV.OriginalRowCount;
                                        validationResult.TransformedRowCount = dataIV.TransformedRowCount;
                                        validationResult.RecordImportErrorCount = dataIV.ImportErrorCount;
                                        validationResult.ImportedRowCount = dataIV.ImportedRowCount;

                                        if (dataIV.ImportErrorCount > 0 && dataIV.ImportErrors != null)
                                        {
                                            try
                                            {
                                                //validationResult.RecordImportErrors.AddRange(dataIV.ImportErrors);
                                                dataIV.ImportErrors.ToList().ForEach(x => { validationResult.RecordImportErrors.Add(x); });
                                            }
                                            catch (Exception ex)
                                            {
                                                string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                                                ErrorMessages.Add(message);
                                            }
                                        }
                                    }
                                #endregion
                                }
                                else
                                {
                                    ValidationError("No data from file, stopping processing.");
                                    ErrorMessages.Add("No data from file, stopping processing.");
                                }

                                #region Codesheet validation
                                if (KMPlatform.BusinessLogic.Enums.GetUADFeature(serviceFeature.SFName) != KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
                                {
                                    try
                                    {
                                        codeSheetWorker.CodeSheetValidation(sourceFile.SourceFileID, processCode, myClient.ClientConnections);
                                    }
                                    catch (Exception ex)
                                    {
                                        string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                                        StringBuilder sbDetail = new StringBuilder();
                                        sbDetail.AppendLine("DQM.Helpers.Validation.FileValidator.ProcessFileAsObject - Codesheet Validation");
                                        sbDetail.AppendLine(System.Environment.NewLine);
                                        sbDetail.AppendLine(message);
                                        ErrorMessages.Add(message.ToString());
                                    }
                                }
                                #endregion

                                #region QSource validation
                                if (KMPlatform.BusinessLogic.Enums.GetUADFeature(serviceFeature.SFName) != KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
                                {
                                    try
                                    {
                                        operationsWorker.QSourceValidation(myClient.ClientConnections, sourceFile.SourceFileID, processCode);
                                    }
                                    catch (Exception ex)
                                    {
                                        string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                                        StringBuilder sbDetail = new StringBuilder();
                                        sbDetail.AppendLine("DQM.Helpers.Validation.FileValidator.ProcessFileAsObject - QSource Validation");
                                        sbDetail.AppendLine(System.Environment.NewLine);
                                        sbDetail.AppendLine(message);
                                        ErrorMessages.Add(message.ToString());
                                    }
                                }
                                #endregion

                                #region Select Demographic ImportErrors
                                try
                                {
                                    //get any error info from CSV - will be written to table ImportError
                                    //add errors to allDataIV and remove bad rows from allDataIV
                                    List<FrameworkUAD.Object.ImportErrorSummary> ieSummaryResp = new List<ImportErrorSummary>();
                                    ieSummaryResp = ieSummaryWorker.Select(sourceFile.SourceFileID, processCode, myClient.ClientConnections);
                                    if (ieSummaryResp != null)
                                    {
                                        listIES = ieSummaryResp;
                                        int errorTotal = listIES.Sum(x => x.ErrorCount);
                                        if (listIES.Count > 0)
                                        {
                                            validationResult.HasError = true;
                                            validationResult.DimensionImportErrorCount += errorTotal;
                                        }
                                        foreach (FrameworkUAD.Object.ImportErrorSummary ies in listIES)
                                            validationResult.DimensionImportErrorSummaries.Add(ies);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                                    StringBuilder sbDetail = new StringBuilder();
                                    sbDetail.AppendLine("DQM.Helpers.Validation.FileValidator.ProcessFileAsObject - Select ImportErrors");
                                    sbDetail.AppendLine(System.Environment.NewLine);
                                    ErrorMessages.Add(message.ToString());
                                }
                                #endregion
                            }
                            else
                            {
                                #region pub code is missing
                                validationResult.OriginalRowCount = 0;
                                validationResult.TransformedRowCount = 0;
                                validationResult.TotalRowCount = 0;

                                validationResult.HasError = true;
                                validationResult.RecordImportErrorCount++;

                                ImportError ie = new ImportError(-1, processCode, sourceFile.SourceFileID, "File requires a PUBCODE column");
                                validationResult.RecordImportErrors.Add(ie);

                                isFileSchemaValid = false;
                                #endregion
                            }
                        }
                        else
                        {
                            #region File has no rows
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("Attention: " + myClient.FtpFolder);
                            sb.AppendLine("We were unable to read this file.");
                            sb.AppendLine("File: " + checkFile.Name);
                            sb.AppendLine("Make sure that the file name does not include invalid characters or punctuation and that it is not too long.");
                            ErrorMessages.Add(sb.ToString());

                            validationResult.OriginalRowCount = 0;
                            validationResult.TransformedRowCount = 0;
                            validationResult.TotalRowCount = 0;

                            validationResult.HasError = true;
                            validationResult.RecordImportErrorCount++;

                            ImportError ie = new ImportError(-1, processCode, sourceFile.SourceFileID, sb.ToString());
                            validationResult.RecordImportErrors.Add(ie);

                            isFileSchemaValid = false;
                            #endregion
                        }

                        mappedColumns = null;
                    }
                    #endregion

                    validationResult.OriginalDuplicateRecordCount = dataIV.OriginalDuplicateRecordCount;
                    validationResult.TransformedDuplicateRecordCount = dataIV.TransformedDuplicateRecordCount;
                }
                else
                {
                    //was set text
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ErrorMessages.Add(Core_AMS.Utilities.StringFunctions.FormatException(ex));
            }


            #region Create Download Files
            try
            {
                Console.WriteLine("create download files");
                FrameworkUAD.BusinessLogic.ValidationResult vrWorker = new FrameworkUAD.BusinessLogic.ValidationResult();

                //Used to send one email per request
                StringBuilder reportBody = GetReportBodyForSingleEmail(IsKnownCustomerFileName, isValidFileType, isFileSchemaValid, validationResult);
                string badDataAttachment = string.Empty;

                if (validationResult.RecordImportErrorCount > 0)
                    badDataAttachment = vrWorker.GetBadData(validationResult, sourceFile);


                Core_AMS.Utilities.FileFunctions ff = new FileFunctions();
                string detailFile = path + sourceFile.FileName + "_Details.html";
                string errorFile = path + sourceFile.FileName + "_ErrorMessages.txt";
                string badDataFile = path + sourceFile.FileName + "_BadData_" + System.IO.Path.GetFileNameWithoutExtension(checkFile.Name) + ".csv";

                if (File.Exists(detailFile))
                    File.Delete(detailFile);
                if (File.Exists(errorFile))
                    File.Delete(errorFile);
                if (File.Exists(badDataFile))
                    File.Delete(badDataFile);

                ff.CreateFile(detailFile, reportBody.ToString());
                downloadFiles.Add("Details", detailFile);

                if (ErrorMessages.Count > 0)
                {
                    StringBuilder sbErrorMessages = new StringBuilder();
                    ErrorMessages.ForEach(x => sbErrorMessages.AppendLine(x));
                    ff.CreateFile(errorFile, sbErrorMessages.ToString());
                    downloadFiles.Add("Errors", errorFile);
                }

                if (!string.IsNullOrEmpty(badDataAttachment))
                {
                    ff.CreateFile(badDataFile, badDataAttachment);
                    downloadFiles.Add("BadData", badDataFile);
                }

                MailPriority mp = MailPriority.Normal;
                if (validationResult.HasError == true || validationResult.DimensionImportErrorCount > 0 || validationResult.RecordImportErrorCount > 0 || validationResult.UnexpectedColumns.Count > 0 || validationResult.NotFoundColumns.Count > 0 || validationResult.DuplicateColumns.Count > 0)
                    mp = MailPriority.High;
                Console.WriteLine("Send email");
                SendEmail(GetMailMessage(myClient, reportBody.ToString(), myCheckFile, isValidFileType, isFileSchemaValid, "text/html", processCode, sourceFile.SourceFileID, badDataAttachment, mp, true, false), myClient);


                //reset variables
                dataIV = null;
                validationResult = null;
                listIES = null;
                sourceFile = null;
            }
            catch (Exception ex){
                Console.WriteLine(ex);
            }
            #endregion
            GC.Collect();
            return downloadFiles;
        }

        #region Processing Methods
        private void ValidateData()
        {
            bool isDataCompare = false;
            if (KMPlatform.BusinessLogic.Enums.GetUADFeature(serviceFeature.SFName) == KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
                isDataCompare = true;

            //create original Subscribers
            List<FrameworkUAD.Entity.SubscriberOriginal> listSubscriberOriginal = new List<SubscriberOriginal>();
            if (sourceFile.IsDQMReady == true)
                listSubscriberOriginal = InsertOriginalSubscribers();

            Dictionary<int, Guid> SubOriginalDictionary = new Dictionary<int, Guid>();
            foreach (SubscriberOriginal so in listSubscriberOriginal)
            {
                SubOriginalDictionary.Add(so.ImportRowNumber, so.SORecordIdentifier);
            }

            #region DATA ROW Validation
            List<SubscriberTransformed> listSubscriberTransformed = new List<SubscriberTransformed>();
            List<SubscriberInvalid> listSubscriberInvalid = new List<SubscriberInvalid>();
            List<SubscriberTransformed> listValidST = new List<SubscriberTransformed>();
            List<SubscriberTransformed> listInvalidValidST = new List<SubscriberTransformed>();
            if (listSubscriberOriginal.Count > 0)
            {
                FrameworkUAS.BusinessLogic.Transformation transformationWorker = new FrameworkUAS.BusinessLogic.Transformation();
                List<FrameworkUAS.Entity.Transformation> respTrans = new List<Transformation>();
                respTrans = transformationWorker.Select(client.ClientID, sourceFile.SourceFileID, true);
                if (respTrans != null)
                {
                    List<FrameworkUAS.Entity.Transformation> allTransformations = respTrans.Where(x => x.IsActive == true).ToList();

                    FrameworkUAS.BusinessLogic.TransformSplit transformSplitWorker = new FrameworkUAS.BusinessLogic.TransformSplit();
                    List<FrameworkUAS.Entity.TransformSplit> respTransformSplit = new List<TransformSplit>();
                    respTransformSplit = transformSplitWorker.SelectSourceFileID(sourceFile.SourceFileID);
                    List<FrameworkUAS.Entity.TransformSplit> allTransformSplit = new List<TransformSplit>();
                    if (respTransformSplit != null)
                    {
                        allTransformSplit = respTransformSplit.Where(x => x.IsActive == true).ToList();
                    }

                    FrameworkUAS.BusinessLogic.AdHocDimensionGroup ahdgWorker = new FrameworkUAS.BusinessLogic.AdHocDimensionGroup();
                    List<FrameworkUAS.Entity.AdHocDimensionGroup> ahdgResp = new List<AdHocDimensionGroup>();
                    ahdgResp = ahdgWorker.Select(client.ClientID, false);
                    List<FrameworkUAS.Entity.AdHocDimensionGroup> ahdGroups = new List<AdHocDimensionGroup>();
                    if (ahdgResp != null)
                    {
                        ahdGroups = ahdgResp.Where(x => x.IsActive == true).OrderBy(y => y.OrderOfOperation).ToList();
                    }


                    List<FrameworkUAD_Lookup.Entity.Code> transTypes = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Transformation);
                    List<FrameworkUAS.Entity.TransformationFieldMap> splitTranFieldMappings = new List<TransformationFieldMap>();
                    List<FrameworkUAS.Entity.Transformation> splitTrans = new List<Transformation>();
                    if (transTypes != null)
                    {
                        int splitIntoRowsId = transTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.TransformationTypes.Split_Into_Rows.ToString().Replace("_", " "))).CodeId;
                        if (allTransformations.Exists(x => x.TransformationTypeID == splitIntoRowsId))
                            splitTrans = allTransformations.Where(x => x.TransformationTypeID == splitIntoRowsId && x.IsActive == true).ToList();
                        if (splitTrans != null)
                        {
                            foreach (FrameworkUAS.Entity.Transformation t in splitTrans)
                            {
                                foreach (FrameworkUAS.Entity.TransformationFieldMap tfm in t.FieldMap.Where(x => x.SourceFileID == sourceFile.SourceFileID))
                                {
                                    FrameworkUAS.Entity.TransformationFieldMap thisTFM = splitTranFieldMappings.FirstOrDefault(x => x.TransformationFieldMapID == tfm.TransformationFieldMapID);
                                    if (thisTFM == null)
                                        splitTranFieldMappings.Add(tfm);
                                }
                            }
                        }
                    }

                    FrameworkUAS.Entity.FieldMapping fm = sourceFile.FieldMappings.SingleOrDefault(x => x.MAFField.Equals("PubCode", StringComparison.CurrentCultureIgnoreCase));
                    foreach (var key in dataIV.DataTransformed.Keys)
                    {
                        StringDictionary myRow = dataIV.DataTransformed[key];
                        string msg = "Creating Transformed Subscriber: " + key.ToString() + " of " + dataIV.DataTransformed.Count.ToString();
                        Core_AMS.Utilities.StringFunctions.WriteLineRepeater(msg, ConsoleColor.Green);
                        //bool isRowValid = true;
                        int pubCodeID = -1;
                        if (KMPlatform.BusinessLogic.Enums.GetUADFeature(serviceFeature.SFName) != KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
                        {
                            if (fm != null && myRow[fm.IncomingField] != null)
                            {
                                if (!string.IsNullOrEmpty(myRow[fm.IncomingField].ToString()))
                                {
                                    string pubCodeValue = myRow[fm.IncomingField].ToString();
                                    int.TryParse(clientPubCodes.SingleOrDefault(x => x.Value.Equals(pubCodeValue.ToUpper())).Key.ToString(), out pubCodeID);
                                }
                            }
                        }

                        int originalRowNumber = 0;
                        if (dataIV.TransformedRowToOriginalRowMap.ContainsKey(key))
                            originalRowNumber = dataIV.TransformedRowToOriginalRowMap[key];

                        try
                        {
                            listSubscriberTransformed.Add(CreateTransformedSubscriber(allTransformSplit, myRow, pubCodeID, originalRowNumber, key, SubOriginalDictionary, ahdGroups, splitTranFieldMappings, splitTrans));
                        }
                        catch (Exception ex)
                        {
                            #region error
                            StringBuilder sbDetail = new StringBuilder();
                            sbDetail.AppendLine("DQM.Helpers.Validation.FileValidator.ProcessFileAsObject - CreateTransformedSubscriber");
                            sbDetail.AppendLine(System.Environment.NewLine);
                            sbDetail.AppendLine(Core_AMS.Utilities.StringFunctions.FormatException(ex));
                            #endregion
                        }
                    }

                    if (KMPlatform.BusinessLogic.Enums.GetUADFeature(serviceFeature.SFName) == KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
                    {
                        listValidST.AddRange(listSubscriberTransformed);
                    }
                    else
                    {
                        #region PubCode Check
                        List<SubscriberTransformed> listPubCodeValid = new List<SubscriberTransformed>();
                        //have a pubCode
                        listPubCodeValid.AddRange(listSubscriberTransformed.Where(x => !string.IsNullOrEmpty(x.PubCode)));
                        //have no pubCode
                        List<SubscriberTransformed> listNoPubCode = new List<SubscriberTransformed>();
                        listNoPubCode.AddRange(listSubscriberTransformed.Where(x => string.IsNullOrEmpty(x.PubCode)));
                        foreach (var np in listNoPubCode)
                        {
                            StringDictionary myRow = dataIV.DataTransformed[np.ImportRowNumber];
                            string msg = String.Format("Blank PUBCODE @ Row {0}", np.ImportRowNumber);
                            Core_AMS.Utilities.StringFunctions.WriteLineRepeater(msg, ConsoleColor.Red);
                            ValidationError(msg, np.ImportRowNumber, myRow);
                            ErrorMessages.Add(msg);
                        }
                        //not exist PubCode
                        List<SubscriberTransformed> notExistPubCode = (from t in listSubscriberTransformed
                                                                       where !clientPubCodes.Any(x => String.Compare(x.Value, t.PubCode, true) == 0)
                                                                       select t).ToList();
                        foreach (var ne in notExistPubCode)
                        {
                            StringDictionary myRow = dataIV.DataTransformed[ne.ImportRowNumber];
                            string msg = String.Format("PUBCODE not found in UAD @ Row {0}", ne.ImportRowNumber);
                            Core_AMS.Utilities.StringFunctions.WriteLineRepeater(msg, ConsoleColor.Red);
                            ValidationError(msg, ne.ImportRowNumber, myRow);
                            ErrorMessages.Add(msg);
                        }
                        #endregion
                        #region Profile Check
                        List<SubscriberTransformed> listEmailValid = new List<SubscriberTransformed>();
                        listEmailValid.AddRange(listPubCodeValid.Where(x => !string.IsNullOrEmpty(x.Email) && x.Email.Contains("@")));

                        HashSet<SubscriberTransformed> remove = new HashSet<SubscriberTransformed>(listEmailValid);
                        listPubCodeValid.RemoveAll(x => remove.Contains(x));
                        var listQualProfileValid = HasQualifiedProfile(listPubCodeValid);

                        listValidST.AddRange(listEmailValid);
                        listValidST.AddRange(listQualProfileValid);
                        listValidST = listValidST.Distinct().ToList();
                        #endregion
                        #region Web Forms Blank/Null Codesheet values mark invalid
                        if (dbFileType != null && FrameworkUAD_Lookup.Enums.GetDatabaseFileType(dbFileType.CodeName) == FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms)
                        {
                            //File Should can contain multiple pubcodes. This is checked before calling the ProcessFileAsObject Method
                            List<string> requiredResponse = new List<string>();

                            if (listPubCodeValid.Count > 0)
                            {
                                List<string> distinctPubs = new List<string>();
                                distinctPubs.AddRange(listPubCodeValid.Select(x => x.PubCode).Distinct());
                                foreach (string pub in distinctPubs)
                                {
                                    if (clientPubCodes.ContainsValue(pub.ToUpper()))
                                    {
                                        int pubID = clientPubCodes.FirstOrDefault(x => x.Value.Equals(pub, StringComparison.CurrentCultureIgnoreCase)).Key;
                                        if (pubID > 0)
                                        {
                                            FrameworkUAD.BusinessLogic.ResponseGroup rgWorker = new FrameworkUAD.BusinessLogic.ResponseGroup();
                                            List<FrameworkUAD.Entity.ResponseGroup> rgList = rgWorker.Select(pubID, client.ClientConnections);
                                            if (rgList != null && rgList.Count > 0)
                                            {
                                                requiredResponse.AddRange(rgList.Where(x => x.IsRequired == true).Select(x => x.ResponseGroupName.ToLower()));
                                                foreach (FrameworkUAD.Entity.SubscriberTransformed st in listValidST.Where(x => x.PubCode.Equals(pub, StringComparison.CurrentCultureIgnoreCase)))
                                                {
                                                    bool isSubTranValid = true;

                                                    #region Check for Missing Response in Demo List
                                                    List<string> distinctMAFFields = new List<string>();
                                                    distinctMAFFields.AddRange(st.DemographicTransformedList.Select(x => x.MAFField.ToLower()).Distinct().ToList());
                                                    foreach (string val in requiredResponse)
                                                    {
                                                        if (!(distinctMAFFields.Contains(val)))
                                                        {
                                                            isSubTranValid = false;
                                                            StringDictionary myRow = dataIV.DataTransformed[st.ImportRowNumber];
                                                            string msg = String.Format("WEB FORM: Missing/Blank Required Codesheet: " + val + " @ Row {0}", st.ImportRowNumber);
                                                            Core_AMS.Utilities.StringFunctions.WriteLineRepeater(msg, ConsoleColor.Red);
                                                            ValidationError(msg, st.ImportRowNumber, myRow);
                                                            ErrorMessages.Add(msg);
                                                        }
                                                    }
                                                    #endregion
                                                    #region Check Values are Valid
                                                    if (isSubTranValid)
                                                    {
                                                        foreach (FrameworkUAD.Entity.SubscriberDemographicTransformed sdt in st.DemographicTransformedList.Where(x => requiredResponse.Contains(x.MAFField.ToLower())))
                                                        {
                                                            //Check Value not missing
                                                            if (requiredResponse.Contains(sdt.MAFField.ToLower()))
                                                            {
                                                                if (string.IsNullOrEmpty(sdt.Value.Trim()))
                                                                {
                                                                    isSubTranValid = false;
                                                                    StringDictionary myRow = dataIV.DataTransformed[st.ImportRowNumber];
                                                                    string msg = String.Format("WEB FORM: Blank Required Codesheet: " + sdt.MAFField + " value is null/empty @ Row {0}", st.ImportRowNumber);
                                                                    Core_AMS.Utilities.StringFunctions.WriteLineRepeater(msg, ConsoleColor.Red);
                                                                    ValidationError(msg, st.ImportRowNumber, myRow);
                                                                    ErrorMessages.Add(msg);
                                                                }

                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                    #region Add Invalid
                                                    if (isSubTranValid == false)
                                                        listInvalidValidST.Add(st);
                                                    #endregion
                                                }
                                                #region Remove Invalid from Valid List
                                                var setToRemove = new HashSet<SubscriberTransformed>(listInvalidValidST);
                                                listValidST.RemoveAll(x => setToRemove.Contains(x));
                                                #endregion
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        #region Invalids
                        listInvalidValidST = (from st in listSubscriberTransformed
                                              where !listValidST.Any(x => x.STRecordIdentifier == st.STRecordIdentifier)
                                              select st).ToList();
                        listSubscriberInvalid = ConvertTransformedToInvalid(listInvalidValidST);
                        #endregion
                    }
                }
            }
            #endregion

            #region DeDupe the Transformed and Invalid lists before saving
            List<SubscriberTransformed> dedupedTransList = DeDupeTransformed(listValidST);

            List<SubscriberInvalid> dedupedInvalidList = DeDupeInvalid(listSubscriberInvalid);
            #endregion

            #region Scranton Custom - make this an Execution Point
            if (client.FtpFolder.Equals("Scranton"))
            {
                dedupedTransList = Scranton_CompanySurvey(dedupedTransList, clientPubCodes, client.ClientID);
            }
            #endregion

            #region Save SubscriberTransformed and SubscriberInvalid
            if (sourceFile.IsDQMReady == true)
            {
                //valid rows will be imported - invalid skipped
                if (dedupedTransList.Count > 0)
                {
                    FrameworkUAD.BusinessLogic.SubscriberTransformed stWorker = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
                    bool respST = new bool();

                    bool success = false;
                    respST = stWorker.SaveBulkSqlInsert(dedupedTransList, client.ClientConnections, isDataCompare);
                    if (respST != null)
                        success = respST;
                    if (success == false)
                    {
                        #region Error
                        StringBuilder msg = new StringBuilder();
                        msg.AppendLine("ERROR: Insert to Transformed Bulk Data Insert " + DateTime.Now.TimeOfDay.ToString());
                        msg.AppendLine("An unexplained error occurred while inserting records into Transformed or DemoTransformed tables.<br/>");
                        msg.AppendLine("The details of this error have been logged in the FileLog table.<br/>");
                        msg.AppendLine("Please run this query.<br/>");
                        msg.AppendLine("Select * From FileLog With(NoLock) Where SourceFileID = " + sourceFile.SourceFileID.ToString() + " AND ProcessCode = '" + processCode + "' ORDER BY LogDate, LogTime; GO");

                        listSubscriberTransformed.Clear();
                        ValidationError(msg.ToString());
                        ErrorMessages.Add(msg.ToString());
                        #endregion
                    }
                }
                else
                    ErrorMessages.Add("NO VALID Transformed data to insert: " + DateTime.Now.TimeOfDay.ToString());

                if (dedupedInvalidList.Count > 0)
                {
                    FrameworkUAD.BusinessLogic.SubscriberInvalid siWorker = new FrameworkUAD.BusinessLogic.SubscriberInvalid();
                    bool respSI = new bool();

                    bool success = false;
                    respSI = siWorker.SaveBulkSqlInsert(dedupedInvalidList, client.ClientConnections);
                    if (respSI != null)
                        success = respSI;
                    if (success == false)
                    {
                        #region Error
                        StringBuilder msg = new StringBuilder();
                        msg.AppendLine("ERROR: Insert to Invalid Bulk Data Insert " + DateTime.Now.TimeOfDay.ToString());
                        msg.AppendLine("An unexplained error occurred while inserting records into Invalid or DemoInvalid tables.<br/>");
                        msg.AppendLine("The details of this error have been logged in the FileLog table.<br/>");
                        msg.AppendLine("Please run this query.<br/>");
                        msg.AppendLine("Select * From FileLog With(NoLock) Where SourceFileID = " + sourceFile.SourceFileID.ToString() + " AND ProcessCode = '" + processCode + "' ORDER BY LogDate, LogTime; GO");

                        listSubscriberInvalid.Clear();
                        ValidationError(msg.ToString());
                        ErrorMessages.Add(msg.ToString());
                        #endregion
                    }
                }
            }
            #endregion

            dataIV.ImportedRowCount = dedupedTransList.Count;
        }

        #region AdHocDimensions
        private StringDictionary SetAdHoc(FrameworkUAS.Entity.AdHocDimensionGroup adg, StringDictionary dr, List<FrameworkUAS.Entity.AdHocDimension> adList)
        {
            string sourceString = dr[adg.StandardField].ToString();
            int sourceInt = 0;
            int.TryParse(sourceString, out sourceInt);

            string generatedValue = string.Empty;
            string cd = adg.CreatedDimension.ToUpper();
            if (String.IsNullOrEmpty(dr[cd].ToString()))
                generatedValue = adg.DefaultValue.ToString();

            generatedValue = ValidatorMethods.SetGeneratedValue(adList, sourceInt, sourceString, generatedValue, false);

            if (generatedValue == null)
                generatedValue = string.Empty;

            if (String.IsNullOrEmpty(generatedValue))
                generatedValue = dr[cd].ToString();

            dr[cd] = generatedValue;
            return dr;
        }
        private void ApplyAdHocDimensions()
        {
            //first check if client has any AHD's
            FrameworkUAS.BusinessLogic.AdHocDimensionGroup ahdgWorker = new FrameworkUAS.BusinessLogic.AdHocDimensionGroup();
            List<FrameworkUAS.Entity.AdHocDimensionGroup> ahdgResp = new List<AdHocDimensionGroup>();
            List<FrameworkUAS.Entity.AdHocDimensionGroup> ahdGroups = new List<AdHocDimensionGroup>();
            ahdgResp = ahdgWorker.Select(client.ClientID, true);
            if (ahdgResp != null)
                ahdGroups = ahdgResp.Where(x => x.IsActive == true).OrderBy(y => y.OrderOfOperation).ToList();

            FrameworkUAS.BusinessLogic.AdHocDimension adWorker = new FrameworkUAS.BusinessLogic.AdHocDimension();
            List<FrameworkUAS.Entity.AdHocDimension> adResp = new List<AdHocDimension>();

            if (ahdGroups.Count > 0)
            {
                //Grab codesheet values for the dimension group for below. Only want to run on invalid values.
                FrameworkUAD.BusinessLogic.CodeSheet csWorker = new FrameworkUAD.BusinessLogic.CodeSheet();
                List<FrameworkUAD.Entity.CodeSheet> csResp = new List<CodeSheet>();
                List<FrameworkUAD.Entity.CodeSheet> codeSheet = new List<CodeSheet>();
                csResp = csWorker.Select(client.ClientConnections);
                if (csResp != null)
                    codeSheet = csResp;

                //Dictionary<int, string> clientPubCodes = FrameworkUAS.Object.DBWorker.GetPubIDAndCodesByClient(client);

                foreach (FrameworkUAS.Entity.AdHocDimensionGroup adg in ahdGroups)
                {
                    //so now create the dimension column OR set the column value
                    //first check if column exists
                    if (dataIV.HeadersTransformed.ContainsKey(adg.StandardField))
                    {
                        List<FrameworkUAS.Entity.AdHocDimension> adList = new List<AdHocDimension>();
                        adResp = adWorker.Select(adg.AdHocDimensionGroupId);
                        if (adResp != null)
                            adList = adResp;

                        //our compare column is there now check for CreatedDimension column if not there create
                        //do a foreach loop instead of contains.
                        bool colExist = false;
                        if (dataIV.HeadersTransformed.ContainsKey(adg.CreatedDimension))
                            colExist = true;

                        if (colExist == false)
                        {
                            dataIV.HeadersTransformed.Add(adg.CreatedDimension, (dataIV.HeadersTransformed.Count + 1).ToString());
                            //add new column to each row with a blank value
                            foreach (var key in dataIV.DataTransformed.Keys)
                            {
                                StringDictionary myRow = dataIV.DataTransformed[key];
                                myRow.Add(adg.CreatedDimension.ToUpper(), string.Empty);
                            }
                        }
                        else
                        {
                            foreach (var key in dataIV.DataTransformed.Keys)
                            {
                                StringDictionary myRow = dataIV.DataTransformed[key];
                                if (!myRow.ContainsKey(adg.CreatedDimension.ToUpper()))
                                    myRow.Add(adg.CreatedDimension.ToUpper(), string.Empty);
                            }
                        }
                        if (adg.IsPubcodeSpecific == true)
                        {
                            foreach (var key in dataIV.DataTransformed.Keys)
                            {
                                StringDictionary myRow = dataIV.DataTransformed[key];
                                string pubCode = string.Empty;
                                if (myRow.ContainsKey("PubCode"))
                                    pubCode = myRow["PubCode"].ToString();

                                if (adg.DimensionGroupPubcodeMappings.Exists(x => x.Pubcode.Equals(pubCode, StringComparison.CurrentCultureIgnoreCase) && x.IsActive == true))
                                {
                                    //Get the Pubcode ID and codesheet values for the pubID and ResponseGroup = CreatedDimension
                                    //Check the codesheet values where current value = null meaning value not found/invalid and run the SetAdHoc
                                    var pubIDKey = clientPubCodes.FirstOrDefault(x => x.Value.Equals(pubCode, StringComparison.CurrentCultureIgnoreCase)).Key;
                                    int pubID = 0;
                                    int.TryParse(pubIDKey.ToString(), out pubID);
                                    List<FrameworkUAD.Entity.CodeSheet> pubCodeSheet = codeSheet.Where(x => x.PubID.Equals(pubID) && x.ResponseGroup.Equals(adg.CreatedDimension, StringComparison.CurrentCultureIgnoreCase)).ToList();
                                    int i = 0;
                                    string cd = adg.CreatedDimension.ToUpper();
                                    if (pubCodeSheet != null && pubCodeSheet.Count > 0)
                                    {
                                        if (int.TryParse(myRow[cd].ToString(), out i))
                                        {
                                            if (pubCodeSheet.FirstOrDefault(x => x.ResponseValue.Equals(myRow[cd].ToString().TrimStart('0'), StringComparison.CurrentCultureIgnoreCase)) == null)
                                            {
                                                SetAdHoc(adg, myRow, adList);
                                            }
                                        }
                                        else
                                        {
                                            if (pubCodeSheet.FirstOrDefault(x => x.ResponseValue.Equals(myRow[adg.CreatedDimension].ToString(), StringComparison.CurrentCultureIgnoreCase)) == null)
                                            {
                                                SetAdHoc(adg, myRow, adList);
                                            }
                                        }
                                    }
                                    else
                                        SetAdHoc(adg, myRow, adList);
                                }
                            }
                        }
                        else
                        {
                            foreach (var key in dataIV.DataTransformed.Keys)
                            {
                                StringDictionary myRow = dataIV.DataTransformed[key];
                                SetAdHoc(adg, myRow, adList);
                            }

                        }
                    }
                }
            }
        }
        #endregion
        #region Subscriber table Inserts - Original - Invalid - Transformed
        private List<FrameworkUAD.Entity.SubscriberOriginal> InsertOriginalSubscribers()
        {
            List<FrameworkUAD.Entity.SubscriberOriginal> listSubscriberOriginal = new List<SubscriberOriginal>();

            FrameworkUAS.BusinessLogic.AdHocDimensionGroup ahdgWorker = new FrameworkUAS.BusinessLogic.AdHocDimensionGroup();
            List<FrameworkUAS.Entity.AdHocDimensionGroup> ahdgResp = new List<AdHocDimensionGroup>();
            List<FrameworkUAS.Entity.AdHocDimensionGroup> ahdGroups = new List<AdHocDimensionGroup>();
            ahdgResp = ahdgWorker.Select(client.ClientID, false);
            if (ahdgResp != null)
                ahdGroups = ahdgResp.Where(x => x.IsActive == true).OrderBy(y => y.OrderOfOperation).ToList();

            FrameworkUAS.Entity.FieldMapping fm = sourceFile.FieldMappings.SingleOrDefault(x => x.MAFField.Equals("PubCode", StringComparison.CurrentCultureIgnoreCase));

            foreach (var key in dataIV.DataOriginal.Keys)
            {
                StringDictionary myRow = dataIV.DataOriginal[key];
                int pubCodeID = -1;
                if (KMPlatform.BusinessLogic.Enums.GetUADFeature(serviceFeature.SFName) != KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
                {
                    if (fm != null && myRow[fm.IncomingField] != null)
                        if (!string.IsNullOrEmpty(myRow[fm.IncomingField].ToString()))
                        {
                            string pubCodeValue = myRow[fm.IncomingField].ToString();
                            int.TryParse(clientPubCodes.SingleOrDefault(x => x.Value.Equals(pubCodeValue)).Key.ToString(), out pubCodeID);
                        }
                }

                listSubscriberOriginal.Add(CreateSubscriber(myRow, pubCodeID, ahdGroups));
            }

            #region dedupe profiles
            //now due to transformations we may have duplicate profiles
            //can only do this after creating Subscriber objects - before insert
            List<FrameworkUAD.Entity.SubscriberOriginal> dedupedSubscriberOriginal = new List<FrameworkUAD.Entity.SubscriberOriginal>();
            dedupedSubscriberOriginal = listSubscriberOriginal.GroupBy(item => new { item.PubCode, item.FName, item.LName, item.Company, item.Title, item.Address, item.Email, item.Phone }).Select(group => group.First()).ToList();

            List<FrameworkUAD.Entity.SubscriberOriginal> excluded = listSubscriberOriginal.Except(dedupedSubscriberOriginal).ToList();
            dataIV.OriginalDuplicateRecordCount = excluded.Count;
            if (excluded.Count > 0)
            {
                int rowCount = 1;
                foreach (FrameworkUAD.Entity.SubscriberOriginal exc in excluded)
                {
                    string msg = "Demo check: " + rowCount.ToString() + " of " + excluded.Count.ToString();
                    Core_AMS.Utilities.StringFunctions.WriteLineRepeater(msg, ConsoleColor.Green);

                    FrameworkUAD.Entity.SubscriberOriginal deDupeMatch = dedupedSubscriberOriginal.SingleOrDefault(x => (x.Address == exc.Address &&
                                                                                            x.PubCode == exc.PubCode &&
                                                                                            x.FName == exc.FName &&
                                                                                            x.LName == exc.LName &&
                                                                                            x.Company == exc.Company &&
                                                                                            x.Title == exc.Title &&
                                                                                            x.Email == exc.Email &&
                                                                                            x.Phone == exc.Phone) && x.SORecordIdentifier != exc.SORecordIdentifier);
                    if (deDupeMatch != null)
                    {
                        //this will always get all as differences cause SORecIdentifiers are always different so need to join on Maff and check for different values
                        //exc.DemographicOriginalList.Except(deDupeMatch.DemographicOriginalList).ToList();
                        List<FrameworkUAD.Entity.SubscriberDemographicOriginal> notMatch = (from excd in exc.DemographicOriginalList
                                                                                            join dd in deDupeMatch.DemographicOriginalList on excd.MAFField equals dd.MAFField
                                                                                            where excd.Value != dd.Value
                                                                                            select excd).Distinct().ToList();

                        List<FrameworkUAD.Entity.SubscriberDemographicOriginal> notExist = (from e in exc.DemographicOriginalList
                                                                                            where !deDupeMatch.DemographicOriginalList.Any(x => x.MAFField == e.MAFField)
                                                                                            select e).ToList();


                        List<FrameworkUAD.Entity.SubscriberDemographicOriginal> combined = new List<SubscriberDemographicOriginal>();
                        combined.AddRange(notMatch);
                        combined.AddRange(notExist);
                        combined.ForEach(x => x.SORecordIdentifier = deDupeMatch.SORecordIdentifier);
                        //deDupeMatch.DemographicOriginalList.AddRange(combined);
                        combined.ForEach(x => { deDupeMatch.DemographicOriginalList.Add(x); });
                        
                        notMatch = null;
                        notExist = null;
                        combined = null;
                    }
                    deDupeMatch = null;

                    rowCount++;
                }
            }
            else
                dedupedSubscriberOriginal = listSubscriberOriginal;
            #endregion

            FrameworkUAD.BusinessLogic.SubscriberOriginal soWorker = new FrameworkUAD.BusinessLogic.SubscriberOriginal();
            bool soResp = new bool();
            bool success = false;
            soResp = soWorker.SaveBulkSqlInsert(dedupedSubscriberOriginal, client.ClientConnections);
            if (soResp != null)
                success = soResp;

            if (success == false)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("ERROR: Insert to Original Bulk Data Insert " + DateTime.Now.TimeOfDay.ToString());
                msg.AppendLine("An unexplained error occurred while inserting records into Original or DemoOriginal tables.<br/>");
                msg.AppendLine("The details of this error have been logged in the FileLog table.<br/>");
                msg.AppendLine("Please run this query.<br/>");
                msg.AppendLine("Select * From FileLog With(NoLock) Where SourceFileID = " + sourceFile.SourceFileID.ToString() + " AND ProcessCode = '" + processCode.ToString() + "' ORDER BY LogDate, LogTime; GO");

                ValidationError(msg.ToString());
                ErrorMessages.Add(msg.ToString());

                dedupedSubscriberOriginal.Clear();
            }

            return dedupedSubscriberOriginal;
        }
        
        private List<SubscriberInvalid> ConvertTransformedToInvalid(List<SubscriberTransformed> listST)
        {
            List<SubscriberInvalid> listSI = new List<SubscriberInvalid>();
            foreach (SubscriberTransformed st in listST)
            {
                SubscriberInvalid si = new SubscriberInvalid(st);
                listSI.Add(si);
            }
            return listSI;
        }
        private List<FrameworkUAD.Entity.SubscriberTransformed> DeDupeTransformed(List<FrameworkUAD.Entity.SubscriberTransformed> listOrigST)
        {
            #region dedupe profiles
            listOrigST.OrderBy(x => x.FName).ThenBy(y => y.LName).ToList();
            //now due to transformations we may have duplicate profiles
            //can only do this after creating Subscriber objects - before insert
            List<FrameworkUAD.Entity.SubscriberTransformed> dedupedST = new List<SubscriberTransformed>();
            //group on PubCode, FirstName, LastName, Company, Title, Address, Email, Phone
            dedupedST = listOrigST.GroupBy(item => new { item.PubCode, item.FName, item.LName, item.Company, item.Title, item.Address, item.Email, item.Phone }).Select(group => group.First()).ToList();
            List<FrameworkUAD.Entity.SubscriberTransformed> excluded = listOrigST.Except(dedupedST).ToList();
            dataIV.TransformedDuplicateRecordCount = excluded.Count;
            if (excluded.Count > 0)
            {
                int rowCount = 1;
                foreach (FrameworkUAD.Entity.SubscriberTransformed exc in excluded)
                {
                    string msg = "Demo check: " + rowCount.ToString() + " of " + excluded.Count.ToString();
                    Core_AMS.Utilities.StringFunctions.WriteLineRepeater(msg, ConsoleColor.Green);

                    FrameworkUAD.Entity.SubscriberTransformed deDupeMatch = dedupedST.SingleOrDefault(x => (x.Address == exc.Address &&
                                                                                            x.PubCode == exc.PubCode &&
                                                                                            x.FName == exc.FName &&
                                                                                            x.LName == exc.LName &&
                                                                                            x.Company == exc.Company &&
                                                                                            x.Title == exc.Title &&
                                                                                            x.Email == exc.Email &&
                                                                                            x.Phone == exc.Phone) && x.STRecordIdentifier != exc.STRecordIdentifier);
                    if (deDupeMatch != null)
                    {
                        List<FrameworkUAD.Entity.SubscriberDemographicTransformed> notMatch = (from excd in exc.DemographicTransformedList
                                                                                               join dd in deDupeMatch.DemographicTransformedList on excd.MAFField equals dd.MAFField
                                                                                               where excd.Value != dd.Value
                                                                                               select excd).Distinct().ToList();

                        List<FrameworkUAD.Entity.SubscriberDemographicTransformed> notExist = (from e in exc.DemographicTransformedList
                                                                                               where !deDupeMatch.DemographicTransformedList.Any(x => x.MAFField == e.MAFField)
                                                                                               select e).ToList();


                        List<FrameworkUAD.Entity.SubscriberDemographicTransformed> combined = new List<SubscriberDemographicTransformed>();
                        combined.AddRange(notMatch);
                        combined.AddRange(notExist);
                        combined.ForEach(x => x.STRecordIdentifier = deDupeMatch.STRecordIdentifier);
                        combined.ForEach(x => x.SORecordIdentifier = deDupeMatch.SORecordIdentifier);
                        //deDupeMatch.DemographicTransformedList.AddRange(combined);
                        combined.ForEach(x => { deDupeMatch.DemographicTransformedList.Add(x); });
                        notMatch = null;
                        notExist = null;
                        combined = null;
                    }
                    deDupeMatch = null;
                    rowCount++;
                }
            }
            else
                dedupedST = listOrigST;
            #endregion

            return dedupedST;
        }
        private List<FrameworkUAD.Entity.SubscriberInvalid> DeDupeInvalid(List<FrameworkUAD.Entity.SubscriberInvalid> listOrig)
        {
            #region dedupe profiles
            //now due to transformations we may have duplicate profiles
            //can only do this after creating Subscriber objects - before insert
            List<FrameworkUAD.Entity.SubscriberInvalid> deduped = new List<SubscriberInvalid>();
            //group on PubCode, FirstName, LastName, Company, Title, Address, Email, Phone
            deduped = listOrig.GroupBy(item => new { item.PubCode, item.FName, item.LName, item.Company, item.Title, item.Address, item.Email, item.Phone }).Select(group => group.First()).ToList();
            List<FrameworkUAD.Entity.SubscriberInvalid> excluded = listOrig.Except(deduped).ToList();
            if (excluded.Count > 0)
            {
                int rowCount = 1;
                foreach (FrameworkUAD.Entity.SubscriberInvalid exc in excluded)
                {
                    string msg = "Demo check: " + rowCount.ToString() + " of " + excluded.Count.ToString();
                    Core_AMS.Utilities.StringFunctions.WriteLineRepeater(msg, ConsoleColor.Green);

                    FrameworkUAD.Entity.SubscriberInvalid deDupeMatch = deduped.SingleOrDefault(x => (x.Address == exc.Address &&
                                                                                            x.PubCode == exc.PubCode &&
                                                                                            x.FName == exc.FName &&
                                                                                            x.LName == exc.LName &&
                                                                                            x.Company == exc.Company &&
                                                                                            x.Title == exc.Title &&
                                                                                            x.Email == exc.Email &&
                                                                                            x.Phone == exc.Phone) && x.SIRecordIdentifier != exc.SIRecordIdentifier);
                    if (deDupeMatch != null)
                    {
                        List<FrameworkUAD.Entity.SubscriberDemographicInvalid> notMatch = (from excd in exc.DemographicInvalidList
                                                                                           join dd in deDupeMatch.DemographicInvalidList on excd.MAFField equals dd.MAFField
                                                                                           where excd.Value != dd.Value
                                                                                           select excd).Distinct().ToList();

                        List<FrameworkUAD.Entity.SubscriberDemographicInvalid> notExist = (from e in exc.DemographicInvalidList
                                                                                           where !deDupeMatch.DemographicInvalidList.Any(x => x.MAFField == e.MAFField)
                                                                                           select e).ToList();
                        List<FrameworkUAD.Entity.SubscriberDemographicInvalid> combined = new List<SubscriberDemographicInvalid>();
                        combined.AddRange(notMatch);
                        combined.AddRange(notExist);
                        combined.ForEach(x => x.SIRecordIdentifier = deDupeMatch.SIRecordIdentifier);
                        combined.ForEach(x => x.SORecordIdentifier = deDupeMatch.SORecordIdentifier);
                        combined.ForEach(x => { deDupeMatch.DemographicInvalidList.Add(x); });
                        notMatch = null;
                        notExist = null;
                        combined = null;
                    }
                    deDupeMatch = null;
                    rowCount++;
                }
            }
            else
                deduped = listOrig;
            #endregion

            return deduped;
        }
        #endregion
       
        #endregion
        #region Exception Logging
        private void LogException(Exception ex, int sourceFileID, string processCode, int clientID, int rowNumber, StringDictionary rowData)
        {
            string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);

            StringBuilder sbDetail = new StringBuilder();
            sbDetail.AppendLine("ClientID: " + clientID.ToString());
            sbDetail.AppendLine("SourceFileID: " + sourceFileID.ToString());
            sbDetail.AppendLine("ProcessCode: " + processCode.ToString());
            sbDetail.AppendLine("Row Number: " + rowNumber.ToString());
            sbDetail.AppendLine(System.Environment.NewLine);
            sbDetail.AppendLine(message);
            ErrorMessages.Add(sbDetail.ToString());
            ValidationError(sbDetail.ToString(), rowNumber, rowData);
        }

        protected override bool ValidationError(string errorMsg, int rowNumber = 0, StringDictionary rowData = null)
        {
            bool exceedThreashold = false;
            var importErrorEntity = ValidatorMethods.CreateImportErrorEntity(dataIV, rowData, rowNumber, errorMsg, true);
            var check = ValidatorMethods.SetImportErrorsValue(dataIV, importErrorEntity);

            if (dataIV.ImportErrorCount >= 100000 || check >= 25)
            {
                //send some email that the processing of this file has stopped due to number of error encountered. 
                //100k errors or 25% of records in error state.
                exceedThreashold = true;
                ErrorMessages.Add("Rejecting file due to error threshold - will delete file from repository then shut down current engine and start new instance - " + DateTime.Now.TimeOfDay.ToString());
            }

            return exceedThreashold;
        }
        #endregion

        #region Custom Client Methods
        private List<FrameworkUAD.Entity.SubscriberTransformed> Scranton_CompanySurvey(List<FrameworkUAD.Entity.SubscriberTransformed> data, Dictionary<int, string> clientPubCodes, int clientId)
        {
            List<FrameworkUAD_Lookup.Entity.Code> demoUpdates = new List<FrameworkUAD_Lookup.Entity.Code>();
            demoUpdates = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Demographic_Update);
            FrameworkUAD_Lookup.Entity.Code demoUpdate = new FrameworkUAD_Lookup.Entity.Code();
            if (demoUpdates != null)
                demoUpdate = demoUpdates.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.DemographicUpdate.Replace.ToString()));

            //execute in Validator.ValidateData after TransformedDedupe - 
            //at this point the incoing ST list has had pubs validated so we know everything exists and matches
            FrameworkUAS.BusinessLogic.AdHocDimensionGroup ahdgWorker = new FrameworkUAS.BusinessLogic.AdHocDimensionGroup();
            List<AdHocDimensionGroup> adgList = ahdgWorker.Select(clientId, true);
            List<AdHocDimension> companyList = new List<AdHocDimension>();
            List<AdHocDimension> domainList = new List<AdHocDimension>();
            int adgCompanyId = 0;
            int adgDomainId = 0;

            if (adgList != null)
            {
                if (adgList.Any(x => x.AdHocDimensionGroupName.Equals("Scranton_Company_Survey")) == true)
                    adgCompanyId = adgList.First(x => x.AdHocDimensionGroupName.Equals("Scranton_Company_Survey")).AdHocDimensionGroupId;

                if (adgList.Any(x => x.AdHocDimensionGroupName.Equals("Scranton_Domains_Survey")) == true)
                    adgDomainId = adgList.First(x => x.AdHocDimensionGroupName.Equals("Scranton_Domains_Survey")).AdHocDimensionGroupId;

                if (adgCompanyId > 0)
                    companyList = adgList.Single(x => x.AdHocDimensionGroupId == adgCompanyId).AdHocDimensions;

                if (adgDomainId > 0)
                    domainList = adgList.Single(x => x.AdHocDimensionGroupId == adgDomainId).AdHocDimensions;
            }
            Core_AMS.Utilities.FuzzySearch fs = new FuzzySearch();

            HashSet<string> matchCompanies = new HashSet<string>();
            HashSet<string> matchEmails = new HashSet<string>();
            foreach (AdHocDimension ad in companyList)
            {
                string[] values = ad.MatchValue.Split(':').ToArray();
                if (values != null && values.Count() == 3)
                {
                    if (!string.IsNullOrEmpty(values[0]))
                        matchCompanies.Add(values[0]);

                    if (!string.IsNullOrEmpty(values[1]))
                        matchEmails.Add(values[1]);

                    if (!string.IsNullOrEmpty(values[2]))
                        matchEmails.Add(values[2]);
                }
            }
            //Need the 
            foreach (FrameworkUAD.Entity.SubscriberTransformed st in data)
            {
                int pubId = clientPubCodes.Single(x => x.Value.ToLower().Equals(st.PubCode.ToLower())).Key;
                #region SURVEY = PBG300
                //if st.Company fuzzy matches OR st.Email domain matches then create SURVEY demo
                bool foundCompanyMatch = false;
                if (matchCompanies.Count > 0 && !string.IsNullOrEmpty(st.Company))
                {
                    if (matchCompanies.Any(x => fs.Search(st.Company, x) >= 80) == true)
                    {
                        foundCompanyMatch = true;
                        FrameworkUAD.Entity.SubscriberDemographicTransformed sdo = new FrameworkUAD.Entity.SubscriberDemographicTransformed();
                        sdo.CreatedByUserID = st.CreatedByUserID;
                        sdo.DateCreated = DateTime.Now;
                        sdo.MAFField = adgList.Single(x => x.AdHocDimensionGroupId == adgCompanyId).CreatedDimension;
                        sdo.NotExists = false;
                        sdo.PubID = pubId;
                        sdo.SORecordIdentifier = st.SORecordIdentifier;
                        sdo.STRecordIdentifier = st.STRecordIdentifier;
                        sdo.Value = "PBG300";
                        sdo.DemographicUpdateCodeId = demoUpdate.CodeId;
                        sdo.IsAdhoc = true;

                        st.DemographicTransformedList.Add(sdo);
                    }
                }

                if (foundCompanyMatch == false && matchEmails.Count > 0)
                {
                    //'GMAIL', 'MSN', 'YAHOO', 'AOL', 'COMCAST', and 'HOTMAIL'
                    if (!string.IsNullOrEmpty(st.Email) && (!st.Email.ToLower().EndsWith("gmail.com") || !st.Email.ToLower().EndsWith("msn.com") || !st.Email.ToLower().EndsWith("yahoo.com") || !st.Email.ToLower().EndsWith("aol.com") || !st.Email.ToLower().EndsWith("comcast.com") || !st.Email.ToLower().EndsWith("hotmail.com")))
                    {
                        string[] profileEmailArray = st.Email.Split('@');
                        if (profileEmailArray.Length == 2 && !string.IsNullOrEmpty(profileEmailArray[1]))
                        {
                            if (matchEmails.Any(x => profileEmailArray[1].Equals(x, StringComparison.CurrentCultureIgnoreCase)) == true)
                            {
                                FrameworkUAD.Entity.SubscriberDemographicTransformed sdo = new FrameworkUAD.Entity.SubscriberDemographicTransformed();
                                sdo.CreatedByUserID = st.CreatedByUserID;
                                sdo.DateCreated = DateTime.Now;
                                sdo.MAFField = adgList.Single(x => x.AdHocDimensionGroupId == adgCompanyId).CreatedDimension;
                                sdo.NotExists = false;
                                sdo.PubID = pubId;
                                sdo.SORecordIdentifier = st.SORecordIdentifier;
                                sdo.STRecordIdentifier = st.STRecordIdentifier;
                                sdo.Value = "PBG300";
                                sdo.DemographicUpdateCodeId = demoUpdate.CodeId;
                                sdo.IsAdhoc = true;

                                st.DemographicTransformedList.Add(sdo);
                            }
                        }
                    }
                }
                #endregion

                if (domainList.Count > 0)
                {
                    if (!string.IsNullOrEmpty(st.Email) && domainList.Any(x => st.Email.ToLower().EndsWith(x.MatchValue.ToLower())) == true)
                    {
                        FrameworkUAD.Entity.SubscriberDemographicTransformed sdo = new FrameworkUAD.Entity.SubscriberDemographicTransformed();
                        sdo.CreatedByUserID = st.CreatedByUserID;
                        sdo.DateCreated = DateTime.Now;
                        sdo.MAFField = adgList.Single(x => x.AdHocDimensionGroupId == adgDomainId).CreatedDimension;
                        sdo.NotExists = false;
                        sdo.PubID = pubId;
                        sdo.SORecordIdentifier = st.SORecordIdentifier;
                        sdo.STRecordIdentifier = st.STRecordIdentifier;
                        sdo.Value = "BDCG300";
                        sdo.DemographicUpdateCodeId = demoUpdate.CodeId;
                        sdo.IsAdhoc = true;

                        st.DemographicTransformedList.Add(sdo);
                    }
                }
            }

            return data;
        }
        #endregion

        #region Validation Result Reports
        public StringBuilder GetReportBodyForSingleEmail(bool isKnownCustomerFileName, bool isValidFileType, bool isFileSchemaValid, FrameworkUAD.Object.ValidationResult vr)
        {
            string unexpectedColumns = vr.UnexpectedColumns != null && vr.UnexpectedColumns.Count > 0 ? string.Join(", ", vr.UnexpectedColumns) : null;
            string notFoundColumns = vr.NotFoundColumns != null && vr.NotFoundColumns.Count > 0 ? string.Join(", ", vr.NotFoundColumns) : null;
            string duplicateColumns = vr.DuplicateColumns != null && vr.DuplicateColumns.Count > 0 ? string.Join(", ", vr.DuplicateColumns) : null;
            StringBuilder clientReportToBeAppended = new StringBuilder();
            try
            {
                if (checkFile != null && vr != null && vr.ImportFile != null)
                {
                    #region valid file processed
                    FrameworkUAD.BusinessLogic.ValidationResult vrWorker = new FrameworkUAD.BusinessLogic.ValidationResult();
                    clientReportToBeAppended.Append("<b>Results for file processed.</b><br/><br/>");
                    clientReportToBeAppended.Append("Processed File Name: " + checkFile.Name + "<br/>");
                    clientReportToBeAppended.Append("Source File Name: " + sourceFile.FileName + "<br/>");
                    clientReportToBeAppended.Append("Source File Id: " + sourceFile.SourceFileID.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Is File Registered: " + BooleanExtensions.ToYesNoString(isKnownCustomerFileName) + "<br/>");
                    clientReportToBeAppended.AppendLine("Is File Type Valid: " + BooleanExtensions.ToYesNoString(isValidFileType) + "<br/>");
                    clientReportToBeAppended.AppendLine("Is File Schema Valid: " + BooleanExtensions.ToYesNoString(isFileSchemaValid) + "<br/>");
                    clientReportToBeAppended.AppendLine("Process Code: " + processCode.ToString() + "<br/><br/>");

                    //int profileIssues = eventMessage.ValidationResult.TransformedRowCount - eventMessage.ValidationResult.TotalRowCount;
                    int dimensionIssues = vr.DimensionImportErrorCount;
                    clientReportToBeAppended.AppendLine("<b>*** Import details for file ***</b><br/>");
                    clientReportToBeAppended.AppendLine("Total row count: " + vr.TotalRowCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Original row count: " + vr.OriginalRowCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Post transformations row count: " + vr.TransformedRowCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Inserted to Transformed: " + vr.ImportedRowCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Were errors encountered at the row level: " + vr.HasError.ToYesNoString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Record Error total: " + vr.RecordImportErrorCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Dimension Error total: " + vr.DimensionImportErrorCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Excluded records: " + vr.RecordImportErrorCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Dimension issues: " + dimensionIssues.ToString() + "<br/><br/>");

                    bool custErrorsAdded = false;

                    if (isKnownCustomerFileName == false)
                    {
                        clientReportToBeAppended.Append("<b>File Status: File is unknown</b><br/><br/>");
                        if (vr.HasError == true)
                        {
                            clientReportToBeAppended.AppendLine(vrWorker.GetCustomerErrorMessage(vr, sourceFile));
                            custErrorsAdded = true;
                        }
                    }

                    if (isValidFileType == false)
                    {
                        clientReportToBeAppended.Append("<b>File Status:</b> Extension type could not be processed<br/><br/>");
                        if (vr.HasError == true && custErrorsAdded == false)
                        {
                            clientReportToBeAppended.AppendLine(vrWorker.GetCustomerErrorMessage(vr, sourceFile));
                            custErrorsAdded = true;
                        }
                    }

                    if (isFileSchemaValid == false)
                    {
                        clientReportToBeAppended.Append("<b>File Status: Exceptions found</b><br/>");
                        if (!string.IsNullOrEmpty(unexpectedColumns))
                            clientReportToBeAppended.Append("<div style=\"margin-left:20px\">Unexpected Columns Found: " + unexpectedColumns + "</div><br/>");
                        if (!string.IsNullOrEmpty(notFoundColumns))
                            clientReportToBeAppended.Append("<div style=\"margin-left:20px\">Columns Not Found: " + notFoundColumns + "</div><br/>");
                        if (!string.IsNullOrEmpty(duplicateColumns))
                            clientReportToBeAppended.Append("<div style=\"margin-left:20px\">Duplicate Columns: " + duplicateColumns + "</div><br/>");
                        if (vr.HasError == true && custErrorsAdded == false)
                        {
                            clientReportToBeAppended.AppendLine(vrWorker.GetCustomerErrorMessage(vr, sourceFile));
                            custErrorsAdded = true;
                        }
                    }

                    if (vr.HasError == true && custErrorsAdded == false)
                    {
                        clientReportToBeAppended.AppendLine(vrWorker.GetCustomerErrorMessage(vr, sourceFile));
                        custErrorsAdded = true;
                    }
                    #endregion
                }
                else
                {
                    clientReportToBeAppended.Append("<b>An unrecognized error has occured.</b><br/><br/>");
                    clientReportToBeAppended.Append("<b>File Name:</b> " + checkFile.Name + "<br/>");
                    clientReportToBeAppended.AppendLine("<b>File Status:</b> " + BooleanExtensions.ToGoodBadString(isFileSchemaValid) + "<br/>");
                    clientReportToBeAppended.AppendLine("<b>Process Code:</b> " + processCode.ToString() + "<br/>");
                }
            }
            catch (Exception ex)
            {
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Operations;
                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".GetReportBodyForSingleEmail", app, string.Empty);
            }
            return clientReportToBeAppended;
        }
        #endregion
        public MailMessage GetMailMessage(KMPlatform.Entity.Client client, string reportBody, System.IO.FileInfo file, bool isValidFileType, bool isFileSchemaValid, string emailType, string processCode, int sourceFileID, string badDataAttachment = "", MailPriority mailPriority = MailPriority.Normal, bool createSummaryReports = false, bool isDataCompare = false)
        {
            //ConsoleMessage("About to send.", processCode, 0, 0);

            MailMessage message = new MailMessage();
            try
            {
                message.Priority = mailPriority;

                if (isValidFileType == false || isFileSchemaValid == false)
                    message.Priority = MailPriority.High;

                KMPlatform.BusinessLogic.Service serviceWorker = new KMPlatform.BusinessLogic.Service();
                FrameworkUAS.BusinessLogic.SourceFile sfWorker = new FrameworkUAS.BusinessLogic.SourceFile();
                FrameworkUAS.Entity.SourceFile sf = sfWorker.SelectSourceFileID(sourceFileID);
                KMPlatform.Entity.Service service = null;
                string serviceName = "Unified Audience Database";
                if (sf != null)
                {
                    service = serviceWorker.Select(sf.ServiceID);
                    if (service != null)
                        serviceName = " " + service.ServiceName;
                }

                if (isDataCompare == false)
                    message.Subject = client.FtpFolder + ": " + file.Name + " - Validator ";
                else
                    message.Subject = client.FtpFolder + ": " + file.Name + " - DATA COMPARE - Valid File Type: " + isValidFileType.ToYesNoString() + " Valid File Schema: " + isFileSchemaValid.ToYesNoString();
                if (!string.IsNullOrEmpty(client.AccountManagerEmails))
                    message.To.Add(client.AccountManagerEmails);
                if (!string.IsNullOrEmpty(client.ClientEmails))
                    message.To.Add(client.ClientEmails);
                message.To.Add("micah.matheson@teamkm.com");

                message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
                //Generic
                Message myMsgs = new Message();
                string htmlMsg = myMsgs.GetMessage(Message.MessageTag.ImportFileReport_HTML).Replace("%%CLIENT%%", client.FtpFolder).Replace("%%FILETYPE%%", serviceName);
                if (htmlMsg == null || string.IsNullOrEmpty(htmlMsg))
                    htmlMsg = string.Empty;

                var emailView = AlternateView.CreateAlternateViewFromString(htmlMsg, null, emailType);
                message.AlternateViews.Add(emailView);

                Attachment details = Attachment.CreateAttachmentFromString(reportBody, "Details.html");
                if (details != null)
                    message.Attachments.Add(details);

                if (!string.IsNullOrEmpty(badDataAttachment))
                {
                    message.Priority = MailPriority.High;
                    Attachment badData = Attachment.CreateAttachmentFromString(badDataAttachment, "BadData_" + file.Name);//application/excel text/plain text/csv
                    if (badData != null)
                        message.Attachments.Add(badData);
                }

                //LinkedResource logo = new LinkedResource("C:\\source\\ADMS\\Emailer\\Images\\KM Logo.png");
                //logo.ContentId = "companylogo";

                //htmlView.LinkedResources.Add(logo);

                //save the email message to the Server
                string path = Core.ADMS.BaseDirs.getClientFileResultEmail() + @"\" + client.FtpFolder + @"\";
                DateTime time = DateTime.Now;
                string format = "MMddyyyy_HH-mm-ss";
                string emailName = time.ToString(format) + "_" + sourceFileID.ToString() + "_" + file.Name.Replace(file.Extension, "") + ".eml";

                var filePath = Path.Combine(path, emailName);
                var emailMessage = GetEmailMessageFrom(message);
                var emailService = new EmailService(new EmailClient(), new ConfigurationProvider());
                emailService.SaveEmail(emailMessage, filePath);

                Core_AMS.Utilities.FileFunctions ff = new Core_AMS.Utilities.FileFunctions();
                ff.CreateFile(path + time.ToString(format) + "_" + sourceFileID.ToString() + "_Details.html", reportBody);

                ff.CreateFile(path + time.ToString(format) + "_" + sourceFileID.ToString() + "_EmailBody.html", htmlMsg);

                if (!string.IsNullOrEmpty(badDataAttachment))
                {
                    string archive = time.ToString(format) + "_" + sourceFileID.ToString() + "_BadData_" + file.Name;
                    ff.CreateFile(path + archive, badDataAttachment);
                }

                if (createSummaryReports == true)
                {
                    FrameworkUAD.BusinessLogic.Reports rworker = new FrameworkUAD.BusinessLogic.Reports();
                    FrameworkUAD.BusinessLogic.ImportErrorSummary rSum = new FrameworkUAD.BusinessLogic.ImportErrorSummary();
                    message.Attachments.Add(new Attachment(rworker.CreateFileSummaryReport(sourceFileID, processCode, System.IO.Path.GetFileNameWithoutExtension(file.Name), client.FtpFolder, client.ClientConnections, Core.ADMS.BaseDirs.getClientArchiveDir().ToString())));
                    message.Attachments.Add(new Attachment(rworker.CreatePubCodeSummaryReport(sourceFileID, processCode, System.IO.Path.GetFileNameWithoutExtension(file.Name), client.FtpFolder, client.ClientConnections, Core.ADMS.BaseDirs.getClientArchiveDir().ToString())));
                    message.Attachments.Add(new Attachment(rSum.CreateDimensionErrorsSummaryReport(sourceFileID, processCode, client.FtpFolder, client.ClientConnections, Core.ADMS.BaseDirs.getClientArchiveDir().ToString())));
                }
                if (service != null && service.ServiceName.Equals(KMPlatform.Enums.Services.FULFILLMENT.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    //ConsoleMessage("Attaching Circ Reports.", processCode, 0, 0);
                    FrameworkUAD.BusinessLogic.CircIntegration ci = new FrameworkUAD.BusinessLogic.CircIntegration();
                    message.Attachments.Add(new Attachment(ci.SelectCircImportSummaryOne(processCode, client.ClientConnections, client.FtpFolder, sourceFileID, Core.ADMS.BaseDirs.getClientArchiveDir().ToString())));
                    message.Attachments.Add(new Attachment(ci.SelectCircImportSummaryTwo(processCode, client.ClientConnections, client.FtpFolder, sourceFileID, Core.ADMS.BaseDirs.getClientArchiveDir().ToString())));
                }
            }
            catch (Exception ex)
            {
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Operations; 
                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".GetMailMessage", app, string.Empty);
            }
            return message;
        }

        private static EmailMessage GetEmailMessageFrom(MailMessage message)
        {
            Guard.NotNull(message, nameof(message));

            var emailMessage = new EmailMessage
            {
                Subject = message.Subject,
                From = message.From.Address,
                Body = message.Body,
                IsHtml = message.IsBodyHtml
            };

            foreach (var address in message.To)
            {
                emailMessage.To.Add(address.Address);
            }

            foreach (var attachment in message.Attachments)
            {
                emailMessage.Attachments.Add(attachment);
            }

            foreach (var view in message.AlternateViews)
            {
                emailMessage.AlternateViews.Add(view);
            }

            return emailMessage;
        }

        public void SendEmail(MailMessage message, KMPlatform.Entity.Client client)
        {
            string random = Core_AMS.Utilities.StringFunctions.RandomAlphaNumericString(6);
            string path = Core.ADMS.BaseDirs.getClientFileResultEmail() + @"\" + client.FtpFolder + @"\" + random + @"\";
            List<Attachment> deleteAttachment = new List<Attachment>();

            foreach (Attachment a in message.Attachments)
            {
                if (a.ContentStream.Length > 10000000)
                {
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    byte[] buffer = new byte[a.ContentStream.Length];
                    a.ContentStream.Read(buffer, 0, buffer.Length);
                    FileStream file = new FileStream(path + a.Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    file.Write(buffer, 0, buffer.Length);
                    file.Dispose();
                    deleteAttachment.Add(a);
                }
            }

            foreach (Attachment a in deleteAttachment)
                message.Attachments.Remove(a);

            //now go through and zip all the files
            Core_AMS.Utilities.FileFunctions ff = new Core_AMS.Utilities.FileFunctions();

            if (Directory.Exists(path))
            {
                foreach (var f in Directory.GetFiles(path))
                {
                    FileInfo info = new FileInfo(f);
                    FileInfo zippedFile = ff.CreateZipFile(info);

                    Attachment zipAttach = new Attachment(zippedFile.FullName);
                    message.Attachments.Add(zipAttach);
                }
            }

            //string userState = "Message";
            SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);
            //smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
            smtp.Send(message);
            //smtp.SendAsync(message, userState);
        }
        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine(string.Format("[{0}] Send canceled.", token));
            }
            if (e.Error != null)
            {
                Console.WriteLine(string.Format("[{0}] {1}", token, e.Error.ToString()));
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
        }

    }

    public static class BooleanExtensions
    {
        public static string ToYesNoString(this bool value)
        {
            return value == true ? "Yes" : "No";
        }
        public static string ToGoodBadString(this bool value)
        {
            return value == true ? "Processed" : "Invalid";
        }
    }
}
