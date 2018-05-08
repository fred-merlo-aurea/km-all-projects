using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FrameworkUAS.Entity;
using FrameworkUAS.Object;
using KM.Common;
using KMApplications = KMPlatform.BusinessLogic.Enums.Applications;
using EntityImportError = FrameworkUAD.Entity.ImportError;
using KMEnums = KMPlatform.BusinessLogic.Enums;
using ObjectImportErrorSummary = FrameworkUAD.Object.ImportErrorSummary;
using ObjectValidationResult = FrameworkUAD.Object.ValidationResult;
using StringFunctions = KM.Common.StringFunctions;

namespace FrameworkUAD.BusinessLogic
{
    public class ValidationResult
    {
        private const char CommaSeparator = ',';
        private const char UnderlineSeparator = '_';
        private const string DqmPath = "C:\\ADMS\\Applications\\DQM\\";
        private const string GetBadDataCommand = "GetBadData";
        private const string GetBadDataFileValidatorCommand = "GetBadDataFileValidator";
        private const string MethodNameGetErrorMessage = "GetCustomerErrorMessage";
        private const string MethodNameGetErrorMessageFileValidator = "GetCustomerErrorMessageFileValidator";
        private const string RootFolderPath = "C:\\ADMS\\Applications\\DQM\\";
        private const string LocationFileNameFormat = "Details_{0}_BadData.csv";
        private const string OriginalImportRow = "originalimportrow";
        private const string BadDataOriginalHeadersKey = "BadDataOriginalHeaders";
        private const string HeadersOriginalKey = "HeadersOriginal";
        
        public string GetCustomerErrorMessage(ObjectValidationResult validation, bool isTextQualifier)
        {
            IEnumerable<EntityImportError> recordImportErrors = validation.RecordImportErrors;
            return BuildValidationMessage(validation, isTextQualifier, recordImportErrors);
        }

        public string GetCustomerErrorMessage(ObjectValidationResult validation, SourceFile source)
        {
            IEnumerable<Entity.ImportError> recordImportErrors = validation.RecordImportErrors?.Distinct().OrderBy(x => x.RowNumber);
            return BuildValidationMessage(validation, source.IsTextQualifier, recordImportErrors);
        }

        public void GetCustomerErrorMessageFileValidator(ObjectValidationResult validation, SourceFile source)
        {
            GetCustomerErrorMessageFileValidator(validation, source.FileName, source.IsTextQualifier);
        }

        public void GetCustomerErrorMessageFileValidator(ObjectValidationResult validation, string fileName, bool isTextQualifier)
        {
            BuildValidationMessage(validation, isTextQualifier, validation.RecordImportErrors, MethodNameGetErrorMessageFileValidator, fileName);
        }

        public string GetBadData(ObjectValidationResult validationResult, bool isTextQualifier)
        {
            var sbDetail = new StringBuilder();
            try
            {
                if (validationResult?.RecordImportErrors != null)
                {
                    var myKeys = new string[validationResult.HeadersOriginal.Count];
                    validationResult.HeadersOriginal.Keys.CopyTo(myKeys, 0);
                    AppendHeaders(validationResult, sbDetail, myKeys, HeadersOriginalKey, isTextQualifier);
                    AppendImportErrors(validationResult.RecordImportErrors, sbDetail, isTextQualifier, false, string.Empty);
                }
            }
            catch (Exception ex)
            {
                LogException(ex, GetBadDataCommand);
            }
            return sbDetail.ToString();
        }

