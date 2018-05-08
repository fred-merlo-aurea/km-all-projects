using System;
using System.IO.Fakes;
using System.Xml;
using System.Collections.Generic;
using System.Net.Fakes;
using System.IO;
using System.Data.Fakes;
using System.Reflection;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_BusinessLayer.Activity.Report.Fakes;
using ECN_Framework_Entities.Activity.Report;
using KMPlatform.BusinessLogic.Fakes;
using Shouldly;
using ecn.automatedreporting;
using ecn.automatedreporting.Reports;
using ecn.automatedreporting.Reports.Helpers;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using Microsoft.Reporting.WebForms.Fakes;
using Microsoft.Reporting.WebForms;
using NUnit.Framework;
using DataDumpReport = ECN_Framework_Entities.Activity.Report.DataDumpReport;
using TestDataDumpReport = ecn.automatedreporting.Reports.DataDumpReport;

namespace ECN.AutomatedReporting.Tests
{
    public partial class ProgramTest
    {
        private const string MethodGetDataDumpReport = "GetDataDumpReport";
        private const string FieldMasterStartDate = "MasterStartDate";
        private const string FieldAssembly = "assembly";
        private const string AssemblyNameECNFrameworkCommon = "ECN_Framework_Common.dll";
        private const string ValueRecurrenceTypeMonthly = "monthly";
        private const string ValueRecurrenceTypeWeekly = "weekly";
        private const string ValueRecurrenceTypeDaily = "daily";
        private const string AppSettingKeyFilePath = "FilePath";
        private const string FileTypeXls = "xls";
        private const string FileTypePdf = "pdf";
        private const string FileTypeXml = "xml";
        private const string FileTypeCsv = "csv";
        private const string DummyFilePath = "/";
        private const string ExceptionMessageInvalidFTPURI = "Invalid URI: The format of the URI could not be determined.";
        private const string SuccessfulResultString = "</BR>Your scheduled export of Group Attribute Report has been moved to Dummy";
        private const string FailureResultString = "</BR>Your scheduled export of Group Attribute Report has failed";
        private const string NoDataResultString = "</BR>Your scheduled export of Group Attribute Report didn't return any data Dummy";
        private const string ExceptionNoReportScheduleParameters = "No Report Schedule Parameters";

