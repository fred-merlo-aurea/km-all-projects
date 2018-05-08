using System;
using System.IO.Fakes;
using System.Text;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using ApplicationLog = KMPlatform.BusinessLogic.Fakes.ShimApplicationLog;
using ImportError = FrameworkUAD.Entity.ImportError;
using ImportErrorSummary = FrameworkUAD.Object.ImportErrorSummary;
using ObjectValidationResult = FrameworkUAD.Object.ValidationResult;

namespace UAS.UnitTests.FrameworkUAD.BusinessLogic
{
    [TestFixture]
    public partial class ValidationResultTest
    {
        private const char CommaSeparator = ',';
        private const string FileFullPath = @"C:\ADMS\Applications\DQM\Details_Filename_BadData.csv";
        private const string FileNameSample = "Filename";
        private const int LogSaveSuccess = 1;
        private const int BadDataRowCellCount = 5;

        private IDisposable _shims;
        private bool previousFileDelete = false;
        private readonly bool previousFileExist = true;
        private readonly StringBuilder fileContentBuilder = new StringBuilder();
        private string errorMessage = string.Empty;
        private ObjectValidationResult validationResultObject;
        private int savedLog;
        private string filePathUsed = string.Empty;

        private StringBuilder _sbDetail;
        private bool _shouldClearSbDetailt;
        private bool _isTextQualifier;

        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();

            fileContentBuilder.Clear();
            ShimFile.AppendAllTextStringString =
                (path, contents) =>
                {
                    filePathUsed = path;
                    fileContentBuilder.Append(contents);
                };
            ShimFile.ExistsString = (path) => previousFileExist;
            ShimFile.DeleteString = (path) => previousFileDelete = true;
            ApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String = (a, b, c, d, e, f, g) => savedLog = 1;
            validationResultObject = new ObjectValidationResult();
            InitData(validationResultObject);

            _sbDetail = new StringBuilder();
        }

        [TearDown]
        public void Teardown()
        {
            _shims?.Dispose();
        }
        
        private void InitData(ObjectValidationResult validationResultObject)
        {
            var itemCount = 1;
            for (int i = 0; i < itemCount; i++)
            {
                validationResultObject.HeadersOriginal.Add($"HeaderKey{i}", $"HeaderValue{i}");
            }
            validationResultObject.HeadersOriginal.Add("originalimportrow", "originalimportrow_value");

            for (var i = 0; i < itemCount; i++)
            {
                var error = new ImportError();
                error.BadDataRow = CreateBadDataRow(i, BadDataRowCellCount);
                error.RowNumber = i + 1;
                error.ClientMessage = $"ClientMessage{i}";
                validationResultObject.RecordImportErrors.Add(error);
            }

            for (var i = 0; i < itemCount; i++)
            {
                var errorSummary = new ImportErrorSummary();
                errorSummary.PubCode = $"PubCode{i}";
                errorSummary.MAFField = $"MAFField{i}";
                errorSummary.ClientMessage = $"ClientMessage{i}";
                errorSummary.ErrorCount = 8;

                validationResultObject.DimensionImportErrorSummaries.Add(errorSummary);
            }
        }

        private string CreateBadDataRow(int index, int cellCount)
        {
            var rowBuilder = new StringBuilder();
            for (var cellIndex = 0; cellIndex < cellCount; cellIndex++)
            {
                rowBuilder.Append($"BadDataRow{index}-{cellIndex},");
            }
            return rowBuilder.ToString();
        }

        private string CustomerErrorFileValidatorResult(bool hasQualifier)
        {
            var fileValidator = true;
            return CustomerErrorBuilder(hasQualifier, fileValidator);
        }

        private string CustomerErrorResult(bool hasQualifier)
        {
            var fileValidator = false;
            return CustomerErrorBuilder(hasQualifier, fileValidator);
        }

        private string CustomerErrorBuilder(bool hasQualifier, bool fileValidator)
        {
            var builder = new StringBuilder();
            builder.AppendLine("<b>*** Headers ***</b>");
            builder.AppendLine("");
            builder.AppendLine("");
            builder.AppendLine("<br/>");
            if (hasQualifier)
            {
                builder.AppendLine("\"headerkey0\"");
            }
            else
            {
                builder.AppendLine("headerkey0");
            }

            builder.AppendLine("");
            builder.AppendLine("");
            builder.AppendLine("");
            builder.AppendLine("");
            builder.AppendLine("<br/>");
            builder.AppendLine("<br/>");
            builder.AppendLine("<b>*** Record Errors ***</b>");
            builder.AppendLine("");
            builder.AppendLine("");
            builder.AppendLine("");
            builder.AppendLine("");
            builder.AppendLine("<br/>");
            builder.AppendLine("<br/>");
            builder.AppendLine("<b>Row Number:</b> 1<br/>");
            builder.AppendLine("ClientMessage0<br/>");
            if (hasQualifier)
            {
                if (fileValidator)
                {
                    builder.AppendLine("<b>Data:</b> \"BadDataRow0-0\",\"BadDataRow0-1\",\"BadDataRow0-2\",\"BadDataRow0-3\",\"BadDataRow0-4\"");
                }
                else
                {
                    builder.AppendLine("<b>Data:</b> \"BadDataRow0-0\",");
                    builder.AppendLine("\"BadDataRow0-1\",");
                    builder.AppendLine("\"BadDataRow0-2\",");
                    builder.AppendLine("\"BadDataRow0-3\",");
                    builder.AppendLine("\"BadDataRow0-4\"");
                }

                builder.AppendLine("");
                builder.AppendLine("");
            }
            else
            {
                builder.AppendLine("BadDataRow0-0,BadDataRow0-1,BadDataRow0-2,BadDataRow0-3,BadDataRow0-4");
                if (!fileValidator)
                {
                    builder.AppendLine("");
                    builder.AppendLine("");
                }
            }
            if (!fileValidator)
            {
                builder.AppendLine("");
                builder.AppendLine("");
            }
            builder.AppendLine("");
            builder.AppendLine("");
            builder.AppendLine("<br/>");
            builder.AppendLine("<br/>");
            builder.AppendLine("<b>*** Dimension Summary Errors ***</b>");
            builder.AppendLine("");
            builder.AppendLine("");
            builder.AppendLine("");
            builder.AppendLine("");
            builder.AppendLine("<br/>");
            builder.AppendLine("<br/>");
            builder.AppendLine("<b>PubCode:</b> PubCode0<br/>");
            builder.AppendLine("<b>MAFField:</b> MAFField0<br/>");
            builder.AppendLine("<b>Value:</b> <br/>");
            builder.AppendLine("<b>ClientMessage:</b> ClientMessage0<br/>");
            builder.AppendLine("<b>ErrorCount:</b> 8<br/>");
            builder.AppendLine("");
            builder.AppendLine("");
            builder.AppendLine("");
            builder.AppendLine("");
            builder.AppendLine("<br/>");
            return builder.ToString();
        }
    }
}

