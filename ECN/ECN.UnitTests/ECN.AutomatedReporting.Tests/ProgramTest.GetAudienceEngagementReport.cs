using System;
using System.IO.Fakes;
using System.Xml;
using System.Collections.Generic;
using System.Net.Mail.Fakes;
using Shouldly;
using ecn.automatedreporting;
using NUnit.Framework;
using ECN_Framework_BusinessLayer.Activity.Report.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using System.Xml.Serialization.Fakes;
using ecn.automatedreporting.Reports;
using KMPlatform.BusinessLogic.Fakes;
using AudienceEngagementReport = ECN_Framework_Entities.Activity.Report.AudienceEngagementReport;
using TestAudienceEngagementReport = ecn.automatedreporting.Reports.AudienceEngagementReport;

namespace ECN.AutomatedReporting.Tests
{
    public partial class ProgramTest
    {
        private const string MethodGetAudienceEngagementReport = "GetAudienceEngagementReport";
        private const string SuccessfulResultStringGetAudienceEngagementReport =
            "Your scheduled report of Audience Engagement has been attached";
        private const string NoRecordStringGetAudienceEngagementReport =
            "Your scheduled report of Audience Engagement didn't return any data";
        private const string FailureMessageForGetGetAudienceEngagementReport =
            "Your scheduled report of Audience Engagement has failed";
        private const string XMLKeyClickPercentage = "ClickPercentage";

        [Test]
        public void GetAudienceEngagementReport_FileTypePdf_RecurrenceDefault_NoException()
        {
            // Arrange
            InitilizeGetAudienceEngagementReportTests(FileTypePdf, String.Empty);
            SettingsForFileTypePdfXls();
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestAudienceEngagementReport(message, constructedBody, _reportSchedule, DateTime.Today);
            
            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetAudienceEngagementReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetAudienceEngagementReport_FileTypeXls_RecurrenceMonthly_NoException()
        {
            // Arrange
            InitilizeGetAudienceEngagementReportTests(FileTypeXls, ValueRecurrenceTypeMonthly);
            SettingsForFileTypePdfXls();
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestAudienceEngagementReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetAudienceEngagementReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetAudienceEngagementReport_FileTypeXml_RecurrenceWeekly_NoException()
        {
            // Arrange
            InitilizeGetAudienceEngagementReportTests(FileTypeXml, ValueRecurrenceTypeWeekly);
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestAudienceEngagementReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetAudienceEngagementReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetAudienceEngagementReport_FileTypeCsv_RecurrenceDaily_NoException()
        {
            // Arrange
            InitilizeGetAudienceEngagementReportTests(FileTypeCsv, ValueRecurrenceTypeDaily);
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestAudienceEngagementReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetAudienceEngagementReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetAudienceEngagementReport_FileTypeDefault_RecurrenceTypeMonthly_NoData()
        {
            // Arrange
            InitilizeGetAudienceEngagementReportTests(String.Empty, ValueRecurrenceTypeMonthly);
            ShimAudienceEngagementReport.GetListByRangeInt32Int32StringStringDateTimeDateTime =
                (x1, x2, x, x3, x4, x5) => new List<AudienceEngagementReport>();
            _testedClass.SetStaticFieldOrProperty(FieldMasterStartDate, new DateTime(2018, 2, 2));
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestAudienceEngagementReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldContain(NoRecordStringGetAudienceEngagementReport);
        }

        [Test]
        public void GetAudienceEngagementReport_FileTypeDefault_RecurrenceDefault_NoReportParameterException()
        {
            // Arrange
            InitilizeGetAudienceEngagementReportTests(String.Empty, String.Empty);
            _reportSchedule = new ECN_Framework_Entities.Communicator.Fakes.ShimReportSchedule
            {
                RecurrenceTypeGet = () => throw new Exception(ExceptionNoReportScheduleParameters)
            };
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestAudienceEngagementReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldContain(FailureMessageForGetGetAudienceEngagementReport);
            _anyException.ShouldBeTrue();
            _exceptionMessage.ShouldBe(ExceptionNoReportScheduleParameters);
        }

        private void InitilizeGetAudienceEngagementReportTests(string fileFormat, string recurrenceType)
        {
            ShimDirectory.ExistsString = (p) => false;
            ShimDirectory.CreateDirectoryString = (p) => null;
            ShimFile.ExistsString = (p) => true;
            ShimFile.DeleteString = (p) => { };
            ShimFile.AppendAllTextStringString = (x1, x2) => { };

            ShimUser.GetByUserIDInt32Boolean = (x1, x2) => null;

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
                    sampleElement = sampleXMLDoc.CreateElement(XMLKeyClickPercentage);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(ValueText1));
                    reportScheduleElement.AppendChild(sampleElement);

                    return sampleXMLDoc.InnerXml;
                },
                ExportFormatGet = () => fileFormat,
                RecurrenceTypeGet = () => recurrenceType,
            };

            ShimAudienceEngagementReport.GetListByRangeInt32Int32StringStringDateTimeDateTime =
                (x1, x2, x, x3, x4, x5) => new List<AudienceEngagementReport>()
                {
                    new AudienceEngagementReport { }
                };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (p) => new Group();
            ShimAttachment.ConstructorString = (x1,x2) => { };
            ShimFileInfo.AllInstances.AppendText = (p) => new ShimStreamWriter();
            ShimXmlSerializer.AllInstances.SerializeTextWriterObject = (x1, x2, x3) => { };            
        }
    }
}
