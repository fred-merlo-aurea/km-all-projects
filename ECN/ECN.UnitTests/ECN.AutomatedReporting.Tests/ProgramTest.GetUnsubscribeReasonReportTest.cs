using System;
using System.Collections.Generic;
using System.Data.Fakes;
using System.IO;
using System.IO.Fakes;
using System.Net.Fakes;
using System.Reflection;
using System.Xml;
using ecn.automatedreporting;
using ecn.automatedreporting.Reports;
using ecn.automatedreporting.Reports.Helpers;
using ECN_Framework_BusinessLayer.Activity.Report.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Activity.Report;
using ECN_Framework_Entities.Communicator;
using KMPlatform.BusinessLogic.Fakes;
using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.WebForms.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.AutomatedReporting.Tests
{
    public partial class ProgramTest
    {
        private const string MethodGetUnsubscribeReasonReport = "GetUnsubscribeReasonReport";
        private const string XMLSearchBy = "SearchBy";
        private const string XMLSearchText = "SearchText";
        private const string SuccessfulResultStringGetUnsubscribeReasonReport = 
            "</BR>Your scheduled export of Unsubscribe Reason Report has been moved to Dummy";
        private const string FTPFailureMessageForGetUnsubscribeReasonReport = 
            "</BR>Your scheduled export of Unsubscribe Reason Report has failed due to an FTP issue";
        private const string FailureMessageForGetUnsubscribeReasonReport = 
            "</BR>Your scheduled export of Unsubscribe Reason Report has failed";
        private const string NoRecordStringGetUnsubscribeReasonReport = 
            "</BR>Your scheduled export of Unsubscribe Reason Report didn't return any data";

        [Test]
        public void GetUnsubscribeReasonReport_RecurrenceTypeMonthly_XMLFileType_NoException()
        {
            // Arrange
            InitilizeGetUnsubscribeReasonReport(FileTypeXml, ValueRecurrenceTypeMonthly, ValueInt1, ValueInt1, ValueInt1);
            ShimFileStream.ConstructorStringFileMode = (x1, x2, x3) => { new ShimFileStream(); };
            ShimReportViewerExport.ToDataTableOf1IListOfM0<UnsubscribeReasonDetail>((p) =>
            {
                return new ShimDataTable
                {
                    WriteXmlStreamBoolean = (x1, x2) => { }
                };
            });
            var constructedBody = String.Empty;

            var methodArguments = new object[] { null, constructedBody, _reportSchedule };

            // Act	
            var returnResult = (ReturnReport)_testedClass.InvokeStatic(
                MethodGetUnsubscribeReasonReport, methodArguments);
            constructedBody = (string)methodArguments[1];

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            constructedBody.ShouldBe(SuccessfulResultStringGetUnsubscribeReasonReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetUnsubscribeReasonReport_RecurrenceTypeWeekly_PdfFileType_NoException()
        {
            // Arrange
            InitilizeGetUnsubscribeReasonReport(FileTypePdf, ValueRecurrenceTypeWeekly, ValueInt1, ValueInt1, ValueInt1);
            ShimBlastFieldsName.GetByBlastFieldIDInt32User = (x1, x2) => new BlastFieldsName();
            var constructedBody = String.Empty;
            var methodArguments = new object[] { null, constructedBody, _reportSchedule };

            // Act	
            var returnResult = (ReturnReport)_testedClass.InvokeStatic(
                MethodGetUnsubscribeReasonReport, methodArguments);
            constructedBody = (string)methodArguments[1];

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            constructedBody.ShouldBe(SuccessfulResultStringGetUnsubscribeReasonReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetUnsubscribeReasonReport_RecurrenceTypeDaily_XlsFileType_NoException()
        {
            // Arrange
            InitilizeGetUnsubscribeReasonReport(FileTypeXls, ValueRecurrenceTypeDaily);
            ShimBlastFieldsName.GetByBlastFieldIDInt32User = (x1, x2) => null;
            var constructedBody = String.Empty;
            var methodArguments = new object[] { null, constructedBody, _reportSchedule };

            // Act	
            var returnResult = (ReturnReport)_testedClass.InvokeStatic(
                MethodGetUnsubscribeReasonReport, methodArguments);
            constructedBody = (string)methodArguments[1];

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            constructedBody.ShouldBe(SuccessfulResultStringGetUnsubscribeReasonReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetUnsubscribeReasonReport_RecurrenceTypeDefault_CsvFileType_NoException()
        {
            // Arrange
            InitilizeGetUnsubscribeReasonReport(FileTypeCsv, String.Empty);
            var constructedBody = String.Empty;
            var methodArguments = new object[] { null, constructedBody, _reportSchedule };

            // Act	
            var returnResult = (ReturnReport)_testedClass.InvokeStatic(
                MethodGetUnsubscribeReasonReport, methodArguments);
            constructedBody = (string)methodArguments[1];

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            constructedBody.ShouldBe(SuccessfulResultStringGetUnsubscribeReasonReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetUnsubscribeReasonReport_RecurrenceTypeDefault_DafaultFileType_FTPException()
        {
            // Arrange
            InitilizeGetUnsubscribeReasonReport(String.Empty, String.Empty);
            ShimWebRequest.CreateString = (p) =>
            {
                throw new Exception(ExceptionMessageFTP);
            };
            var constructedBody = String.Empty;
            var methodArguments = new object[] { null, constructedBody, _reportSchedule };

            // Act	
            var returnResult = (ReturnReport)_testedClass.InvokeStatic(
                MethodGetUnsubscribeReasonReport, methodArguments);
            constructedBody = (string)methodArguments[1];

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeFalse();
            constructedBody.ShouldBe(FTPFailureMessageForGetUnsubscribeReasonReport);
            _anyException.ShouldBeTrue();
            _exceptionMessage.ShouldBe(ExceptionMessageFTP);
        }

        [Test]
        public void GetUnsubscribeReasonReport_RecurrenceTypeMonthly_DafaultFileType_NoData()
        {
            // Arrange
            InitilizeGetUnsubscribeReasonReport(String.Empty, ValueRecurrenceTypeMonthly);
            ShimUnsubscribeReasonDetail.GetReportStringStringDateTimeDateTimeInt32String =
                (searchField, searchCriteria, fromDate, toDate, customerID, reason) => new List<UnsubscribeReasonDetail>();
            _testedClass.SetStaticFieldOrProperty(FieldMasterStartDate, new DateTime(2018, 2, 2));
            var constructedBody = String.Empty;
            var methodArguments = new object[] { null, constructedBody, _reportSchedule };

            // Act	
            var returnResult = (ReturnReport)_testedClass.InvokeStatic(
                MethodGetUnsubscribeReasonReport, methodArguments);
            constructedBody = (string)methodArguments[1];

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeFalse();
            constructedBody.ShouldBe(NoRecordStringGetUnsubscribeReasonReport);
        }

        [Test]
        public void GetUnsubscribeReasonReport_RecurrenceTypeDefault_DafaultFileType_NoReportParameterException()
        {
            // Arrange
            InitilizeGetUnsubscribeReasonReport(String.Empty, String.Empty);
            _reportSchedule = new ECN_Framework_Entities.Communicator.Fakes.ShimReportSchedule
            {
                ReportParametersGet = () => throw new Exception(ExceptionNoReportScheduleParameters)
            };
            var constructedBody = String.Empty;
            var methodArguments = new object[] { null, constructedBody, _reportSchedule };

            // Act	
            var returnResult = (ReturnReport)_testedClass.InvokeStatic(
                MethodGetUnsubscribeReasonReport, methodArguments);
            constructedBody = (string)methodArguments[1];

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeFalse();
            constructedBody.ShouldBe(FailureMessageForGetUnsubscribeReasonReport);
            _anyException.ShouldBeTrue();
            _exceptionMessage.ShouldBe(ExceptionNoReportScheduleParameters);
        }

        private void InitilizeGetUnsubscribeReasonReport(string fileType, string recurrenceType, int delivery = 0, int topen = 0, int usend = 0)
        {
            FakeReportSchedule(fileType, recurrenceType);
            FakeIOMethods();
            FakeReport();
            FakeWebRequest();

            ReportsHelper.Assembly = Assembly.GetExecutingAssembly();
            ShimUser.GetByUserIDInt32Boolean = (x1, x2) => null;

            _testedClass.SetStaticFieldOrProperty(FieldMasterStartDate, new DateTime(2018, 1, 1));
        }

        private void FakeIOMethods()
        {
            ShimDirectory.ExistsString = (p) => false;
            ShimDirectory.CreateDirectoryString = (p) => null;
            ShimFile.ExistsString = (p) => true;
            ShimFile.DeleteString = (p) => { };
            ShimFile.ReadAllBytesString = (p) => { return new byte[0]; };
            ShimFile.WriteAllBytesStringByteArray = (x1, x2) => { };
            ShimFile.WriteAllTextStringString = (x1, x2) => { };
        }

        private void FakeReportSchedule(string fileType, string recurrenceType)
        {
            _reportSchedule = new ECN_Framework_Entities.Communicator.Fakes.ShimReportSchedule
            {
                CustomerIDGet = () => ValueInt1,
                ReportParametersGet = () =>
                {
                    var sampleXMLDoc = new XmlDocument();
                    var reportScheduleElement = sampleXMLDoc.CreateElement(XMLKeyReportSchedule);
                    sampleXMLDoc.AppendChild(reportScheduleElement);
                    var sampleElement = sampleXMLDoc.CreateElement(XMLSearchBy);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(DummyText));
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement = sampleXMLDoc.CreateElement(XMLKeyFTPURL);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(DummyText));
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement = sampleXMLDoc.CreateElement(XMLSearchText);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(DummyText));
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
                RecurrenceTypeGet = () => recurrenceType
            };
        }

        private void FakeReport()
        {
            ShimReport.AllInstances.LoadReportDefinitionStream = (x1, x2) => { };
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
            ShimUnsubscribeReasonDetail.GetReportStringStringDateTimeDateTimeInt32String =
                (searchField, searchCriteria, fromDate, toDate, customerID, reason) => new List<UnsubscribeReasonDetail>
                { new UnsubscribeReasonDetail { EmailSubject = DummyText } };
        }

        private void FakeWebRequest()
        {
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
        }
    }
}
