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
using TestEmailPerformanceByDomainReport = ecn.automatedreporting.Reports.EmailPerformanceByDomainReport;

namespace ECN.AutomatedReporting.Tests
{
    public partial class ProgramTest
    {
        private const string MethodGetEmailPerformanceByDomainReport = "GetEmailPerformanceByDomainReport";
        private const string SuccessfulResultStringGetEmailPerformanceByDomainReport =
            "Your scheduled report of Email Performance By Domain has been attached";
        private const string NoRecordStringGetEmailPerformanceByDomainReport =
            "Your scheduled report of Email Performance By Domain didn't return any data";
        private const string FailureMessageForGetEmailPerformanceByDomainReport =
            "Your scheduled report of Email Performance By Domain has failed";
        private const string XmlKeyDrillDownOther = "DrillDownOther";
        private const string ValueTextTrue = "True";

        [Test]
        public void GetEmailPerformanceByDomainReport_FileTypePdf_RecurrenceDefault_NoException()
        {
            // Arrange
            InitilizeGetEmailPerformanceByDomainReportTests(FileTypePdf, String.Empty);
            SettingsForFileTypePdfXls();
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestEmailPerformanceByDomainReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetEmailPerformanceByDomainReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetEmailPerformanceByDomainReport_FileTypeXls_RecurrenceMonthly_NoException()
        {
            // Arrange
            InitilizeGetEmailPerformanceByDomainReportTests(FileTypeXls, ValueRecurrenceTypeMonthly);
            SettingsForFileTypePdfXls();
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestEmailPerformanceByDomainReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetEmailPerformanceByDomainReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetEmailPerformanceByDomainReport_FileTypeXml_RecurrenceWeekly_NoException()
        {
            // Arrange
            InitilizeGetEmailPerformanceByDomainReportTests(FileTypeXml, ValueRecurrenceTypeWeekly);
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestEmailPerformanceByDomainReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetEmailPerformanceByDomainReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetEmailPerformanceByDomainReport_FileTypeCsv_RecurrenceDaily_NoException()
        {
            // Arrange
            InitilizeGetEmailPerformanceByDomainReportTests(FileTypeCsv, ValueRecurrenceTypeDaily);
            ShimEmailPerformanceByDomainReport.AddDelimiterListOfEmailPerformanceByDomain =
                (p) => String.Empty;
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestEmailPerformanceByDomainReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetEmailPerformanceByDomainReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetEmailPerformanceByDomainReport_FileTypeDefault_RecurrenceTypeMonthly_NoData()
        {
            // Arrange
            InitilizeGetEmailPerformanceByDomainReportTests(String.Empty, ValueRecurrenceTypeMonthly);
            ShimEmailPerformanceByDomainReport.GetInt32DateTimeDateTimeBoolean =
                (x1, x2, x3, x4) => new List<EmailPerformanceByDomain>();
            _testedClass.SetStaticFieldOrProperty(FieldMasterStartDate, new DateTime(2018, 2, 2));
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestEmailPerformanceByDomainReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldContain(NoRecordStringGetEmailPerformanceByDomainReport);
        }

        [Test]
        public void GetEmailPerformanceByDomainReport_FileTypeDefault_RecurrenceDefault_NoReportParameterException()
        {
            // Arrange
            InitilizeGetEmailPerformanceByDomainReportTests(String.Empty, String.Empty);
            _reportSchedule = new ECN_Framework_Entities.Communicator.Fakes.ShimReportSchedule
            {
                ReportParametersGet = () => throw new Exception(ExceptionNoReportScheduleParameters)
            };
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestEmailPerformanceByDomainReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldContain(FailureMessageForGetEmailPerformanceByDomainReport);
            _anyException.ShouldBeTrue();
            _exceptionMessage.ShouldBe(ExceptionNoReportScheduleParameters);
        }

        private void InitilizeGetEmailPerformanceByDomainReportTests(string fileFormat, string recurrenceType)
        {
            ShimDirectory.ExistsString = (p) => false;
            ShimDirectory.CreateDirectoryString = (p) => null;
            ShimFile.ExistsString = (p) => true;
            ShimFile.DeleteString = (p) => { };
            ShimFile.AppendAllTextStringString = (x1, x2) => { };

            _reportSchedule = new ECN_Framework_Entities.Communicator.Fakes.ShimReportSchedule
            {
                CustomerIDGet = () => ValueInt1,
                ReportParametersGet = () =>
                {
                    var sampleXMLDoc = new XmlDocument();
                    var reportScheduleElement = sampleXMLDoc.CreateElement(XMLKeyReportSchedule);
                    sampleXMLDoc.AppendChild(reportScheduleElement);
                    var sampleElement = sampleXMLDoc.CreateElement(XmlKeyDrillDownOther);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(ValueTextTrue));
                    reportScheduleElement.AppendChild(sampleElement);

                    return sampleXMLDoc.InnerXml;
                },
                ExportFormatGet = () => fileFormat,
                RecurrenceTypeGet = () => recurrenceType,
                ReportScheduleIDGet = () => ValueInt1
            };

            ShimEmailPerformanceByDomainReport.GetInt32DateTimeDateTimeBoolean =
                (x1, x2, x3, x4) => new List<EmailPerformanceByDomain>() { new EmailPerformanceByDomain { } };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (p) => new Group();
            ShimAttachment.ConstructorString = (x1,x2) => { };
            ShimFileInfo.AllInstances.AppendText = (p) => new ShimStreamWriter();
            ShimXmlSerializer.AllInstances.SerializeTextWriterObject = (x1, x2, x3) => { };
        }
    }
}