        [Test]
        public void GetDataDumpReport_RecurrenceTypeMonthly_XMLFileType_NoException()
        {
            // Arrange
            InitilizeGetDataDumpReportTests(FileTypeXml, ValueRecurrenceTypeMonthly, ValueInt1, ValueInt1, ValueInt1);
            ShimFileStream.ConstructorStringFileMode = (x1, x2, x3) => { new ShimFileStream(); };
            ShimReportViewerExport.ToDataTableOf1IListOfM0<DataDumpReport>( (p) => 
            {
                return new ShimDataTable
                {
                    WriteXmlStreamBoolean = (x1, x2) => { }
                };
            });
            var constructedBody = String.Empty;
            var report = new TestDataDumpReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldBe(SuccessfulResultString);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetDataDumpReport_RecurrenceTypeWeekly_PdfFileType_NoException()
        {
            // Arrange
            InitilizeGetDataDumpReportTests(FileTypePdf, ValueRecurrenceTypeWeekly, ValueInt1, ValueInt1, ValueInt1);
            SettingsForFileTypePdfXls();
            ShimBlastFieldsName.GetByBlastFieldIDInt32User = (x1, x2) => new BlastFieldsName();
            var constructedBody = String.Empty;
            var report = new TestDataDumpReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldBe(SuccessfulResultString);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetDataDumpReport_RecurrenceTypeDaily_XlsFileType_NoException()
        {
            // Arrange
            InitilizeGetDataDumpReportTests(FileTypeXls,ValueRecurrenceTypeDaily);
            SettingsForFileTypePdfXls();
            ShimBlastFieldsName.GetByBlastFieldIDInt32User = (x1, x2) => null;
            var constructedBody = String.Empty;
            var report = new TestDataDumpReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldBe(SuccessfulResultString);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetDataDumpReport_RecurrenceTypeDefault_CsvFileType_NoException()
        {
            // Arrange
            InitilizeGetDataDumpReportTests(FileTypeCsv, String.Empty);
            var constructedBody = String.Empty;
            var report = new TestDataDumpReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldBe(SuccessfulResultString);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetDataDumpReport_RecurrenceTypeDefault_DafaultFileType_FTPException()
        {
            // Arrange
            InitilizeGetDataDumpReportTests(String.Empty, String.Empty);
            ShimWebRequest.CreateString = (p) =>
            {
                throw new Exception(ExceptionMessageFTP);
            };
            var constructedBody = String.Empty;
            var report = new TestDataDumpReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldBe(FailureResultString);
            _anyException.ShouldBeTrue();
            _exceptionMessage.ShouldBe(ExceptionMessageFTP);
        }

        [Test]
        public void GetDataDumpReport_RecurrenceTypeMonthly_DafaultFileType_NoData()
        {
            // Arrange
            InitilizeGetDataDumpReportTests(String.Empty, ValueRecurrenceTypeMonthly);
            ShimDataDumpReport.GetListInt32DateTimeDateTimeString = (x1, x2, x3, x4) => new List<DataDumpReport>();
            _testedClass.SetStaticFieldOrProperty(FieldMasterStartDate, new DateTime(2018, 2, 2));
            var constructedBody = String.Empty;
            var report = new TestDataDumpReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldBe(NoDataResultString);
        }

        [Test]
        public void GetDataDumpReport_RecurrenceTypeDefault_DafaultFileType_NoReportParameterException()
        {
            // Arrange
            InitilizeGetDataDumpReportTests(String.Empty, String.Empty);
            _reportSchedule = new ECN_Framework_Entities.Communicator.Fakes.ShimReportSchedule
            {
                ReportParametersGet = () => throw new Exception(ExceptionNoReportScheduleParameters)
            };
            var constructedBody = String.Empty;
            var report = new TestDataDumpReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldBe(FailureResultString);
            _anyException.ShouldBeTrue();
            _exceptionMessage.ShouldBe(ExceptionNoReportScheduleParameters);
        }

        private void SettingsForFileTypePdfXls()
        {
            ShimReport.AllInstances.LoadReportDefinitionStream = (x1,x2) => { };
            ShimLocalReport.AllInstances.SetParametersIEnumerableOfReportParameter = (x1, x2) => { };
            ShimReport.AllInstances.RenderStringStringStringOutStringOutStringOutStringArrayOutWarningArrayOut =
                (Report instance, string format, string deviceInfo, out string mimeType, out string encoding, out string fileNameExtension, out string[] streams, out Warning[] warnings) =>
                {
                    mimeType = DummyText;
                    encoding = DummyText;
                    fileNameExtension = DummyText;
                    streams = new string[0];
                    warnings = new Warning[0];

                    return new Byte[0];
                };
            ReportsHelper.Assembly = Assembly.GetExecutingAssembly();
        }

        private void InitilizeGetDataDumpReportTests(string fileType, string recurrenceType, int delivery = 0, int topen = 0, int usend = 0)
        {
            _reportSchedule = new ECN_Framework_Entities.Communicator.Fakes.ShimReportSchedule
            {
                CustomerIDGet = () => ValueInt1,
                ReportParametersGet = () =>
                {
                    var sampleXMLDoc = new XmlDocument();
                    var reportScheduleElement = sampleXMLDoc.CreateElement(XMLKeyReportSchedule);
                    sampleXMLDoc.AppendChild(reportScheduleElement);
                    var sampleElement = sampleXMLDoc.CreateElement(XMLKeyGroupIDs);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(ValueText1));
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement = sampleXMLDoc.CreateElement(XMLKeyFTPURL);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(DummyText));
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement = sampleXMLDoc.CreateElement(XMLKeyCustomerID);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(ValueText1));
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement = sampleXMLDoc.CreateElement(XMLKeyFTPUsername);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(DummyText));
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement = sampleXMLDoc.CreateElement(XMLKeyFTPPassword);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(DummyText));
                    reportScheduleElement.AppendChild(sampleElement);

                    return sampleXMLDoc.InnerXml;
                },
                ExportFormatGet = () => fileType,
                RecurrenceTypeGet = () => recurrenceType,
                CreatedUserIDGet = () => ValueInt1
            };

            ShimDirectory.ExistsString = (p) => false;
            ShimDirectory.CreateDirectoryString = (p) => null;
            ShimFile.ExistsString = (p) => true;
            ShimFile.DeleteString = (p) => { };
            ShimFile.ReadAllBytesString = (p) => { return new byte[0]; };
            ShimFile.WriteAllBytesStringByteArray = (x1,x2) => { };
            ShimFile.WriteAllTextStringString = (x1, x2) => { };

            ShimDataDumpReport.GetListInt32DateTimeDateTimeString = (x1, x2, x3, x4) =>
            {
                var reportList = new List<DataDumpReport>();
                var dataReport = new DataDumpReport();
                dataReport.EmailSubject = DummyText;
                dataReport.Delivery = delivery;
                dataReport.usend = usend;
                dataReport.topen = topen;
                dataReport.uopen = topen;
                reportList.Add(dataReport);
                return reportList;
            };

            ShimUser.GetByUserIDInt32Boolean = (x1, x2) => null;
            ShimWebRequest.CreateString = (p) =>
            {
                return new ShimFtpWebRequest
                {
                    GetRequestStream = () =>
                    {
                        return (Stream)new ShimFileStream
                        {
                            WriteByteArrayInt32Int32 = (x1, x2, x3) => { }
                        };
                    },
                    GetResponse = () =>
                    {
                        return new ShimFtpWebResponse
                        {
                            StatusDescriptionGet = () => DummyText
                        };
                    }
                };
            };

            _testedClass.SetStaticFieldOrProperty(FieldMasterStartDate, new DateTime(2018, 1, 1));
        }
    }
}