        public static void AppendHeaders(
            ObjectValidationResult validationResult, 
            StringBuilder sbDetail, 
            string[] myKeys,
            string headerType,
            bool isTextQualifier)
        {
            Guard.NotNull(validationResult, nameof(validationResult));
            Guard.NotNull(sbDetail, nameof(sbDetail));
            var headers = new StringBuilder();

            if (myKeys != null)
            {
                foreach (var key in myKeys.OrderBy(name => name))
                {
                    if (key.Equals(OriginalImportRow, StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }

                    if (headerType.Equals(HeadersOriginalKey))
                    {
                        if (validationResult.HeadersOriginal.ContainsKey(key))
                        {
                            headers.Append(isTextQualifier ? $"\"{key}\"," : $"{key},");
                        }
                    }
                    else if (headerType.Equals(BadDataOriginalHeadersKey))
                    {
                        if (validationResult.BadDataOriginalHeaders.ContainsKey(key))
                        {
                            headers.Append(isTextQualifier ? $"\"{key}\"," : $"{key},");
                        }
                    }
                }
                sbDetail.AppendLine(headers.ToString().Trim().TrimEnd(CommaSeparator));
            }
        }

        public string GetBadData(ObjectValidationResult validationResult, SourceFile sourceFile)
        {
            var sbDetail = new StringBuilder();
            try
            {
                if (validationResult != null && validationResult.RecordImportErrors != null && sourceFile != null && validationResult.BadDataOriginalHeaders != null)
                {
                    var myKeys = new string[validationResult.BadDataOriginalHeaders.Count];
                    validationResult.BadDataOriginalHeaders.Keys.CopyTo(myKeys, 0);
                    AppendHeaders(validationResult, sbDetail, myKeys, BadDataOriginalHeadersKey, sourceFile.IsTextQualifier);

                    var importErrors = new HashSet<EntityImportError>();
                    foreach (var item in validationResult.RecordImportErrors.Distinct().OrderBy(x => x.RowNumber))
                    {
                        importErrors.Add(item);
                    }

                    AppendImportErrors(importErrors, sbDetail, sourceFile.IsTextQualifier, false, string.Empty);
                }
            }
            catch (Exception ex)
            {
                LogException(ex, GetBadDataCommand);
            }
            return sbDetail.ToString();
        }

        public string GetBadDataFileValidator(ObjectValidationResult validationResult, string fileName, bool isTextQualifier)
        {
            var sbDetail = new StringBuilder();

            try
            {
                var detLoc = CleanBadDataLocation(fileName);

                if (validationResult?.RecordImportErrors != null)
                {
                    var myKeys = new string[validationResult.HeadersOriginal.Count];
                    validationResult.HeadersOriginal.Keys.CopyTo(myKeys, 0);
                    AppendHeaders(validationResult, sbDetail, myKeys, HeadersOriginalKey, isTextQualifier);
                    AppendImportErrors(validationResult.RecordImportErrors, sbDetail, isTextQualifier, true, detLoc);
                }
            }
            catch (Exception ex)
            {
                LogException(ex, GetBadDataFileValidatorCommand);
            }
            return sbDetail.ToString();
        }

        public static string CleanBadDataLocation(string fileName)
        {
            var location = $"BadData_{fileName}.csv";
            var invalidFileChars = Path.GetInvalidFileNameChars();
            var cleanLocation = location;

            foreach (var invalidFileChar in invalidFileChars)
            {
                cleanLocation = cleanLocation.Replace(invalidFileChar, UnderlineSeparator);
            }

            var detLoc = $"{DqmPath}{cleanLocation}";
            if (File.Exists(detLoc))
            {
                File.Delete(detLoc);
            }

            return detLoc;
        }

        public static void AppendImportErrors(
            HashSet<EntityImportError> recordImportErrors,
            StringBuilder sbDetail,
            bool isTextQualifier,
            bool shoudClearSbDetail,
            string filePath)
        {
            Guard.NotNull(recordImportErrors, nameof(recordImportErrors));
            Guard.NotNull(sbDetail, nameof(sbDetail));

            sbDetail.AppendLine(Environment.NewLine);
            AppendAllTextToExistingFile(filePath, sbDetail.ToString());

            if (shoudClearSbDetail)
            {
                sbDetail.Clear();
            }

            foreach (var error in recordImportErrors.Distinct().OrderBy(x => x.RowNumber))
            {
                if (!string.IsNullOrWhiteSpace(error.BadDataRow))
                {
                    error.BadDataRow = error.BadDataRow.Trim().TrimEnd(CommaSeparator);
                    if (isTextQualifier)
                    {
                        var data = new StringBuilder();
                        var quoteRow = error.BadDataRow.Split(',');

                        foreach (var quote in quoteRow)
                        {
                            data.Append($"\"{quote}\",");
                        }

                        sbDetail.AppendLine(data.ToString().Trim().TrimEnd(CommaSeparator));
                    }
                    else
                    {
                        sbDetail.AppendLine(error.BadDataRow.Trim().TrimEnd(CommaSeparator));
                    }

                    AppendAllTextToExistingFile(filePath, sbDetail.ToString());
                    if (shoudClearSbDetail)
                    {
                        sbDetail.Clear();
                    }
                }
            }
        }

        private static void AppendAllTextToExistingFile(string filePath, string text)
        {
            if (File.Exists(filePath))
            {
                File.AppendAllText(filePath, text);
            }
        }

        public string GetBadDataFileValidator(ObjectValidationResult validationResult, SourceFile sourceFile)
        {
            var sbDetail = new StringBuilder();

            try
            {
                var detLoc = CleanBadDataLocation(sourceFile.FileName);

                if (validationResult?.RecordImportErrors != null)
                {
                    var myKeys = new string[validationResult.HeadersOriginal.Count];
                    validationResult.HeadersOriginal.Keys.CopyTo(myKeys, 0);
                    AppendHeaders(validationResult, sbDetail, myKeys, HeadersOriginalKey, sourceFile.IsTextQualifier);
                    AppendImportErrors(validationResult.RecordImportErrors, sbDetail, sourceFile.IsTextQualifier, true, detLoc);
                }
            }
            catch (Exception ex)
            {
                LogException(ex, GetBadDataFileValidatorCommand);
            }
            return sbDetail.ToString();
        }

        private string BuildValidationMessage(
            ObjectValidationResult validation,
            bool isTextQualifier,
            IEnumerable<EntityImportError> recordImportErrors)
        {
            var fileName = String.Empty;
            return BuildValidationMessage(validation, isTextQualifier, recordImportErrors, MethodNameGetErrorMessage, fileName);
        }

        private string BuildValidationMessage(
            ObjectValidationResult validation,
            bool isTextQualifier,
            IEnumerable<EntityImportError> recordImportErrors,
            string methodName,
            string fileName)
        {
            var detailBuilder = new StringBuilder();

            try
            {
                var filePath = ProcessFile(fileName);

                ProcessHeaderLabel(detailBuilder);
                ProcessHeaders(detailBuilder, validation, isTextQualifier);
                ProcessRecordLabels(detailBuilder);
                AppendToFile(filePath, detailBuilder);

                foreach (EntityImportError error in recordImportErrors)
                {
                    ProcessRecordDetail(detailBuilder, error, isTextQualifier, filePath);
                }

                ProcessDimensionSummaryHeader(detailBuilder);
                AppendToFile(filePath, detailBuilder);

                foreach (var errorSummary in validation.DimensionImportErrorSummaries)
                {
                    ProcessDimensionSummaryDetail(detailBuilder, errorSummary);
                    AppendToFile(filePath, detailBuilder);
                }
            }
            catch (Exception ex)
            {
                LogException(ex, methodName);
            }

            return detailBuilder.ToString();
        }
                
        private static void ProcessDimensionSummaryHeader(StringBuilder detailBuilder)
        {
            detailBuilder.AppendLine("<b>*** Dimension Summary Errors ***</b>");
            detailBuilder.AppendLine(Environment.NewLine);
            detailBuilder.AppendLine(Environment.NewLine);
            detailBuilder.AppendLine("<br/>");
            detailBuilder.AppendLine("<br/>");
        }

        private static void ProcessDimensionSummaryDetail(StringBuilder detailBuilder, ObjectImportErrorSummary errorSummary)
        {
            detailBuilder.AppendLine($"<b>PubCode:</b> {errorSummary.PubCode}<br/>");
            detailBuilder.AppendLine($"<b>MAFField:</b> {errorSummary.MAFField}<br/>");
            detailBuilder.AppendLine($"<b>Value:</b> { errorSummary.Value}<br/>");
            detailBuilder.AppendLine($"<b>ClientMessage:</b> {errorSummary.ClientMessage}<br/>");
            detailBuilder.AppendLine($"<b>ErrorCount:</b> {errorSummary.ErrorCount}<br/>");
            detailBuilder.AppendLine(Environment.NewLine);
            detailBuilder.AppendLine(Environment.NewLine);
            detailBuilder.AppendLine("<br/>");
        }

        private static void ProcessRecordLabels(StringBuilder detailBuilder)
        {
            detailBuilder.AppendLine(Environment.NewLine);
            detailBuilder.AppendLine(Environment.NewLine);
            detailBuilder.AppendLine("<br/>");
            detailBuilder.AppendLine("<br/>");
            detailBuilder.AppendLine("<b>*** Record Errors ***</b>");
            detailBuilder.AppendLine(Environment.NewLine);
            detailBuilder.AppendLine(Environment.NewLine);
            detailBuilder.AppendLine("<br/>");
            detailBuilder.AppendLine("<br/>");
        }

        private static void ProcessRecordDetail(
            StringBuilder detailBuilder,
            EntityImportError error,
            bool hasTextQualifier,
            string filePath)
        {
            var fileCall = !string.IsNullOrWhiteSpace(filePath);

            if (error.RowNumber > 0)
            {
                detailBuilder.AppendLine($"<b>Row Number:</b> {error.RowNumber}<br/>");
            }

            if (!string.IsNullOrEmpty(error.ClientMessage))
            {
                detailBuilder.AppendLine($"{error.ClientMessage}<br/>");
            }

            if (!string.IsNullOrEmpty(error.BadDataRow))
            {
                error.BadDataRow = error.BadDataRow.Trim().TrimEnd(',');
                if (hasTextQualifier == true)
                {
                    var data = new StringBuilder();
                    var quoteRow = error.BadDataRow.Split(',');

                    foreach (var badDataItem in quoteRow)
                    {
                        var badDataItemQuoted = $"\"{badDataItem}\",";
                        if (fileCall)
                        {
                            data.Append(badDataItemQuoted);
                        }
                        else
                        {
                            data.AppendLine(badDataItemQuoted);
                        }
                    }

                    var dataRow = $"<b>Data:</b> {data.ToString().Trim().TrimEnd(',')}";
                    detailBuilder.AppendLine(dataRow);
                    detailBuilder.AppendLine(Environment.NewLine);
                }
                else
                {
                    detailBuilder.AppendLine(error.BadDataRow.Trim().TrimEnd(','));
                    if (!fileCall)
                    {
                        detailBuilder.AppendLine(Environment.NewLine);
                    }
                }
            }

            if (!fileCall)
            {
                detailBuilder.AppendLine(Environment.NewLine);
            }
            detailBuilder.AppendLine(Environment.NewLine);
            detailBuilder.AppendLine("<br/>");
            detailBuilder.AppendLine("<br/>");

            if (fileCall)
            {
                AppendToFile(filePath, detailBuilder);
            }
        }
        
        private static void ProcessHeaders(StringBuilder detailBuilder, ObjectValidationResult validation, bool isTextQualifier)
        {
            var headers = new StringBuilder();
            var myKeys = new String[validation.HeadersOriginal.Count];
            validation.HeadersOriginal.Keys.CopyTo(myKeys, 0);

            foreach (var key in myKeys.OrderBy(x => x))
            {
                if (key.Equals(OriginalImportRow, StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                if (validation.HeadersOriginal.ContainsKey(key))
                {
                    if (isTextQualifier)
                    {
                        headers.Append($"\"{key}\"");
                    }
                    else
                    {
                        headers.Append($"{key},");
                    }
                }
            }

            detailBuilder.AppendLine(headers.ToString().Trim().TrimEnd(','));
        }

        private static void ProcessHeaderLabel(StringBuilder detailBuilder)
        {
            detailBuilder.AppendLine("<b>*** Headers ***</b>");
            detailBuilder.AppendLine(Environment.NewLine);
            detailBuilder.AppendLine("<br/>");
        }

        private static string ProcessFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return string.Empty;
            }

            var location = string.Format(LocationFileNameFormat, fileName);
            var cleanLocation = location;

            var invalidFileChars = Path.GetInvalidFileNameChars();
            foreach (var v in invalidFileChars)
            {
                cleanLocation = cleanLocation.Replace(v, '_');
            }

            var filePath = RootFolderPath + cleanLocation;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return filePath;
        }

        private static void AppendToFile(string filePath, StringBuilder detailBuilder)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return;
            }

            File.AppendAllText(filePath, detailBuilder.ToString());
            detailBuilder.Clear();
        }

        private void LogException(Exception ex, string methodName)
        {
            var app = KMApplications.ADMS_Engine;
            if (AppData.myAppData != null && AppData.myAppData.CurrentApp != null)
            {
                app = KMEnums.GetApplication(AppData.myAppData.CurrentApp.ApplicationName);
            }
            var appLogWorker = new KMPlatform.BusinessLogic.ApplicationLog();
            var formatException = StringFunctions.FormatException(ex);
            var location = GetType().Name + "." + methodName;
            appLogWorker.LogCriticalError(formatException, location, app, string.Empty);
        }
    }
}
