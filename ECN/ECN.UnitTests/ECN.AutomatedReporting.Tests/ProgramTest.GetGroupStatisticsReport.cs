using System;
using System.IO.Fakes;
using System.Xml;
using System.Collections.Generic;
using System.Net.Mail.Fakes;
using System.Xml.Serialization.Fakes;
using Shouldly;
using ecn.automatedreporting;
using NUnit.Framework;
using ECN_Framework_BusinessLayer.Activity.Report.Fakes;
using ECN_Framework_Entities.Activity.Report;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using KMPlatform.BusinessLogic.Fakes;
using Microsoft.Reporting.WebForms.Fakes;
using TestGroupStatisticsReport = ecn.automatedreporting.Reports.GroupStatisticsReport;

namespace ECN.AutomatedReporting.Tests
{
    public partial class ProgramTest
    {
        private const string MethodGetGroupStatisticsReport = "GetGroupStatisticsReport";
        private const string SuccessfulResultGetGroupStatisticsReport =
            "Your scheduled report of Group Statistics has been attached";
        private const string NoRecordStringGetGroupStatisticsReport =
            "Your scheduled report of Group Statistics didn't return any data";
        private const string FailureMessageForGetGroupStatisticsReport =
            "Your scheduled report of Group Statistics has failed";
        private const string XMLKeyShowBroswerDetails = "ShowBroswerDetails";
        private const string ValueTextyes = "yes";

        [Test]
        public void GetGroupStatisticsReport_FileTypePdf_RecurrenceDefault_NoException()
        {
            // Arrange
            InitilizeGetGroupStatisticsReportTests(FileTypePdf, String.Empty, String.Empty);
            SettingsForGetGroupStatisticsReportFileTypePdfXls();
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestGroupStatisticsReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultGetGroupStatisticsReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetGroupStatisticsReport_FileTypeXls_RecurrenceMonthly_NoException()
        {
            // Arrange
            InitilizeGetGroupStatisticsReportTests(FileTypeXls, ValueRecurrenceTypeMonthly, String.Empty);
            SettingsForGetGroupStatisticsReportFileTypePdfXls();
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestGroupStatisticsReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultGetGroupStatisticsReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetGroupStatisticsReport_FileTypeXml_RecurrenceWeekly_NoException()
        {
            // Arrange
            InitilizeGetGroupStatisticsReportTests(FileTypeXml, ValueRecurrenceTypeWeekly, String.Empty);
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestGroupStatisticsReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultGetGroupStatisticsReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetGroupStatisticsReport_FileTypeCsv_RecurrenceDaily_NoException()
        {
            // Arrange
            InitilizeGetGroupStatisticsReportTests(FileTypeCsv, ValueRecurrenceTypeDaily, String.Empty);
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestGroupStatisticsReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultGetGroupStatisticsReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetGroupStatisticsReport_FileTypeXls_RecurrenceMonthly_ShowDetails_NoException()
        {
            // Arrange
            InitilizeGetGroupStatisticsReportTests(FileTypeXls, ValueRecurrenceTypeMonthly, ValueTextyes);
            SettingsForGetGroupStatisticsReportFileTypePdfXls();
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestGroupStatisticsReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultGetGroupStatisticsReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetGroupStatisticsReport_FileTypeXml_RecurrenceWeekly_ShowDetails_NoException()
        {
            // Arrange
            InitilizeGetGroupStatisticsReportTests(FileTypeXml, ValueRecurrenceTypeWeekly, ValueTextyes);
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestGroupStatisticsReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultGetGroupStatisticsReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetGroupStatisticsReport_FileTypeCsv_RecurrenceDaily_ShowDetails_NoException()
        {
            // Arrange
            InitilizeGetGroupStatisticsReportTests(FileTypeCsv, ValueRecurrenceTypeDaily, ValueTextyes);
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestGroupStatisticsReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultGetGroupStatisticsReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetGroupStatisticsReport_FileTypeDefault_RecurrenceTypeMonthly_NoData()
        {
            // Arrange
            InitilizeGetGroupStatisticsReportTests(String.Empty, ValueRecurrenceTypeMonthly, String.Empty);
            ShimGroupStatisticsReport.GetInt32DateTimeDateTime =
                (x1, x2, x3) => new List<GroupStatisticsReport>();
            _testedClass.SetStaticFieldOrProperty(FieldMasterStartDate, new DateTime(2018, 2, 2));
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestGroupStatisticsReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldContain(NoRecordStringGetGroupStatisticsReport);
        }

        [Test]
        public void GetGroupStatisticsReport_FileTypeDefault_RecurrenceDefault_NoReportParameterException()
        {
            // Arrange
            InitilizeGetGroupStatisticsReportTests(String.Empty, String.Empty, String.Empty);
            _reportSchedule = new ECN_Framework_Entities.Communicator.Fakes.ShimReportSchedule
            {
                ReportParametersGet = () => throw new Exception(ExceptionNoReportScheduleParameters)
            };
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestGroupStatisticsReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldContain(FailureMessageForGetGroupStatisticsReport);
            _anyException.ShouldBeTrue();
            _exceptionMessage.ShouldBe(ExceptionNoReportScheduleParameters);
        }

        private void InitilizeGetGroupStatisticsReportTests(string fileFormat, string recurrenceType, string showBroweserDetails)
        {
            ShimDirectory.ExistsString = (p) => false;
            ShimDirectory.CreateDirectoryString = (p) => null;
            ShimFile.ExistsString = (p) => true;
            ShimFile.DeleteString = (p) => { };
            ShimFile.AppendAllTextStringString = (x1, x2) => { };

            ShimUser.GetByAccessKeyStringBoolean = (x1, x2) => null;

            _reportSchedule = new ECN_Framework_Entities.Communicator.Fakes.ShimReportSchedule
            {
                CustomerIDGet = () => ValueInt1,
                ReportParametersGet = () =>
                {
                    var sampleXMLDoc = new XmlDocument();
                    var reportScheduleElement = sampleXMLDoc.CreateElement(XMLKeyReportSchedule);
                    sampleXMLDoc.AppendChild(reportScheduleElement);
                    var sampleElement = sampleXMLDoc.CreateElement(XMLKeyGroupID);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(ValueText1));
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement = sampleXMLDoc.CreateElement(XMLKeyShowBroswerDetails);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(showBroweserDetails));
                    reportScheduleElement.AppendChild(sampleElement);

                    return sampleXMLDoc.InnerXml;
                },
                ExportFormatGet = () => fileFormat,
                RecurrenceTypeGet = () => recurrenceType,
            };

            ShimGroupStatisticsReport.GetInt32DateTimeDateTime =
                (x1, x2, x3) => new List<GroupStatisticsReport>()
                {
                    new GroupStatisticsReport { EmailSubject = DummyText }
                };

            ShimGroupStatisticsReport.GetReportDetailsListOfGroupStatisticsReportBoolean =
                (x1, x2) => new List<GroupStatisticsReport>();

            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (p) => new Group();
            ShimAttachment.ConstructorString = (x1,x2) => { };
            ShimFileInfo.AllInstances.AppendText = (p) => new ShimStreamWriter();
            ShimXmlSerializer.AllInstances.SerializeTextWriterObject = (x1, x2, x3) => { };
        }

        private void SettingsForGetGroupStatisticsReportFileTypePdfXls()
        {
            SettingsForFileTypePdfXls();
            ShimLocalReport.AllInstances.LoadSubreportDefinitionStringStream = (x1, x2, x3) => { };
        }
    }
}
