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
using ECN_Framework_Entities.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Report.Fakes;
using ECN_Framework_Entities.Communicator.Report;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using TestEmailPreviewUsageReport = ecn.automatedreporting.Reports.EmailPreviewUsageReport;

namespace ECN.AutomatedReporting.Tests
{
    public partial class ProgramTest
    {
        private const string MethodGetEmailPreviewUsageReport = "GetEmailPreviewUsageReport";
        private const string SuccessfulResultStringGetEmailPreviewUsageReport =
            "Your scheduled report of EmailPreview Usage has been attached";
        private const string NoRecordStringGetEmailPreviewUsageReport =
            "Your scheduled report of Blast Delivery didn't return any data.";
        private const string FailureMessageForGetEmailPreviewUsageReport =
            "Your scheduled report of Email Preview Usage has failed.";

        [Test]
        public void GetEmailPreviewUsageReport_FileTypePdf_RecurrenceDefault_NoException()
        {
            // Arrange
            InitilizeGetEmailPreviewUsageReportTests(FileTypePdf, String.Empty);
            SettingsForFileTypePdfXls();
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestEmailPreviewUsageReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetEmailPreviewUsageReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetEmailPreviewUsageReport_FileTypeXls_RecurrenceMonthly_NoException()
        {
            // Arrange
            InitilizeGetEmailPreviewUsageReportTests(FileTypeXls, ValueRecurrenceTypeMonthly);
            SettingsForFileTypePdfXls();
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestEmailPreviewUsageReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetEmailPreviewUsageReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetEmailPreviewUsageReport_FileTypeXml_RecurrenceWeekly_NoException()
        {
            // Arrange
            InitilizeGetEmailPreviewUsageReportTests(FileTypeXml, ValueRecurrenceTypeWeekly);
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestEmailPreviewUsageReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetEmailPreviewUsageReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetEmailPreviewUsageReport_FileTypeCsv_RecurrenceDaily_NoException()
        {
            // Arrange
            InitilizeGetEmailPreviewUsageReportTests(FileTypeCsv, ValueRecurrenceTypeDaily);
            ShimEmailPerformanceByDomainReport.AddDelimiterListOfEmailPerformanceByDomain =
                (p) => String.Empty;
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestEmailPreviewUsageReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetEmailPreviewUsageReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetEmailPreviewUsageReport_FileTypeDefault_RecurrenceTypeMonthly_NoData()
        {
            // Arrange
            InitilizeGetEmailPreviewUsageReportTests(String.Empty, ValueRecurrenceTypeMonthly);
            ShimEmailPreviewUsage.GetUsageDetailsAutomatedStringDateTimeDateTime =
                (x1, x2, x3) => new List<EmailPreviewUsage>();
            _testedClass.SetStaticFieldOrProperty(FieldMasterStartDate, new DateTime(2018, 2, 2));
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestEmailPreviewUsageReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldContain(NoRecordStringGetEmailPreviewUsageReport);
        }

        [Test]
        public void GetEmailPreviewUsageReport_FileTypeDefault_RecurrenceDefault_NoReportParameterException()
        {
            // Arrange
            InitilizeGetEmailPreviewUsageReportTests(String.Empty, String.Empty);
            _reportSchedule = new ECN_Framework_Entities.Communicator.Fakes.ShimReportSchedule
            {
                ReportParametersGet = () => throw new Exception(ExceptionNoReportScheduleParameters)
            };
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestEmailPreviewUsageReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldContain(FailureMessageForGetEmailPreviewUsageReport);
            _anyException.ShouldBeTrue();
            _exceptionMessage.ShouldBe(ExceptionNoReportScheduleParameters);
        }

        private void InitilizeGetEmailPreviewUsageReportTests(string fileFormat, string recurrenceType)
        {
            ShimDirectory.ExistsString = (p) => false;
            ShimDirectory.CreateDirectoryString = (p) => null;
            ShimFile.ExistsString = (p) => true;
            ShimFile.DeleteString = (p) => { };
            ShimFile.AppendAllTextStringString = (x1, x2) => { };

            ShimCustomer.GetByCustomerIDInt32Boolean = (x1, x2) => null;

            _reportSchedule = new ECN_Framework_Entities.Communicator.Fakes.ShimReportSchedule
            {
                CustomerIDGet = () => ValueInt1,
                ReportParametersGet = () =>
                {
                    var sampleXMLDoc = new XmlDocument();
                    var reportScheduleElement = sampleXMLDoc.CreateElement(XMLKeyReportSchedule);
                    sampleXMLDoc.AppendChild(reportScheduleElement);
                    var sampleElement = sampleXMLDoc.CreateElement(XMLKeyCustomerID);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(ValueText1));
                    reportScheduleElement.AppendChild(sampleElement);

                    return sampleXMLDoc.InnerXml;
                },
                ExportFormatGet = () => fileFormat,
                RecurrenceTypeGet = () => recurrenceType
            };

            ShimEmailPreviewUsage.GetUsageDetailsAutomatedStringDateTimeDateTime = 
                (x1, x2, x3) => new List<EmailPreviewUsage>() { new EmailPreviewUsage { } };
            ShimAttachment.ConstructorString = (x1,x2) => { };
            ShimFileInfo.AllInstances.AppendText = (p) => new ShimStreamWriter();
            ShimXmlSerializer.AllInstances.SerializeTextWriterObject = (x1, x2, x3) => { };
        }
    }
}
